﻿using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_loginHistoryService : ServiceBase<sys_loginHistory>
    {
       
    }

    public class sys_loginHistory : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string HostIP { get; set; }
        public string LoginCity { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}
