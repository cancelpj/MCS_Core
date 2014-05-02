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
    public partial class RepairRecord : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "维修员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                //Methods.DDLBind(this.ddltype, "select id value,Bug text  from ta_bugType");
                this.GridBind("");
            }
        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            string tmpPid = this.txtID.Text.Trim();
            if (this.RadioButton2.Checked)
            {
                tmpPid = Methods.getProductIDFromPSN(this.txtID.Text.Trim());//由SN找产品序列号 
            }

            if (!Methods.HasExceptonInHistory(tmpPid))
            {
                Methods.AjaxMessageBox(this, "此产品没有异常记录！"); return;
            }
            this.ctrlEnable(true);

            string tmpModelID = Methods.getModelIDByProductID(tmpPid);
            int tmpProcedureID = Methods.getProcedureIDByProductID(tmpPid);

            ViewState["tmpPid"] = tmpPid;//本页全局产品ID
            ViewState["tmpModelID"] = tmpModelID;//本页全局品号ID
            ViewState["tmpProcedureID"] = tmpProcedureID;//本页全局工艺流程ID

            //Methods.DDLBugPointBind(this.ddlPoint, tmpModelID);
            //这里添加设置的发现异常的部分实现 

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@p1", this.txtID.Text.Trim());
            cmd.CommandText = " select top 1 t.productid,t.process,t.result, t.employeeid,te.name,t.exception,t.begintime from tb_procedureHistory t,ta_employee te where t.employeeid=te.id and t.productid = @p1 order by t.begintime desc";
            DataSet tmpds = sqlaccess.OpenQuerry(cmd);
            if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
            {
                DataRow r = tmpds.Tables[0].Rows[0];
                int result = r.IsNull("result") ? 0 : (int)r["result"];

                if (result == 1)
                {
                    this.lblEProcess.Text = tmpds.Tables[0].Rows[0]["process"].ToString();
                    this.lblFinder.Text = tmpds.Tables[0].Rows[0]["name"].ToString();
                    this.lblFinderID.Text = tmpds.Tables[0].Rows[0]["employeeid"].ToString();
                    this.lblFindTime.Text = tmpds.Tables[0].Rows[0]["begintime"].ToString();
                    this.lblException.Text = tmpds.Tables[0].Rows[0]["exception"].ToString();
                }
                else
                {
                    Methods.AjaxMessageBox(this, "此产品没有异常记录!"); return;
                }

            }
            else
            {
                Methods.AjaxMessageBox(this, "此产品没有异常记录!"); return;
            }
            this.ddlReInP.Items.Clear();//将原有的记录清除掉
            Methods.DDLLastProcess(this.ddlReInP, tmpProcedureID, this.lblEProcess.Text.Trim());

            this.GridBind(tmpPid);

        }

        private void ctrlEnable(bool enable)
        {
            this.btnSave.Enabled = enable;
            this.txtBugPoint.Enabled = enable;
            this.txtBugType.Enabled = enable;
            //this.ddlPoint.Enabled = enable;
            this.ddlReInP.Enabled = enable;
            //this.ddltype.Enabled = enable;
        }


        protected void GridBind(string ProductID)
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "  SELECT t.ProductID, t.DetectProcess, t.DetectEmployeeID AS dEmpID,"
                                + "  tdu.Name AS dEmpName, t.DetectTime AS FindTime, t.Exception,t.RepairEmployeeID,"
                                + "  tru.Name AS rEmpName, tt.Bug, t.BugPointCode, t.RepairInfo"
                                + "  FROM TB_RepairRecord AS t INNER JOIN"
                                + "  TA_Employee AS tdu ON t.DetectEmployeeID = tdu.ID INNER JOIN"
                                + "  TA_Employee AS tru ON t.RepairEmployeeID = tru.ID INNER JOIN"
                                + "  TA_BugType AS tt ON t.BugID = tt.ID INNER JOIN"
                                + "  (SELECT ID, ModelID"
                                + "  FROM TA_Product"
                                + "  WHERE (ID = '" + ProductID + "')) AS tm ON t.ProductID = tm.ID "
                                + "  WHERE (t.ProductID = '" + ProductID + "')"
                                + "  ORDER BY t.DetectTime DESC";

                ds = sqlaccess.OpenQuerry(tmpsql);
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["tmpPid"] == null || ViewState["tmpPid"].ToString() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "无产品序列号，不能完成操作!"); return;

                }
                if (this.lblFindTime.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "无发现时间，不能完成操作!"); return;
                }
                if (this.txtBugType.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "请输入缺陷类别!"); return;
                }
                if (this.txtBugPoint.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "请输入缺陷定位点品号!"); return;
                }
                if (this.txtBugPosition.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "请输入缺陷定位点位号!"); return;
                }
                if (ddlReInP == null || ddlReInP.Items.Count == 0)
                {
                    Methods.AjaxMessageBox(this, "无重入工序，无法完成操作!"); return;
                }
                if (txtRPInfo.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "请输入修理信息!"); return;
                }

                sqlaccess.Open();

                sqlaccess.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@p1", ViewState["tmpPid"].ToString());
                cmd.Parameters.AddWithValue("@p2", this.lblFindTime.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", this.lblEProcess.Text.Trim());
                cmd.Parameters.AddWithValue("@p4", this.lblFinderID.Text.Trim());
                cmd.Parameters.AddWithValue("@p5", this.lblException.Text.Trim());
                cmd.Parameters.AddWithValue("@p6", this.txtBugType.Text.Trim());
                cmd.Parameters.AddWithValue("@p7", this.txtBugPoint.Text.Trim() + "[" + this.txtBugPosition.Text.Trim().ToUpper() + "]");
                cmd.Parameters.AddWithValue("@p8", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@p9", SessionUser.ID);
                cmd.Parameters.AddWithValue("@p10", this.txtRPInfo.Text.Trim());


                cmd.CommandText = "INSERT INTO TB_RepairRecord"
                                + " (ProductID, DetectTime, DetectProcess, DetectEmployeeID, Exception, BugID,"
                                + " BugPointCode, RepairTime, RepairEmployeeID, RepairInfo) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)";

                sqlaccess.ExecuteQuerry(cmd);

                //在流程历史表中记录维修工序信息
                cmd.CommandText = "INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, " +
                             "Result, [Exception], Data, DataID, Dispatch, BeginTime, EndTime) " +
                             "VALUES (@ProductID, @Process, @EmployeeID, @Result, @Exception, " +
                             "@Data, @DataID, @Dispatch, @BeginTime, @EndTime)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@ProductID"].Value = ViewState["tmpPid"].ToString();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@Process"].Value = "返修";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@EmployeeID"].Value = SessionUser.ID;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Result", System.Data.SqlDbType.Int, 50));
                cmd.Parameters["@Result"].Value = 0;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Exception", System.Data.SqlDbType.VarChar, 2000));
                cmd.Parameters["@Exception"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Data", System.Data.SqlDbType.Text));
                cmd.Parameters["@Data"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DataID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@DataID"].Value = Guid.NewGuid();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Dispatch", System.Data.SqlDbType.VarChar, 2000));
                cmd.Parameters["@Dispatch"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginTime", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@BeginTime"].Value = DateTime.Now;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndTime", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@EndTime"].Value = DateTime.Now;

                sqlaccess.ExecuteQuerry(cmd);

                //sqlaccess.ExecuteQuerry(" update tb_ProcedureHistory set result=0 where ");//这里有？ 更新到维修记录TB_RepairRecord表中，历史记录 中的result字段如何更新 置为0 还是保持原来的1
                DropDownList ddlDel = new DropDownList();

                Methods.findNextProcedure(this.ddlReInP.SelectedItem.Text, (int)ViewState["tmpProcedureID"], ddlDel);

                for (int i = 0; i < ddlDel.Items.Count; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@p1", ViewState["tmpPid"].ToString());
                    cmd.Parameters.AddWithValue("@p2", ddlDel.Items[i].Text);

                    cmd.CommandText = "delete TB_ProcedureState where productid=@p1 and process=@p2";
                    sqlaccess.ExecuteQuerry(cmd);
                }

                sqlaccess.Commit();

                this.GridBind(ViewState["tmpPid"].ToString());
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                Methods.AjaxMessageBox(this, "已存在相同记录！"); return;
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        protected void txtBugType_TextChanged(object sender, EventArgs e)
        {
            string tmpbt = Methods.getBugNameByID(this.txtBugType.Text.Trim());
            if ("" == tmpbt)
            {
                Methods.AjaxMessageBox(this, "缺陷类型不存在");
                this.ctrlEnable(false);
                this.txtBugType.Enabled = true;
            }
            else
            {
                this.lblBugType.Text = tmpbt;
                this.ctrlEnable(true);
                this.txtBugPoint.Focus();
            }
        }

        protected void txtBugPoint_TextChanged(object sender, EventArgs e)
        {
            string tmpbpoint = Methods.getItem(ViewState["tmpModelID"].ToString(), this.txtBugPoint.Text.Trim());//由品号ID与定位点ID找到描述信息
            if ("" == tmpbpoint)
            {
                Methods.AjaxMessageBox(this, "缺陷定位品号不存在");
                this.ctrlEnable(false);
                this.txtBugPoint.Enabled = true;
            }
            else
            {
                this.lblBugPoint.Text = tmpbpoint;
                this.ctrlEnable(true);
                this.txtBugPosition.Focus();
            }
        }
    }
}
