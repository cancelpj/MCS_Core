<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerReturn.aspx.cs" Inherits="MCSApp.Application.ProcedureManage.CustomerReturn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>产品档案</title>    
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
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
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
                            <b>客退产品登记</b>
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
                                                        runat="server" Font-Size="10pt" GroupName="group1" Text="产品序列号" />
                                                    <asp:RadioButton ID="RadioButton2" runat="server" Font-Size="10pt" Checked="True"
                                                        GroupName="group1" Text="产品条码" />
                                                    &nbsp;<asp:TextBox ID="txtID" 
                                                        runat="server" Width=300px MaxLength="50"></asp:TextBox>

                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddRcd" OnClick="btnAddRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="查找产品" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                                                                                
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Enabled="False" Font-Size="10pt" 
                                                        Height="20px" OnClick="btnSave_Click" Text="保存记录" ValidationGroup="group1" 
                                                        Width="70px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="style1" colspan="2">
                                                    产品条码:<asp:Label ID="lblProductID" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp; 产品序列号:<asp:Label ID="lblSN" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp; 产品品号:<asp:Label ID="lblModelID" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="style1" colspan="2">
                                                    原生产计划号:<asp:Label ID="lblOldPlanID" runat="server"></asp:Label>
                                                    &nbsp; 当前生产计划号:<asp:TextBox ID="txtPlanIDNew" runat="server" CssClass="input" 
                                                        Font-Size="10pt" ValidationGroup="group2" Width="100px"></asp:TextBox>
                                                    <a href="javascript:var SelcontrolName=null;SelcontrolName=document.getElementById('txtPlanIDNew') ;x_open('计划单列表', '../PlanManage/QueryPlan.aspx',600,400); ">
                                                    <img src="../../images/search.gif" style="cursor: hand" style="border: 0" /></a></td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="style1" colspan="2">
                                                    备注信息:<asp:TextBox ID="txtRPInfo" runat="server" MaxLength="50" Width="545px"></asp:TextBox>
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

