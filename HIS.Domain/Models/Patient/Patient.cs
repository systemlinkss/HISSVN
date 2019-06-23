﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Domain.Models.Common;

namespace HIS.Domain.Models.Patient
{
    public class Patient : CommonFields
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TitleId { get; set; }
        public int GenderId { get; set; }
        public int CountryId { get; set; }
        public int BloodGroupId { get; set; }
        public string Mrno { get; set; }
        public string ContactNumber { get; set; }
        public string HouseNo { get; set; }
        public string StreetNo { get; set; }
        public string Area { get; set; }
        public string PostalCode { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyContact { get; set; }
        public string State { get; set; }
        public int UserId { get; set; }
        public string AccessCode { get; set; }
        public bool IsActive { get; set; }
        public string FatherHusbandName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public string SignatureUrl { get; set; }
        public int EmergencyRelationId { get; set; }
        public string Cnic { get; set; }
        public int MaritalStatus { get; set; }
        public int ReligionId { get; set; }
        public int PreferredLanguage { get; set; }
        public string Nationality { get; set; }
        public string MotherMaidenName { get; set; }
        public int EthnicGroup { get; set; }
        public string BirthPlace { get; set; }
        public string ImageDataUri { get; set; }

    }
}
