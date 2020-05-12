using RoomAid.Authorization;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace RoomAid.ManagerLayer
{
    public enum StandardUser
    {
        EnabledAccount,
        DisableAccount,
        EditProfile,
        ViewProfile,
        DeleteAccount,
        SearchHousehold,
        SendMessage,
        ReplyMessage,
        ViewMessage,
        MarkMessage,
        DeleteMessage, 
        ViewInvite,
        AcceptInvite,
        DeclineInvite
    }
    public class UserManagementManager
    {
        private readonly ICreateAccountDAO _newAccountDAO;
        private readonly ICreateAccountDAO _newMappingDAO;
        private readonly IMapperDAO _mapperDAO;
        private readonly ICreateAccountDAO _newUserDAO;
        private readonly ISqlDAO _systemDB;
        private readonly ISqlDAO _mappingDB; 
        private readonly ISqlDAO _accountDB;
        //private readonly CreateAccountDAOs _daos; //newAccount, newMapping, newUser, mapper

        public SqlCreateAccountService CreateAccountService { get; set; }
        public DeleteAccountSQLService DeleteAccountService { get; set; }
        public UpdateAccountSqlService UpdateAccountService { get; set; }
        public PermissionUpdateSqlService UpdatePermissionService { get; set; }
        public JWTService AuthService { get; set; }


        public UserManagementManager(SqlCreateAccountService createAccountService, DeleteAccountSQLService deleteAccountService, UpdateAccountSqlService updateAccountService, PermissionUpdateSqlService updatePermissionService) {
            //_newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            //_newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            //_mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_systemDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            //_mappingDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_accountDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            UpdatePermissionService = updatePermissionService;
            UpdateAccountService = updateAccountService;
            CreateAccountService = createAccountService;
            DeleteAccountService = deleteAccountService;

        }

        public IResult CreateAccount(Account account, User user)
        {
            var users = new List<User> { user };
            var accounts = new List<Account> { account };
            CreateAccountService._newAccounts = accounts;
            UpdateAccountService._newUsers = users;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var createAccountResult = CreateAccountService.Create();
                    if (createAccountResult.IsSuccess)
                        break;
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if(i==2)
                {
                    throw new Exception("Failed to create Account");
                }
            }
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    var updateAccountResult = UpdateAccountService.Update();
                    if (updateAccountResult.IsSuccess)
                        break;
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if (i == 2)
                {
                    throw new Exception("Failed to create Account");
                }
            }
            var id= UpdatePermissionService.SysIdFinder(user);
            Permission perms = new Permission(id);
            foreach(string s in Enum.GetValues(typeof(StandardUser)))
            {
                perms.AddPermission(s);
            }
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    UpdatePermissionService._permissions = new List<Permission> { perms };
                    var permResult = UpdatePermissionService.Update();
                    if (permResult.IsSuccess)
                        break;
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if (i == 2)
                    throw new Exception("Failed to create Permissions");
            }

            
            var checkResult = new CheckResult("Account Successfully Created", true);
            LogService.Log(checkResult.Message);
            return checkResult;

        }

        public IResult DeleteAccount(User user)
        {

            var users = new List<User> { user };
            DeleteAccountService._targetUsers = users;
            
            var userId = UpdatePermissionService.SysIdFinder(user);
            for(int i = 0; i < 3; i++)
            {
                Permission perms = new Permission(userId);
                foreach(string s in Enum.GetValues(typeof(AuthZEnum)))
                {
                    perms.RemovePermission(s);
                }
                try
                {
                    UpdatePermissionService._permissions = new List<Permission> { perms };
                    var permResult = UpdatePermissionService.Update();
                    if (permResult.IsSuccess)
                        break;
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if (i == 2)
                    throw new Exception("Permissions failed to delete");
            }
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var deleteResult = DeleteAccountService.Delete();
                    if (deleteResult.IsSuccess)
                    {
                        LogService.Log(deleteResult.Message);
                        return deleteResult;
                    }
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if(i == 2)
                {
                    throw new Exception("Failed to delete Account");
                }
            }
            return new CheckResult("Failed to Delete Account", false);

            
        }


        public IResult UpdateAccount(User user)
        {
            //var id = UpdatePermissionService.SysIdFinder(user);
            //foreach (string s in Enum.GetValues(typeof(StandardUser)))
            //{
            //    perms.AddPermission(s);
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    try
            //    {
            //        UpdatePermissionService._permissions = new List<Permission> { perms };
            //        var permResult = UpdatePermissionService.Update();
            //        if (permResult.IsSuccess)
            //            break;
            //    }
            //    catch (Exception e)
            //    {
            //        ErrorController handler = new ErrorController();
            //        handler.Handle(e);
            //    }
            //    if (i == 2)
            //        throw new Exception("Failed to create Permissions");
            //}

            UpdateAccountService._newUsers = new List<User> { user };
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var updateAccountResult = UpdateAccountService.Update();
                    if (updateAccountResult.IsSuccess)
                        break;
                }
                catch (Exception e)
                {
                    ErrorController handler = new ErrorController();
                    handler.Handle(e);
                }
                if (i == 2)
                {
                    throw new Exception("Failed to create Account");
                }
            }

            return new CheckResult("Account updated successfully", true);


    
    }
}
