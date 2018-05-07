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
        //define the page/record count to be displayed per page
        private int pageSize = System.Convert.ToInt32(Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Person.PageSize"));

        /// <summary>
        /// Method to get person/contacts from Sitecore using the index, based on email and phone
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="page"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public PersonSearchListing GetPersons(string email, string phone, int page, string dbname)
        {
            //Get index context
            string indexName = String.Format(Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Person.IndexName"), dbname);
            var index = ContentSearchManager.GetIndex(indexName);

            List<Sitecore.Feature.Persons.Models.Person> filteredList = new List<Sitecore.Feature.Persons.Models.Person>();
            PersonSearchListing pl = new PersonSearchListing();

            //Populate the search result return view model with the requisite properties
            pl.PageSize = pageSize;
            pl.Email = email;
            pl.Phone = phone;
            pl.PageNumber = page;

            //build predicate for filters - build initial perdicate, because we want a 'and' clause between email and phone
            var filterPredicate = PredicateBuilder.True<PersonSearchResultItem>();

            //TODO: this predicate build can also be made dynamic
            //Instead of passing email and phone, we can pass a a key/value pair of all the fields to search, and build the predicate from that
            //This way the predicates don't have to hard coded.
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

                    //calculate take/skip numbers based on page number
                    int skip = pageSize * (page == 1 ? 0 : page - 1);
                    int take = ((result.Hits.Count() - skip) >= pageSize ? pageSize : (result.Hits.Count() - skip));

                    pl.StartRecord = skip + 1;
                    pl.EndRecord = (skip + pageSize) >= result.Hits.Count() ? result.Hits.Count() : (skip + pageSize);

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