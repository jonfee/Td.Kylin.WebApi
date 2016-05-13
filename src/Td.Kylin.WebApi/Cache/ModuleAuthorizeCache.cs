using System.Collections.Generic;
using System.Linq;
using Td.AspNet.Utils;
using Td.Kylin.WebApi.Data;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Cache
{
    public class ModuleAuthorizeCache
    {
        private static string ServerId
        {
            get
            {
                return WebApiConfig.Options.ServerID;
            }
        }

        public static string CacheKey
        {
            get
            {
                return ServerId + "ModuleAuthorize";
            }
        }

        public static List<System_ModuleAuthorize> GetCache()
        {
            var cache = LocalCacheProvider.Get<List<System_ModuleAuthorize>>(CacheKey);
            if (cache != null)
            {
                return cache;
            }
            return null;
        }

        /// <summary>
        /// 通过模块ID获取对应的授权信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static System_ModuleAuthorize GetSecret(string id)
        {
            var data = GetCache();
            if (data == null)
            {
                var list = ModuleAuthorizeProvider.GetBaseList(ServerId);
                LocalCacheProvider.Set(CacheKey, list);
                return list.SingleOrDefault(p => p.ModuleID == id);
            }
            else
            {
                return data.SingleOrDefault(p => p.ModuleID == id);
            }
        }
    }
}
