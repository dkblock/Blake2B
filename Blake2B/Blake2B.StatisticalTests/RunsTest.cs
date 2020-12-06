using Accord.Math;
using System;
using System.Collections;

namespace Blake2B.StatisticalTests
{
    public class RunsTest : IStatisticalTest
    {
        public double GetPValue(BitArray input)
        {
            var pi = 0.0;

            foreach (var bit in input)
            {
                if ((bool)bit)
                    pi++;
            }

            pi /= input.Length;

            if (Math.Abs(pi - 0.5) >= 2.0 / Math.Sqrt(input.Length))
                return 0.0;

            var V = 1.0;

            for (int k = 0; k < input.Length - 1; k++)
            {
                if (input[k] != input[k + 1])
                    V++;
            }

            var erfcArg = Math.Abs(V - 2 * input.Length * pi * (1 - pi)) / (2 * Math.Sqrt(2 * input.Length) * pi * (1 - pi));
            var pValue = Special.Erfc(erfcArg);

            return pValue;
        }
    }
}
