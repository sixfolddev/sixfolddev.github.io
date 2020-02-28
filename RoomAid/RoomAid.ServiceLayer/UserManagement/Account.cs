using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class Account
    {
        //private string _userEmail;
        //private string _hashedPassword;
        //private string _salt;

        public string UserEmail { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }

        //public Account()
        //{
        //    UserEmail = _userEmail;
        //    HashedPassword = _hashedPassword;
        //    Salt = _salt;
        //}

        public Account(string email, string password, string salt)
        {
            UserEmail = email;
            HashedPassword = password;
            Salt = salt;
        }
    }
}
