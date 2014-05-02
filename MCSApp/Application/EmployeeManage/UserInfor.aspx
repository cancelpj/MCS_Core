<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfor.aspx.cs" Inherits="MCSApp.EmployeeManage.UserInfor" %>

<html>
<head runat="server">
    <title>用户列表</title>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body >
    <form id="Form1" runat="server">
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
                            <b>员工档案管理</b>
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
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                </td>
                                                <td  align=right>
                                                    <asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                        BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="lbtNew_Click" Text="添加员工"
                                                        ValidationGroup="group1" />
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
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                    ShowMessageBox="False" Font-Size="10pt" />
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!" Visible="False"
                    Font-Size="10pt"></asp:Label>
                <asp:Label ID="lblAccMsg" runat="server" ForeColor="Red" Text="工号已经存在，请更换后再提交！"
                    Visible="False" Font-Size="10pt"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                    OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" 
                    OnRowDataBound="GridView1_RowDataBound" DataKeyNames="state" 
                       AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" 
                       PageSize="15" EmptyDataText="没有相关记录！" >
                    <Columns>
                        <asp:TemplateField HeaderText="工号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtid" runat="server" Text='<%# Bind("ID") %>' Width="100px" 
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorid" runat="server" ControlToValidate="txtid"
                                    ErrorMessage="工号不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="员工名称">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="员工名称不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="隐列" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblp" runat="server" Text='<%# Bind("state") %>' Width="1px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="帐号状态">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlState" runat="server" Width="150px">
                                    <asp:ListItem Value="1">启用</asp:ListItem>
                                    <asp:ListItem Value="2">停用</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlState" runat="server" Width="150px" Enabled="false">
                                    <asp:ListItem Value="1">启用</asp:ListItem>
                                    <asp:ListItem Value="2">停用</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtremark" runat="server" MaxLength="50" Text='<%# Bind("remark") %>'
                                    Width="300px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("remark") %>' Width="300px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" ShowHeader="False" ItemStyle-Wrap=false>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/update.gif" ToolTip="更新" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                        runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.gif"
                                        ToolTip="取消" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.gif" ToolTip="编辑" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ImageButton2"
                                        runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif"
                                        ToolTip="删除" Visible="false" />
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
    </form>
</body>
</html>
