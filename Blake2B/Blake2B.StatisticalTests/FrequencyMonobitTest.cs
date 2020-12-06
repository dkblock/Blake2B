using Accord.Math;
using System;
using System.Collections;

namespace Blake2B.StatisticalTests
{
    public class FrequencyMonobitTest : IStatisticalTest
    {
        public double GetPValue(BitArray input)
        {
            var sum = 0.0;

            foreach (var bit in input)
                sum += (bool)bit ? 1 : -1;

            var stat = Math.Abs(sum) / Math.Sqrt(input.Length);
            var pValue = Special.Erfc(stat / Math.Sqrt(2));

            return pValue;
        }
    }
}
