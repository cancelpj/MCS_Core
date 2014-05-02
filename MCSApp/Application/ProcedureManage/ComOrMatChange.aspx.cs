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
using System.Text;
using System.Data.SqlClient;
using MCSApp.DataAccess;
using MCSApp.DataAccess.LogManage;
using MCSApp.Common;

namespace MCSApp.Application.ProcedureManage
{
    public partial class ComOrMatChange : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "维修员" };
            string outStr;
            //
            this.btnSaveRcd.Attributes.Add("onclick","return confirm('你确定要将记录保存到数据库吗?')");

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                DataTable tmpdt = new DataTable();
                //tmpdt.Columns.Add("cmark");
                //tmpdt.Columns.Add("cid");
                tmpdt.Columns.Add("dnid");//换下ID
                tmpdt.Columns.Add("upid");//换上ID

                tmpdt.AcceptChanges();

                ViewState["dt"] = tmpdt ;
                ViewState["productID"] = "";
                this.GridBind();
            }
        }

        private void GridBind()
        {
            this.GridView1.DataSource=(DataTable)ViewState["dt"];
            this.GridView1.DataBind();
        }

        protected void btnSaveRcd_Click(object sender, EventArgs e)
        {
            try 
            {                
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                for (int i = 0; i < ((DataTable)ViewState["dt"]).Rows.Count;i++ )
                {
                    if(((DataTable)ViewState["dt"]).Rows[i]["upid"].ToString()==""||
                        ((DataTable)ViewState["dt"]).Rows[i]["dnid"].ToString()==""
                        )
                    {
                        Methods.AjaxMessageBox(this, "换上或换下的序列号不能为空！"); return;
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@p1", ViewState["productID"].ToString());
                    cmd.Parameters.AddWithValue("@p2",((DataTable)ViewState["dt"]).Rows[i]["upid"].ToString());
                    cmd.Parameters.AddWithValue("@p3", ((DataTable)ViewState["dt"]).Rows[i]["dnid"].ToString());

                    cmd.CommandText = "update ta_relationship set itemid=@p2 where id=@p1 and itemid=@p3 ";

                    sqlaccess.ExecuteQuerry(cmd);
                }
                sqlaccess.Commit();
                this.txtDownID.Text = this.txtUPID.Text = this.txtProductID.Text = "";
                ((DataTable)ViewState["dt"]).Clear();//清空表中的数据
                this.GridView1.DataSource = (DataTable)ViewState["dt"];
                this.GridView1.DataBind();

            }
            catch(Exception ex)
            {
                sqlaccess.Rollback();
                Methods.AjaxMessageBox(this,"保存的过程中出现异常!");
                LogManager.Write(this,ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }


        protected void btnAddRecord_Click(object sender, EventArgs e)
        {

        }

        protected void txtProductID_TextChanged(object sender, EventArgs e)
        {
                try
                {
                    DataTable dt = (DataTable)ViewState["dt"];
                    sqlaccess.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT ID FROM TA_Relationship where id ='"+this.txtProductID.Text.Trim()+"'");

                    ds = sqlaccess.OpenQuerry(sb.ToString());
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["productID"] = this.txtProductID.Text.Trim();
                        this.GridView1.DataSource = null;
                        this.GridView1.DataBind();
                        this.txtDownID.Focus();//产品添加成功后 设置换下的部件或物料输入框的焦点
                    }
                    else
                    {
                        Methods.AjaxMessageBox(this,"产品档案表中不存在此记录！");
                        this.txtProductID.Focus();//失败后 焦点不转移
                        this.GridView1.DataSource = null;
                        this.GridView1.DataBind();                        
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Write(this, ex.Message);
                }
                finally
                {
                    sqlaccess.Close();
                }

        }

        protected void UPOrDownID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(this.txtDownID.Text.Trim()==""||this.txtUPID.Text.Trim()=="")// if up or down id is empty then return 
                {
                    if (this.txtDownID.Text.Trim() == "") this.txtDownID.Focus();
                    if (this.txtUPID.Text.Trim() == "") this.txtUPID.Focus();
                    return;
                }
                DataTable dt = (DataTable)ViewState["dt"];
                //if(dt.Rows.Count>0)
                //{
                //    for (int i = 0; i < dt.Rows.Count;i++ )
                //    {
                //        if(this.txtUPID.Text==dt.Rows[i]["upid"].ToString()||this.txtDownID.Text.Trim()==dt.Rows[i]["dnid"].ToString())
                //        {
                //            Methods.AjaxMessageBox(this,"此序列号已在更换列表中！"); return;
                //        }
                //    }
                //}

                if (this.txtUPID.Text.Trim() == this.txtDownID.Text.Trim())
                {
                    Methods.AjaxMessageBox(this, "换下与换上的序列号相同！"); return;
                }

                string tmpsql = "select itemid from ta_relationship where id='" + ViewState["productID"].ToString() + "' and itemid='" + this.txtDownID.Text.Trim() + "'";
                DataSet tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    Methods.AjaxMessageBox(this, "此序列号不属于当前产品"); return;
                }

                int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
                if (this.txtUPID.Text.Trim().Length <= Len || this.txtDownID.Text.Trim().Length <= Len)
                {
                    Methods.AjaxMessageBox(this, "输入的序列号不合法");
                    return;
                }

                string mid1 = this.txtUPID.Text.Trim().Substring(0, Len);
                string mid2 = this.txtDownID.Text.Trim().Substring(0, Len);
                if (mid1 != mid2)
                {
                    Methods.AjaxMessageBox(this, "换下与换上的品号不一致！"); 
                    return;
                }

                //tmpsql = "select modelid from ta_product where id='" + this.txtUPID.Text.Trim() + "' and modelid=(select modelid from ta_product where id='" + this.txtDownID.Text.Trim() + "')";
                //tmpds = sqlaccess.OpenQuerry(tmpsql);
                //if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                //{

                //}
                //else
                //{
                //    tmpsql = "select modelid from TA_Materiel where id='" + this.txtUPID.Text.Trim() + "' and modelid=(select modelid from TA_Materiel where id='" + this.txtDownID.Text.Trim() + "')";
                //    tmpds = sqlaccess.OpenQuerry(tmpsql);
                //    if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                //    {

                //    }
                //    else
                //    {
                //        Methods.AjaxMessageBox(this, "换下与换上的品号不一致！"); return;
                //    }

                //}

                dt.Clear();
                DataRow r = dt.NewRow();
                r["upid"] = this.txtUPID.Text.Trim();
                r["dnid"] = this.txtDownID.Text.Trim();
                dt.Rows.Add(r);
                dt.AcceptChanges();
                ViewState["dt"] = dt;
                this.GridBind();
                this.btnSaveRcd.Focus();//成功后 保存焦点

            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                this.txtDownID.Focus();//失败后后不转移焦点
                Methods.AjaxMessageBox(this, "在查询的过程中出现异常!");
            }
            finally
            {
                sqlaccess.Close();
            }

        }

    }
}
