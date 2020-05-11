using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ManagerLayer
{
    public class UserManagementManager
    {
        private readonly ICreateAccountDAO newAccountDAO;
        private readonly ICreateAccountDAO newMappingDAO;
        private readonly IMapperDAO mapperDAO;
        private readonly ICreateAccountDAO newUserDAO;
        private readonly CreateAccountDAOs daos; //newAccount, newMapping, newUser, mapper

        public ICreateAccountService CreateAccountService { get; set; }
        public IDeleteAccountService DeleteAccountService { get; set; }
        public IUpdateAccountService UpdateAccountService { get; set; }
        public IUpdateAccountService UpdatePermissionService { get; set; }
        public JWTService AuthService { get; set; }


        public UserManagementManager() { }
    
    }
}
