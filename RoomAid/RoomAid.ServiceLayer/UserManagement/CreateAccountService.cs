using RoomAid.ServiceLayer;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    public class CreateAccountService
    {
        private ICreationAccountDAO sqlDAO;
        public CreateAccountService(ICreationAccountDAO sqlDAO)
        {
            this.sqlDAO = sqlDAO;
        }

        public IResult CreateAccount(User newUser)
        {
            IResult addUser = null;
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            string message = "";
            bool ifSuccess = true;
            int retryTimes = 0;

            if (!sqlDAO.IfUserExist(newUser.UserEmail))
            {
                while (retryTimes < retryLimit)
                {
                    addUser = sqlDAO.Create(newUser);
                    if (!addUser.IsSuccess)
                        retryTimes++;

                    else
                        retryTimes = retryLimit;
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

    }
}
