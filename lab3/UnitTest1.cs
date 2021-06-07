using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.PasswordHashingUtils;
using System;
using System.Reflection;
using System.Text;

namespace TestPasswordHashingUtils_Zavad
{
    [TestClass]
    public class Test_Init_routs
    {
        PasswordHasher myPasswordHasher = new PasswordHasher();
        string defaultSalt = "put your soul(or salt) here";
        uint defaultAdler = 65521;
        static Type hasherType = typeof(PasswordHasher);
        FieldInfo currSalt = hasherType.GetField("_salt", BindingFlags.Static | BindingFlags.NonPublic);
        FieldInfo currAdler = hasherType.GetField("_modAdler32", BindingFlags.Static | BindingFlags.NonPublic);

        [TestMethod]
        public void TestNullInSaltNullInAdler()     //route [0, 1, 5, 7]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init(null, 0);
            Assert.AreEqual(defaultSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestEmptyStringInSaltNumberInAdler()    //route [0, 1, 6, 7]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            uint modAdler32 = 5;
            PasswordHasher.Init("", modAdler32);
            Assert.AreEqual(defaultSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(modAdler32, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestStringInSaltNullInAdler()   //route [0, 2, 4, 5, 7] 
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string salt = "saaaalt";
            PasswordHasher.Init(salt, 0);
            Assert.AreEqual(salt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestStringInSaltNumberInAdler()     //route [0, 2, 4, 6, 7]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string salt = "saaaalt";
            uint modAdler32 = 10;
            PasswordHasher.Init(salt, modAdler32);
            Assert.AreEqual(salt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(modAdler32, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestExceptionStringInSaltNullInAdler()  //route [0, 2, 3, 4, 5, 7]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string saltWithException = "¢€𐍈";
            string encodedSalt = Encoding.ASCII.GetString(Encoding.Unicode.GetBytes(saltWithException));
            PasswordHasher.Init(saltWithException, 0);
            Assert.AreEqual(encodedSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestExceptionStringInSaltNumberInAdler()    //route [0, 2, 3, 4, 6, 7]    
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string saltWithException = "¢€𐍈";
            string encodedSalt = Encoding.ASCII.GetString(Encoding.Unicode.GetBytes(saltWithException));
            uint modAdler32 = 15;
            PasswordHasher.Init(saltWithException, modAdler32);
            Assert.AreEqual(encodedSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(modAdler32, currAdler.GetValue(myPasswordHasher));
        }
    }

    [TestClass]
    public class Test_GetHash_routes
    {
        PasswordHasher myPasswordHasher = new PasswordHasher();
        string defaultSalt = "put your soul(or salt) here";
        uint defaultAdler = 65521;
        static Type hasherType = typeof(PasswordHasher);
        FieldInfo currSalt = hasherType.GetField("_salt", BindingFlags.Static | BindingFlags.NonPublic);
        FieldInfo currAdler = hasherType.GetField("_modAdler32", BindingFlags.Static | BindingFlags.NonPublic);

        [TestMethod]
        public void TestNullInPassword()    //route [0, 1, 3, 6]
        {
            Assert.IsNull(PasswordHasher.GetHash(null, "saaaalt", 20));
        }

        [TestMethod]
        public void TestNotNullInPasswordNotException()     //route [0, 1, 2, 5, 6]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string password = "passwd";
            string before = PasswordHasher.GetHash(password);
            string after = PasswordHasher.GetHash(password, "another salt", 3);
            Assert.IsNotNull(after);
            Assert.AreNotEqual(before, after);
        }

        [TestMethod]
        public void TestNotNullInPasswordException()    //route [0, 1, 2, 4, 5, 6]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string password = "¢€𐍈";
            string encodedPassword = Encoding.ASCII.GetString(Encoding.Unicode.GetBytes(password));
            string salt = "another salt";
            uint modAdler32 = 15;
            string hashBefore = PasswordHasher.GetHash(password);
            string hashAfter = PasswordHasher.GetHash(password, salt, modAdler32);
            string hashFromEncoded = PasswordHasher.GetHash(encodedPassword, salt, modAdler32);
            Assert.IsNotNull(hashAfter);
            Assert.AreNotEqual(hashBefore, hashAfter);
            Assert.AreEqual(hashAfter, hashFromEncoded);
        }
    }

    [TestClass]
    public class Test_methods_special
    {
        PasswordHasher myPasswordHasher = new PasswordHasher();
        string defaultSalt = "put your soul(or salt) here";
        uint defaultAdler = 65521;
        static Type hasherType = typeof(PasswordHasher);
        FieldInfo currSalt = hasherType.GetField("_salt", BindingFlags.Static | BindingFlags.NonPublic);
        FieldInfo currAdler = hasherType.GetField("_modAdler32", BindingFlags.Static | BindingFlags.NonPublic);

        [TestMethod]
        public void TestNonAsciiStringInSaltNumberInAdler()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            uint modAdler32 = 2;
            string salt = "络";
            PasswordHasher.Init(salt, modAdler32);
            Assert.AreNotEqual(salt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(modAdler32, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestHashingSamePasswordsWithDefaultParams()
        {
            string password = "passwd";
            string hash1 = PasswordHasher.GetHash(password);
            string hash2 = PasswordHasher.GetHash(password);
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void TestHashingSamePasswordsWithSameParams()
        {
            string password = "passwd";
            string salt = "another interesting salt";
            uint modAdler32 = 3;
            string hash1 = PasswordHasher.GetHash(password, salt, modAdler32);
            string hash2 = PasswordHasher.GetHash(password, salt, modAdler32);
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void TestHashingDiffPasswordsWithSameParams()
        {
            string salt = "another cool salt";
            uint modAdler32 = 4;
            string hash1 = PasswordHasher.GetHash("passwd1", salt, modAdler32);
            string hash2 = PasswordHasher.GetHash("passwd2", salt, modAdler32);
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
