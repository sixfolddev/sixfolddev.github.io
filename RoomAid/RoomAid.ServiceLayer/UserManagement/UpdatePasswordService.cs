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
    public class UpdatePasswordService:IUpdateAccountService
    {
        private readonly IUpdateAccountDAO _dao;
        private readonly Account _acc;
        public UpdatePasswordService(IUpdateAccountDAO dao, Account acc)
        {
            _acc = acc;
            _dao = dao;
        }

        public IResult Update()
        {
            String message = "";
            bool isSuccess = false;
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["updatePassword"]);
            List <SqlCommand> cmds = new List<SqlCommand>();
            command.Parameters.AddWithValue("@HashPW", _acc.HashedPassword);
            command.Parameters.AddWithValue("@Salt", _acc.Salt);
            command.Parameters.AddWithValue("@Email", _acc.UserEmail);

            cmds.Add(command);
            int rowsChanged = _dao.Update(cmds);
            if (rowsChanged == 1)
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
