using System;
using Xunit;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;
using IIG.FileWorker;
using IIG.BinaryFlag;

namespace Zavodovska_lab4
{
    public class CredentialsTests
    {
        private const string Server = @"DESKTOP-5R8QLR1\MSSQLSERVER2";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = true;
        private const string Login = @"coswe";
        private const string Password = @"L}EjpfCgru9X@GLj";
        private const int ConnectionTimeout = 75;
        readonly AuthDatabaseUtils db = new(Server, Database, IsTrusted, Login, Password, ConnectionTimeout);

        [Fact]
        public void TestDeleteCredentials()
        {
            string login = "qwerty";
            string hashedPassword = PasswordHasher.GetHash("a1'");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
            Assert.False(db.CheckCredentials(login, hashedPassword));
        }

        [Fact]
        public void TestDeleteUnexistCredentials()
        {
            string login = "qwerty";
            string hashedPassword = PasswordHasher.GetHash("a1'");
            Assert.False(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
            Assert.False(db.CheckCredentials(login, hashedPassword));
        }

        [Fact]
        public void TestClassicCredentials()
        {
            string login = "aaa";
            string hashedPassword = PasswordHasher.GetHash("bbb");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestCyrilicCredentials()
        {
            string login = "ыэъ";
            string hashedPassword = PasswordHasher.GetHash("эээ");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestUkrCredentials()
        {
            string login = "привітики.";
            string hashedPassword = PasswordHasher.GetHash("ЇЄ");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestCredentialsWithNumbers()
        {
            string login = "aaa123";
            string hashedPassword = PasswordHasher.GetHash("4567890");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestCredentialsWithSpecialChars()
        {
            string login = "$'€_/@#";
            string hashedPassword = PasswordHasher.GetHash("{<->]");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestCredentialsWithEmoji()
        {
            string login = "🌼🌷🌻";
            string hashedPassword = PasswordHasher.GetHash("🐬🐳🦩");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestCredentialsWithNonAsciiChars()
        {
            string login = "भारत";
            string hashedPassword = PasswordHasher.GetHash("网络");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestEmptyLogin()
        {
            string login = "";
            string hashedPassword = PasswordHasher.GetHash("passwd");
            db.AddCredentials(login, hashedPassword);
            Assert.False(db.CheckCredentials(login, hashedPassword));
        }

        [Fact]
        public void TestEmptyPassword()
        {
            string login = "qwerty";
            string hashedPassword = PasswordHasher.GetHash("");
            db.AddCredentials(login, hashedPassword);
            Assert.True(db.CheckCredentials(login, hashedPassword));
            db.DeleteCredentials(login, hashedPassword);
        }

        [Fact]
        public void TestUpdateCredentials()
        {
            string login = "jal[a";
            string loginModified = "dmW@s;";
            string hashedPassword = PasswordHasher.GetHash("dsk*jl");
            string hashedPasswordModified = PasswordHasher.GetHash("jkdsjk48");
            db.AddCredentials(login, hashedPassword);
            db.UpdateCredentials(login, hashedPassword, loginModified, hashedPasswordModified);
            Assert.False(db.CheckCredentials(login, hashedPassword));
            Assert.True(db.CheckCredentials(loginModified, hashedPasswordModified));
            db.DeleteCredentials(loginModified, hashedPasswordModified);
        }

        [Fact]
        public void TestUpdateOnlyLogin()
        {
            string login = "qwerty";
            string loginModified = "zxcvbn";
            string hashedPassword = PasswordHasher.GetHash("asdfgh");
            db.AddCredentials(login, hashedPassword);
            db.UpdateCredentials(login, hashedPassword, loginModified, hashedPassword);
            Assert.False(db.CheckCredentials(login, hashedPassword));
            Assert.True(db.CheckCredentials(loginModified, hashedPassword));
            db.DeleteCredentials(loginModified, hashedPassword);
        }

        [Fact]
        public void TestUpdateOnlyPassword()
        {
            string login = "qwerty";
            string hashedPassword = PasswordHasher.GetHash("a1'");
            string hashedPasswordModified = PasswordHasher.GetHash("b7}");
            db.AddCredentials(login, hashedPassword);
            db.UpdateCredentials(login, hashedPassword, login, hashedPasswordModified);
            Assert.False(db.CheckCredentials(login, hashedPassword));
            Assert.True(db.CheckCredentials(login, hashedPasswordModified));
            db.DeleteCredentials(login, hashedPasswordModified);
        }

        [Fact]
        public void TestUpdateUnexistCredentials()
        {
            string login = "qwer90";
            string loginModified = "zxcv45";
            string hashedPassword = PasswordHasher.GetHash("l)2");
            string hashedPasswordModified = PasswordHasher.GetHash("o&9");
            db.UpdateCredentials(login, hashedPassword, loginModified, hashedPasswordModified);
            Assert.False(db.CheckCredentials(login, hashedPassword));
            Assert.False(db.CheckCredentials(login, hashedPasswordModified));
        }
    }
    public class BinaryFlagTests
    {
        string path = "C:/Users/User/file.txt";
        [Fact]
        public void TestConstructorDefault()
        {
            MultipleBinaryFlag testBinaryFlag = new(2);
            bool? condition = testBinaryFlag.GetFlag();
            Assert.True(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("True", fromFile);
        }

        [Fact]
        public void TestConstructorWithFalse()
        {
            MultipleBinaryFlag testBinaryFlag = new(3, false);
            bool? condition = testBinaryFlag.GetFlag();
            Assert.False(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("False", fromFile);
        }

        [Fact]
        public void TestConstructorOnlyOneFlag()
        {
            uint length = 1;
            string err = "ArgumentOutOfRangeException";
            try
            {
                MultipleBinaryFlag testBinaryFlag = new(Convert.ToUInt64(length));
            }
            catch (ArgumentOutOfRangeException)
            {
                BaseFileWorker.Write(err, path);
                Assert.True(true);
            }
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal(err, fromFile);
        }

        [Fact]
        public void TestDisposeFlag()
        {
            MultipleBinaryFlag testBinaryFlag = new(3);
            testBinaryFlag.Dispose();
            bool? condition = testBinaryFlag.GetFlag();
            Assert.Null(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("", fromFile);
        }

        [Fact]
        public void TestDisposeFlagTwice()
        {
            MultipleBinaryFlag testBinaryFlag = new(3);
            testBinaryFlag.Dispose();
            testBinaryFlag.Dispose();
            bool? condition = testBinaryFlag.GetFlag();
            Assert.Null(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("", fromFile);
        }

        [Fact]
        public void TestSetFlag()
        {
            MultipleBinaryFlag testBinaryFlag = new(10, false);
            int pos = 6;
            testBinaryFlag.SetFlag(Convert.ToUInt32(pos));
            char condition = testBinaryFlag.ToString()[pos];
            Assert.Equal("T", condition.ToString());
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("T", fromFile);
        }

        [Fact]
        public void TestResetFlag()
        {
            int pos = 8;
            MultipleBinaryFlag testBinaryFlag = new(13);
            testBinaryFlag.ResetFlag(Convert.ToUInt32(pos));
            char condition = testBinaryFlag.ToString()[pos];
            Assert.Equal("F", condition.ToString());
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("F", fromFile);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void TestSetDisposedFlag()
        {
            MultipleBinaryFlag testBinaryFlag = new(2, false);
            testBinaryFlag.Dispose();
            testBinaryFlag.SetFlag(0);
            testBinaryFlag.SetFlag(1);
            bool? condition = testBinaryFlag.GetFlag();
            Assert.Null(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("", fromFile);
        }

        [Fact]
        public void TestResetDisposedFlag()
        {
            MultipleBinaryFlag testBinaryFlag = new(10);
            testBinaryFlag.Dispose();
            testBinaryFlag.ResetFlag(6);
            bool? condition = testBinaryFlag.GetFlag();
            Assert.Null(condition);
            BaseFileWorker.Write(condition.ToString(), path);
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal("", fromFile);
        }

        [Fact]
        public void TestSetUnexistFlag()
        {
            uint length = 17;
            string err = "ArgumentOutOfRangeException";
            MultipleBinaryFlag testBinaryFlag = new(Convert.ToUInt64(length));
            try
            {
                testBinaryFlag.SetFlag(length);
            }
            catch (ArgumentOutOfRangeException)
            {
                BaseFileWorker.Write(err, path);
                Assert.True(true);
            }
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal(err, fromFile);
        }

        [Fact]
        public void TestResetUnexistFlag()
        {
            uint length = 19;
            string err = "ArgumentOutOfRangeException";
            MultipleBinaryFlag testBinaryFlag = new(Convert.ToUInt64(length));
            try
            {
                testBinaryFlag.ResetFlag(length + 3);
            }
            catch(ArgumentOutOfRangeException) {
                BaseFileWorker.Write(err, path);
                Assert.True(true);
            }
            string fromFile = BaseFileWorker.ReadAll(path);
            Assert.Equal(err, fromFile);
        }
    }
}
