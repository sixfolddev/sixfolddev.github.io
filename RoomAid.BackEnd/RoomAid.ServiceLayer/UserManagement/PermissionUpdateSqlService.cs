using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class PermissionUpdateSqlService:IUpdateAccountService
    {
        private readonly ISqlDAO _dao;
        private readonly IMapperDAO _mapperDAO;
        public List<Permission> _permissions { get; set; }
       


        /// <summary>
        /// class to update permissions for a list of accounts or only one account
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="permissions"></param>
        /// 
        public PermissionUpdateSqlService(ISqlDAO dao, IMapperDAO mapperDao)
        {
            _dao = dao;
            _mapperDAO = mapperDao;
        }
        public PermissionUpdateSqlService(ISqlDAO dao, List<Permission> permissions)
        {
            _dao = dao;
            _permissions = permissions;
        }

        public PermissionUpdateSqlService(ISqlDAO dao, Permission permission)
        {
            _dao = dao;
            _permissions = new List<Permission> { permission };
        }

        public int SysIdFinder(User user)
        {
            IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            return mapperDAO.GetSysID(user.UserEmail);
        }


        /// <summary>
        /// iterates through all different permissoins of each permission object and creates a command for each of them
        /// it does make  alot of commands but it okay
        /// </summary>
        /// <returns></returns>
        public IResult Update()
        {
            String message = "";
            bool isSuccess = false;

            var commands = new List<SqlCommand>();

            foreach(Permission per in _permissions)
            {
                foreach(Tuple<String, bool> tup in per.Permissions)
                {
                    //var action = "";
                    SqlCommand cmd;
                    if (tup.Item2 == true)
                    {
                        cmd = new SqlCommand($"INSERT INTO {Environment.GetEnvironmentVariable("tableNamePermissions", EnvironmentVariableTarget.User)} (SysID, Permission) VALUES (@userID, @singlePermission)");
                    }
                    else
                    {
                       cmd = new SqlCommand($"DELETE FROM {Environment.GetEnvironmentVariable("tableNamePermissions", EnvironmentVariableTarget.User)} WHERE SysID = @userID AND Permission = @singlePermission") ;
                    }

                    //cmd.Parameters.AddWithValue("@action", action);
                    //cmd.Parameters.AddWithValue("@tableName", Environment.GetEnvironmentVariable("tableNamePermissions", EnvironmentVariableTarget.User));
                    cmd.Parameters.AddWithValue("@singlePermission", tup.Item1);
                    cmd.Parameters.AddWithValue("@userID", per.UserID);
                    commands.Add(cmd);  
                }
            }

           var rowsChanged = _dao.RunCommand(commands);

           if(rowsChanged!=commands.Count)
            {
                isSuccess = false;
                message += ConfigurationManager.AppSettings["failureMessage"];
            }
           else
            {
                isSuccess = true;
                message += ConfigurationManager.AppSettings["successMessage"];
            }



            return new CheckResult(message, isSuccess);
        }





    }
}
