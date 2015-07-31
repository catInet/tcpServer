using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace tcpServer
{   
    class ExtendedStatistics : DescriptiveStatistics
    {
        //private List<double> values;
        public readonly double mad;
        public readonly double iqr;
        public readonly double entropy;
        public readonly double maxIndex;
        public readonly double autocorr;

        public ExtendedStatistics(List<double> values)
            : base(values)
        {
            mad = computeMad(values);
            iqr = computeIQR(values);
            entropy = computeEntropy(values);
            maxIndex = computeMaxIndex(values);
            autocorr = computeAutocorr(values);
        }
        

        //median absolute deviation
        public static double computeMad(List<double> values)
        {    var median = Statistics.Median(values);
             for (int i = 0; i < values.Count; ++i) {
                 values[i] = Math.Abs(values[i] - median);
             }
             return Statistics.Median(values);
        }

        //Interquartile range
        public static double computeIQR(List<double> values)
        {
            //var median = Statistics.Median()
            values.Sort();
            int chunkLength = (int) values.Count / 2;
            List<double> firstHalf = values.GetRange(0, chunkLength)
                       , secondHalf = values.GetRange(values.Count() - chunkLength, chunkLength);

            return Statistics.Median(secondHalf) - Statistics.Median(firstHalf);
        }

        //entropy
        public static double computeEntropy(List<double> values)
        {    
            List<double> probability = new List<double>();
            foreach (var x in values.GroupBy(p => p)) {
                var currProbability = x.Count() / values.Count;
                probability.Add(currProbability * Math.Log(currProbability));
            }
            return -probability.Sum();
        }

        //maxIndex - index of the frequency component with largest magnitude
        public static double computeMaxIndex(List<double> values)
        {
            List<double> probability = new List<double>();
            foreach (var x in values.GroupBy(p => p))
            {
                probability.Add(x.Count() / values.Count);
            }
            return probability.Max();
        }

        //autocorr
        public static double computeAutocorr(List<double> values)
        {
            if (values.Count < 3) return 0;
            List<double> originList = values.GetRange(0, values.Count - 1),
                         shiftedList = values.GetRange(1, values.Count - 1);
            return Statistics.Covariance(originList, shiftedList) / (Statistics.StandardDeviation(originList) * Statistics.StandardDeviation(shiftedList));
        }
    }
}
