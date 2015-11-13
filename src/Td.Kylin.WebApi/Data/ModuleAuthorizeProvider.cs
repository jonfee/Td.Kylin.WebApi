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
    }
}
