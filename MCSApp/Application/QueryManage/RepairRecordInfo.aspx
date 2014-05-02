<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairRecordInfo.aspx.cs" Inherits="MCSApp.Application.QueryManage.RepairRecordInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>产品维修记录</title>    
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
                            <b>查询维修记录</b>
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
                                                    <asp:DropDownList ID="ddlDate" runat="server" Enabled="False">
                                                        <asp:ListItem Value="1">异常发现时间</asp:ListItem>
                                                        <asp:ListItem Value="2">修理时间</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span style="font-size: 10pt"><asp:CheckBox ID="ckCancelTime" 
                                                        runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="ckCancelTime_CheckedChanged" />
                                                    从</span><input id="dt_begin_UseBox" maxlength="15" name="dt_begin_UseBox" onblur="__DateBoxOnBlur(this,'true');"
                                        ondblclick="selectDate(this);" style="text-align: center; width: 68px;" type="text"
                                        value='' runat="server" disabled="disabled" /><img onclick="selectDate(document.getElementById ('dt_begin_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />
                                    <span style="font-size: 10pt">至</span><input type="text" id="dt_end_UseBox" 
                                                        name="dt_end_UseBox" value='' style="text-align:center; width: 68px;" 
                                                        maxlength="15"  onDblClick="selectDate(this);" 
                                                        onBlur="__DateBoxOnBlur(this,'true');" runat="server" disabled="disabled" /><img onclick="selectDate(document.getElementById ('dt_end_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" /><asp:RadioButton ID="RadioButton1" runat="server"
                                                        Text="计划单号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="产品品号" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>
                                                    <asp:RadioButton ID="RadioButton3" 
                                                        runat="server"  Font-Size="10pt" GroupName="group1" Text="生产条码" />
                                                    <asp:RadioButton ID="RadioButton4" runat="server" Font-Size="10pt" 
                                                        GroupName="group1" Text="客户条码" /><asp:TextBox ID="txtID" 
                                                        runat="server" Width=166px MaxLength="50"></asp:TextBox>    &nbsp;&nbsp;&nbsp;    <asp:Button ID="btnQuery" OnClick="btnQuery_Click" runat="server" ValidationGroup="group1"
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
                       <asp:GridView ID="GridView1" runat="server" AllowPaging="false" 
                           AutoGenerateColumns="False" PageSize="20" 
                            Width="100%" EmptyDataText="没有相关记录！">
                           <Columns>
                               <asp:TemplateField HeaderText="产品序列号">
                                   <ItemTemplate>
                                       <asp:Label ID="lblPID" runat="server" Text='<%# Bind("productid") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="异常发现工序">
                                   <ItemTemplate>
                                       <asp:Label ID="lblProcess" runat="server" Text='<%# Bind("DetectProcess") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="异常发现者">
                                   <ItemTemplate>
                                       <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("dEmpName") %>' ></asp:Label>[
                                       <asp:Label ID="lblDetectEmployeeID" runat="server" Text='<%# Bind("DetectEmployeeID") %>' ></asp:Label>]
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="异常发现时间">
                                   <ItemTemplate>
                                       <asp:Label ID="lblFindTime" runat="server" Text='<%# Bind("FindTime") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>  
                               <asp:TemplateField HeaderText="异常描述">
                                   <ItemTemplate>
                                       <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Exception") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>  
                               <asp:TemplateField HeaderText="缺陷类别">
                                   <ItemTemplate>
                                       <asp:Label ID="lblType" runat="server" Text='<%# Bind("Bug") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                               <asp:TemplateField HeaderText="缺陷定位点">
                                   <ItemTemplate>
                                       <asp:Label ID="lblPoint" runat="server" Text='<%# Bind("BugPointCode") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="修理员工">
                                   <ItemTemplate>
                                       <asp:Label ID="lblrepairer" runat="server" Text='<%# Bind("repairer") %>' ></asp:Label>[
                                       <asp:Label ID="lblRepairEmployeeID" runat="server" Text='<%# Bind("RepairEmployeeID") %>' ></asp:Label>]
                                   </ItemTemplate>
                               </asp:TemplateField> 
                                <asp:TemplateField HeaderText="修理时间">
                                   <ItemTemplate>
                                       <asp:Label ID="lblrepairTime" runat="server" Text='<%# Bind("repairTime") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                                    
                                <asp:TemplateField HeaderText="修理信息">
                                   <ItemTemplate>
                                       <asp:Label ID="lblddlRePID" runat="server" Text='<%# Bind("repairInfo") %>' Width=100></asp:Label>
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