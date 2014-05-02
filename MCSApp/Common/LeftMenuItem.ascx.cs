using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MCSApp.Common
{
    public partial class LeftMenuItem : System.Web.UI.UserControl
    {
        public string tmpHtml = "";
        public global::System.Web.UI.WebControls.TreeView TreeViewMenu;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)
            //{
                this.witeHtml();
            //}
        }
        public string witeHtml()
        {
            
            tmpHtml += "<TABLE height=\"100%\" width=\"97%\" border=0 cellspacing=0 cellpadding=0 align=\"left\" class=\"menubar\">";
            tmpHtml += "<TR>";
            tmpHtml += "<TD valign=top>";

            for (int j = 0; j < TreeViewMenu.Nodes.Count; j++)
            {
                TreeNode  node = TreeViewMenu.Nodes[j];
                bool hasSubNode = false;

                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].ChildNodes.Count > 0)
                    {
                        for (int k = 0; k < node.ChildNodes[i].ChildNodes.Count;k++ )
                        {
                            if (checkShow(node.ChildNodes[i].ChildNodes[k].Value))
                            {
                                hasSubNode = true; break;
                            }
                        }
                        if(hasSubNode == true) break; 
                    }
                    else
                    {
                        if (checkShow(node.ChildNodes[i].Value))
                        {
                            hasSubNode = true; break;
                        } 
                    }
                }

                if(hasSubNode)
                {
                    tmpHtml += " <div id='bar" + j + "' class='clsPart'>";
                    tmpHtml += "	<TABLE height='25' width='100%' border=0 cellspacing=0 cellpadding=0 align='center'>";
                    tmpHtml += " <TR height='25'>";
                    tmpHtml += " <TD class='bartitle' align='center'>" + node.Text + "</TD>";
                    tmpHtml += " </TR>";
                    tmpHtml += " </TABLE>";
                    tmpHtml += " <div id='bar" + j + "content' class='barcontent'>";
                    //if (node.ChildNodes[i].ChildNodes.Count == 0)
                    tmpHtml += " <TABLE width='100%' border=0 cellspacing=0 cellpadding=0>";

                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes[i].ChildNodes.Count > 0)
                        {
                            bool cntnu = false;
                            for (int k = 0; k < node.ChildNodes[i].ChildNodes.Count; k++)
                            {
                                if (checkShow(node.ChildNodes[i].ChildNodes[k].Value))
                                {
                                    cntnu = true; break;
                                }
                            }
                            if (cntnu == false) continue;
                        }
                        else
                        {
                            if (!checkShow(node.ChildNodes[i].Value))
                            {
                                continue;
                            }
                        }

                        if (node.ChildNodes[i].ChildNodes.Count > 0)
                        {
                            tmpHtml += " <TABLE width='100%' border=0 cellspacing=0 cellpadding=0>";
                            tmpHtml += " <TR class=\"clstr\">";
                            tmpHtml += "     <TD width=\"35\" height=\"30\" bgcolor=\"#D6DFF\" align=\"center\"><IMG SRC=\"../images/Arrow1.gif\" WIDTH=\"20\" HEIGHT=\"20\" BORDER=0 ALT=\"\"></TD>";
							tmpHtml += "     <TD>" + node.ChildNodes[i].Text + "</TD>";
				            tmpHtml += " </TR>";
					        tmpHtml += " <TR class=\"clstrc\">";
						    tmpHtml += "     <TD bgcolor=\"#D6DFFF\">&nbsp;</TD>";
						    tmpHtml += "     <TD class=\"datalist\">";

                            TreeNode sonNode=node.ChildNodes[i];
                            for(int k=0;k<sonNode.ChildNodes.Count;k++)
                            {
							        tmpHtml += "<div class=\"clsdiv\">";
                                    tmpHtml += "    <A HREF='" + sonNode.ChildNodes[k].ImageUrl + "' href='#' onclick='gettabWin()'>" + sonNode.ChildNodes[k].Text + "</A>";
							        tmpHtml += "</div>";                                
                            }
						    tmpHtml += "     </TD>";
					        tmpHtml += " </TR>";
                            tmpHtml += " </TABLE>";
                        }
                        else
                        {
                            tmpHtml += "<TR>";
                            tmpHtml += "<TD width='35' height='30' bgcolor='#D6DFF' align='center'><IMG SRC='../images/Arrow1.gif' WIDTH='20' HEIGHT='20' BORDER=0 ALT=''></TD>";
                            tmpHtml += "<TD><div class='clsdiv'><A HREF='" + node.ChildNodes[i].ImageUrl + "' href='#' onclick='gettabWin()'>" + node.ChildNodes[i].Text + "</A></div></TD>";
                            tmpHtml += "</TR>";
                        }
                    }

                    //if (node.ChildNodes[i].ChildNodes.Count == 0)
                    //tmpHtml += " </TABLE>";
                    tmpHtml += " <TABLE height='100%' width='100%' border=0 cellspacing=0 cellpadding=0>";
                    tmpHtml += " <TR>";
                    tmpHtml += " <TD bgcolor='#D6DFF' width='35'>&nbsp;</TD>";
                    tmpHtml += " <TD>&nbsp;</TD>";
                    tmpHtml += " </TR>";
                    tmpHtml += " </TABLE>";
                    tmpHtml += " </div>";
                    tmpHtml += " </div>";
                }

            }
            tmpHtml += "</TD>";
            tmpHtml += "</TR>";
            tmpHtml += "</TABLE>";
            return tmpHtml;
            //this.Response.Write(tmpHtml);

        }

        private bool checkShow(string rigntStr)
        {
            foreach (string str in SessionUser.Roles)
            {
                if (str == rigntStr) return true;
            }
            return false;
        }
        
    }
}