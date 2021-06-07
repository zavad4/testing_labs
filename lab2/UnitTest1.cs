using System;
using Xunit;
using IIG.BinaryFlag;

namespace BinaryFlag.UnitTests
{
    public class BinaryFlagConstructor_test_limitValues
    {
        ulong min = 2;
        ulong max = 17179868704;

        [Fact]
        public void Test_Constructor_lessMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(min - 1);
            });
        }

        [Fact]
        public void Test_Constructor_Min()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(min);
            Assert.True(testBinaryFlag is MultipleBinaryFlag);
        }

        [Fact]
        public void Test_Constructor_moreMin()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(min + 1);
            Assert.True(testBinaryFlag is MultipleBinaryFlag);
        }

        [Fact]
        public void Test_Constructor_lessMax()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(max - 1);
            Assert.True(testBinaryFlag is MultipleBinaryFlag);
        }

        [Fact]
        public void Test_Constructor_Max()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(max);
            Assert.True(testBinaryFlag is MultipleBinaryFlag);
        }

        [Fact]
        public void Test_Constructor_moreMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(max + 1);
            });
        }
    }
    public class BinaryFlagMethods_test
    {
        [Fact]
        public void Test_after_inizialization_true()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(5);
            Assert.True(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_after_inizialization_false()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(5, false);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_setting()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2, false);
            testBinaryFlag.SetFlag(0);
            testBinaryFlag.SetFlag(1);
            Assert.True(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_resetting()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            testBinaryFlag.ResetFlag(0);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_setting_wrong_position()
        {
            uint length = 5;
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(length, false);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                testBinaryFlag.SetFlag(length);
            });
        }

        [Fact]
        public void Test_resetting_wrong_position()
        {
            uint length = 5;
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(length);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                testBinaryFlag.ResetFlag(length);
            });
        }

        [Fact]
        public void Test_setting_one_position()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2, false);
            testBinaryFlag.SetFlag(0);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_after_double_setting()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            uint pos = 0;
            testBinaryFlag.SetFlag(pos);
            testBinaryFlag.SetFlag(pos);
            testBinaryFlag.ResetFlag(pos);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_after_double_resetting()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            uint pos = 1;
            testBinaryFlag.ResetFlag(pos);
            testBinaryFlag.ResetFlag(pos);
            testBinaryFlag.SetFlag(pos);
            Assert.True(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_after_switching_true()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            uint pos = 0;
            testBinaryFlag.ResetFlag(pos);
            testBinaryFlag.SetFlag(pos);
            Assert.True(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_after_switching_false()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            uint pos = 1;
            testBinaryFlag.SetFlag(pos);
            testBinaryFlag.ResetFlag(pos);
            Assert.False(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_disposing()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            testBinaryFlag.Dispose();
            Assert.Null(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_setting_after_disposing()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            testBinaryFlag.Dispose();
            testBinaryFlag.SetFlag(0);
            testBinaryFlag.SetFlag(1);
            Assert.Null(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_resetting_after_disposing()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            testBinaryFlag.Dispose();
            testBinaryFlag.ResetFlag(0);
            Assert.Null(testBinaryFlag.GetFlag());
        }

        [Fact]
        public void Test_resetting_all_positions()
        {
            MultipleBinaryFlag testBinaryFlag = new MultipleBinaryFlag(2);
            testBinaryFlag.ResetFlag(0);
            testBinaryFlag.ResetFlag(1);
            Assert.False(testBinaryFlag.GetFlag());
        }
    }
}