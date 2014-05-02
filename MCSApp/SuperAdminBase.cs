using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MCSApp.Common;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;

namespace MCSApp
{
    public class SuperAdmin : System.Configuration.IConfigurationSectionHandler    
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Hashtable hashtabl = new Hashtable();
            foreach (XmlNode node in section.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    hashtabl.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
            return hashtabl;

        }

    }
}
