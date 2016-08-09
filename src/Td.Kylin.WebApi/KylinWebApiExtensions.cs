
using Microsoft.AspNetCore.Builder;
using System;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi
{
    public static class KylinWebApiExtensions
    {
        /// <summary>
        /// 注入授权
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKylinWebApi(this IApplicationBuilder builder, KylinWebApiOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.Use(next => new KylinWebApiMiddleware(next, options).Invoke);
        }

        /// <summary>
        /// 注入授权
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serverID">当前接口服务ID</param>
        /// <param name="sqlConnection"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKylinWebApi(this IApplicationBuilder builder, string serverID, string sqlConnection, SqlProviderType sqlType)
        {
            return UseKylinWebApi(builder, new KylinWebApiOptions
            {
                SqlConnectionString = sqlConnection,
                SqlType = sqlType,
                ServerID = serverID
            });
        }
        /// <summary>
        /// 注入授权
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serverID">当前接口服务ID</param>
        /// <param name="sqlConnection"></param>
        /// <param name="sqlType"></param>
        /// <param name="LogUrl">错误日志接口Url</param>
        /// <returns></returns>
        public static IApplicationBuilder UseKylinWebApi(this IApplicationBuilder builder, string serverID, string sqlConnection, SqlProviderType sqlType, LogOptions LogEntity)
        {
            return UseKylinWebApi(builder, new KylinWebApiOptions
            {
                SqlConnectionString = sqlConnection,
                SqlType = sqlType,
                ServerID = serverID,
                Log = LogEntity
            });
        }
    }
}
