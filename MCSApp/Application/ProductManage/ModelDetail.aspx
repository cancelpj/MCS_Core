<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelDetail.aspx.cs" Inherits="MCSApp.Application.ProductManage.ModelDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>详细信息</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            width: 128px;
        }
        .style2
        {
            width: 135px;
        }
        .style3
        {
            width: 275px;
        }
        .style5
        {
            width: 277px;
        }
        .style6
        {
            width: 280px;
        }
        .style7
        {
            width: 141px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <table height="40%" cellspacing="0" cellpadding="0" width="80%" align="center" border="0">
        <TR>
            		<TD width="5" height="5"><IMG height="7" alt="" src="../../images/corner_01.gif" width="5"></TD>
					<TD background="../../images/corner_02.gif"><IMG height="7" alt="" src="../../images/corner_02.gif" width="120"></TD>
					<TD width="5" height="5"><IMG height="7" alt="" src="../../images/corner_03.gif" width="5"></TD>
				</TR>
				<TR>
					<TD width="5" background="../../images/corner_04.gif"></TD>
					<TD vAlign="top" align=center>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate> 
                <asp:Panel ID="Panel1" runat="server">
         
        <table class=table90>
            <tr>
                <td align="left" class="style2" >
                    序列号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    品号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblModelID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style2">
                    条码号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblSN" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    名称</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style2">
                    建档时间</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblFoundTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    状态</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style2">
                    备注</td>
                <td align="left" colspan="3">
                    <asp:Label ID="lblReMark" runat="server"></asp:Label>
                </td>
            </tr>
        </table></asp:Panel> 
                        <asp:Panel ID="Panel2" runat="server">
         
        <table class=table90>
            <tr>
                <td align="left" class="style7" >
                    序列号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    &nbsp;</td>
                <td align="left" class="style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    品号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMMID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    名称</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblMName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    批次号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblBatch" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    供应商</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style7">
                    建档时间</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMFoundTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    生产员工工号</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblEID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style7">
                    备注</td>
                <td align="left" colspan="3">
                    <asp:Label ID="lblMremark" runat="server"></asp:Label>
                </td>
            </tr>
        </table></asp:Panel> 
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