using Blake2B.StatisticalTests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Blake2B.Demo
{
    public class StatisticalTestsDemo
    {
        private const double EXPECTED_VALUE = 0.01;

        private readonly IEnumerable<IStatisticalTest> _statisticalTests;
        private readonly BitArray _testValue;

        public StatisticalTestsDemo(byte[] testValue)
        {
            _testValue = new BitArray(testValue);
            _statisticalTests = new List<IStatisticalTest>
            {
                new FrequencyMonobitTest(),
                new FrequencyBlockTest(),
                new RunsTest(),
                new LongestRunOfOnesTest(),
                new BinaryMatrixRankTest(),
                new CumulativeSumsTests()
            };
        }

        public void Run()
        {
            foreach (var test in _statisticalTests)
                GetTestResult(test);

            Console.ReadLine();
        }

        private void GetTestResult(IStatisticalTest test)
        {
            var pValue = test.GetPValue(_testValue);
            var result = pValue >= EXPECTED_VALUE
                ? "The sequence is random"
                : "The sequence is non-random";

            Console.WriteLine(test.GetType().Name);
            Console.WriteLine($"P-Value: {pValue}");
            Console.WriteLine($"Result: {result}\n");
        }
    }
}
