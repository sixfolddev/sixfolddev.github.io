using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;

namespace RoomAid.DataMasking.Tests
{
    [TestClass]
    public class HashTest
    {
        private string value1 = "testvalue1";
        private string value2 = "value2test";

        [TestMethod]
        public void GenerateHash_SameInputReturnsSameHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string hash1 = "", hash2 = "";

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hash1 = hasher.GenerateHash(value1);
            hash2 = hasher.GenerateHash(value1);
            if (hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateHash_DiffInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string hash1 = "", hash2 = "";

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hash1 = hasher.GenerateHash(value1);
            hash2 = hasher.GenerateHash(value2);
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHash_SameInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string hash1 = "", hash2 = "";
            HashObject hashobject = new HashObject();

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hashobject = hasher.GenerateSaltedHash(value1);
            hash1 = hashobject.HashedValue;
            hashobject = hasher.GenerateSaltedHash(value1);
            hash2 = hashobject.HashedValue;
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHash_SameInputReturnsDiffSalt_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string salt1 = "", salt2 = "";
            HashObject hashobject = new HashObject();

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hashobject = hasher.GenerateSaltedHash(value1);
            salt1 = hashobject.Salt;
            hashobject = hasher.GenerateSaltedHash(value1);
            salt2 = hashobject.Salt;
            if (!salt1.Equals(salt2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHash_DiffInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string hash1 = "", hash2 = "";
            HashObject hashobject = new HashObject();

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hashobject = hasher.GenerateSaltedHash(value1);
            hash1 = hashobject.HashedValue;
            hashobject = hasher.GenerateSaltedHash(value2);
            hash2 = hashobject.HashedValue;
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHash_DiffInputReturnsDiffSalt_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            string salt1 = "", salt2 = "";
            HashObject hashobject = new HashObject();

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            hashobject = hasher.GenerateSaltedHash(value1);
            salt1 = hashobject.Salt;
            hashobject = hasher.GenerateSaltedHash(value2);
            salt2 = hashobject.Salt;
            if (!salt1.Equals(salt2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }
    }
}
