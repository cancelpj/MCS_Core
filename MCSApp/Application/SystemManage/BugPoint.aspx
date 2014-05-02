<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BugPoint.aspx.cs" Inherits="MCSApp.Application.SystemManage.BugPoint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>缺陷类型定义</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
<link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
                    <table height="40%" cellspacing="0" cellpadding="0" width="80%" align="center" border="0">
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
                            <b>品号列表</b>
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
                                                            Text="品号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                                ID="RadioButton2" runat="server" Text="缺陷定位点代码" GroupName="group1" 
                                                            Font-Size="10pt">
                                                        </asp:RadioButton>&nbsp;
                                                        <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1" CssClass="input"
                                                            Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                        <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                            Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                            BackColor="Transparent" Width="70px"></asp:Button>
                                                    </td>
                                                    <td  align=right>
                                                        &nbsp;<asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="lbtNew_Click" Text="新建缺陷定位点"
                                                            ValidationGroup="group1" />
                                                    </td>
                                                </tr>
                                                </tbody></table>

                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!" Visible="False"
                                            Font-Size="10pt"></asp:Label>
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                                        DisplayMode="List" />
                                                                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                                                                        AutoGenerateColumns="False"  DataKeyNames="BugPointCode" 
                                                                        OnPageIndexChanging="GridView2_PageIndexChanging" 
                                                                        OnRowCancelingEdit="GridView2_RowCancelingEdit" 
                                                                        OnRowDataBound="GridView2_RowDataBound" OnRowDeleting="GridView2_RowDeleting" 
                                                                        OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" 
                                                                        PageSize="20"
                                                                        EmptyDataText="没有相关记录！" Width=100%>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="品号">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtModelID" runat="server" 
                                                                                        Text='<%# Bind("ModelID") %>' Width=100px MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                                        ControlToValidate="txtModelID" Display="None" ErrorMessage="品号不能为空！"></asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblModelID" runat="server" 
                                                                                        Text='<%# Bind("ModelID") %>' Width=100px></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="缺陷定位点代码">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtBugPointCode" runat="server" 
                                                                                        Text='<%# Bind("BugPointCode") %>' Width=100px></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                                        ControlToValidate="txtBugPointCode" Display="None" ErrorMessage="缺陷定位点代码不能为空！"></asp:RequiredFieldValidator>
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBugPointCode" runat="server" 
                                                                                        Text='<%# Bind("BugPointCode") %>' Width=100px></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="缺陷定位点描述">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtBugPointDsc" runat="server" 
                                                                                        Text='<%# Bind("BugPointDsc") %>' Width=200px MaxLength="100"></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBugPointDsc" runat="server" Text='<%# Bind("BugPointDsc") %>' Width=100px></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="操作" ShowHeader="False">
                                                                                <EditItemTemplate>
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" 
                                                                                        CommandName="Update" ImageUrl="~/images/update.gif" ToolTip="更新" />
                                                                                    &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                                                                                        CommandName="Cancel" ImageUrl="~/images/cancel.gif" ToolTip="取消" />
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                                                                        CommandName="Edit" ImageUrl="~/images/edit.gif" ToolTip="编辑" />
                                                                                    &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                                                                                        CommandName="Delete" ImageUrl="~/images/delete.gif" ToolTip="删除" />
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
