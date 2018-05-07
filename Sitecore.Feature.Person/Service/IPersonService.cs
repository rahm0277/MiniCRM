using Sitecore.Feature.Persons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Feature.Persons.Service
{
    /// <summary>
    /// Interface for the Person Service Implementation
    /// </summary>
    public interface IPersonService
    {
        PersonSearchListing GetPersons(string email, string phone, int page, string dbname);
    }
}
