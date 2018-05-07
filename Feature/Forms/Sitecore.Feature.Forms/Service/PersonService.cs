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
    /// <summary>
    /// Service class for managing Person/Contacts
    /// </summary>
    public class PersonService : IPersonService
    {
        /// <summary>
        /// Method for adding the person/contact. Called from the custom submit action from the form.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns></returns>
        public ID AddPerson(string firstname, string lastname, string email, string phone, string address, string city, string state, string zip)
        {
            //Create the Json object to send to the Item Service API
            JObject o = new JObject
            {
                { "ItemName", firstname + " " + lastname},
                { "TemplateID", Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Forms.PersonTemplateId") },
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
            string url = Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Forms.AddPersonUrl");

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

                    //Sitecore.Diagnostics.Log.Info("PERSONID: " + res, this);
                }

                //TODO: Fix this. Item Service isn't returning the ID that was created. 
                return new ID();
            }
            else
            {
                return null;

            }
        }
    }
}