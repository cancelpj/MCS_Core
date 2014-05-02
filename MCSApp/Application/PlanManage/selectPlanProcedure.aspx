<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectPlanProcedure.aspx.cs" Inherits="MCSApp.Application.PlanManage.selectPlanProcedure" %>

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
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    </head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                            产品流程选择表
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=right nowrap colspan="2">
                                                    <asp:Button ID="btnOK" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                        OnClick="btnOK_Click" Text="保存" ValidationGroup="group1" Width="70px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width=50% nowrap>
                                                    产品品号：<asp:Label ID="lblProductModel" runat="server"></asp:Label>
                                                    &nbsp; 产品品名：<asp:Label ID="lblProductModelName" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" nowrap>
                                                    <%--计划单操作：--%><asp:DropDownList ID="ddlop" runat="server">
                                                        <asp:ListItem Value="2">激活</asp:ListItem>
                                                        <asp:ListItem Value="3">关闭</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap valign="middle" colspan="2">
                                                    产品流程：
                                                    <asp:RadioButtonList ID="rblProductModel" runat="server" RepeatColumns="5" 
                                                        RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                    <asp:Label ID="rblProductModelOld" runat="server" Text="Label" Visible="false"></asp:Label>
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
                       onrowdatabound="GridView1_RowDataBound" >
                    <Columns>
                        <asp:TemplateField HeaderText="部件品号">
                            <ItemTemplate>
                                <asp:Label ID="lblitemid" runat="server" Text='<%# Bind("itemid") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="部件名称">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="可供选择的流程号">
                            <ItemTemplate>
                                <asp:RadioButtonList ID="rblComModel" runat="server" RepeatColumns="5" 
                                    RepeatDirection="Horizontal">
                                </asp:RadioButtonList>                               
                                <asp:Label ID="rblComModelOld" runat="server" Text="Label" Visible="false"></asp:Label>
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
			</table>
    </form>
</body>
</html>
