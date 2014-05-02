<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleDispatch.aspx.cs" Inherits="MCSApp.Application.EmployeeManage.RoleDispatch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <table class="tableFace" cellspacing="0" cellpadding="0" width="100%" align="center" border="1">
                    <tr class="tableRow">
                        <td align="center" width="20%" height="22">
                            <b>员工权限分配</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=left>
                                                    <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" runat="server"
                                                        Text="工号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="用户名" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton3" runat="server" Text="角色" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
         <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!"
                                                Visible="False" Font-Size="10pt"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            Width="100%" OnRowEditing="GridView1_RowEditing" 
            OnRowCancelingEdit="GridView1_RowCancelingEdit" 
            OnRowUpdating="GridView1_RowUpdating"  
            OnRowDataBound="GridView1_RowDataBound" DataKeyNames="id" 
            onpageindexchanging="GridView1_PageIndexChanging" AllowPaging="True" PageSize="20" EmptyDataText="没有相关记录！" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="工号">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="txtID" runat="server" Text='<%# Bind("ID") %>' Height="16px" 
                                                                Width="100px"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="用户名" >
                                                        <EditItemTemplate>
                                                            <asp:Label ID="txtUserName" runat="server" Text='<%# Bind("Name") %>'   Width="100px"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("Name") %>' Width="100px" ></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="False"  />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="权限组列表">
                                                        <EditItemTemplate>
                                                            <asp:CheckBoxList ID="ckblstRight" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" Font-Size="10pt">
                                                            </asp:CheckBoxList>                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBoxList ID="ckblstRight" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" Font-Strikeout="False" ForeColor="DarkCyan" Font-Size="10pt">
                                                            </asp:CheckBoxList>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="操作" ShowHeader="False">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/update.gif" ToolTip="更新" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                                                    runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.gif"
                                                                    ToolTip="取消" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.gif" ToolTip="编辑" />&nbsp;
                                                            <asp:Label ID="Label1" runat="server" Text="&nbsp;&nbsp;&nbsp;"></asp:Label>
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
