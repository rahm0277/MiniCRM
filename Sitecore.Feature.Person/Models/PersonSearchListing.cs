using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Persons.Models
{
    public class PersonSearchListing
    {
        public List<Sitecore.Feature.Persons.Models.Person> ListingResults { get; set; }
        public int TotalResults { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int NumberOfPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalResults / (double)PageSize);
            }
        }


    }
}