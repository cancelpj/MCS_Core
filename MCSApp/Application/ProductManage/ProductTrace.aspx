<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductTrace.aspx.cs" Inherits="MCSApp.Application.ProductManage.ProductTrace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
   <script src="../../scripts/CustomWin.js"></script>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
      function showDetail(objName,objid)
      {
          x_open(objName+'详细信息', '../ProductManage/ModelDetail.aspx?id='+objid+'&et='+Math.random(),800,400); 
      }
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
                            产品追溯
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
                                        <asp:RadioButton ID="rdoPID" runat="server" Checked="True" 
                                            Font-Size="10pt" GroupName="group1" Text="产品序列号" />
                                        <asp:RadioButton ID="rdoPSN" runat="server" Font-Size="10pt" 
                                            GroupName="group1" Text="产品条码" />
                                            <asp:RadioButton ID="rdoCID" runat="server" Font-Size="10pt" 
                                            GroupName="group1" Text="部件序列号" />
                                            <asp:RadioButton ID="rdoMID" runat="server" Font-Size="10pt" 
                                            GroupName="group1" Text="物料序列号" />
                                            <asp:RadioButton ID="rdoPlanID" runat="server" Font-Size="10pt" 
                                            GroupName="group1" Text="生产计划号" />
                                        &nbsp;
                                        <asp:TextBox ID="txtcondition" runat="server" CssClass="input" Font-Size="10pt" 
                                            ValidationGroup="group1" Width="200px"></asp:TextBox>
                                        &nbsp;
                                        <asp:Button ID="btnquery" runat="server" BackColor="Transparent" 
                                            BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                            OnClick="btnquery_Click" Text="查询" ValidationGroup="group1" Width="70px" />
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
                                            <td width=50%>
                                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" CellPadding="1"
                                                    EmptyDataText="没有相关记录！" onpageindexchanging="GridView1_PageIndexChanging" 
                                                    PageSize="20" Width="100%" AutoGenerateSelectButton="True" 
                                                    onselectedindexchanged="GridView1_SelectedIndexChanged">
                                                    <Columns>                                                        
                                                        <asp:TemplateField HeaderText="产品序列号">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="产品条码">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsn" runat="server" Text='<%# Bind("SN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>                                                        
                                                        <asp:TemplateField HeaderText="品号">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblmodelid" runat="server" Text='<%# Bind("ModelID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="名称">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
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
                                            <td valign="top" align=left width=50% >
                                                <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="4" 
                                                    ImageSet="Arrows" ShowLines="True">
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
