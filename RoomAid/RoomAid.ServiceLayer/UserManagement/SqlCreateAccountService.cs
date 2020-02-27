
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

        /// <summary>
        /// Service that crafts queries for inserting a new row in the account table for new account
        /// </summary>
        /// <param name="newAccount"></param>
        public SqlCreateAccountService(Account newAccount)
        {
            this._newAccounts = new List<Account>();
            this._newAccounts.Add(newAccount);
        }

        /// <summary>
        /// Service that crafts queries for inserting multiple rows in account table
        /// </summary>
        /// <param name="newAccounts"></param>
        public SqlCreateAccountService(List<Account> newAccounts)
        {
            this._newAccounts = newAccounts;
        }


        /// <summary>
        /// Service that crafts queries for insert new ros in the account table for the new account
        /// </summary>
        ///<returns>IResult true or false with error message</returns>
        public IResult Create()
        {
            ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
            ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
            IMapperDAO mapperDAO = new SqlMapperDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
            ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);

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
                        int userID = mapperDAO.GetSysID(newAccount.UserEmail);
                        if (userID!=-1)
                        {
                            cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateUser"]);
                            cmd.Parameters.AddWithValue("@sysId", userID);
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
