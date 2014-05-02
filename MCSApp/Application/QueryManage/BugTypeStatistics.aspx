<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BugTypeStatistics.aspx.cs" Inherits="MCSApp.Application.QueryManage.BugTypeStatistics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>缺陷类型统计</title>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
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
                            <b>缺陷类型统计</b>
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
                                                        Text="计划单号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="产品品号" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="250px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td width=10%>&nbsp; 品号：</td>
                                            <td width=90% >
                                                <asp:Label ID="lblModelID" runat="server" Text="" Width=90%></asp:Label></td>
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
                        <asp:TemplateField HeaderText="缺陷类型">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("Bug") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="缺陷数量">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("bugcount") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="缺陷率">
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("bugrate") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="占总缺陷数百分比">
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("bugperc") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="总缺陷数" Visible=false>
                            <ItemTemplate>
                                <asp:Label ID="lblallbugs" runat="server" Text='<%# Bind("allbugs") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="总产品数" Visible=false>
                            <ItemTemplate>
                                <asp:Label ID="lblallpros" runat="server" Text='<%# Bind("allpros") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
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
