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
        private readonly IDeleteAccountDAO _deleteAccountdb;
        private readonly IDeleteAccountDAO _deleteMappingdb;
        private readonly IDeleteAccountDAO _deleteSystemdb;
        /// <summary>
        /// Craft queries based off a single user
        /// </summary>
        /// <returns></returns>
        public DeleteAccountSQLService(User targetUser, IDeleteAccountDAO deleteAccount, IDeleteAccountDAO deleteMapping, IDeleteAccountDAO deleteSystem)
        {
            this._targetUsers = new List<User>();
            this._targetUsers.Add(targetUser);
            this._deleteAccountdb = deleteAccount;
            this._deleteMappingdb = deleteMapping;
            this._deleteSystemdb = deleteSystem;
        }
        /// <summary>
        /// Craft queries based off of multiple users
        /// </summary>
        /// <returns></returns>
        public DeleteAccountSQLService(List<User> targetUsers, IDeleteAccountDAO deleteAccount, IDeleteAccountDAO deleteMapping, IDeleteAccountDAO deleteSystem)
        {
            this._targetUsers = targetUsers;
            this._deleteAccountdb = deleteAccount;
            this._deleteMappingdb = deleteMapping;
            this._deleteSystemdb = deleteSystem;
        }
        /// <summary>
        /// Delete a user, starting with their system information, 
        /// </summary>
        /// <returns></returns>
        public IResult Delete()
        {
            string message = "";
            bool isSuccess = true;
            int totalSuccess = 0;
            List<SqlCommand> commands = new List<SqlCommand>();
            IMapperDAO mapperDAO = new SqlMapperDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
            foreach (User targetUser in _targetUsers)
            {
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteSystem"]);
                cmd.Parameters.AddWithValue("@sysID", targetUser.SystemID);
                commands.Add(cmd);

                int rowsDeleted = _deleteSystemdb.Delete(commands);
                if(rowsDeleted > 0)
                {
                    cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteMapping"]);
                    cmd.Parameters.AddWithValue("@sysID", mapperDAO.GetSysID(targetUser.UserEmail));
                    commands.Add(cmd);
                    rowsDeleted = _deleteMappingdb.Delete(commands);
                    if(rowsDeleted > 0)
                    {
                        cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteAccount"]);
                        cmd.Parameters.AddWithValue("@email", targetUser.UserEmail);
                        commands.Add(cmd);
                        rowsDeleted = _deleteAccountdb.Delete(commands);
                        if(rowsDeleted > 0)
                        {
                            totalSuccess += 1;
                        }
                        else
                        {
                            message += "Failed to delete from Accounts!";
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        message += "Failed to delete from Mapping!";
                        isSuccess = false;
                    }
                }
                else
                {
                    message += "Failed to delete from System!";
                    isSuccess = false;
                }
            }
            if(totalSuccess == _targetUsers.Count)
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
