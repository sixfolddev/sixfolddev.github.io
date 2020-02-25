using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public interface ICreateUserDAO
    {
        IResult Create(User newUser);
        int GetSystemID(User newUser);
    }
}
