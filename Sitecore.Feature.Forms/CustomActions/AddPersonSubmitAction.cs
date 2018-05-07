using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Feature.Forms.Service;
using static System.FormattableString;

namespace Sitecore.Feature.Forms.CustomActions
{
    /// <summary>
    /// Custom submit action for adding a new person/contact. This class is hooked up in Sitecore in system/settings/forms/submit actions
    /// </summary>
    public class AddPersonSubmitAction : SubmitActionBase<string>
    {
        /// <summary>
        /// Empty constructor for invoking base class
        /// </summary>
        /// <param name="submitActionData"></param>
        public AddPersonSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        /// <summary>
        /// Helper method for testing string value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Gelper method for getting the value from form parameter
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="formSubmitContext"></param>
        /// <returns></returns>
        private string GetValue(string fieldName, FormSubmitContext formSubmitContext)
        {
            string val = string.Empty;
            //var postedFormData = formSubmitContext.PostedFormData;
            var field = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals(fieldName));
            if (field != null)
            {
                var property = field.GetType().GetProperty("Value");
                if (property != null)
                {
                    var postedVal = property.GetValue(field);
                    val = postedVal != null ? postedVal.ToString() : string.Empty;
                }
            }

            return val;
        }

        /// <summary>
        /// Execute method that is called when the submit action for the form is invoked.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="formSubmitContext"></param>
        /// <returns></returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            if (!formSubmitContext.HasErrors)
            {
                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);

                //TODO: hard coded the form values. In order to make this dynamic, it needs to be tied to the form and it's form fields
                //This may be possible by looping over formSubmitContext.Fields
                string firstName = GetValue("FirstName", formSubmitContext);
                string lastName = GetValue("LastName", formSubmitContext);
                string email = GetValue("Email", formSubmitContext);
                string phone = GetValue("Phone", formSubmitContext);
                string address = GetValue("Address", formSubmitContext);
                string city = GetValue("City", formSubmitContext);
                string state = GetValue("State", formSubmitContext);
                string zip = GetValue("Zip", formSubmitContext);

                //TODO: hook this up with constructor injection
                IPersonService ps = new PersonService();

                ID personID = ps.AddPerson(firstName, lastName, email, phone, address, city, state, zip);

                if (!ID.IsNullOrEmpty(personID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
                
            
        }
    }
}