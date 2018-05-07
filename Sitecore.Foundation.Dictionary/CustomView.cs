using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Foundation.BaseView
{
    /// <summary>
    /// Base class that all views will inherit automatically. Set in the web.config in the Views folder
    /// To use in other projects, add a reference to this project in the other project, and then set the baseview setting in web.config to this class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CustomView<T> : WebViewPage<T>
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