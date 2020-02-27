using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using RoomAid.DataAccessLayer;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class DeleteAccountSQLService : IDeleteAccountService
    {
        private readonly List<User> _targetUsers;
        private readonly IDeleteAccountDAO _delete;
        /// <summary>
        /// Craft queries based off a single user
        /// </summary>
        /// <returns></returns>
        public DeleteAccountSQLService(User targetUser, IDeleteAccountDAO delete)
        {
            this._targetUsers = new List<User>();
            this._targetUsers.Add(targetUser);
            this._delete = delete;
        }
        /// <summary>
        /// Craft queries based off of multiple users
        /// </summary>
        /// <returns></returns>
        public DeleteAccountSQLService(List<User> targetUsers, IDeleteAccountDAO delete)
        {
            this._targetUsers = targetUsers;
            this._delete = delete;
        }
        public IResult Delete()
        {
            string message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();
            foreach(User targetUser in _targetUsers)
            {
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["deleteCmd"]);
                cmd.Parameters.AddWithValue("@email", targetUser.UserEmail);
                commands.Add(cmd);
            }
            int rowsDeleted = _delete.Delete(commands);
            if(rowsDeleted == commands.Count)
            {
                message += ConfigurationManager.AppSettings["DeleteAccountSuccess"];
            }
            else
            {
                message += ConfigurationManager.AppSettings["DeleteAccountFailure"];
                isSuccess = false;
            }
            return new CheckResult(message, isSuccess);
        }
    }
}
