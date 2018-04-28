using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Foundation.Services.Service
{
    public interface ISSCItemService
    {
        HttpResponseMessage CallItemServiceAPI(string url, string jsonContent, string contentType, string method, string domain, string username, string password);

    }
}
