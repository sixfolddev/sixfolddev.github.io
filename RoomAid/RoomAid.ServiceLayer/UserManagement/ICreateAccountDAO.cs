using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public interface ICreateAccountDAO
    {
        IResult CreateAccount(string connectionString);
        IResult CreateUser(string connectionString);
        IResult CreateMapping(string connectionString);    
    }
}
