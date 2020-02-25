using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class PasswordUpdateSqlService : IUpdateAccountService
    {
        private readonly IUpdateAccountDAO _dao;
        private readonly List<Account> _accountList;

        public PasswordUpdateSqlService(IUpdateAccountDAO dao, List<Account> accounts)
        {
            _dao = dao;
            _accountList = accounts;
        }

        public PasswordUpdateSqlService(IUpdateAccountDAO dao, Account acc)
        {
            _dao = dao;
            _accountList = new List<Account>();
            _accountList.Add(acc);
        }

        public IResult Update()
        {
            String message = "";
            bool isSuccess = true;

            var commands = new List<SqlCommand>();
            var tableName = ConfigurationManager.AppSettings["tableNamePassword"];

            foreach(Account acc in _accountList)
            {
                var cmd = new SqlCommand("UPDATE @table SET Password = @pass WHERE UserEmail = @email");
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@pass", acc.Password);
                cmd.Parameters.AddWithValue("@email", acc.UserEmail);
                commands.Add(cmd);
            }


            int rowsChanged = _dao.Update(commands);


            if(rowsChanged == commands.Count)
            {
                message = ConfigurationManager.AppSettings["SuccessMessage"];

            }
            else
            {
                message = ConfigurationManager.AppSettings["FailureMessage"];
                isSuccess = false;
            }

            return new CheckResult(message, isSuccess);
        }
    }
}
