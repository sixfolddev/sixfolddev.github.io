using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class LoginAttemptModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginAttemptModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
