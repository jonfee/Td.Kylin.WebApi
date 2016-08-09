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
        /// <summary>
        /// 错误日志接口Url
        /// </summary>
        public LogOptions Log;
    }
    /// <summary>
    /// 日志记录
    /// </summary>
    public class LogOptions
    {
        /// <summary>
        /// 错误日志接口Url
        /// </summary>
        public string LogUrl;
        /// <summary>
        /// 程序版本
        /// </summary>
        public string ProgramVersion;
        /// <summary>
        /// 运行环境
        /// </summary>
        public string RunEnvironmental;
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemVersion;

    }

}
