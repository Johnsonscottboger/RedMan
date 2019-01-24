using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 获取或设置角色 ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
