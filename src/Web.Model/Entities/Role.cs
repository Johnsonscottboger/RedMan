using RedMan.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public Int64 RoleId { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}
