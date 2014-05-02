<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductToEmpty.aspx.cs" Inherits="MCSApp.Application.ProductManage.ProductToEmpty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品拆空</title>
   <script src="../../scripts/CustomWin.js"></script>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
      function showDetail(objName,objid)
      {
          x_open(objName+'详细信息', '../ProductManage/ModelDetail.aspx?id='+objid+'&et='+Math.random(),600,400); 
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
                            腔体拆空
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
                                        &nbsp;<asp:TextBox ID="txtcondition" runat="server" CssClass="input" Font-Size="10pt" 
                                            ValidationGroup="group1" Width="200px"></asp:TextBox>
                                        &nbsp;
                                        <asp:Button ID="btnquery" runat="server" BackColor="Transparent" 
                                            BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                            OnClick="btnquery_Click" Text="查询" ValidationGroup="group1" Width="70px" />
                                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                       <asp:Button ID="btnChaiChu" runat="server" BackColor="Transparent" 
                                                           BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                           OnClick="btnChaiChu_Click" Text="拆空选中产品" ValidationGroup="group1" 
                                                           Width="96px" />
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
                                            <td width=50% style="width: 100%">
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
