﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupPage.aspx.cs" Inherits="MCSApp.Application.EmployeeManage.GroupPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>班组档案</title>
    <script type="text/javascript">
     
      function pageLoad() {
      }
    
    </script>
    <script src="../../scripts/CustomWin.js" language=javascript></script>
<link href="../../css/style.css" type="text/css" rel="stylesheet">

</head>
<body >
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
                            <b>班组档案</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                                        <table width=100% class="table90">
                                            <tr>
                                                <td >
                                                    <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="rdioBugID" runat="server"
                                                        Text="班组名称" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="rdioBug" runat="server" Text="班组长" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1" CssClass="input"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                        OnClick="btnquery_Click" Text="查询" ValidationGroup="group1" Width="70px" />
                                                    
                                                    </td>
                                                <td  align=right>
                                                    <asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                        BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="lbtNew_Click" Text="添加班组"
                                                        ValidationGroup="group1" />
                                                </td>
                                            </tr>
                                        </table>                        
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                                            ShowMessageBox="True" Font-Size="10pt" />
                                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!" Visible="False"
                                            Font-Size="10pt"></asp:Label>
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                                Width="100%" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                                OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" 
                                                OnRowDataBound="GridView1_RowDataBound" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                PageSize="15" DataKeyNames="id" EmptyDataText="没有相关记录！">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="班组名称" >
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' Width="100"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                ControlToValidate="txtName" Display="None" ErrorMessage="班组名称不能为空！" 
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="班组长工号">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblLeaderID" runat="server" Text='<%# Bind("LeaderID") %>' Width="100px" Visible=false></asp:Label>
                                                            <asp:TextBox ID="txtLeaderID" runat="server" Text='<%# Bind("LeaderID") %>' Width="100px"
                                                                MaxLength="50"></asp:TextBox>
                                                            <A href="javascript:var SelcontrolName=null;SelcontrolName=document.getElementById('<%=clid %>') ;x_open('班组长列表', 'EmployeeForQuery.aspx',800,600); "><img style="border:0" src="../../images/search.gif" style="cursor:hand" /></A> 
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLeaderID"
                                                                ErrorMessage="班组长不能为空!" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("LeaderID") %>' Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" />                                                        
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="班组排工" >
                                                        <EditItemTemplate>
                                                            <asp:Label ID="txtWorkDispatch" runat="server" Text='<%# Bind("WorkDispatch") %>' Width="100"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWorkDispatch" runat="server" Text='<%# Bind("WorkDispatch") %>' Width="100"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>                                                    
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
                                                                    ToolTip="删除" />
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
