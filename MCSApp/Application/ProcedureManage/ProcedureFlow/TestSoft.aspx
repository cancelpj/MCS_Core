<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSoft.aspx.cs" Inherits="MCSApp.Application.ProcedureManage.ProcedureFlow.TestSoft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <script type="text/javascript">
    
      function setContolValue() 
      {
          var vRdoIDs=document.getElementsByName('eids');
          var i=0;
          for(i=0;i<vRdoIDs.length;i++)
          {
              if(vRdoIDs[i].checked)
              {
                 
                 window.parent.SelcontrolName.value=vRdoIDs[i].value; 
                 window.parent.closeit()//关闭窗体；
                 break;                
              }
          }

      }
    
    </script>
    <link href="../../../css/style.css" type="text/css" rel="stylesheet">
    </head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table height="40%" cellspacing="0" cellpadding="0" width="80%" align="center" border="0">
        <TR>
            		<TD width="5" height="5"><IMG height="7" alt="" src="../../../images/corner_01.gif" width="5"></TD>
					<TD background="../../../images/corner_02.gif"><IMG height="7" alt="" src="../../../images/corner_02.gif" width="120"></TD>
					<TD width="5" height="5"><IMG height="7" alt="" src="../../../images/corner_03.gif" width="5"></TD>
				</TR>
				<TR>
					<TD width="5" background="../../../images/corner_04.gif"></TD>
					<TD vAlign="top">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <table class="tableFace" cellspacing="0" cellpadding="0" width="100%" align="center" border="1">
                    <tr class="tableRow">
                        <td align="center" width="20%" height="22">
                            测试软件维护
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=right nowrap>
                                                <table width="100%"><tr><td align="left">
                                                    软件名称：&nbsp;<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>_
                                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    版本号：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                    </td><td align="right">
                                                    <asp:Button ID="btnOK" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                        OnClick="btnOK_Click" Text="保存" ValidationGroup="group1" Width="70px" />
                                                        </td></tr></table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap valign="middle">
                                                    相关信息：
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
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                       Width="100%" 
                       PageSize="50" EmptyDataText="没有相关记录！" 
                        >
                    <Columns>
                        <asp:TemplateField HeaderText="软件名称">
                            <ItemTemplate>
                                <asp:Label ID="lblitemid" runat="server" Text='<%# Bind("Name") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="版本号">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Version") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%# Bind("Name") %>' OnClientClick="javascript:return confirm('你确认要删除吗?');" OnClick="LinkButton1_Click">删除</asp:LinkButton>
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
					<TD width="5" background="../../../images/corner_05.gif">
					</TD>
				</TR>
				<TR>
					<TD width="5" height="5"><IMG height="5" alt="" src="../../../images/corner_06.gif" width="5"></TD>
					<TD background="../../../images/corner_07.gif"><IMG height="5" alt="" src="../../../images/corner_07.gif" width="120"></TD>
					<TD width="5" height="5"><IMG height="5" alt="" src="../../../images/corner_08.gif" width="5"></TD>
				</TR>
			</TABLE>
    </form>
</body>
</html>