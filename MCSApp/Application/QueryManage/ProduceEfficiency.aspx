<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProduceEfficiency.aspx.cs" Inherits="MCSApp.Application.QueryManage.ProduceEfficiency" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>统计生产效率</title>    
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>
    <script type="text/javascript">
    __DateTimeBoxInit();
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
            
        <table height="40%" cellspacing="0" cellpadding="0" width="99%" align="center" border="0">
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
                            <b>统计生产效率</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90 >
                                        <tbody>
                                            <tr>
                                                <td  align=left  nowrap=nowrap width=100%>
                                                    <span style="font-size: 10pt"><asp:CheckBox ID="ckCancelTime" 
                                                        runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="ckCancelTime_CheckedChanged" Checked="True" 
                                                        Visible="False" />
                                                    从</span><input id="dt_begin_UseBox" maxlength="15" name="dt_begin_UseBox" onblur="__DateBoxOnBlur(this,'true');"
                                        ondblclick="selectDate(this);" style="text-align: center; width: 68px;" type="text"
                                        value='' runat="server" /><img onclick="selectDate(document.getElementById ('dt_begin_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />
                                    <span style="font-size: 10pt">至</span><input type="text" id="dt_end_UseBox" name="dt_end_UseBox" value='' style="text-align:center; width: 68px;" maxlength="15"  onDblClick="selectDate(this);" onBlur="__DateBoxOnBlur(this,'true');" runat="server" /><img onclick="selectDate(document.getElementById ('dt_end_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" /><asp:RadioButton 
                                                        ID="RadioButton1" runat="server"
                                                        Text="品号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="品名" GroupName="group1" 
                                                        Font-Size="10pt">
                                                    </asp:RadioButton>
                                                    <asp:RadioButton ID="RadioButton3" 
                                                        runat="server"  Font-Size="10pt" GroupName="group1" Text="代号" />
                                                    <asp:TextBox ID="txtID" 
                                                        runat="server" Width=309px MaxLength="50"></asp:TextBox>    &nbsp;&nbsp;&nbsp;    <asp:Button ID="btnQuery" OnClick="btnQuery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                                                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    &nbsp;&nbsp; 品&nbsp; 号：<asp:Label ID="lblModelID" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    &nbsp;&nbsp; 品&nbsp; 名：<asp:Label ID="lblModelName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    &nbsp;&nbsp; 代&nbsp; 号：<asp:Label ID="lblModelCode" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    日均产量：<asp:Label ID="lblDayAvgOutput" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap="nowrap" width="100%">
                                                    总 产 量：<asp:Label ID="lblWholeOutput" runat="server"></asp:Label>
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
                       <asp:GridView ID="GridView1" runat="server" 
                           AutoGenerateColumns="False" PageSize="20" 
                            Width="100%" EmptyDataText="没有相关记录！">
                           <Columns>
                                <asp:TemplateField HeaderText="工序">
                                   <ItemTemplate>
                                       <asp:Label ID="lblProcess" runat="server" Text='<%# Bind("Process") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField  HeaderText="耗时(分钟)" DataFormatString="{0:f2}" DataField="avuseTime"/>
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