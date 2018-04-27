using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static System.FormattableString;

namespace Sitecore.Feature.Forms.CustomActions
{
    public class AddPersonSubmitAction : SubmitActionBase<string>
    {
        public AddPersonSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        private string GetValue(string fieldName, FormSubmitContext formSubmitContext)
        {
            string val = string.Empty;
            //var postedFormData = formSubmitContext.PostedFormData;
            var field = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals(fieldName));
            if (field != null)
            {
                var property = field.GetType().GetProperty("Value");
                var postedVal = property.GetValue(field);
                val = postedVal.ToString();
            }

            return val;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            if (!formSubmitContext.HasErrors)
            {
                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);

                string firstName = GetValue("FirstName", formSubmitContext);
                string lastName = GetValue("LastName", formSubmitContext);
                string email = GetValue("Email", formSubmitContext);
                string phone = GetValue("Phone", formSubmitContext);
                string address = GetValue("Address", formSubmitContext);
                string city = GetValue("City", formSubmitContext);
                string state = GetValue("State", formSubmitContext);
                string zip = GetValue("Zip", formSubmitContext);

                string contentType = "application/json";
                string url = @"https://minicrm/sitecore/api/ssc/auth/login";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                
                HttpResponseMessage response;
                var method = new HttpMethod("POST");

                JObject o = new JObject
                {
                    { "domain", "sitecore" },
                    { "username", "admin" },
                    { "password", "b" }
                };
                
                string json = o.ToString();
                
                HttpContent body = new StringContent(json);
                body.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response = client.PostAsync(url, body).Result;

                if (response.IsSuccessStatusCode)
                {
                    Logger.Info("LOGIN RESPONSE: " + response.Content.ReadAsStringAsync().Result, this);

                    //client = new HttpClient();
                    url = @"https://minicrm/sitecore/api/ssc/item/sitecore%2Fcontent%2FMiniCRM%2FSite%20Content%2FPersons?database=master";
                    //client.BaseAddress = new Uri(url);

                    o = new JObject
                    {
                        { "ItemName", "Test Person 1" },
                        { "TemplateID", "9D5BEACE-14DB-4EA2-8F1F-A087A8DAC5F0" },
                        { "First Name", firstName },
                        { "Last Name", lastName }
                    };

                    json = o.ToString();

                    body = new StringContent(json);
                    body.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    response = client.PostAsync(url, body).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Logger.Info("RESPONSE: " + response.Content.ReadAsStringAsync().Result, this);

                    }
                    else
                    {
                        Logger.Info("ADD PERSON BOMBED: " + response.ReasonPhrase, this);
                    }

                        //// Parse the response body. Blocking!
                        //var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;
                        //foreach (var d in dataObjects)
                        //{
                        //    Console.WriteLine("{0}", d.Name);
                        //}
                    }
                else
                {
                    //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    Logger.Info("LOGIN FAILED: " + response.ReasonPhrase, this);
                }


            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }
            return true;
        }
    }
}