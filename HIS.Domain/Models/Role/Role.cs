﻿using HIS.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Domain.Models.Role
{
    public class Role: CommonFields
    {
        public Int32 iRoleId { get; set; }
        public String vRoleName { get; set; }
        public String vRoleDescription { get; set; }
        public int? iOrganizationId { get; set; }  
    }
}
