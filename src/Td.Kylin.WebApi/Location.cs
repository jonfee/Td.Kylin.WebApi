namespace Td.Kylin.WebApi
{
    /// <summary>
    /// LBS位置
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 当前操作区域
        /// </summary>
        public int OperatorArea { get; set; }

        /// <summary>
        /// 当前操作位置经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 当前操作位置纬度
        /// </summary>
        public double Latitude { get; set; }
    }
}
