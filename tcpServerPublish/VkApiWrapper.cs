using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace tcpServerPublish
{   //cлишком много отвественности. Пусть в Бд кто-то другой пишет!!
    class VkApiWrapper
    {
        private static string ServiceBaseUri = ConfigurationSettings.AppSettings["VKServiceBaseUri"];//"https://api.datamarket.azure.com/";
        private static string requestBaseUri = ConfigurationSettings.AppSettings["VKRequestUri"] + "?user_id=USER_ID_STR&message=MESSAGE_STR&access_token=ACCESS_TOKEN_STR";
        private static string clientID = ConfigurationSettings.AppSettings["VKClientID"];
        private static string clientSecret = ConfigurationSettings.AppSettings["VKClientSecret"];
        private static string accessToken = ConfigurationSettings.AppSettings["VKAccessToken"];
        private static string serviceOAuthUri = ConfigurationSettings.AppSettings["VKServiceOAuthUrl"];
        
        public static void getAccessToken()
        {
            var oauthStrUri = serviceOAuthUri.Replace("CLIENT_ID_STR", clientID).Replace("CLIENT_SECRET_STR", clientSecret);
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> responseTask = httpClient.GetAsync(oauthStrUri);
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
            AccessToken token = JsonConvert.DeserializeObject<AccessToken>(content);
            var config = WebConfigurationManager.OpenWebConfiguration("/");
            config.AppSettings.Settings["VkAccessToken"].Value = token.access_token;
            config.Save(ConfigurationSaveMode.Modified); //Modified
        }

        public static void sendToVk(string message, string userID) {
            var messageEncoded = System.Web.HttpUtility.UrlEncode(message);
            var sendMessageUrlEncoded = requestBaseUri.Replace("MESSAGE_STR", messageEncoded).Replace("USER_ID_STR", userID).Replace("ACCESS_TOKEN_STR", accessToken);
            //var sendMessageUrlEncoded = System.Web.HttpUtility.UrlEncode(sendMessageUrl);
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> responseTask = httpClient.GetAsync(sendMessageUrlEncoded);
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
            //распарсить и вытащить код ошибки. Если 14 - нужно что-то делать с капчой
            try
            {
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<Dictionary<string, string>>(content);
                var json_error = jss.Deserialize<Dictionary<string, string>>(dict["error"]);
                if (json_error["error_code"] == "14")
                {
                    //сохранить в базу данных
                    TableStorageConnector.SaveCaptchaToDb(json_error["captcha_sid"], json_error["captcha_img"]);
                }
            }
            catch (Exception) // если не парсится по такому сценарию, считаем, что ошибок нет
            {

            }

        }
    }
    public class AccessToken
    {
        public string access_token { get; set; }
    }

    
}
