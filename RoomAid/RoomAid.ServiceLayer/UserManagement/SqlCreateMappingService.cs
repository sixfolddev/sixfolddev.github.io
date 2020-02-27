
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

        /// <summary>
        /// Service that crafts queries for insert a new row in mapping table for new account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <param name="sqlDAO"></param>
        public SqlCreateMappingService(Account newAccount, IUpdateAccountDAO sqlDao)
        {
            this._newAccount=newAccount; //Account object contains the new created account with ID, email and hashed password
            _sqlDAO = sqlDao; // DAO contains the connection string for mapping database

        }

        /// <summary>
        /// The method that crafts queries for insert new ros in the mapping table for the new account
        /// </summary>
       ///<returns>The systemID for new account</returns>

        public int CreateMapping()
        {

            int userId = -1;
            //Check if the email already exist in the table, an account should be mapped twice
            if (!IfExist())
            {
                //Craft the query for DAO
                List<SqlCommand> commands = new List<SqlCommand>();
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateMapping"]);
                cmd.Parameters.AddWithValue("@email", _newAccount.UserEmail);
                commands.Add(cmd);
                int rowsChanged = _sqlDAO.Update(commands);

                //Once the mapping successed, return the systemID for the new account
                if (rowsChanged == commands.Count)
                {
                    userId = GetSysID();
                }
            } 

            return userId;
        }

        /// <summary>
        /// Method that helps check if the email is already exist
        /// </summary>
        ///<returns>if exist true/false</returns>
        private bool IfExist()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionMapping"]))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT UserEmail FROM dbo.Mapping Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", _newAccount.UserEmail);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return true;
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        /// <summary>
        /// Method that return the systemID for the new account
        /// </summary>
        ///<returns>The systemID for new account</returns>
        private int GetSysID()
        {
            int userId = -1;
            try
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
            catch (Exception)
            {
                throw;
            }
           

            return userId;
        }
    }
}
