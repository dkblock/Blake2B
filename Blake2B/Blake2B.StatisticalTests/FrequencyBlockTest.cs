using Accord.Math;
using System;
using System.Collections;

namespace Blake2B.StatisticalTests
{
    public class FrequencyBlockTest : IStatisticalTest
    {
        private readonly int _blockLength;

        public FrequencyBlockTest(int blockLength)
        {
            _blockLength = blockLength;
        }

        public double GetPValue(BitArray input)
        {
            var blocksCount = Math.Truncate((double)input.Length / _blockLength);
            var pi = new double[(int)blocksCount];

            for (int i = 0; i < blocksCount; i++)
            {
                var blockSum = 0.0;

                for (int j = 0; j < _blockLength; j++)
                    blockSum += input[j + i * _blockLength] ? 1 : 0;

                pi[i] = blockSum / _blockLength;
            }

            var chiSquare = 0.0;

            for (int i = 0; i < blocksCount; i++)
                chiSquare += (pi[i] - 0.5) * (pi[i] - 0.5);

            chiSquare *= 4.0 * _blockLength;

            var pValue = 1 - Gamma.LowerIncomplete(blocksCount / 2, chiSquare / 2);

            return pValue;
        }
    }
}
