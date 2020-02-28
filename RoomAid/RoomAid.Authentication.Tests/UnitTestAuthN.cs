using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;

namespace UnitTestAuthentication
{
    [TestClass]
    public class UnitTestAuthN
    {
        private readonly AuthenticationService authService = new AuthenticationService("tester01");

        /*[TestMethod]
        public void hashing()
        {
            //check if hashed pw is the same is the one stored to user ID in pw file
            //If this passes, user is authenticated
            Assert.IsTrue(authentication.Authenticate());
        }*/

        /// <summary>
        /// Test to see if authentication fails when the hashed input does not match the stored hash.
        /// Since there is no actual connection to the database and our DataStoreHash currently returns a
        /// dummy string, this should fail.
        /// </summary>
        [TestMethod]
        public void Authenticate_NotPass()
        {
            //Arrange
            var actual = true;

            //Act
            if (!authService.Authenticate("password1"))
            {
                actual = false;
            }

            //Assert
            Assert.IsFalse(actual);
        }
    }
}

