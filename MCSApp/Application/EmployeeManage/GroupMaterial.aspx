<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupMaterial.aspx.cs" Inherits="MCSApp.Application.ProductManage.GroupMaterial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>班组物料登记</title>

    <script type="text/javascript">
    
      function pageLoad() {
      }
    var modeltype;
    </script>
        <link href="../../css/style.css" type="text/css" rel="stylesheet">
        <script src="../../scripts/CustomWin.js"></script>
</head>
<body>
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
                            <b>班组物料登记</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
<table width="100%"  class=table90>
                                            <tbody>
                                                <tr>
                                                   <td>                                        
                                                       产品品号<asp:TextBox ID="txtModelID" runat="server" 
                                                           Width="202px" AutoPostBack="True" ontextchanged="txtModelID_TextChanged"></asp:TextBox>
                                                       <asp:Button ID="btnOK" runat="server" BackColor="Transparent" 
                                                           BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                           Text="确定" ValidationGroup="group1" Width="70px" onclick="btnOK_Click" />
                                        </td>
                                                    <td>
                                                        <asp:Panel ID="Panel1" runat="server">
                                                            物料条码<asp:TextBox ID="txtMaterialCode" runat="server" 
                                                                Width="193px" AutoPostBack="True" 
                                                                ontextchanged="txtMaterialCode_TextChanged"></asp:TextBox>
                                                            <asp:Button ID="btnOKforM" runat="server" BackColor="Transparent" 
                                                                BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                                onclick="btnOKforM_Click" Text="确定" ValidationGroup="group1" Width="70px" />
                                                            </asp:Panel>
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
                                    <table border=0 width=100%>                                        
                                    <tr>
                                            <td width="100%" style="width: 100%">
                                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                                                        AllowPaging="False" 
                                                                        PageSize="20" EmptyDataText="没有相关记录！" DataKeyNames="ID"
                                                                        onselectedindexchanged="GridView1_SelectedIndexChanged" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="班组名称" DataField="name" />
                                                                            <asp:BoundField HeaderText="计划单号" DataField="PlanID" />                                                                            
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
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnDelete" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                    BorderWidth="1px" Font-Size="10pt" Height="20px" Text="删除" ValidationGroup="group1"
                                                    Width="70px" onclick="btnDelete_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnSave" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                    BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="btnSave_Click" Text="保存"
                                                    ValidationGroup="group1" Width="70px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" style="width: 100%">
                                                <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="4" ImageSet="Arrows" 
                                                    ShowLines="True">
                                                    <SelectedNodeStyle BackColor="Silver" />
                                                </asp:TreeView>
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
