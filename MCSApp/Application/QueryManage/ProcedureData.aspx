<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcedureData.aspx.cs" Inherits="MCSApp.Application.QueryManage.ProcedureData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>产品在各个工序中产生的数据</title>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
    <script src="../../scripts/CustomWin.js" language=javascript></script>
    <script language=javascript>
    
    function showDetailData(DataID)
    {
         x_open('记录表', 'DataDetail.aspx?DataID='+DataID,800,300); 
    }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                            <b>产品工序数据</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=left nowrap valign="middle" colspan=2>
                                                    <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" runat="server"
                                                        Text="生产条码" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="客户条码" GroupName="group1" 
                                                        Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="250px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td width=10%>产品序列号：&nbsp;</td>
                                            <td width=90% >
                                                <asp:Label ID="lblPID" runat="server" Width="90%"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    &nbsp; 产品条码：</td>
                                                <td width="90%">
                                                    <asp:Label ID="lblSN" runat="server" Width="90%"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    &nbsp;&nbsp;品名：</td>
                                                <td width="90%">
                                                    <asp:Label ID="lblName" runat="server" Width="90%"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td  width=10%>&nbsp; 代号：</td>
                                            <td  width=90%><asp:Label ID="lblDaiHao" runat="server" Text="" Width=90%></asp:Label></td>
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
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" 
                       PageSize="20" EmptyDataText="没有相关记录！" 
                       onrowdatabound="GridView1_RowDataBound" >
                    <Columns>
                               <asp:TemplateField HeaderText="序号">
                                   <ItemTemplate>
                                       <asp:Label ID="lblNO" runat="server" Text='<%# Container.DataItemIndex + 1%> ' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="工序名称">
                                   <ItemTemplate>
                                       <asp:Label ID="lblProcess" runat="server" Text='<%# Bind("Process") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="作业结果">
                                   <ItemTemplate>
                                       <asp:Label ID="lblresult" runat="server" Text='<%# Bind("resultstr") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                                <asp:TemplateField HeaderText="作业记录">
                                   <ItemTemplate>
                                       <asp:Label ID="lblData" runat="server" Text='<%# Bind("data") %>' Width=100></asp:Label>
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
    </form>
</body>
</html>
