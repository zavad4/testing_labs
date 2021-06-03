using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.PasswordHashingUtils;
using System;
using System.Reflection;

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
        public void TestNullInSaltNullInAdler()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init(null, 0);
            Assert.AreEqual(defaultSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestEmptyStringInSaltNumberInAdler()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init("", 5);
            Assert.AreEqual(defaultSalt, currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(5, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
        }

        [TestMethod]
        public void TestStringInSaltNullInAdler()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init("saaaalt", 0);
            Assert.AreEqual("saaaalt", currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        }

        [TestMethod]
        public void TestStringInSaltNumberInAdler()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            PasswordHasher.Init("saaaalt", 10);
            Assert.AreEqual("saaaalt", currSalt.GetValue(myPasswordHasher).ToString());
            Assert.AreEqual(10, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
        }

        //[TestMethod]
        //public void TestExceptionStringInSaltNullInAdler()
        //{
            //currSalt.SetValue(myPasswordHasher, defaultSalt);
            //currAdler.SetValue(myPasswordHasher, defaultAdler);
            //string largeSalt = new String('s', int.MaxValue);
            //Assert.ThrowsException<OverflowException>(() => {
                //PasswordHasher.Init(largeSalt, 0);
            //});
            //Assert.AreNotEqual(largeSalt, currSalt.GetValue(myPasswordHasher).ToString());
            //Assert.AreEqual(defaultAdler, currAdler.GetValue(myPasswordHasher));
        //}

        //[TestMethod]
        //public void TestExceptionStringInSaltNumberInAdler()
        //{
            //currSalt.SetValue(myPasswordHasher, defaultSalt);
            //currAdler.SetValue(myPasswordHasher, defaultAdler);
            //string largeSalt = new String('s', int.MaxValue);
            //Assert.ThrowsException<OverflowException>(() => {
                //PasswordHasher.Init(largeSalt, 15);
            //});
            //Assert.AreNotEqual(largeSalt, currSalt.GetValue(myPasswordHasher).ToString());
            //Assert.AreEqual(15, Convert.ToInt32(currAdler.GetValue(myPasswordHasher)));
        //}
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
        public void TestNullInPassword()
        {
            Assert.IsNull(PasswordHasher.GetHash(null, "saaaalt", 20));
        }

        [TestMethod]
        public void TestNotNullInPasswordNotException()
        {
            currSalt.SetValue(myPasswordHasher, defaultSalt);
            currAdler.SetValue(myPasswordHasher, defaultAdler);
            string before = PasswordHasher.GetHash("passwd");
            string after = PasswordHasher.GetHash("passwd", "another salt", 3);
            Assert.IsNotNull(after);
            Assert.AreNotEqual(before, after);
        }

        //[TestMethod]
        //public void TestNotNullInPasswordException()
        //{
            //currSalt.SetValue(myPasswordHasher, defaultSalt);
            //currAdler.SetValue(myPasswordHasher, defaultAdler);
            //string largeSalt = new String('s', int.MaxValue);
            //string before = PasswordHasher.GetHash(largeSalt);
            //string after = PasswordHasher.GetHash(largeSalt, "another salt", 3);
            //Assert.IsNotNull(after);
            //Assert.AreNotEqual(before, after);
        //}
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
