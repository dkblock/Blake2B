using Blake2B.StatisticalTests;
using System;
using System.Collections;
using Xunit;

namespace Blake2B.Tests
{
    public class CorrectnessTests
    {
        [Fact]
        public void FrequencyMonobitTest_ReturnsCorrectValue()
        {
            var input = FromStringToBit("1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000");
            var frequencyMonobitTest = new FrequencyMonobitTest();

            var expected = 0.109599;
            var actual = frequencyMonobitTest.GetPValue(input);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void FrequencyBlockTest_ReturnsCorrectValue()
        {
            var blockLength = 10;
            var input = FromStringToBit("1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000");
            var frequencyBlockTest = new FrequencyBlockTest(blockLength);

            var expected = 0.706438;
            var actual = frequencyBlockTest.GetPValue(input);

            Assert.True(AreEqual(expected, actual));
        }

        private bool AreEqual(double expected, double actual)
        {
            var eps = 0.000001;
            return Math.Abs(expected - actual) < eps;
        }

        private BitArray FromStringToBit(string input)
        {
            var bools = new bool[input.Length];

            for (int i = 0; i < input.Length; i++)
                bools[i] = input[i] == '1' ? true : false;

            return new BitArray(bools);
        }
    }
}
