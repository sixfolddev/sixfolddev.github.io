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
        private string _value1 = "testvalue1";
        private string _value2 = "value2test";
        private string _salt1 = "testsalt1";
        private string _salt2 = "test2salt";

        [TestMethod]
        public void GenerateHash_SameInputReturnsSameHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateHash(_value1);
            string hash2 = hasher.GenerateHash(_value1);
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

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateHash(_value1);
            string hash2 = hasher.GenerateHash(_value2);
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

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            HashObject hashobject = hasher.GenerateSaltedHash(_value1);
            string hash1 = hashobject.HashedValue;
            hashobject = hasher.GenerateSaltedHash(_value1);
            string hash2 = hashobject.HashedValue;
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

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            HashObject hashobject = hasher.GenerateSaltedHash(_value1);
            string salt1 = hashobject.Salt;
            hashobject = hasher.GenerateSaltedHash(_value1);
            string salt2 = hashobject.Salt;
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

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            HashObject hashobject = hasher.GenerateSaltedHash(_value1);
            string hash1 = hashobject.HashedValue;
            hashobject = hasher.GenerateSaltedHash(_value2);
            string hash2 = hashobject.HashedValue;
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

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            HashObject hashobject = hasher.GenerateSaltedHash(_value1);
            string salt1 = hashobject.Salt;
            hashobject = hasher.GenerateSaltedHash(_value2);
            string salt2 = hashobject.Salt;
            if (!salt1.Equals(salt2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHashGivenSalt_SameSaltSameValueInputReturnsSameHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            
            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateSaltedHash(_value1, _salt1);
            string hash2 = hasher.GenerateSaltedHash(_value1, _salt1);
            if (hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHashGivenSalt_SameSaltDiffValueInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateSaltedHash(_value1, _salt1);
            string hash2 = hasher.GenerateSaltedHash(_value2, _salt1);
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHashGivenSalt_DiffSaltSameValueInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateSaltedHash(_value1, _salt1);
            string hash2 = hasher.GenerateSaltedHash(_value1, _salt2);
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void GenerateSaltedHashGivenSalt_DiffSaltDiffValueInputReturnsDiffHash_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;

            //Act
            Hasher hasher = new Hasher(new SHA256Cng());
            string hash1 = hasher.GenerateSaltedHash(_value1, _salt1);
            string hash2 = hasher.GenerateSaltedHash(_value2, _salt2);
            if (!hash1.Equals(hash2))
            {
                actual = true;
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }
    }
}
