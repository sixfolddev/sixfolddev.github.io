using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;
namespace RoomAid.Registration.Test
{
    [TestClass]
    public class RegistrationTest
    {
        //Part A ValidationService test
        //Get the instance of validation service
        private ValidationService testVs = new ValidationService();
        //The success condition for Name Check
        [TestMethod]
        public void NameCheckPass()
        {
            //Arrange
            bool expected = true;
            //Act
            IResult checkResult = testVs.NameValidation("AlbertDu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The faile condtion for Name check
        //With an input that is out of range, the method should return false
        [TestMethod]
        public void NameCheckNotPassA()
        {
            //Arrange
            bool expected = false;
            int checkRange = Int32.Parse(ConfigurationManager.AppSettings["nameLength"]);
            string input = "a";
            for (int i = 0; i < checkRange + 1; i++)
            {
                input = input + "a";
            }
            //Act
            IResult checkResult = testVs.NameValidation(input);
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The faile condtion for Name check
        //With an input that is white space, the method should return false
        [TestMethod]
        public void NameCheckNotPassB()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.NameValidation(" ");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The faile condtion for Name check
        //With an input that is empty, the method should return false
        [TestMethod]
        public void NameCheckNotPassC()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.NameValidation("");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The faile condtion for Name check
        //With an input that is null, the method should return false
        [TestMethod]
        public void NameCheckNotPassD()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.NameValidation(null);
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The success condition for Name Check
        [TestMethod]
        public void PasswordCheckPass()
        {
            //Arrange
            bool expected = true;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy19970205014436615");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion forpassword check
        //With an input that contains '<' and '>', the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassA()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy1997020>501<36615");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that is less than 12, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassB()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy19970205");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains repetitive numbers, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassC()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy1999967205");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains repetitive letters, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassD()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy1bbbbb67205");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains sequential numbers, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassE()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy123456albertdu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains sequential letters, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassF()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djyabcdelbertdu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains sequential numners in reversed order, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassG()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy54321albertdu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that contains sequential letters in reversed order, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassH()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djyzyxwu33267albertdu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion forpassword check
        //With an input that contains sequential and repetitive contents, the method should return false
        [TestMethod]
        public void PasswordCheckNOTPassI()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordValidation("Djy45678bbbbbbbalbertdu");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for password check
        //With an input that caontains or the same as user's email/username, the method should return false
        [TestMethod]
        public void PasswordUserNameCheckNOTPass()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.PasswordUserNameValidation("bbcdalbertdu233@gmail.com", "albertdu233@gmail.com");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The success condtion for Email check
        [TestMethod]
        public void EmailCheckPass()
        {
            //Arrange
            bool expected = true;
            //Act
            IResult checkResult = testVs.EmailValidation("albertdu233@gmail.com");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }
        //The fail condtion for Email check
        //with bad format of email input, should return false
        [TestMethod]
        public void EmailCheckNotPass()
        {
            //Arrange
            bool expected = false;
            //Act
            IResult checkResult = testVs.EmailValidation("dhajdhadh@kjshdjakhg@mail.com");
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        //The success function for age validation
        //if age is > 18, should return true
        [TestMethod]
        public void AgeCheckPass()
        {
            //Arrange
            bool expected = true;

            //Act
            DateTime dt = new DateTime(1997, 2, 5);
            IResult checkResult = testVs.AgeValidation(dt, Int32.Parse(ConfigurationManager.AppSettings["ageRequired"]));
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //The success function for age validation
        //if age is < 18, should return false
        [TestMethod]
        public void AgeCheckNotPass()
        {
            //Arrange
            bool expected = false;

            //Act
            DateTime dt = DateTime.Today;
            IResult checkResult = testVs.AgeValidation(dt, Int32.Parse(ConfigurationManager.AppSettings["ageRequired"]));
            bool actual = checkResult.IsSuccess;
            Console.WriteLine(checkResult.Message);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //partB test to see if the whole RegistrationService can work

        //The success function for age validation
        //if age is < 18, should return false
        [TestMethod]
        public void RegistrationPass()
        {
            //Arrange
            bool expected = true;

            //Act
            RegistrationRequestDTO testDTO = new RegistrationRequestDTO("Tester@email.com","testerFname", "testerLname", DateTime.Today, "Male", "testpassword", "testpassword");
            DeleteUser(testDTO.Email);
            DeleteMapping(testDTO.Email);
            DeleteAccount(testDTO.Email);
            RegistrationService rs = new RegistrationService(testDTO);
            IResult checkResult = rs.RegisterUser();
            bool actual = checkResult.IsSuccess;
            DeleteUser(testDTO.Email);
            DeleteMapping(testDTO.Email);
            DeleteAccount(testDTO.Email);
            Console.WriteLine(checkResult.Message);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Cleanning tools
        public void DeleteMapping(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionMapping"]))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Mapping Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }
        public void DeleteAccount(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionAccount"]))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Accounts Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }
        //Tool method to clean testing account created by the test method
        public void DeleteUser(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Users Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }
    }
}