using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Foundation.CustomView
{
    public abstract class BaseView<T> : WebViewPage<T>
    {
        /// <summary>
        /// Property to get the dictionary domain, as set in settings. Also set at the <sites> node level for the site definition
        /// </summary>
        public string DictionaryDomain
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Dictionary.Domain");
            }
        }
    }
}