<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermitRetesting.aspx.cs" Inherits="MCSApp.Application.ProcedureManage.PermitRetesting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>批准复检</title>    
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>
    <script type="text/javascript">
    document.onkeydown=function()
    {
      if(event.keyCode==13)
      {document.getElementById("btnAddRcd").click();}
    }//IE only
      function pageLoad() 
      {
      }
	  
    </script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet"/>
    <style type="text/css">
        .style1
        {
            width: 640px;
        }
    </style>
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
                            <b>批准复检</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90 >
                                        <tbody>
                                            <tr>
                                                <td  align=left class="style1">
                                                    <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" 
                                                        runat="server" Checked="True" Font-Size="10pt" GroupName="group1" Text="产品序列号" />
                                                    <asp:RadioButton ID="RadioButton2" runat="server" Font-Size="10pt" 
                                                        GroupName="group1" Text="产品条码" />
                                                    &nbsp;<asp:TextBox ID="txtID" 
                                                        runat="server" Width=250px MaxLength="50"></asp:TextBox>

                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddRcd" OnClick="btnAddRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                                                                                
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Enabled="False" Font-Size="10pt" 
                                                        Height="20px" OnClick="btnSave_Click" Text="批准" ValidationGroup="group1" 
                                                        Width="70px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="style1" colspan="2">
                                                    异常发现工序：<asp:Label ID="lblEProcess" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <br />
                                                    发现者：<asp:Label ID="lblFinder" runat="server"></asp:Label>
                                                    (<asp:Label ID="lblFinderID" runat="server"></asp:Label>
                                                    )&nbsp;&nbsp;&nbsp;
                                                    <br />
                                                    发现时间：<asp:Label ID="lblFindTime" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <br />
                                                    异常描述：<asp:Label ID="lblException" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
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

