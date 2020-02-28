
using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateAccountService : ICreateAccountService
    {
        private readonly List<Account> _newAccounts;
        private readonly ICreateAccountDAO newAccountDAO; 
        private readonly ICreateAccountDAO newMappingDAO;
        private readonly IMapperDAO mapperDAO; 
        private readonly ICreateAccountDAO newUserDAO; 

        /// <summary>
        /// Service that crafts queries for inserting a new row in the account table for new account
        /// </summary>
        /// <param name="newAccount"></param>
        public SqlCreateAccountService(Account newAccount, CreateAccountDAOs daos)
        {
            this._newAccounts = new List<Account>();
            this._newAccounts.Add(newAccount);
            newAccountDAO = daos.CreateAccountDAO;
            newMappingDAO = daos.CreateMappingDAO;
            mapperDAO = daos.MapperDAO;
            newUserDAO = daos.CreateUserDAO;

        }

        /// <summary>
        /// Service that crafts queries for inserting multiple rows in account table
        /// </summary>
        /// <param name="newAccounts"></param>
        public SqlCreateAccountService(List<Account> newAccounts, CreateAccountDAOs daos)
        {
            this._newAccounts = newAccounts;
            newAccountDAO = daos.CreateAccountDAO;
            newMappingDAO = daos.CreateMappingDAO;
            mapperDAO = daos.MapperDAO;
            newUserDAO = daos.CreateUserDAO;
        }


        /// <summary>
        /// Service that crafts queries for insert new ros in the account table for the new account
        /// </summary>
        ///<returns>IResult true or false with error message</returns>
        public IResult Create()
        {

            string message = "";
            bool isSuccess = true;
            int totalSuccessed = 0;
            foreach (Account newAccount in _newAccounts)
            {
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateAccount"]);
                cmd.Parameters.AddWithValue("@email", newAccount.UserEmail);
                cmd.Parameters.AddWithValue("@hashedPw", newAccount.HashedPassword);
                cmd.Parameters.AddWithValue("@salt", newAccount.Salt);

                int rowsChanged = newAccountDAO.RunQuery(cmd);
                if (rowsChanged > 0)
                {
                    cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateMapping"]);
                    cmd.Parameters.AddWithValue("@email", newAccount.UserEmail);
                    rowsChanged = newMappingDAO.RunQuery(cmd);
                    if (rowsChanged > 0)
                    {
                        int sysID = mapperDAO.GetSysID(newAccount.UserEmail);

                        if (sysID!=-1)
                        {
                            cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateUser"]);
                            cmd.Parameters.AddWithValue("@sysId", sysID);
                            cmd.Parameters.AddWithValue("@email", newAccount.UserEmail);
                            cmd.Parameters.AddWithValue("@status", "Enable");
                            rowsChanged = newUserDAO.RunQuery(cmd);
                            if (rowsChanged > 0)
                            {
                                totalSuccessed +=1;
                            }
                            else
                                message +="failed create user for "+newAccount.UserEmail + "\n";
                        }
                    }
                    else
                        message +="failed create mapping for " +newAccount.UserEmail + "\n";
                }
                else
                {
                    message +=newAccount.UserEmail+"\n";
                }
            }


            if (totalSuccessed == _newAccounts.Count)
            {
                message = ConfigurationManager.AppSettings["CreateAccountSuccess"];
            }
            else
            {
                isSuccess = false;
            }
            return new CheckResult(message, isSuccess);
        }

    }      
}
