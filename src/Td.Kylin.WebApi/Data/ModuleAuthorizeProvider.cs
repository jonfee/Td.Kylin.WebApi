using System.Collections.Generic;
using System.Linq;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Data
{
    public class ModuleAuthorizeProvider
    {
        /// <summary>
        /// 获取系统授权数据
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static List<System_ModuleAuthorize> GetBaseList(string serverId)
        {
            using (var db = new DataContext())
            {
                var query = from p in db.ModuleAuthorize
                            where p.ServerID == serverId
                            select p;
                return query.ToList();
            }
        }

        ///// <summary>
        ///// 获取授权信息
        ///// </summary>
        ///// <param name="serverID"></param>
        ///// <param name="moduleID"></param>
        ///// <returns></returns>
        //public static ApiModuleAuthorizeCacheModel GetAuth(string serverID, string moduleID)
        //{
        //    using (var db = new DataContext())
        //    {
        //        var query = from p in db.ModuleAuthorize
        //                    where p.ServerID.Equals(serverID, System.StringComparison.OrdinalIgnoreCase) && p.ModuleID.Equals(moduleID, System.StringComparison.OrdinalIgnoreCase)
        //                    select new ApiModuleAuthorizeCacheModel
        //                    {
        //                        AppSecret = p.AppSecret,
        //                        ModuleID = p.ModuleID,
        //                        Role = p.Role,
        //                        ServerID = p.ServerID
        //                    };
        //        return query.FirstOrDefault();
        //    }
        //}
    }
}
