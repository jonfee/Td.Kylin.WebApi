using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Models
{
	[Table("System_ModuleAuthorize")]
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
        public int Role { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 行版本
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
