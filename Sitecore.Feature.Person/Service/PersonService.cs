using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Feature.Persons.Models;

namespace Sitecore.Feature.Persons.Service
{
    public class PersonService :IPersonService
    {
        public List<Sitecore.Feature.Persons.Models.Person> GetPersons(string email, string phone, int page)
        {
            var index = ContentSearchManager.GetIndex("minicrm_person_master_index");

            List<Sitecore.Feature.Persons.Models.Person> filteredList = new List<Sitecore.Feature.Persons.Models.Person>();

            var filterPredicate = PredicateBuilder.True<PersonSearchResultItem>();

            if (!String.IsNullOrEmpty(email))
            {
               
                var emailPredicate = PredicateBuilder.False<PersonSearchResultItem>();
                emailPredicate = emailPredicate.Or(x => x.Email.Contains(email.ToLower()));
                filterPredicate = filterPredicate.And(emailPredicate);

            }

            if (!String.IsNullOrEmpty(phone))
            {

                var emailPredicate = PredicateBuilder.False<PersonSearchResultItem>();
                emailPredicate = emailPredicate.Or(x => x.Phone.Contains(phone.ToLower()));
                filterPredicate = filterPredicate.And(emailPredicate);

            }

            using (var context = index.CreateSearchContext())
            {
                IQueryable<SearchResultItem> query = context.GetQueryable<PersonSearchResultItem>().Where(filterPredicate);
                
                var result = query.GetResults();
                var resultItems = result.Hits;

                foreach (var searchResult in resultItems)
                {
                    SearchResultItem gsi = searchResult.Document;
                    Sitecore.Feature.Persons.Models.Person p = new Sitecore.Feature.Persons.Models.Person(gsi.GetItem());
                    filteredList.Add(p);
                }

            }
            
            return filteredList;
        }

        

    }
}