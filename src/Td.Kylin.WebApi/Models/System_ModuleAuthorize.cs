using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Models
{
    public class System_ModuleAuthorize
    {
        /// <summary>
        /// 服务系统编号（系统硬编码代号）
        /// </summary>
        public string ServerID { get; set; }

        /// <summary>
        /// 模块编号(系统硬编码代号)
        /// </summary>
        public string ModuleID { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public byte Role { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
