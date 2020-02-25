using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public interface ICreateAccountService
    {
        IResult Create(User newUser);
       bool IfExist(User newUser);
    }
}
