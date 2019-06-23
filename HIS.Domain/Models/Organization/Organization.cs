﻿using HIS.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Domain.Models.Organization
{
    public class Organization : CommonFields
    {
        public Int32 iOrganizationId { get; set; }
        [Display(Name = "Organization Name")]
        [Required(ErrorMessage = "Organization Name is required.")]
        public String vOrganizationName { get; set; }
        public String vOrganizationShortName { get; set; }
        public String vAddress { get; set; }
        public String vPhoneNo { get; set; }
        public Nullable<Int32> iCountryId { get; set; }
        public Nullable<Int32> iCityId { get; set; }
        public Nullable<System.DateTime> dActivationDate { get; set; }
        public Nullable<System.DateTime> dExpiryDate { get; set; }
        public Nullable<System.DateTime> dRegistrationDate { get; set; }
        public bool bIsActive { get; set; }  
        public String vContactPersonName { get; set; }
        public String vContactPersonPhoneNo { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String vEmail { get; set; }
        public String vDomainName { get; set; }
        public int iTimeZoneId { get; set; }
        public int iLanguageId { get; set; }
        public string OrganizationLogo { get; set; }
        public int iStatus { get; set; }
        public string Remarks { get; set; }
        public int StatusUserId { get; set; }
        public bool FirstTimeLogin { get; set; }

    }

    public class OrganizationStatus
    {
        public int StatusId { get; set; }
        public string StatusText { get; set; }
    }
}
