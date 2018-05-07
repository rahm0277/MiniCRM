using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Foundation.Services.Service
{
    /// <summary>
    /// Interface for SC Item Service 
    /// </summary>
    public interface ISSCItemService
    {
        string Login(string domain, string username, string password);
        HttpResponseMessage CallItemServiceAPI(string url, string jsonContent, string contentType, string method, string domain, string username, string password);

    }
}
