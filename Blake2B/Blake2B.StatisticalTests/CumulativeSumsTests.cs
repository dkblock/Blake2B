using Accord.Math;
using System;
using System.Collections;
using System.Linq;

namespace Blake2B.StatisticalTests
{
    public class CumulativeSumsTests : IStatisticalTest
    {
        private readonly int _mode;

        public CumulativeSumsTests(int mode = 0)
        {
            _mode = mode;
        }

        public double GetPValue(BitArray input)
        {
            var X = new int[input.Length];
            var partialSums = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
                X[i] = input[i] ? 1 : -1;

            if (_mode == 0)
                ComputePartialSumsForwardMode(partialSums, X);
            else
                ComputePartialSumsBackwardMode(partialSums, X);

            var absoluteSums = partialSums.Select(s => Math.Abs(s));
            var z = absoluteSums.Max();

            var sum1 = 0.0;
            var sum2 = 0.0;

            for (var k = (-input.Length / z + 1) / 4; k <= (input.Length / z - 1) / 4; k++)
            {
                sum1 += F((4 * k + 1) * z / Math.Sqrt(input.Length));
                sum1 -= F((4 * k - 1) * z / Math.Sqrt(input.Length));
            }

            for (var k = (-input.Length / z - 3) / 4; k <= (input.Length / z - 1) / 4; k++)
            {
                sum2 += F((4 * k + 3) * z / Math.Sqrt(input.Length));
                sum2 -= F((4 * k + 1) * z / Math.Sqrt(input.Length));
            }

            var pValue = 1 - sum1 + sum2;

            return pValue;
        }

        private void ComputePartialSumsForwardMode(int[] partialSums, int[] X)
        {
            partialSums[0] = X[0];

            for (int i = 1; i < X.Length; i++)
                partialSums[i] = partialSums[i - 1] + X[i];
        }

        private void ComputePartialSumsBackwardMode(int[] partialSums, int[] X)
        {
            partialSums[0] = X[X.Length - 1];

            for (int i = 1; i < X.Length; i++)
                partialSums[i] = partialSums[i - 1] + X[X.Length - i - 1];
        }

        private double F(double x)          // Standard Normal Cumulative Probability Distribution Function
        {
            if (x > 0)
            {
                var arg = x / Math.Sqrt(2);
                return 0.5 * (1 + Special.Erf(arg));
            }
            else
            {
                var arg = -x / Math.Sqrt(2);
                return 0.5 * (1 - Special.Erf(arg));
            }
        }
    }
}
