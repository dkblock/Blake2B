using Blake2B.StatisticalTests;
using System;
using System.Collections;
using System.Text;
using Xunit;

namespace Blake2B.Tests
{
    public class CorrectnessTests
    {
        private readonly BitArray _testInput;

        public CorrectnessTests()
        {
            _testInput = FromOnesAndZeroesToBit("1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000");
        }

        [Fact]
        public void FrequencyMonobitTest_ReturnsCorrectValue()
        {
            var frequencyMonobitTest = new FrequencyMonobitTest();

            var expected = 0.109599;
            var actual = frequencyMonobitTest.GetPValue(_testInput);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void FrequencyBlockTest_ReturnsCorrectValue()
        {
            var blockLength = 10;
            var frequencyBlockTest = new FrequencyBlockTest(blockLength);

            var expected = 0.706438;
            var actual = frequencyBlockTest.GetPValue(_testInput);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void RunsTest_ReturnsCorrectValue()
        {
            var runsTest = new RunsTest();

            var expected = 0.500798;
            var actual = runsTest.GetPValue(_testInput);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void CumulativeSumsTest_InForwardMode_ReturnsCorrectValue()
        {
            var cusumTest = new CumulativeSumsTests(0);

            var expected = 0.219194;
            var actual = cusumTest.GetPValue(_testInput);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void CumulativeSumsTest_InBackwardMode_ReturnsCorrectValue()
        {
            var cusumTest = new CumulativeSumsTests(1);

            var expected = 0.114866;
            var actual = cusumTest.GetPValue(_testInput);

            Assert.True(AreEqual(expected, actual));
        }

        private bool AreEqual(double expected, double actual)
        {
            var eps = 0.000001;
            return Math.Abs(expected - actual) < eps;
        }

        private BitArray FromOnesAndZeroesToBit(string input)
        {
            var bools = new bool[input.Length];

            for (int i = 0; i < input.Length; i++)
                bools[i] = input[i] == '1' ? true : false;

            return new BitArray(bools);
        }

        private BitArray FromStringToBit(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return new BitArray(bytes);
        }
    }
}
