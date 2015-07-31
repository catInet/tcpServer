using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpServer
{   //парсит строку входных данных
    //сериализует выход в JSON
    class InputHandler
    {
        public static string handleInputString(string instr, double timestamp) 
        {
            string outstr;
            var valuesDict = splitToValDict(instr);
            var featuresDict = computeFeautures(valuesDict);
            outstr = generateJSONOutput(featuresDict, timestamp);
            return outstr;
        }

        private static Dictionary<string, List<double>> splitToValDict(string instr)
        {
            var dict = new Dictionary<string, List<double> >();
            var packets = instr.Split(';');
            List<double> acc_x = new List<double>(),
                         acc_y = new List<double>(),
                         acc_z = new List<double>(),
                         gyr_x = new List<double>(),
                         gyr_y = new List<double>(),
                         gyr_z = new List<double>();

            foreach (string packet in packets)
            {
                var values = packet.Split(',');
                acc_x.Add(Double.Parse(values[0]));
                acc_y.Add(Double.Parse(values[1]));
                acc_z.Add(Double.Parse(values[2]));
                gyr_x.Add(Double.Parse(values[3]));
                gyr_y.Add(Double.Parse(values[4]));
                gyr_z.Add(Double.Parse(values[5]));
            }

            dict.Add("acc_x", acc_x);
            dict.Add("acc_y", acc_y);
            dict.Add("acc_z", acc_z);
            dict.Add("gyr_x", gyr_x);
            dict.Add("gyr_y", gyr_y);
            dict.Add("gyr_z", gyr_z);

            return dict;
        }

        private static Dictionary<string, double> computeFeautures(Dictionary<string, List<double>> valuesDict)
        {
            var dict = new Dictionary<string, double>();
            _3DStatistic acc_3dstat = new _3DStatistic(valuesDict["acc_x"], valuesDict["acc_y"], valuesDict["acc_z"]),
                         gyr_3dstat = new _3DStatistic(valuesDict["gyr_x"], valuesDict["gyr_y"], valuesDict["gyr_z"]);

            //means
            dict.Add("acc_x_mean", acc_3dstat.xStat.Mean);
            dict.Add("acc_y_mean", acc_3dstat.yStat.Mean);
            dict.Add("acc_z_mean", acc_3dstat.zStat.Mean);
            dict.Add("gyr_x_mean", gyr_3dstat.xStat.Mean);
            dict.Add("gyr_y_mean", gyr_3dstat.yStat.Mean);
            dict.Add("gyr_z_mean", gyr_3dstat.zStat.Mean);

            //variance
            dict.Add("acc_x_var", acc_3dstat.xStat.Variance);
            dict.Add("acc_y_var", acc_3dstat.yStat.Variance);
            dict.Add("acc_z_var", acc_3dstat.zStat.Variance);
            dict.Add("gyr_x_var", gyr_3dstat.xStat.Variance);
            dict.Add("gyr_y_var", gyr_3dstat.yStat.Variance);
            dict.Add("gyr_z_var", gyr_3dstat.zStat.Variance);

            //StandardDeviation
            dict.Add("acc_x_std", acc_3dstat.xStat.StandardDeviation);
            dict.Add("acc_y_std", acc_3dstat.yStat.StandardDeviation);
            dict.Add("acc_z_std", acc_3dstat.zStat.StandardDeviation);
            dict.Add("gyr_x_std", gyr_3dstat.xStat.StandardDeviation);
            dict.Add("gyr_y_std", gyr_3dstat.yStat.StandardDeviation);
            dict.Add("gyr_z_std", gyr_3dstat.zStat.StandardDeviation);

            //Min
            dict.Add("acc_x_min", acc_3dstat.xStat.Minimum);
            dict.Add("acc_y_min", acc_3dstat.yStat.Minimum);
            dict.Add("acc_z_min", acc_3dstat.zStat.Minimum);
            dict.Add("gyr_x_min", gyr_3dstat.xStat.Minimum);
            dict.Add("gyr_y_min", gyr_3dstat.yStat.Minimum);
            dict.Add("gyr_z_min", gyr_3dstat.zStat.Minimum);

            //Max
            dict.Add("acc_x_max", acc_3dstat.xStat.Maximum);
            dict.Add("acc_y_max", acc_3dstat.yStat.Maximum);
            dict.Add("acc_z_max", acc_3dstat.zStat.Maximum);
            dict.Add("gyr_x_max", gyr_3dstat.xStat.Maximum);
            dict.Add("gyr_y_max", gyr_3dstat.yStat.Maximum);
            dict.Add("gyr_z_max", gyr_3dstat.zStat.Maximum);

            //Median absolute deviation
            dict.Add("acc_x_mad", acc_3dstat.xStat.mad);
            dict.Add("acc_y_mad", acc_3dstat.yStat.mad);
            dict.Add("acc_z_mad", acc_3dstat.zStat.mad);
            dict.Add("gyr_x_mad", gyr_3dstat.xStat.mad);
            dict.Add("gyr_y_mad", gyr_3dstat.yStat.mad);
            dict.Add("gyr_z_mad", gyr_3dstat.zStat.mad);

            //Kurtosis
            dict.Add("acc_x_kurt", acc_3dstat.xStat.Kurtosis);
            dict.Add("acc_y_kurt", acc_3dstat.yStat.Kurtosis);
            dict.Add("acc_z_kurt", acc_3dstat.zStat.Kurtosis);
            dict.Add("gyr_x_kurt", gyr_3dstat.xStat.Kurtosis);
            dict.Add("gyr_y_kurt", gyr_3dstat.yStat.Kurtosis);
            dict.Add("gyr_z_kurt", gyr_3dstat.zStat.Kurtosis);

            //Skewness
            dict.Add("acc_x_skew", acc_3dstat.xStat.Skewness);
            dict.Add("acc_y_skew", acc_3dstat.yStat.Skewness);
            dict.Add("acc_z_skew", acc_3dstat.zStat.Skewness);
            dict.Add("gyr_x_skew", gyr_3dstat.xStat.Skewness);
            dict.Add("gyr_y_skew", gyr_3dstat.yStat.Skewness);
            dict.Add("gyr_z_skew", gyr_3dstat.zStat.Skewness);

            //Interquartile range 
            dict.Add("acc_x_iqr", acc_3dstat.xStat.iqr);
            dict.Add("acc_y_iqr", acc_3dstat.yStat.iqr);
            dict.Add("acc_z_iqr", acc_3dstat.zStat.iqr);
            dict.Add("gyr_x_iqr", gyr_3dstat.xStat.iqr);
            dict.Add("gyr_y_iqr", gyr_3dstat.yStat.iqr);
            dict.Add("gyr_z_iqr", gyr_3dstat.zStat.iqr);

            //Entropy
            dict.Add("acc_x_entropy", acc_3dstat.xStat.entropy);
            dict.Add("acc_y_entropy", acc_3dstat.yStat.entropy);
            dict.Add("acc_z_entropy", acc_3dstat.zStat.entropy);
            dict.Add("gyr_x_entropy", gyr_3dstat.xStat.entropy);
            dict.Add("gyr_y_entropy", gyr_3dstat.yStat.entropy);
            dict.Add("gyr_z_entropy", gyr_3dstat.zStat.entropy);

            //Autocorrelation
            dict.Add("acc_x_autocorr", acc_3dstat.xStat.autocorr);
            dict.Add("acc_y_autocorr", acc_3dstat.yStat.autocorr);
            dict.Add("acc_z_autocorr", acc_3dstat.zStat.autocorr);
            dict.Add("gyr_x_autocorr", gyr_3dstat.xStat.autocorr);
            dict.Add("gyr_y_autocorr", gyr_3dstat.yStat.autocorr);
            dict.Add("gyr_z_autocorr", gyr_3dstat.zStat.autocorr);

            //index of the frequency component with largest magnitude
            dict.Add("acc_x_maxIndex", acc_3dstat.xStat.maxIndex);
            dict.Add("acc_y_maxIndex", acc_3dstat.yStat.maxIndex);
            dict.Add("acc_z_maxIndex", acc_3dstat.zStat.maxIndex);
            dict.Add("gyr_x_maxIndex", gyr_3dstat.xStat.maxIndex);
            dict.Add("gyr_y_maxIndex", gyr_3dstat.yStat.maxIndex);
            dict.Add("gyr_z_maxIndex", gyr_3dstat.zStat.maxIndex);

            //correlation
            dict.Add("acc_xy_corr", acc_3dstat.xy_corr);
            dict.Add("acc_yz_corr", acc_3dstat.yz_corr);
            dict.Add("acc_zx_corr", acc_3dstat.xz_corr);
            dict.Add("gyr_xy_corr", gyr_3dstat.xy_corr);
            dict.Add("gyr_yz_corr", gyr_3dstat.yz_corr);
            dict.Add("gyr_zx_corr", gyr_3dstat.xy_corr);

            //Signal magnitude area
            dict.Add("acc_sma", acc_3dstat.sma);
            dict.Add("gyr_sma", gyr_3dstat.sma);

            //Energy
            dict.Add("acc_energy", acc_3dstat.energy);
            dict.Add("gyr_energy", gyr_3dstat.energy);
            return dict;
        }

        private static string generateJSONOutput(Dictionary<string, double> featuresDict, double timestamp)
        {
            //string outstring = "";
            featuresDict.Add("timestamp", timestamp);
            return dictToJson(featuresDict);
            //return outstring;
        }

        private static string dictToJson(Dictionary<string, double> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": {1}", d.Key,  d.Value));
            return "{" + string.Join(",", entries) + "}";
        }
    }
}
