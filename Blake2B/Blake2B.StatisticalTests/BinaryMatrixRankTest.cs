using Accord.Math;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Blake2B.StatisticalTests
{
    public class BinaryMatrixRankTest : IStatisticalTest
    {
        private readonly int _rows;
        private readonly int _columns;

        public BinaryMatrixRankTest(int rows = 3, int columns = 3)
        {
            if (rows != 3 || columns != 3)
                throw new Exception("Unsupported rows/columns number. Try 3x3 for example");

            _rows = rows;
            _columns = columns;
        }

        public double GetPValue(BitArray input)
        {
            var blocksCount = Math.Truncate((double)input.Length / (_rows * _columns));
            var matrixes = FillMatrixes(input, (int)blocksCount);

            var fm = 0;         // count of matrixes with full rank
            var fm1 = 0;        // count of matrixes with full rank - 1

            foreach (var matrix in matrixes)
            {
                if (Matrix.Rank(matrix) == _rows)
                    fm++;
                else if (Matrix.Rank(matrix) == _rows - 1)
                    fm1++;
            }

            var chiSquare = (fm - 0.2888 * blocksCount) * (fm - 0.2888 * blocksCount) / (0.2888 * blocksCount) +
                (fm1 - 0.5776 * blocksCount) * (fm1 - 0.5776 * blocksCount) / (0.5776 * blocksCount) +
                (blocksCount - fm - fm1 - 0.1336 * blocksCount) * (blocksCount - fm - fm1 - 0.1336 * blocksCount) / (0.1336 * blocksCount);

            var pValue = Math.Exp(-chiSquare / 2);

            return pValue;
        }

        private IEnumerable<double[,]> FillMatrixes(BitArray input, int blocksCount)
        {
            var matrixes = new List<double[,]>();
            var matrixSize = _columns * _rows;

            for (int i = 0; i < blocksCount; i++)
            {
                var matrix = new double[_rows, _columns];

                for (int j = 0; j < _rows; j++)
                    for (int k = 0; k < _columns; k++)
                        matrix[j, k] = input[i * matrixSize + j * _rows + k] ? 1 : 0;

                matrixes.Add(matrix);
            }

            return matrixes;
        }
    }
}
