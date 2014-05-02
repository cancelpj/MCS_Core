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
using MCSApp.Common;

namespace MCSApp.Main
{
    public partial class left : PageBase
    {
        public string tmpHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool flag = ConfigurationManager.AppSettings["FunctionFlag"].ToString() == "true" ? true : false;

                TreeView itemView = new TreeView();
                TreeNode node = new TreeNode("计划");
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;制定生产计划单", "计划员", "../Application/PlanManage/MakePlan.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;查看生产计划单", "生产主管", "../Application/PlanManage/ActiveOrClosePlan.aspx"));
                itemView.Nodes.Add(node);

                node = new TreeNode("生产");
                if (flag)
                {
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;建立产品档案", "作业员", "../Application/ProductManage/ProductArchives.aspx"));
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;建立物料档案", "物料管理员", "../Application/ProductManage/MatierialArchives.aspx"));
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;建立部件档案", "作业员", "../Application/ProductManage/ComponentArchives.aspx"));
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;关联产品、部件、物料", "作业员", "../Application/ProductManage/BindProduct.aspx"));
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;输入产品条码", "作业员", "../Application/ProductManage/InputProductSN.aspx"));
                    node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;条码扫描", "作业员", "../Application/ProductManage/ProcedureScan.aspx"));
                }
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;返修作业", "维修员", "../Application/ProcedureManage/RepairRecord.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;部件或物料更换", "维修员", "../Application/ProcedureManage/ComOrMatChange.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;腔体拆空", "维修员", "../Application/ProductManage/ProductToEmpty.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;批准复检", "班组长", "../Application/ProcedureManage/PermitRetesting.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;客退产品登记", "作业员", "../Application/ProcedureManage/CustomerReturn.aspx"));
                itemView.Nodes.Add(node);

                node = new TreeNode("生产管理");
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;品号管理", "制造工程师", "../Application/ProductManage/ModelList.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;产品版本升级", "制造工程师", "../Application/ProductManage/ProductVersionUpdate.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;产品结构定义", "制造工程师", "../Application/ProductManage/productStructure.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;工艺流程定义", "制造工程师", "../Application/ProcedureManage/ProcedureFlow/ProcedureList.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;班组排工", "班组长", "../Application/EmployeeManage/GroupDispatch.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;管理班组", "班组长", "../Application/EmployeeManage/ManageGroup.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;班组物料登记", "班组长", "../Application/EmployeeManage/GroupMaterial.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;缺陷类别定义", "制造工程师", "../Application/SystemManage/BugType.aspx"));
                //node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;缺陷定位点定义", "制造工程师", "../Application/SystemManage/BugPoint.aspx"));
                itemView.Nodes.Add(node);

                node = new TreeNode("系统管理");
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;员工档案管理", "人事管理员", "../Application/EmployeeManage/UserInfor.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;员工权限分配", "系统管理员", "../Application/EmployeeManage/RoleDispatch.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;班组档案", "系统管理员", "../Application/EmployeeManage/GroupPage.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;软件版本管理", "系统管理员", "../Application/SystemManage/SoftwareVersion.aspx"));
                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;查看事件日志", "系统管理员", "../Application/SystemManage/EventPage.aspx"));
                itemView.Nodes.Add(node);

                node = new TreeNode("数据查询统计及报表");

                node.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;生产看板", "一级数据查看者", "../Application/QueryManage/ProduceBoard.aspx"));

                TreeNode sonNode = new TreeNode("&nbsp;&nbsp;统计");
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;统计生产完成情况", "一级数据查看者", "../Application/QueryManage/ProduceFinished.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;统计生产效率", "一级数据查看者", "../Application/QueryManage/ProduceEfficiency.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;统计产品检验质量", "一级数据查看者", "../Application/QueryManage/ProductQuality.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;缺陷分类统计", "二级数据查看者", "../Application/QueryManage/BugTypeStatistics.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;缺陷定位统计", "二级数据查看者", "../Application/QueryManage/BugPointStatistics.aspx"));
                node.ChildNodes.Add(sonNode);

                sonNode = new TreeNode("&nbsp;&nbsp;查询");

                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;查询维修记录", "二级数据查看者", "../Application/QueryManage/RepairRecordInfo.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;查询单台产品生产中的详情", "一级数据查看者", "../Application/QueryManage/SingleProductInfo.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;查询产品在各工序中的数据", "二级数据查看者", "../Application/QueryManage/ProcedureData.aspx"));
                node.ChildNodes.Add(sonNode);

                sonNode = new TreeNode("&nbsp;&nbsp;追溯");

                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;产品追溯查询", "一级数据查看者", "../Application/ProductManage/ProductTrace.aspx"));
                sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;流程追溯查询", "一级数据查看者", "../Application/ProcedureManage/ProcedureTrace.aspx"));
                node.ChildNodes.Add(sonNode);

                if (flag)
                {
                    sonNode = new TreeNode("&nbsp;&nbsp;报表");
                    sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;CPK报表", "二级数据查看者", "Working.htm"));
                    sonNode.ChildNodes.Add(new TreeNode("&nbsp;&nbsp;SPC报表", "二级数据查看者", "Working.htm"));
                    node.ChildNodes.Add(sonNode);
                }

                itemView.Nodes.Add(node);

                this.LeftMenuItem1.TreeViewMenu = itemView;
            }
        }
    }
}
