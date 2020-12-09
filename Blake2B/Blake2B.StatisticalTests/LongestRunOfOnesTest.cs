using Accord.Math;
using System;
using System.Collections;

namespace Blake2B.StatisticalTests
{
    public class LongestRunOfOnesTest : IStatisticalTest
    {
        private int _blockLength;
        private int _blocksCount;
        private int _runsCount;
        private double[] _pi;
        private int[] _v;
        private int[] _onesLength;

        public double GetPValue(BitArray input)
        {
            InitValues(input.Length);

            for (int i = 0; i < _blocksCount; i++)
            {
                var longestRun = 0;
                var currentRun = 0;

                for (int j = 0; j < _blockLength; j++)
                {
                    if (input[i * _blockLength + j])
                    {
                        currentRun++;

                        if (currentRun > longestRun)
                            longestRun = currentRun;
                    }
                    else
                        currentRun = 0;
                }

                if (longestRun < _v[0])
                    _onesLength[0]++;

                for (int j = 0; j <= _runsCount; j++)
                {
                    if (longestRun == _v[j])
                        _onesLength[j]++;
                }

                if (longestRun > _v[_runsCount])
                    _onesLength[_runsCount]++;
            }

            var chiSquare = 0.0;

            for (int i = 0; i <= _runsCount; i++)
                chiSquare += (_onesLength[i] - _blocksCount * _pi[i]) * (_onesLength[i] - _blocksCount * _pi[i]) / (_blocksCount * _pi[i]);

            var pValue = Gamma.UpperIncomplete((double)_runsCount / 2, chiSquare / 2);

            return pValue;
        }

        private void InitValues(int inputLength)
        {
            if (inputLength < 128)
                throw new Exception("Input value is too short");
            else if (inputLength < 6272)
            {
                _blockLength = 8;
                _runsCount = 3;
                _v = new int[] { 1, 2, 3, 4 };
                _pi = new double[] { 0.2148, 0.3672, 0.2305, 0.1875 };
            }
            else if (inputLength < 750000)
            {
                _blockLength = 128;
                _runsCount = 5;
                _v = new int[] { 4, 5, 6, 7, 8, 9 };
                _pi = new double[] { 0.1174, 0.2430, 0.2494, 0.1752, 0.1027, 0.1124 };
            }
            else
            {
                _blockLength = 10000;
                _runsCount = 6;
                _v = new int[] { 10, 11, 12, 13, 14, 15, 16 };
                _pi = new double[] { 0.0882, 0.2092, 0.2483, 0.1933, 0.1208, 0.0675, 0.0727 };
            }

            _blocksCount = inputLength / _blockLength;
            _onesLength = new int[_runsCount + 1];
        }
    }
}
