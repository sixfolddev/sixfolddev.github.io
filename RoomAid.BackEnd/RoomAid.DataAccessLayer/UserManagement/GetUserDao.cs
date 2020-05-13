using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.UserManagement
{
    public class GetUserDao
    {
        private readonly string _connection;

        public GetUserDao(string connection)
        {
            _connection = connection;
        }

        public Dictionary<string, string> RetrieveUser(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM dbo.Users WHERE UserEmail = @email", connection);
                command.Parameters.AddWithValue("@email", email);
                var reader = command.ExecuteReader();
                var dictionary = new Dictionary<string, string>();

                while (reader.Read())
                {
                    dictionary["FirstName"] = reader["FirstName"].ToString();
                    dictionary["LastName"] = reader["LastName"].ToString();
                    dictionary["SystemID"] = reader["SysID"].ToString();
                    dictionary["DateOfBirth"] = reader["DateOfBirth"].ToString();
                    dictionary["Gender"] = reader["Gender"].ToString();
                    dictionary["AccountStatus"] = reader["AccountStatus"].ToString();
                }
                connection.Close();
                return dictionary;
            }


        }
    }
}

