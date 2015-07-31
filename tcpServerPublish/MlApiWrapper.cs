using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace tcpServerPublish
{
    class MlApiWrapper
    {

        private static string ServiceBaseUri = ConfigurationSettings.AppSettings["MLServiceBaseUri"];//"https://api.datamarket.azure.com/";
        private static string requestBaseUri = ConfigurationSettings.AppSettings["MLRequestUri"];
        private static string accountKey = ConfigurationSettings.AppSettings["MLAccountKey"];
        //возвращает номер класса
        public static int sendStringToML(string preprocessedString)
        {
            using (var httpClient = new HttpClient())
            {
                string inputTextEncoded = System.Web.HttpUtility.UrlEncode(preprocessedString);
                httpClient.BaseAddress = new Uri(ServiceBaseUri);
                string creds = "AccountKey:" + accountKey;
                string authorizationHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(creds));
                httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // get key phrases
                string keyPhrasesRequest = requestBaseUri + "GetKeyPhrases?Text=" + inputTextEncoded;//"data.ashx/amla/text-analytics/v1.1/GetKeyPhrases?Text=" + inputTextEncoded;
                Task<HttpResponseMessage> responseTask = httpClient.GetAsync(keyPhrasesRequest);
                responseTask.Wait();
                HttpResponseMessage response = responseTask.Result;
                Task<string> contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                string content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Call to get key phrases failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                KeyPhraseResult keyPhraseResult = JsonConvert.DeserializeObject<KeyPhraseResult>(content);
                Console.WriteLine("Key phrases: " + string.Join(",", keyPhraseResult.KeyPhrases));

                // get sentiment
                string sentimentRequest = requestBaseUri + "GetSentiment?Text=" + inputTextEncoded;
                responseTask = httpClient.GetAsync(sentimentRequest);
                responseTask.Wait();
                response = responseTask.Result;
                contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Call to get sentiment failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                SentimentResult sentimentResult = JsonConvert.DeserializeObject<SentimentResult>(content);
                return Convert.ToInt32(1 + sentimentResult.Class * 7.0);
            }
        }
    }

    /// <summary>
    /// Class to hold result of Key Phrases call
    /// </summary>
    public class KeyPhraseResult
    {
        public List<string> KeyPhrases { get; set; }
    }

    /// <summary>
    /// Class to hold result of Sentiment call
    /// </summary>
    public class SentimentResult
    {
        public double Class { get; set; }
    }

}
