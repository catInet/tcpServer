using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpServerPublish
{
    class DataPreprocessor
    {
        public static string generateStringToSend(string boardString, long timestamp)
        { 
            var dataPackages = boardString.ToString().Split(';');
            string    acc_x = "", acc_y = "", acc_z = "",
                      gyr_x = "", gyr_y = "", gyr_z = "";
            foreach (string package in dataPackages)
            {
                if (!(String.IsNullOrEmpty(package)))
                {
                    var singleReadings = package.Split(',');
                    int testInt = 0;
                    try
                    {
                        acc_x += singleReadings[0] + "_";
                        testInt = Convert.ToInt32(singleReadings[0]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        acc_x += "0_";
                    }

                    try
                    {
                        acc_y += singleReadings[1] + "_";
                        testInt = Convert.ToInt32(singleReadings[1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        acc_y += "0_";
                    }

                    try
                    {
                        acc_z += singleReadings[2] + "_";
                        testInt = Convert.ToInt32(singleReadings[2]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        acc_z += "0_";

                    }

                    try
                    {
                        gyr_x += singleReadings[3] + "_";
                        testInt = Convert.ToInt32(singleReadings[3]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        gyr_x += "0_";
                    }

                    try
                    {
                        gyr_y += singleReadings[4] + "_";
                        testInt = Convert.ToInt32(singleReadings[4]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        gyr_y += "0_";
                    }

                    try
                    {
                        gyr_z += singleReadings[5] + "_";
                        testInt = Convert.ToInt32(singleReadings[5]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        gyr_z += "0_";
                    }
                }
            }

            var textToSend = "{\"acc_x\": \"" + acc_x.Substring(0, acc_x.Length - 1) + "\", " +
                             "\"acc_y\": \"" + acc_y.Substring(0, acc_y.Length - 1) + "\", " +
                             "\"acc_z\": \"" + acc_z.Substring(0, acc_z.Length - 1) + "\", " +
                             "\"gyr_x\": \"" + gyr_x.Substring(0, gyr_x.Length - 1) + "\", " +
                             "\"gyr_y\": \"" + gyr_y.Substring(0, gyr_y.Length - 1) + "\", " +
                             "\"gyr_z\": \"" + gyr_z.Substring(0, gyr_z.Length - 1) + "\", " +
                             "\"timestamp\" :" + timestamp +
                             "}";
            return textToSend;
        }
    }
}
