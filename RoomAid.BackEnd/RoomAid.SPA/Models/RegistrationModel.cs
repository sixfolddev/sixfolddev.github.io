using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomAid.SPA.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Email{get; set;}
        [Required]
        public string Firstname {get; set;}
        [Required]
        public string Lastname {get;set;}
        [Required]
        public string Password {get;set;}
        [Required]
        public string Repeatpassword {get;set;}
        [Required]
        public DateTime DateofBirth{get; set;}
    }
}