using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Forms.Models
{
    public class SimpleText : IRenderingModel
    {

        Item _item;

        public void Initialize(Mvc.Presentation.Rendering rendering)
        {
            _item = rendering.Item;
        }

        public string TextBody
        {
            get
            {
                return FieldRenderer.Render(_item, "Text Body");
            }
        }

        
    }
}