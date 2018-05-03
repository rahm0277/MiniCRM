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
        private int pageSize = 4;

        public PersonSearchListing GetPersons(string email, string phone, int page)
        {
            var index = ContentSearchManager.GetIndex("minicrm_person_master_index");

            List<Sitecore.Feature.Persons.Models.Person> filteredList = new List<Sitecore.Feature.Persons.Models.Person>();
            PersonSearchListing pl = new PersonSearchListing();

            pl.PageSize = pageSize;
            pl.Email = email;
            pl.Phone = phone;
            pl.PageNumber = page;

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

                if (result.Hits.Any())
                {
                    foreach (var searchResult in resultItems)
                    {
                        SearchResultItem gsi = searchResult.Document;
                        Sitecore.Feature.Persons.Models.Person p = new Sitecore.Feature.Persons.Models.Person(gsi.GetItem());
                        filteredList.Add(p);
                    }

                    int skip = pageSize * (page == 1 ? 0 : page - 1);
                    int take = ((result.Hits.Count() - skip) >= pageSize ? pageSize : (result.Hits.Count() - skip));

                    filteredList = filteredList.Skip(skip).Take(pageSize).ToList<Sitecore.Feature.Persons.Models.Person>();
                    pl.ListingResults = filteredList;
                    pl.TotalResults = result.Hits.Count();
                }
                else
                {
                    pl.TotalResults = 0;
                }
            }
            
            return pl;
        }

        

    }
}