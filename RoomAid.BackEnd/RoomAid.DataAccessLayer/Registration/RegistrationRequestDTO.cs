using System;
using System.ComponentModel.DataAnnotations;

namespace RoomAid.DataAccessLayerLayer
{
   public class RegistrationRequestDTO
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Dob { get; set; }
        public string Password { get; set; }
        public string Repeatpassword { get; set; }
    }
}
