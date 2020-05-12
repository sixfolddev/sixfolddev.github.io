using System;
using System.ComponentModel.DataAnnotations;

namespace RoomAid.DataAccessLayerLayer
{
   public class RegistrationRequestDTO
    {

        [Required]
        public string Email { get; }
        [Required]
        public string Firstname { get; }
        [Required]
        public string Lastname { get; }
        [Required]
        public DateTime Dob { get; }
        [Required]
        public string Password { get; }
        [Required]
        public string Repeatpassword { get; }

        public RegistrationRequestDTO(string email, string fname, string lname, DateTime dob,  string password, string repassword)
        {
            Email = email;
            Firstname = fname;
            Lastname = lname;
            Dob = dob;
            Password = password;
            Repeatpassword = repassword;
        }
    }
}
