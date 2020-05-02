using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    class MessageDAO : ISqlDAO
    {
        private readonly string _connectionString;
        public MessageDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User); // Default connection string
        }

        public MessageDAO(string connection)
        {
            _connectionString = connection;
        }
        
        public int RunCommand(List<SqlCommand> commands)
        {
            throw new NotImplementedException();
        }
    }
}
