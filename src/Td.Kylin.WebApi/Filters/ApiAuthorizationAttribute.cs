
using System;
using System.Collections.Generic;
using Td.AspNet.Utils;
using Td.Common;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Cache;
using Td.Kylin.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Td.Kylin.WebApi.Filters
{
    public class ApiAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 合作身份
        /// </summary>
        public class Partner
        {
            public string PartnerId { get; set; }

            public string Sign { get; set; }

            public DateTime Timestamp { get; set; }

            public System_ModuleAuthorize Authorize { get; set; }
        }

        public Role Code { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;
            IDictionary<string, string> queryDic = Strings.SplitUrlQuery(request.QueryString.Value);
            if (queryDic == null)
            {
                context.Result = ActionResultHelper.Error(ResultCode.ParametersMissing, "URL参数缺失");
                return;
            }

            #region //合作者身份

            Partner partner = new Partner { Timestamp = DateTime.Parse("1970-01-01").ToUniversalTime() };

            foreach (var item in queryDic)
            {
                if (item.Key == RequestParameterNames.Sign)
                {
                    partner.Sign = item.Value;
                }
                if (item.Key == RequestParameterNames.PartnerId)
                {
                    partner.PartnerId = item.Value;
                }
                if (item.Key == RequestParameterNames.Timestamp)
                {
                    long ticks = 0;
                    long.TryParse(item.Value, out ticks);
                    partner.Timestamp = DateTime.FromBinary(ticks);
                }
            }
            #endregion

            if (string.IsNullOrEmpty(partner.PartnerId))
            {
                context.Result = ActionResultHelper.Error(ResultCode.ParametersError, "必须参数PartnerId缺失");
                return;
            }

            #region 授权信息

            var moduleInfo = new System_ModuleAuthorize();
            try
            {
                moduleInfo = ModuleAuthorizeCache.GetSecret(partner.PartnerId);
            }
            catch (Exception ex)
            {
                // 记录授权异常信息。
                Td.Diagnostics.Logger.Error(ex);

                context.Result = ActionResultHelper.Error(ResultCode.GetModuleException, ex.Message);
                return;
            }
            if (moduleInfo == null)
            {
                context.Result = ActionResultHelper.Error(ResultCode.GetModuleException, "模块授权信息不存在");
                return;
            }

            partner.Authorize = moduleInfo;

            #endregion

            if (method == "POST")
            {
                PostExecuting(context, partner, queryDic);
            }
            else if (method == "GET")
            {
                GetExecuting(context, partner, queryDic);
            }
            else
            {
                context.Result = ActionResultHelper.Error(ResultCode.RequestModeInvalid, "请求的模式不正确");
                return;
            }
            
            UpdateHttpContextItems(context.HttpContext, queryDic);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="context"></param>
        /// <param name="partner"></param>
        /// <param name="queryDic"></param>
        private void PostExecuting(ActionExecutingContext context, Partner partner, IDictionary<string, string> queryDic)
        {
            var request = context.HttpContext.Request;

            if (string.IsNullOrEmpty(partner.Sign))
            {
                context.Result = ActionResultHelper.Error(ResultCode.ParametersError, "必须参数Sign缺失");
                return;
            }

            //采用协调世界时进行校验（接口请求时同样采用协调世界时处理）
            if (partner.Timestamp.AddMinutes(5) < DateTime.Now.ToUniversalTime())
            {
                context.Result = ActionResultHelper.Error(ResultCode.RequestExpires, "API请求时间超时，服务过期，请检查Timestamp或同步服务器时间");
                return;
            }

            if (partner.Authorize == null)
            {
                context.Result = ActionResultHelper.Error(ResultCode.GetModuleException, "模块授权信息不存在");
                return;
            }

            queryDic.Remove(RequestParameterNames.Sign);
            try
            {
                var data = request.Form;
                var dic = new Dictionary<string, string>();
                foreach (var item in data)
                {
                    if (!queryDic.ContainsKey(item.Key))
                        queryDic.Add(item.Key, item.Value[0]);
                }
            }
            catch (Exception ex)
            {
                //ex
                context.Result = ActionResultHelper.Error(ResultCode.DataException, "request.Form 获取表单数据异常");
                return;
            }
            var s = Strings.SignRequest(queryDic, partner.Authorize.AppSecret ?? string.Empty);
            if (partner.Sign != s)
            {
                context.Result = ActionResultHelper.Error(ResultCode.SignException, "未通过签名验证，请检查签名的参数和顺序是否正确");
                return;
            }

            //是否有权限访问
            bool powerSuccess = CheckModulePower(partner);

            //权限
            if (!powerSuccess)
            {
                context.Result = ActionResultHelper.Error(ResultCode.AuthorizationFailed, "模块权限不足");
                return;
            }
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="context"></param>
        /// <param name="partner"></param>
        /// <param name="queryDic"></param>
        private void GetExecuting(ActionExecutingContext context, Partner partner, IDictionary<string, string> queryDic)
        {
            //如果为编辑权限或管理权限，则必须检测数字签名
            if (Code == Role.Editor || Code == Role.Admin)
            {
                if (string.IsNullOrEmpty(partner.Sign))
                {
                    context.Result = ActionResultHelper.Error(ResultCode.ParametersError, "必须参数Sign缺失");
                    return;
                }

                var s = Strings.SignRequest(queryDic, partner.Authorize.AppSecret);
                if (partner.Sign != s)
                {
                    context.Result = ActionResultHelper.Error(ResultCode.SignException, "未通过签名验证，请检查签名的参数和顺序是否正确");
                    return;
                }
            }

            //是否有权限访问
            bool powerSuccess = CheckModulePower(partner);

            //权限
            if (!powerSuccess)
            {
                context.Result = ActionResultHelper.Error(ResultCode.AuthorizationFailed, "模块权限不足");
                return;
            }
        }

        /// <summary>
        /// 检测合作者权限
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        private bool CheckModulePower(Partner partner)
        {
            //不需要权限
            if (Code <= 0) return true;

            //访问的模块为Admin权限，通关
            if (EnumUtility.ContainsEnumItem(partner.Authorize.Role, Role.Admin)) return true;

            //Use权限，且访问的模块拥有Editor或Use权限
            if (EnumUtility.ContainsEnumItem((int)Code, Role.Use) && (EnumUtility.ContainsEnumItem(partner.Authorize.Role, Role.Editor) || EnumUtility.ContainsEnumItem(partner.Authorize.Role, Role.Use))) return true;

            //Editor权限，且访问的模块拥有Editer权限
            if (EnumUtility.ContainsEnumItem((int)Code, Role.Editor) && EnumUtility.ContainsEnumItem(partner.Authorize.Role, Role.Editor)) return true;

            return false;
        }

        /// <summary>
        /// 设置请求上下文键值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="queryDic"></param>
        void UpdateHttpContextItems(HttpContext context, IDictionary<string, string> queryDic)
        {
            int lbsArea = 0;
            double lbsLongitude = 0d;
            double lbsLatitude = 0d;

            if (null != queryDic && queryDic.Count > 0)
            {
                string area = queryDic.ContainsKey(RequestParameterNames.LBSArea) ? queryDic[RequestParameterNames.LBSArea] : "0";
                string longitude = queryDic.ContainsKey(RequestParameterNames.LBSLongitude) ? queryDic[RequestParameterNames.LBSLongitude] : "0";
                string latitude = queryDic.ContainsKey(RequestParameterNames.LBSLatitude) ? queryDic[RequestParameterNames.LBSLatitude] : "0";

                int.TryParse(area, out lbsArea);
                double.TryParse(longitude, out lbsLongitude);
                double.TryParse(latitude, out lbsLatitude);
            }
            context.Items[RequestParameterNames.LBSArea] = lbsArea;
            context.Items[RequestParameterNames.LBSLongitude] = lbsLongitude;
            context.Items[RequestParameterNames.LBSLatitude] = lbsLatitude;
            //context.Items.Add(RequestParameterNames.LBSArea, lbsArea);//当前操作区域
            //context.Items.Add(RequestParameterNames.LBSLongitude, lbsLongitude);//当前操作位置经度
            //context.Items.Add(RequestParameterNames.LBSLatitude, lbsLatitude);//当前操作位置纬度
        }


    }
}
