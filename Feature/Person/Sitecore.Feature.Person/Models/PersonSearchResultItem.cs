using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Persons.Models
{
    /// <summary>
    /// Search result item to map to index
    /// </summary>
    public class PersonSearchResultItem : SearchResultItem
    {
        [IndexField("first name")]
        public string FirstName { get; set; }

        [IndexField("last name")]
        public string LastName { get; set; }

        [IndexField("email")]
        public string Email { get; set; }

        [IndexField("phone")]
        public string Phone { get; set; }
    }
}