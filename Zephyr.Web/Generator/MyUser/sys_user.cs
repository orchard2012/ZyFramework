using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_userService : ServiceBase<sys_user>
    {
       
    }

    public class sys_user : ModelBase
    {
        [PrimaryKey]   
        public string UserCode { get; set; }
        public string UserSeq { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string OrganizeName { get; set; }
        public string ConfigJSON { get; set; }
        public bool? IsEnable { get; set; }
        public int? LoginCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
