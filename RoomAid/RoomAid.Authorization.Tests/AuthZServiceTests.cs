using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoomAid.Authorization.Tests
{
    [TestClass]
    public class AuthZServiceTests
    {
        private AuthZClaims claim;
        private AuthZService service;

        /*
         * These test methods are for the function:
         *  public bool Authorize(AuthZEnum.AuthZ[] permissions, string userID = defaultUserID, string householdID=defaultHouseholdID)
         *  the three parameters to be tested for are:
         *      collection of permissions/claims
         *      user unique ID
         *          Default Value: ""
         *      household unique ID:
         *          Default Value: ""
         */

        /*
         * Below are the variables used to test the VALID parameters.
         * Check the testmethod function name to see which values are used.
         */
        private const string userTestID = "Jill";
        private const string householdTestID = "012782";
        AuthZEnum.AuthZ[] testAuthorizations = { AuthZEnum.AuthZ.AcceptInvite, AuthZEnum.AuthZ.CreateExpense };

        /*
         * Below are the variables used to test INVALID parameters.
         * Check the testmethod function name to see which values are used.
         */
        private const string userTestID_negative = "I want to die";
        private const string householdTestID_negative = "But I guess that coding this makes me feel better";
        AuthZEnum.AuthZ[] testAuthorizations_negative = { AuthZEnum.AuthZ.CreateTask, AuthZEnum.AuthZ.EditTask };


        [TestInitialize]
        public void startup()
        {
            
            claim = new AuthZClaims(userTestID, householdTestID, testAuthorizations);
            service = new AuthZService(claim);
        }

        [TestMethod]
        public void AuthZService_EmptyClaims_True()
        {
            AuthZEnum.AuthZ[] permissions = {};
            Assert.IsTrue(service.Authorize(permissions));
        }

        [TestMethod]
        public void Authorize_ValidClaims_True()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            bool expected = service.Authorize(permissions);
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void Authorize_ValidClaims_ValidUserID_True()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            string testID = userTestID;
            bool expected = service.Authorize(permissions, testID);
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void Authorize_ValidClaims_ValidUserID_ValidHouseholdID_True()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            string testID = userTestID;
            string testHouseholdID = householdTestID;
            bool expected = service.Authorize(permissions, testID, testHouseholdID);
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void Authorize_InvalidClaims_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations_negative;
            bool expected = service.Authorize(permissions);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void Authorize_ValidClaims_InvalidUserID_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            string testID = userTestID_negative;
            bool expected = service.Authorize(permissions, testID);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void Authorize_InvalidClaims_ValidUserID_ValidHouseholdID_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations_negative;
            string testID = userTestID;
            string testHouseholdID = householdTestID;
            bool expected = service.Authorize(permissions, testID, testHouseholdID);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void Authorize_ValidClaims_InvalidUserID_ValidHouseholdID_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            string testID = userTestID_negative;
            string testHouseholdID = householdTestID;
            bool expected = service.Authorize(permissions, testID, testHouseholdID);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void Authorize_ValidClaims_ValidUserID_InvalidHouseholdID_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations;
            string testID = userTestID;
            string testHouseholdID = householdTestID_negative;
            bool expected = service.Authorize(permissions, testID, testHouseholdID);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void Authorize_InvalidClaims_InalidUserID_InvalidHouseholdID_False()
        {
            AuthZEnum.AuthZ[] permissions = testAuthorizations_negative;
            string testID = userTestID_negative;
            string testHouseholdID = householdTestID_negative;
            bool expected = service.Authorize(permissions, testID, testHouseholdID);
            Assert.IsFalse(expected);
        }



    }
}
