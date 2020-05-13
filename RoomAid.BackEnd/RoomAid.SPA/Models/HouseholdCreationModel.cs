using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomAid.SPA.Models
{
    public class HouseholdCreationModel
    {
        [Required]
        public string RequesterEmail { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string SuiteNumber { get; set; }
        [Required]
        public double Rent { get; set; }
    }
}