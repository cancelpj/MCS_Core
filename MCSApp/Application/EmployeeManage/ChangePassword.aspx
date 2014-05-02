<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="MCSApp.Application.EmployeeManage.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>密码修改</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
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
        <table class=table90>
            <tr>
                <td align="left" width="100" >
                    工号</td>
                <td>
                    <asp:Label ID="lblID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="200">
                    姓名</td>
                <td>
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left">
                    原密码<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtOldPsw" Display="None" ErrorMessage="原密码不能为空！"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtOldPsw" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" width="200">
                    新密码<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtNewPsw" Display="None" ErrorMessage="新密码不能为空！"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPsw" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr  >
                <td align="left" width="200">
                    确认密码</td>
                <td>
                    <asp:TextBox ID="txtConfirmPsw" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" width="200">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ControlToCompare="txtConfirmPsw" ControlToValidate="txtNewPsw" Display="None" 
                        ErrorMessage="新密码与确认密码不一致！"></asp:CompareValidator>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        ShowMessageBox="True" ShowSummary="False" />
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server"  BackColor="Transparent" BorderColor="Gray"
                                                        BorderWidth="1px" Font-Size="10pt"  
                        onclick="btnSubmit_Click" Text="提交" Width="120px" />
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