using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class PermissionUpdateSqlService:IUpdateAccountService
    {
        private readonly IUpdateAccountDAO _dao;
        private readonly List<Permission> _permissions;

        public PermissionUpdateSqlService(IUpdateAccountDAO dao, List<Permission> permissions)
        {
            _dao = dao;
            _permissions = permissions;
        }

        public PermissionUpdateSqlService(IUpdateAccountDAO dao, Permission permission)
        {
            _dao = dao;
            _permissions = new List<Permission>();
            _permissions.Add(permission);
        }

        public IResult Update()
        {
            String message = "";
            bool isSuccess = false;







            return new CheckResult(message, isSuccess);
        }





    }
}
