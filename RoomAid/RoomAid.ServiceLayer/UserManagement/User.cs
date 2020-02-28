using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    // TODO: Implement admin parameter
    public class User
    {
        // Public accessors
        public int SystemID { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountStatus { get; set; }
        public DateTime DateOfBirth { get; set; } // To calculate age
        public string Gender { get; set; } // Male or female
        //public bool Admin { get; set; }

        public User(int systemID, string email,  string fname, string lname, string status, DateTime dob, string gender/*, bool admin*/)
        {
            SystemID = systemID;
            UserEmail = email;
            FirstName = fname;
            LastName = lname;
            AccountStatus = status;
            DateOfBirth = dob;
            Gender = gender;
            //Admin = admin;
        }
    }
}
