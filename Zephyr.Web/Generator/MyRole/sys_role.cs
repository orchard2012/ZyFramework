using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_roleService : ServiceBase<sys_role>
    {
       
    }

    public class sys_role : ModelBase
    {
        [PrimaryKey]   
        public string RoleCode { get; set; }
        public string RoleSeq { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
