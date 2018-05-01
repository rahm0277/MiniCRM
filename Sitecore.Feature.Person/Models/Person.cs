using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Persons.Models
{
    public class Person
    {
        Item _item;
        public Person(Item innerItem)
        {
            _item = innerItem;
        }

        public string FirstName
        {
            get
            {
                return FieldRenderer.Render(_item, "First Name");
            }
        }

        public string LastName
        {
            get
            {
                return FieldRenderer.Render(_item, "Last Name");
            }
        }

        public string Email
        {
            get
            {
                return FieldRenderer.Render(_item, "Email");
            }
        }

        public string Phone
        {
            get
            {
                return FieldRenderer.Render(_item, "Phone");
            }
        }

        public string Address
        {
            get
            {
                return FieldRenderer.Render(_item, "Address");
            }
        }

        public string City
        {
            get
            {
                return FieldRenderer.Render(_item, "City");
            }
        }

        public string State
        {
            get
            {
                return FieldRenderer.Render(_item, "State");
            }
        }

        public string Zip
        {
            get
            {
                return FieldRenderer.Render(_item, "Zip");
            }
        }
    }
}