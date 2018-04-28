using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Newtonsoft.Json.Linq;
using Sitecore.Foundation.Services.Service;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sitecore.Feature.Forms.Service
{
    public class PersonService : IPersonService
    {
        public ID AddPerson(string firstname, string lastname, string email, string phone, string address, string city, string state, string zip)
        {
            JObject o = new JObject
            {
                { "ItemName", firstname + " " + lastname},
                { "TemplateID", "9D5BEACE-14DB-4EA2-8F1F-A087A8DAC5F0" },
                { "First Name", firstname },
                { "Last Name", lastname },
                { "Email", email },
                { "Phone", phone },
                { "Address", address },
                { "City", city },
                { "State", state },
                { "Zip", zip }
                
            };

            string jsonContent = o.ToString();

            //TODO: hook this up with constructor injection
            SSCItemService ssc = new SSCItemService();
            string url = @"https://minicrm/sitecore/api/ssc/item/sitecore%2Fcontent%2FMiniCRM%2FSite%20Content%2FPersons?database=master";

            string domain = Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Forms.ServiceAPIDomain");
            string username = Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Forms.ServiceAPIUsername");
            string password = Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Forms.ServiceAPIPassword");

            HttpResponseMessage response = ssc.CallItemServiceAPI(url, jsonContent, "application/json", "POST", domain, username, password);

            if(response.IsSuccessStatusCode)
            {

                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    Task<string> result = content.ReadAsStringAsync();
                    string res = result.Result;

                    Sitecore.Diagnostics.Log.Info(res, this);
                }

                return new ID();
            }
            else
            {
                return null;

            }
        }
    }
}