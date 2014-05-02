<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputProductSN.aspx.cs" Inherits="MCSApp.Application.ProductManage.InputProductSN" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            
        <table height="40%" cellspacing="0" cellpadding="0" width="80%" align="center" border="0">
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
                            <b>产品条码录入</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90 >
                                        <tbody>
                                            <tr>
                                                <td  align=left>
                                                    <span style="font-size: 10pt">
                                                    <asp:Label ID="lblCode" runat="server" Text="扫描产品序列号"></asp:Label>
                                                    </span>
                                                    <asp:TextBox ID="txtID" 
                                                        runat="server" Width=200px MaxLength="50"></asp:TextBox>
                                                           <asp:TextBox ID="txtSN" 
                                                        runat="server" Width=200px MaxLength="50" Visible="False"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddRcd" OnClick="btnAddRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="验证产品序列号" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="95px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnSaveRcd" OnClick="btnSaveRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="保存" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                        
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
                       <asp:GridView ID="GridView1" runat="server" AllowPaging="False" 
                           AutoGenerateColumns="False" PageSize="20" 
                            Width="100%" EmptyDataText="没有相关记录！" 
    onrowdeleting="GridView1_RowDeleting" DataKeyNames="ID">
                           <Columns>
                               <asp:BoundField DataField="ID" HeaderText="产品序列号" />
                               <asp:BoundField DataField="ModelID" HeaderText="品号" />
                               <asp:BoundField DataField="PlanID" HeaderText="生产计划号" />
                               <asp:TemplateField HeaderText="产品条码">
                                   <ItemTemplate>
                                       <asp:TextBox runat='server' ID='txtSN' Width='300px' 
                                       Text='<%# Eval("SN") %>' MaxLength=50 ReadOnly=true></asp:TextBox>
                                   </ItemTemplate>
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

