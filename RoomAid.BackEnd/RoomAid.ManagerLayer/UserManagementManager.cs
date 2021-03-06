﻿using RoomAid.Authorization;
using RoomAid.DataAccessLayer;
using RoomAid.DataAccessLayer.UserManagement;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.UserManagement;
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

        private readonly AuthenticationService _authNService;
        private readonly SqlCreateAccountService _createAccountService;
        private readonly DeleteAccountSQLService _deleteAccountService;
        private readonly UpdateAccountSqlService _updateAccountService;
        private readonly PermissionUpdateSqlService _updatePermissionService;
        private readonly JWTService _authService;

        public UserManagementManager()
        {
            var sqlDao = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            var createAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            var newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            var newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            var mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            var bunchedDaos = new CreateAccountDAOs(createAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            _updatePermissionService = new PermissionUpdateSqlService(new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)),new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User)));
            _updateAccountService = new UpdateAccountSqlService(new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)));
            _createAccountService = new SqlCreateAccountService(bunchedDaos);
            _deleteAccountService = new DeleteAccountSQLService(new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)), new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User)), new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User)));
            _authNService = new AuthenticationService(new GetUserDao(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)));
            _authService = new JWTService();
        }


        public UserManagementManager(JWTService authService, AuthenticationService authenticationService, SqlCreateAccountService createAccountService, DeleteAccountSQLService deleteAccountService, UpdateAccountSqlService updateAccountService, PermissionUpdateSqlService updatePermissionService) {
            //_newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            //_newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            //_mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_systemDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            //_mappingDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            //_accountDB = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            _updatePermissionService = updatePermissionService;
            _updateAccountService = updateAccountService;
            _createAccountService = createAccountService;
            _deleteAccountService = deleteAccountService;
            _authNService = authenticationService;
            _authService = authService;
        }

        public string LoginAccount(LoginAttemptModel account)
        {
            for (int i = 0; i < 3; i++)
            {

                try
                {
                    _authNService._userEmail = account.Email;
                    var authNSuccess = _authNService.Authenticate(account.Password);
                    if (authNSuccess)
                        break;

                }
                catch(Exception e)
                {
                    var handler = new ErrorController();
                    handler.Handle(e);
                }
                if (i == 2)
                    throw new Exception("Failed to Authenticate User");
            }

            for(int i = 0; i < 3; i++)
            {
                try
                {
                    var user = _authNService.FindUser(account);
                    if(user is object)
                    {
                        return _authService.GenerateJWT(user);
                    }
                }
                catch (Exception e)
                {
                    var handler = new ErrorController();
                    handler.Handle(e);
                }
            }


            throw new Exception("Failed to Login");


        }

        public IResult CreateAccount(Account account, User user)
        {
            var users = new List<User> { user };
            var accounts = new List<Account> { account };
            _createAccountService._newAccounts = accounts;
            _updateAccountService._newUsers = users;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var createAccountResult = _createAccountService.Create();
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
                    var updateAccountResult = _updateAccountService.Update();
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
            var id= _updatePermissionService.SysIdFinder(user);
            Permission perms = new Permission(id);
            foreach(string s in Enum.GetValues(typeof(StandardUser)))
            {
                perms.AddPermission(s);
            }
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    _updatePermissionService._permissions = new List<Permission> { perms };
                    var permResult = _updatePermissionService.Update();
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
            _deleteAccountService._targetUsers = users;
            
            var userId = _updatePermissionService.SysIdFinder(user);
            for(int i = 0; i < 3; i++)
            {
                Permission perms = new Permission(userId);
                foreach(string s in Enum.GetValues(typeof(AuthZEnum)))
                {
                    perms.RemovePermission(s);
                }
                try
                {
                    _updatePermissionService._permissions = new List<Permission> { perms };
                    var permResult = _updatePermissionService.Update();
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
                    var deleteResult = _deleteAccountService.Delete();
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
            //var id = _updatePermissionService.SysIdFinder(user);
            //foreach (string s in Enum.GetValues(typeof(StandardUser)))
            //{
            //    perms.AddPermission(s);
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    try
            //    {
            //        _updatePermissionService._permissions = new List<Permission> { perms };
            //        var permResult = _updatePermissionService.Update();
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

            _updateAccountService._newUsers = new List<User> { user };
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var updateAccountResult = _updateAccountService.Update();
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
}
