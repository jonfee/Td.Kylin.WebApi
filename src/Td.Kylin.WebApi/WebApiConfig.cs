using Td.Kylin.EnumLibrary;

namespace Td.Kylin.WebApi
{
    public class WebApiConfig
    {
        public static KylinWebApiOptions Options;
    }

    /// <summary>
    /// 请求的参数名
    /// </summary>
    public sealed class RequestParameterNames
    {
        /// <summary>
        /// 验证签名串
        /// </summary>
        public const string Sign = "Sign";

        /// <summary>
        /// 合作者ID
        /// </summary>
        public const string PartnerId = "PartnerId";

        /// <summary>
        /// 时间戳
        /// </summary>
        public const string Timestamp = "Timestamp";

        /// <summary>
        /// 当前操作区域
        /// </summary>
        public const string LBSArea = "LBSArea";

        /// <summary>
        /// 当前操作位置经度
        /// </summary>
        public const string LBSLongitude = "LBSLongitude";

        /// <summary>
        /// 当前操作位置纬度
        /// </summary>
        public const string LBSLatitude = "LBSLatitude";
    }
}
