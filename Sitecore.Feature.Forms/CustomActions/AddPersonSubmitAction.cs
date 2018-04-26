using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

                Logger.Info("firstName: " + firstName);
                Logger.Info("lastName: " + lastName);
                Logger.Info("email: " + email);
                Logger.Info("phone: " + phone);

            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }
            return true;
        }
    }
}