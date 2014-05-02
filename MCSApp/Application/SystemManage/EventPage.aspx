<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventPage.aspx.cs" Inherits="MCSApp.Application.SystemManage.EventPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>事件记录</title>    
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>
    <script type="text/javascript">
    __DateTimeBoxInit();
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
                            <b>事件日志</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=left>
                                                    <span style="font-size: 10pt">
                                                    查询日期:<asp:CheckBox ID="ckCancelTime" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="ckCancelTime_CheckedChanged" />
                                                    从</span><input id="dt_begin_UseBox" maxlength="15" name="dt_begin_UseBox" onblur="__DateBoxOnBlur(this,'true');"
                                        ondblclick="selectDate(this);" style="text-align: center; width: 68px;" type="text"
                                        value='' runat="server" /><img onclick="selectDate(document.getElementById ('dt_begin_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />
                                    <span style="font-size: 10pt">至</span><input type="text" id="dt_end_UseBox" name="dt_end_UseBox" value='' style="text-align:center; width: 68px;" maxlength="15"  onDblClick="selectDate(this);" onBlur="__DateBoxOnBlur(this,'true');" runat="server" /><img onclick="selectDate(document.getElementById ('dt_end_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />

                                                    &nbsp;&nbsp;&nbsp;&nbsp;<span class="style1">工号</span><asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
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
                       <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                           AutoGenerateColumns="False" PageSize="20" 
                           onpageindexchanging="GridView1_PageIndexChanging" Width="100%" EmptyDataText="没有相关记录！" >
                           <Columns>
                               <asp:BoundField DataField="EmplyeeID" HeaderText="工号" />
                               <asp:BoundField DataField="EventTime" HeaderText="时间" />
                               <asp:BoundField DataField="EventRecord" HeaderText="事件记录" />
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
