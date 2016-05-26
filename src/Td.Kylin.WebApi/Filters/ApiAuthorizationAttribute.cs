using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections.Generic;
using Td.AspNet.Utils;
using Td.Common;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Cache;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Filters
{
    public class ApiAuthorizationAttribute : ActionFilterAttribute
    {
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
            string Sign = string.Empty;
            DateTime Timestamp = DateTime.Parse("1970-01-01 00:00:00");
            string PartnerId = string.Empty;

            foreach (var item in queryDic)
            {
                if (item.Key == RequestParameterNames.Sign)
                {
                    Sign = item.Value;
                }
                if (item.Key == RequestParameterNames.PartnerId)
                {
                    PartnerId = item.Value;
                }
                if (item.Key == RequestParameterNames.Timestamp)
                {
                    long ticks = 0;
                    long.TryParse(item.Value, out ticks);
                    Timestamp = DateTime.FromBinary(ticks);
                }
            }

            if (string.IsNullOrEmpty(PartnerId) || string.IsNullOrEmpty(Sign))
            {
                context.Result = ActionResultHelper.Error(ResultCode.ParametersError, "参数 PartnerId 或 Sign丢失，或者值不正常");
                return;
            }

            //采用协调世界时进行校验（接口请求时同样采用协调世界时处理）
            if (Timestamp.AddMinutes(5) < DateTime.Now.ToUniversalTime())
            {
                context.Result = ActionResultHelper.Error(ResultCode.RequestExpires, "API请求时间超时，服务过期，请检查Timestamp或同步服务器时间");
                return;
            }

            var moduleInfo = new System_ModuleAuthorize();
            try
            {
                moduleInfo = ModuleAuthorizeCache.GetSecret(PartnerId);
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
            var secret = moduleInfo.AppSecret;
            if (string.IsNullOrEmpty(moduleInfo.AppSecret))
            {
                context.Result = ActionResultHelper.Error(ResultCode.AuthorizationFailed, "非法访问，授权未通过");
                return;
            }

            if (method == "POST")
            {
                queryDic.Remove(RequestParameterNames.Sign);
                try
                {
                    var data = request.Form;
                    var dic = new Dictionary<string, string>();
                    foreach (var item in data)
                    {
                        queryDic.Add(item.Key, item.Value[0]);
                    }
                }
                catch (Exception ex)
                {
                    context.Result = ActionResultHelper.Error(ResultCode.DataException, "request.Form 获取表单数据异常");
                    return;
                }
                var s = Strings.SignRequest(queryDic, secret);
                if (Sign != s)
                {
                    context.Result = ActionResultHelper.Error(ResultCode.SignException, "未通过签名验证，请检查签名的参数和顺序是否正确");
                    return;
                }

            }
            else if (method == "GET")
            {
                queryDic.Remove(RequestParameterNames.Sign);
                var s = Strings.SignRequest(queryDic, secret);
                if (Sign != s)
                {
                    context.Result = ActionResultHelper.Error(ResultCode.SignException, "未通过签名验证，请检查签名的参数和顺序是否正确");
                    return;
                }
            }
            else
            {
                context.Result = ActionResultHelper.Error(ResultCode.RequestModeInvalid, "请求的模式不正确");
                return;
            }

            //是否有权限访问
            bool powerSuccess = new Func<bool>(() =>
            {
                //不需要权限
                if (Code <= 0) return true;

                //访问的模块为Admin权限，通关
                if (EnumUtility.ContainsEnumItem(moduleInfo.Role, Role.Admin)) return true;

                //Use权限，且访问的模块拥有Editor或Use权限
                if (EnumUtility.ContainsEnumItem((int)Code, Role.Use) && (EnumUtility.ContainsEnumItem(moduleInfo.Role, Role.Editor) || EnumUtility.ContainsEnumItem(moduleInfo.Role, Role.Use))) return true;

                //Editor权限，且访问的模块拥有Editer权限
                if (EnumUtility.ContainsEnumItem((int)Code, Role.Editor) && EnumUtility.ContainsEnumItem(moduleInfo.Role, Role.Editor)) return true;

                return false;

            }).Invoke();

            //权限
            if (!powerSuccess)
            {
                context.Result = ActionResultHelper.Error(ResultCode.AuthorizationFailed, "模块权限不足");
                return;
            }

            UpdateHttpContextItems(context.HttpContext, queryDic);
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
