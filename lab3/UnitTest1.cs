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
            PasswordHasher.Init("", 5);
            Assert.AreEqual(defaultSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(5, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
        }

        [TestMethod]
        public void TestStringInSaltNullInAdler()   //route [0, 2, 4, 5, 7] 
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init("saaaalt", 0);
            Assert.AreEqual("saaaalt", currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestStringInSaltNumberInAdler()     //route [0, 2, 4, 6, 7]
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init("saaaalt", 10);
            Assert.AreEqual("saaaalt", currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(10, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
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
            PasswordHasher.Init(saltWithException, 15);
            Assert.AreEqual(encodedSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(15, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
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
            string before = PasswordHasher.GetHash("passwd");
            string after = PasswordHasher.GetHash("passwd", "another salt", 3);
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
            string before = PasswordHasher.GetHash(password);
            string hash = PasswordHasher.GetHash(password, "another salt", 3);
            string hashFromEncoded = PasswordHasher.GetHash(encodedPassword, "another salt", 3);
            Assert.IsNotNull(hash);
            Assert.AreNotEqual(before, hash);
            Assert.AreEqual(hash, hashFromEncoded);
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
            PasswordHasher.Init("络", 15); ;
            Assert.AreNotEqual("络", currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(15, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
        }

        [TestMethod]
        public void TestHashingSamePasswordsWithDefaultParams()
        {
            string hash1 = PasswordHasher.GetHash("passwd");
            string hash2 = PasswordHasher.GetHash("passwd");
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void TestHashingSamePasswordsWithSameParams()
        {
            string hash1 = PasswordHasher.GetHash("passwd", "another salt", 3);
            string hash2 = PasswordHasher.GetHash("passwd", "another salt", 3);
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void TestHashingDiffPasswordsWithSameParams()
        {
            string hash1 = PasswordHasher.GetHash("passwd1", "another salt", 3);
            string hash2 = PasswordHasher.GetHash("passwd2", "another salt", 3);
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
