using System;


namespace RoomAid.ServiceLayer
{
   public class RegistrationRequestDTO
    {
     
        public string Email { get; }
        public string Fname { get; }
        public string Lname { get; }
        public DateTime Dob { get; }
        public string Gender { get; }
        public string Password { get; }
        public string Repassword { get; }

        public RegistrationRequestDTO(string email, string fname, string lname, DateTime dob, string gender, string password, string repassword)
        {
            Email = email;
            Fname = fname;
            Lname = lname;
            Dob = dob;
            Gender = gender;
            Password = password;
            Repassword = repassword;
        }
    }
}
