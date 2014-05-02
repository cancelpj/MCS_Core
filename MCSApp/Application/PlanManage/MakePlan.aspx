<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakePlan.aspx.cs" Inherits="MCSApp.Application.PlanManage.MakePlan" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>制定计划列表</title>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body >
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <ext:ScriptManager ID="ScriptManager2" runat="server">
    </ext:ScriptManager>
    
           <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="IDName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
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
                            <b>制定生产计划</b>
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
                                                    <span style="font-size: 10pt">生产计划号</span><asp:TextBox ID="txtcondition" 
                                                        runat="server" ValidationGroup="group1"
                                                        Width="263px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                </td>
                                                <td  align=right>
                                                    <asp:Button ID="btnAdd" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                        BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="lbtNew_Click" Text="添加计划"
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
                <asp:Label ID="lblAccMsg" runat="server" ForeColor="Red" Text="计划号已经存在，请更换后再提交！"
                    Visible="False" Font-Size="10pt"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                    OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" 
                    OnRowDataBound="GridView1_RowDataBound" DataKeyNames="id" 
                       AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" 
                       PageSize="15" EmptyDataText="没有相关记录！" >
                    <Columns>
                        <asp:TemplateField HeaderText="计划号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtid" runat="server" Text='<%# Bind("ID") %>' Width="100px" 
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorid" runat="server" ControlToValidate="txtid"
                                    ErrorMessage="计划号不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品号">
                            <EditItemTemplate>
                            
                            
                                <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store1" DisplayField="IDName" ValueField="ID"
                                           SelectOnFocus="true" ForceSelection="true" Mode="Local">
                                </ext:ComboBox>
                            
                            
                                <asp:TextBox ID="txtModelID" runat="server" Text='<%# Bind("ModelID") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtModelID"
                                    ErrorMessage="品号名称不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModelID" runat="server" Text='<%# Bind("ModelID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="数量">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOutput" runat="server" Text='<%# Bind("Output") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOutput"
                                    ErrorMessage="数量不能为空!" Display="None"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                    ControlToValidate="txtOutput" Display="None" ErrorMessage="数量应大于0" 
                                    MaximumValue="65535" MinimumValue="1" Type="Integer"></asp:RangeValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOutput" runat="server" Text='<%# Bind("Output") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="客户订单号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrderID" runat="server" Text='<%# Bind("OrderID") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>                         
                        <asp:TemplateField HeaderText="生产计划单类型">
                            <EditItemTemplate>
                                <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("PlanType") %>' Width="100px" Visible=false></asp:Label>                            
                                <asp:DropDownList ID="ddlPlanType" runat="server" Width=50>
                                <asp:ListItem Text="常规" Value=1></asp:ListItem>
                                <asp:ListItem Text="客退" Value=2></asp:ListItem>
                                </asp:DropDownList>         
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("PlanTypestr") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="备注">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtremark" runat="server" MaxLength="100" Text='<%# Bind("remark") %>'
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
                                    ImageUrl="~/images/edit.gif" ToolTip="编辑" />&nbsp;<asp:ImageButton ID="ImageButton2"
                                        runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif"
                                        ToolTip="删除" Visible="true" />
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
