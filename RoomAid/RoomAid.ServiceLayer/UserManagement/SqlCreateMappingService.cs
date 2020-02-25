
using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateMappingService : ICreateMappingService
    {
        private readonly Account _newAccount;
        private readonly IUpdateAccountDAO _sqlDAO;
        public SqlCreateMappingService(Account newAccount, IUpdateAccountDAO sqlDao)
        {
            this._newAccount=newAccount;
            _sqlDAO = sqlDao;

        }
        public int CreateMapping()
        {
            int userId = -1;
            List<SqlCommand> commands = new List<SqlCommand>();
            var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateMapping"]);
            cmd.Parameters.AddWithValue("@email", _newAccount.UserEmail);
            commands.Add(cmd);        
            int rowsChanged = _sqlDAO.Update(commands);


            if (rowsChanged == commands.Count)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionMapping"]))
                {
                    string query = ConfigurationManager.AppSettings["querySelectMapping"];
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", _newAccount.UserEmail);
                        connection.Open();
                        command.ExecuteNonQuery();
                         userId = (Int32)command.ExecuteScalar();
                    }
                }
            }
            return userId;
        }
    }
}
