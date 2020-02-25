using RoomAid.ServiceLayer;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateAccountService : ICreateAccountService
    {
        private ICreateAccountDAO accountDAO;
        private ICreateUserDAO userDAO;
        private ICreateMappingDAO mappingDAO;
        public SqlCreateAccountService()
        {
            accountDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
            userDAO = new SqlCreateUserDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
            mappingDAO = new SqlCreateMappingDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        }

        public IResult Create(User newUser)
        {
            IResult addUser = null;
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            string message = "";
            bool ifSuccess = true;
            int retryTimes = 0;

            if (IfExist(newUser))
            {
                addUser = accountDAO.Create(newUser);
                if (!addUser.IsSuccess)
                {
                    addUser = Retry(accountDAO.Create, newUser);
                }

                if (addUser.IsSuccess)
                {
                    addUser = userDAO.Create(newUser);

                    if (!addUser.IsSuccess)
                    {
                        addUser = Retry(userDAO.Create, newUser);
                    }
                }
            }

            else
            {
                message = message + ConfigurationManager.AppSettings["userExist"];
                ifSuccess = false;
                return new CheckResult(message, ifSuccess);
            }


            if (addUser.IsSuccess)
            {
                message = message + ConfigurationManager.AppSettings["success"];
            }

            if (!addUser.IsSuccess)
            {
                message = message + addUser.Message;
                ifSuccess = false;
            }

            return new CheckResult(message, ifSuccess);
        }

        /// <summary>
        /// Method IfExist will use query to check if the email is already registered in database
        /// <param name="email">The email you want to check</param>
        /// <returns>true if exist, false if the email is not registered</returns>
        public bool IfExist(User newUser)
        {
            string email = newUser.UserEmail;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["connectionStringAccount"]))
            {
                SqlCommand command = new SqlCommand("SELECT UserEmail FROM dbo.Accounts WHERE UserEmail = @userEmail", connection);
                command.Parameters.AddWithValue("@userEmail", email);
                connection.Open();

                var UserEmail = command.ExecuteScalar();

                if (UserEmail != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private IResult Retry(Func<User, IResult> method, User newUser)
        {
            //Set a bool as the retry result
            IResult retryResult = new CheckResult("", false);

            //Retry until it reached the limit time of retry or it successed
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            for (int i = 0; i < retryLimit; i++)
            {
                //Call method again to check if certain method can be executed successfully
                retryResult = method(newUser);

                //If the result is true, then stop the retry, set retrySuccess as true
                if (retryResult.IsSuccess == true)
                {
                    return retryResult;
                }
            }

            return retryResult;
        }

    }
}
