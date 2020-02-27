
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
        private readonly IUpdateAccountDAO _sqlDAO;

        /// <summary>
        /// Service that crafts queries for inserting a new row in the account table for new account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <param name="sqlDAO"></param>
        public SqlCreateAccountService(Account newAccount, IUpdateAccountDAO sqlDAO)
        {
            this._newAccounts = new List<Account>();
            this._newAccounts.Add(newAccount);
            _sqlDAO = sqlDAO;  
        }

        /// <summary>
        /// Service that crafts queries for inserting multiple rows in account table
        /// </summary>
        /// <param name="newAccounts"></param>
        /// <param name="sqlDAO"></param>
        public SqlCreateAccountService(List<Account> newAccounts, IUpdateAccountDAO sqlDAO)
        {
            this._newAccounts = newAccounts;
            _sqlDAO = sqlDAO;
        }


        /// <summary>
        /// Service that crafts queries for insert new ros in the account table for the new account
        /// </summary>
        ///<returns>IResult true or false with error message</returns>
        public IResult Create()
        {
            string message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();

            foreach (Account newAccount in _newAccounts)
            {
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateAccount"]);
                cmd.Parameters.AddWithValue("@email", newAccount.UserEmail);
                cmd.Parameters.AddWithValue("@hashedPw", newAccount.HashedPassword);
                cmd.Parameters.AddWithValue("@salt", newAccount.Salt);
                commands.Add(cmd);
            }
             
            int rowsChanged = _sqlDAO.Update(commands);

            if (rowsChanged == commands.Count)
            {
                message += ConfigurationManager.AppSettings["successMessage"];
            }
            else
            {
                message += ConfigurationManager.AppSettings["failureMessage"];
                isSuccess = false;
            }
            return new CheckResult(message, isSuccess);
        }
    }
}
