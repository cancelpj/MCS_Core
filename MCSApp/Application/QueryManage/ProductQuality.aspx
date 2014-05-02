<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductQuality.aspx.cs" Inherits="MCSApp.Application.QueryManage.ProductQuality" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>统计产品质量</title>
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>
    <script type="text/javascript">
    __DateTimeBoxInit();
    
      function pageLoad() {
      }
    
    </script>
        <link href="../../css/style.css" type="text/css" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            width: 444px;
        }
        .style2
        {
            width: 288px;
        }
        .style3
        {
            width: 407px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table height="40%" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
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
        <table class=table90>
            <tr>
                <td align="left" colspan="2" >
                    <span style="font-size: 10pt">生产开始时间：从</span><input ID="dt_begin_UseBox" 
                        runat="server" maxlength="15" name="dt_begin_UseBox" 
                        onblur="__DateBoxOnBlur(this,'true');" ondblclick="selectDate(this);" 
                        style="text-align: center; width: 68px;" type="text" value="" /><img 
                        align="middle" 
                        onclick="selectDate(document.getElementById ('dt_begin_UseBox'));" 
                        src="../../images/Calendar_scheduleHS.png" 
                        style="border-width:0px;cursor:hand;" /> <span style="font-size: 10pt">至</span><input 
                        ID="dt_end_UseBox" runat="server" maxlength="15" name="dt_end_UseBox" 
                        onBlur="__DateBoxOnBlur(this,'true');" onDblClick="selectDate(this);" 
                        style="text-align:center; width: 68px;" type="text" value="" /><img 
                        align="middle" onclick="selectDate(document.getElementById ('dt_end_UseBox'));" 
                        src="../../images/Calendar_scheduleHS.png" 
                        style="border-width:0px;cursor:hand;" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 生产计划号<asp:TextBox ID="txtPlanHao" runat="server" Width="224px"></asp:TextBox>
                    <asp:Button ID="btnQuery" runat="server" BackColor="Transparent" 
                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" onclick="btnSubmit_Click" 
                        Text="统计" Width="120px" />
                </td>
            </tr>
            <tr>
                <td align="left" class="style2" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    生产计划号：<asp:Label ID="lblPlanID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    计划产量：<asp:Label ID="lblOutput" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    品号：<asp:Label ID="lblModelID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style3">
                    启动时间：<asp:Label ID="lblBeginTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    品名：<asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    关闭时间：<asp:Label ID="lblCloseTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    代号：<asp:Label ID="lblDaiHao" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style2" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr  >
                <td align="left" class="style3">
                    返修次数：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblRepairCount" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    返修台数：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblRepairTaiShu" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    合格数：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblOKCount" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    合格率：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblOKRate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    一次合格数：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblOnceOKCount" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    一次合格率：</td>
                <td align="left" class="style1">
                    <asp:Label ID="lblOnceOKRate" runat="server"></asp:Label>
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
    </form>
</body>
</html>