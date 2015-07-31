using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace tcpServer
{
    class _3DStatistic
    {
        public readonly ExtendedStatistics xStat, yStat, zStat;
        public readonly double xy_corr, xz_corr, yz_corr;
        public readonly double sma;
        public readonly double energy;
        _3DStatistic(List<double> x, List<double> y, List<double> z) {
            xStat = new ExtendedStatistics(x);
            yStat = new ExtendedStatistics(y);
            zStat = new ExtendedStatistics(z);
            xy_corr =  computeCorrelation(x, y);
            xz_corr = computeCorrelation(x, z);
            yz_corr = computeCorrelation(y, z);
            sma = computeSMA(x, y, z);
            energy = computeEnergy(x, y, z);
        }

        public static double computeCorrelation(List<double> list1, List<double> list2)
        {
            return Statistics.Covariance(list1, list2) / (Statistics.StandardDeviation(list1) * Statistics.StandardDeviation(list2));
        }

        public static double computeSMA(List<double> x, List<double> y, List<double> z)
        {   
            var conctatedList = x.Concat(y).ToList().Concat(z).ToList();
            for (int i = 0; i < conctatedList.Count; ++i) {
                conctatedList[i] = Math.Abs(conctatedList[i]);
            }
            return conctatedList.Sum() / x.Count();
        }

        public static double computeEnergy(List<double> x, List<double> y, List<double> z)
        {
            var conctatedList = x.Concat(y).ToList().Concat(z).ToList();
            for (int i = 0; i < conctatedList.Count; ++i)
            {
                conctatedList[i] = Math.Pow(conctatedList[i], 2);
            }
            return conctatedList.Sum() / x.Count();
        }
    }
}
