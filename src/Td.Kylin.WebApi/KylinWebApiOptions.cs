using Td.Kylin.EnumLibrary;

namespace Td.Kylin.WebApi
{
    public class KylinWebApiOptions
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string SqlConnectionString;

        /// <summary>
        /// 数据库提供程序类型
        /// </summary>
        public SqlProviderType SqlType;

        /// <summary>
        /// 当前授权的接口服务ID
        /// </summary>
        public string ServerID;
    }
}
