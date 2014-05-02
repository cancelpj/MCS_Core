<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageGroup.aspx.cs" Inherits="MCSApp.Application.EmployeeManage.ManageGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>管理班组</title>

    <script type="text/javascript">
    
      function pageLoad() {
      }
      
      function patchAddEmployee()
      {
          x_open('可供选择的班组成员列表', 'patchAddEmployee.aspx',800,600); 
      }

      function AddPlan(GroupID) {
          x_open('计划单列表', '../PlanManage/QueryPlan.aspx?GroupID='+GroupID, 600, 400);
      }
    </script>
<script src="../../scripts/CustomWin.js"></script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
            <table height="40%" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <TR>
            		<TD width="5" height="5"><IMG height="7" alt="" src="../../images/corner_01.gif" width="5"></TD>
					<TD background="../../images/corner_02.gif"><IMG height="7" alt="" src="../../images/corner_02.gif" width="120"></TD>
					<TD width="5" height="5"><IMG height="7" alt="" src="../../images/corner_03.gif" width="5"></TD>
				</TR>
				<TR>
					<TD width="5" background="../../images/corner_04.gif"></TD>
					<TD vAlign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table class="tableFace" cellspacing="0" cellpadding="0" width="100%" align="center"
                                border="1">
                                <tr class="tableRow">
                                    <td align="center" width="20%" height="22">
                                        <b>管理班组</b>
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2">
                                        <table width="100%" class="table90">
                                            <tbody>
                                                <tr>
                                                    <td id=querytd runat=server visible=false align="left" nowrap='nowrap'>
                                                        <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" runat="server"
                                                            Text="班组" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                                ID="RadioButton2" runat="server" Text="计划单号" GroupName="group1" Font-Size="10pt">
                                                            </asp:RadioButton>&nbsp;
                                                        <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1" CssClass="input"
                                                            Width="100px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                        <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                            Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                            BackColor="Transparent" Width="40px"></asp:Button>&nbsp;&nbsp;
                                                    </td>
                                                    <td id=InsertPlan runat=server align="left" nowrap='nowrap'>
                                                        <span style="font-size: 10pt">班组</span>：&nbsp;
                                                        <asp:DropDownList ID="sltBz" runat="server" ValidationGroup="group2" CssClass="input"
                                                            Width="100px" Font-Size="10pt" >
                                                        </asp:DropDownList>&nbsp;
                                                        <span style="font-size: 10pt">计划单号</span>：&nbsp;
                                                        <asp:TextBox ID="txtPlanIDNew" runat="server" ValidationGroup="group2" CssClass="input"
                                                            Width="100px" Font-Size="10pt"></asp:TextBox>
                                                        <a href="javascript:var SelcontrolName=null;SelcontrolName=document.getElementById('txtPlanIDNew') ;x_open('计划单列表', '../PlanManage/QueryPlan.aspx',600,400); ">
                                                            <img style="border: 0" src="../../images/search.gif" style="cursor: hand" /></a>
                                                        <asp:Button ID="btnInsertPlan" OnClick="btnInsertPlan_Click" runat="server" ValidationGroup="group2"
                                                            Text="新增计划" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                            BackColor="Transparent" Width="60px"></asp:Button>&nbsp;&nbsp;
                                                    </td>
                                                    <td align="center" nowrap=nowrap>
                                                        <asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" Text="添加班组成员" ValidationGroup="group1"
                                                            OnClick="btnAdd_Click" Width="100px" />
                                                        &nbsp;<asp:Button ID="btndelete" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="btndelete_Click" Text="删除勾选的班组成员"
                                                            ValidationGroup="group1" Width="120px" />
                                                    </td>
                                                    <td align="right" nowrap=nowrap>
                                                        <asp:Button ID="btnrefresh" runat="server" ValidationGroup="group1"
                                                            Text="刷新" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                            BackColor="Transparent" Width="40px" onclick="btnrefresh_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table width="100%">
                                            <tbody>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!" Visible="False"
                                                                        Font-Size="10pt"></asp:Label>
                                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                                                        BorderWidth="3px" BorderColor="LightBlue" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                                        PageSize="20" EmptyDataText="没有相关记录！" DataKeyNames="ID"
                                                                        OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing"
                                                                        OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting" 
                                                                        onselectedindexchanged="GridView1_SelectedIndexChanged">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="班组">
                                                                                <EditItemTemplate>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="150px"></asp:Label>
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="150px"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="计划单号">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtPlanID" runat="server" Text='<%# Bind("PlanID") %>' Width="150px"></asp:TextBox>
                                                                                    <a href="javascript:var SelcontrolName=null;SelcontrolName=document.getElementById('<%=clid %>') ;x_open('计划单列表', '../PlanManage/QueryPlan.aspx',600,400); ">
                                                                                        <img style="border: 0" src="../../images/search.gif" style="cursor: hand" /></a>
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPlanID" runat="server" Text='<%# Bind("PlanID") %>' Width="150px"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="操作" ShowHeader="False" ItemStyle-Wrap="false">
                                                                                <EditItemTemplate>
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update"
                                                                                        ImageUrl="~/images/update.gif" ToolTip="更新" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                                                                            runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.gif"
                                                                                            ToolTip="取消" />
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit" Visible="false"
                                                                                        ImageUrl="~/images/edit.gif" ToolTip="编辑" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                                                                            runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif"
                                                                                            ToolTip="删除" OnClientClick="return confirm('你确定要终止该计划?');" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                                                        <RowStyle CssClass="GridViewRowStyle" />
                                                                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                                    </asp:GridView>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:GridView ID="GridView2" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                        DataKeyNames="employeeID"  BorderWidth="3px" BorderColor="LightBlue"
                                                                        EmptyDataText="没有相关记录！">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="班组成员">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ckID" runat="server" Text='<%# Bind("Name") %>' Enabled='true' Width='220px' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                                                        <RowStyle CssClass="GridViewRowStyle" />
                                                                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
					</TD>
					<TD width="5" background="../../images/corner_05.gif">
					</TD>
				</TR>
				<TR>
					<TD width="5" height="5"><IMG height="5" alt="" src="../../images/corner_06.gif" width="5"></TD>
					<TD background="../../images/corner_07.gif"><IMG height="5" alt="" src="../../images/corner_07.gif" width="120"></TD>
					<TD width="5" height="5"><IMG height="5" alt="" src="../../images/corner_08.gif" width="5"></TD>
				</TR>
			</TABLE>
    </div>
    </form>
</body>
</html>
