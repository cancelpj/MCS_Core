<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelList.aspx.cs" Inherits="MCSApp.Application.ProductManage.ModelList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>品号管理</title>

    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
        <link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body class="mainBody">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
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
                            <b>品号管理</b>
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
                                                                ID="RadioButton2" runat="server" Text="名称" GroupName="group1" Font-Size="10pt">
                                                        </asp:RadioButton><asp:RadioButton
                                                                ID="RadioButton3" runat="server" Text="代号" GroupName="group1" Font-Size="10pt">
                                                        </asp:RadioButton>&nbsp;
                                                        <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1" CssClass="input"
                                                            Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                        <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                            Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                            BackColor="Transparent" Width="70px"></asp:Button>
                                                    </td>
                                                    <td  align=right>
                                                        &nbsp;<asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="lbtNew_Click" Text="添加品号"
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
                                        <asp:Label ID="lblAccMsg" runat="server" ForeColor="Red" Text="品号或代号已经存在，请更换后再提交！"
                                            Visible="False" Font-Size="10pt"></asp:Label>
                                        <table width="100%" border="1" id="TABLE1" runat="server">
                                        </table>
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                            OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                            OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" 
                                            OnRowDataBound="GridView1_RowDataBound" DataKeyNames="ModelType" 
                                            CellPadding="1" AllowPaging="True" 
                                            onpageindexchanging="GridView1_PageIndexChanging1" PageSize="20" EmptyDataText="没有相关记录！" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="品号">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtid" runat="server" Text='<%# Bind("ID") %>' Width="100px" 
                                                            MaxLength="50"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorid" runat="server" ControlToValidate="txtid"
                                                            ErrorMessage="品号不能为空!" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="名称">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"
                                                            Width="100px"></asp:TextBox>&nbsp;
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                            ErrorMessage="名称不能为空!" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="代号">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtcode" runat="server" MaxLength="50" Text='<%# Bind("code") %>'
                                                            Width="100px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcode" runat="server" Text='<%# Bind("code") %>' Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="隐列" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblp" runat="server" Text='<%# Bind("ModelType") %>' Width="1px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品号类型">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlModelType" runat="server" Width="150px">
                                                            <asp:ListItem Value="1">产品</asp:ListItem>
                                                            <asp:ListItem Value="2">部件</asp:ListItem>
                                                            <asp:ListItem Value="3">物料</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlModelType" runat="server" Width="150px" Enabled="false">
                                                            <asp:ListItem Value="1">产品</asp:ListItem>
                                                            <asp:ListItem Value="2">部件</asp:ListItem>
                                                            <asp:ListItem Value="3">物料</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="客户品号">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCustomerID" runat="server" MaxLength="50" Text='<%# Bind("CustomerID") %>'
                                                            Width="100px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Width="100px"></asp:Label>
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
                                                            ImageUrl="~/images/edit.gif" ToolTip="编辑" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                                                runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif"
                                                                ToolTip="删除" Visible="true" />
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
