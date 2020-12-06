using System.Collections;

namespace Blake2B.StatisticalTests
{
    public interface IStatisticalTest
    {
        double GetPValue(BitArray input);
    }
}