using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;




namespace Sitecore.Foundation.Services.Service
{
    /// <summary>
    /// Service class to use the Sitecore Item Service functionality.
    /// Implements ISSCItemService Interface
    /// </summary>
    public class SSCItemService : ISSCItemService
    {

      /// <summary>
      /// Authenticates against Sitecore for the Sitecore Item Service and returns a token string to indicate successful login.
      /// Token must be passed in header with subsequent calls to the API
      /// </summary>
      /// <param name="domain"></param>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
        public string Login(string domain, string username, string password)
        {
            HttpClient client = new HttpClient();
            string url = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Services.ItemService.LoginUrl");

            JObject o = new JObject
            {
                { "domain", domain },
                { "username", username },
                { "password", password }
            };

            string json = o.ToString();

            HttpContent body = new StringContent(json);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, body).Result;

            if (response.IsSuccessStatusCode)
            {
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    Task<string> result = content.ReadAsStringAsync();
                    string res = result.Result;

                    JObject resp = JObject.Parse(res);

                    Sitecore.Diagnostics.Log.Info(res, this);

                    client.Dispose();
                    return (string)resp["token"];

                }


            }
            else
            {
                client.Dispose();
                return "";
            }
        }

        /// <summary>
        /// Calls the Item Service with URL and Json content passed in from calling class
        /// </summary>
        /// <param name="url">The URL to call</param>
        /// <param name="jsonContent">string JSON content of the body</param>
        /// <param name="contentType">mime type - usually application/json</param>
        /// <param name="method">Method as needed by the specific method of the API</param>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public HttpResponseMessage CallItemServiceAPI(string url, string jsonContent, string contentType, string method, string domain, string username, string password)
        {
            string token = Login(domain, username, password);

            //Sitecore.Diagnostics.Log.Info("TOKEN: " + token, this);

            if (token != "")
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response;
                //var m = new HttpMethod(method);

                HttpContent body = new StringContent(jsonContent);
                body.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                client.DefaultRequestHeaders.Add("token", token);
                response = client.PostAsync(url, body).Result;

                if (response.IsSuccessStatusCode)
                {
                    client.Dispose();
                    return response;
                }
                else
                {
                    client.Dispose();
                    return null;
                    //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    //Sitecore.Diagnostics.Log.Info("LOGIN FAILED: " + response.ReasonPhrase, this);
                }

            }
            else
            {
                return null;
            }

            
        }
    }
}