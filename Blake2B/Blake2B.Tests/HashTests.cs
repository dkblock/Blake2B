using Blake2B.Core;
using Blake2B.StatisticalTests;
using System.Collections;
using System.Text;
using Xunit;

namespace Blake2B.Tests
{
    public class HashTests
    {
        private const double EXPECTED_VALUE = 0.01;
        private readonly BitArray _testValue;

        public HashTests()
        {
            var blake = new Blake2B_Algorithm();
            var testString = "The quick brown fox jumps over the lazy dog";
            var testData = Encoding.ASCII.GetBytes(testString);
            var hash = blake.ComputeHash(testData);

            _testValue = new BitArray(hash);
        }

        [Fact]
        public void FrequencyMonobitTest_ShouldPass()
        {
            var frequencyMonobitTest = new FrequencyMonobitTest();
            var actual = frequencyMonobitTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }

        [Fact]
        public void FrequencyBlockTest_ShouldPass()
        {
            var frequencyBlockTest = new FrequencyBlockTest();
            var actual = frequencyBlockTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }

        [Fact]
        public void RunsTest_ShouldPass()
        {
            var runsTest = new RunsTest();
            var actual = runsTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }

        [Fact]
        public void CumulativeSumsTest_InForwardMode_ShouldPass()
        {
            var cusumTest = new CumulativeSumsTests(0);
            var actual = cusumTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }

        [Fact]
        public void CumulativeSumsTest_InBackwardMode_ShouldPass()
        {
            var cusumTest = new CumulativeSumsTests(1);
            var actual = cusumTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }
    }
}
