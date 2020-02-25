
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
        public SqlCreateAccountService(Account newAccount, IUpdateAccountDAO sqlDAO)
        {
            this._newAccounts = new List<Account>();
            this._newAccounts.Add(newAccount);
            _sqlDAO = sqlDAO;  
        }

        public SqlCreateAccountService(List<User> newAccounts, IUpdateAccountDAO sqlDAO)
        {
            this._newAccounts = new List<Account>();
            _sqlDAO = sqlDAO;
        }

        public IResult Create()
        {
            string message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();

            foreach (Account newAccount in _newAccounts)
            {
                var cmd = new SqlCommand("INSERT INTO dbo.Accounts (UserEmail, HashedPassword, Salt) VALUES (@email, @hashedPw, @salt)");
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
