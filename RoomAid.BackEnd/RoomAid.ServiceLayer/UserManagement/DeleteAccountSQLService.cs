﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using RoomAid.DataAccessLayer;

namespace RoomAid.ServiceLayer
{
    public class DeleteAccountSQLService : IDeleteAccountService
    {
        public List<User> _targetUsers { get; set; }
        private readonly ISqlDAO _deleteAccountdb;
        private readonly ISqlDAO _deleteMappingdb;
        private readonly ISqlDAO _deleteSystemdb;
        /// <summary>
        /// Craft queries based off a single user
        /// </summary>
        /// <returns></returns>
        /// 
        public DeleteAccountSQLService(ISqlDAO deleteSystem, ISqlDAO deleteMapping, ISqlDAO deleteAccount)
        {
            this._deleteAccountdb = deleteAccount;
            this._deleteMappingdb = deleteMapping;
            this._deleteSystemdb = deleteSystem;
        }
        public DeleteAccountSQLService(User targetUser, ISqlDAO deleteSystem, ISqlDAO deleteMapping, ISqlDAO deleteAccount)
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
        public DeleteAccountSQLService(List<User> targetUsers, ISqlDAO deleteSystem, ISqlDAO deleteMapping, ISqlDAO deleteAccount)
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
            IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));

            foreach (User targetUser in _targetUsers)
            {
                List<SqlCommand> deleteUserCommands = new List<SqlCommand>();
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteSystem"]);
                cmd.Parameters.AddWithValue("@sysID", targetUser.SystemID);
                deleteUserCommands.Add(cmd);

                int rowsDeleted = _deleteSystemdb.RunCommand(deleteUserCommands);
                if(rowsDeleted > 0)
                {
                    List<SqlCommand> deleteMapperCommands = new List<SqlCommand>();
                    deleteMapperCommands = new List<SqlCommand>();
                    cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteMapping"]);
                    cmd.Parameters.AddWithValue("@sysID", mapperDAO.GetSysID(targetUser.UserEmail));
                    deleteMapperCommands.Add(cmd);
                    rowsDeleted = _deleteMappingdb.RunCommand(deleteMapperCommands);
                    if(rowsDeleted > 0)
                    {
                        List<SqlCommand> deleteAccountCommands = new List<SqlCommand>();
                        deleteAccountCommands = new List<SqlCommand>();
                        cmd = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteAccount"]);
                        cmd.Parameters.AddWithValue("@email", targetUser.UserEmail);
                        deleteAccountCommands.Add(cmd);
                        rowsDeleted = _deleteAccountdb.RunCommand(deleteAccountCommands);
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
