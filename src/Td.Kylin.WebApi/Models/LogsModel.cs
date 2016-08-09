using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Models
{
    internal class LogsModel
    {
        /// <summary>
		/// 类型
		/// </summary>
		public int Type
        {
            get; set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string Click
        {
            get; set;
        }
        /// <summary>
        /// 来源
        /// </summary>
        public int Source
        {
            get; set;
        }
        /// <summary>
        /// 程序版本
        /// </summary>
        public string ProgramVersion
        {
            get; set;
        }
        /// <summary>
        /// 运行环境
        /// </summary>
        public string RunEnvironmental
        {
            get; set;
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemVersion
        {
            get; set;
        }
        /// <summary>
        /// 内容/详情
        /// </summary>
        public string Content
        {
            get; set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get; set;
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime
        {
            get; set;
        }
    }
}
