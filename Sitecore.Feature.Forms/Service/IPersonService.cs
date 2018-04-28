using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace Sitecore.Feature.Forms.Service
{
    interface IPersonService
    {
        ID AddPerson(string firstname, string lastname, string email, string phone, string address, string city, string state, string zip);

    }
}
