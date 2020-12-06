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
            var testString = "The quick brown fox jumps over the lazy dog";
            var testData = Encoding.ASCII.GetBytes(testString);
            _testValue = new BitArray(testData);
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
            var blockLength = 3;
            var frequencyBlockTest = new FrequencyBlockTest(blockLength);

            var actual = frequencyBlockTest.GetPValue(_testValue);

            Assert.True(actual > EXPECTED_VALUE);
        }
    }
}
