<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleProductInfo.aspx.cs" Inherits="MCSApp.Application.QueryManage.SingleProductInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>单台产品生产详情</title>    
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>    
    <script src="../../scripts/CustomWin.js" language=javascript></script>
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
      
    function showDetailData(DataID)
    {
         x_open('记录表', 'DataDetail.aspx?DataID='+DataID,800,300); 
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
                            <b>单台产品生产详情</b>
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
                                                    查询条件：<asp:RadioButton ID="RadioButton3" 
                                                        runat="server"  Font-Size="10pt" GroupName="group1" Text="生产条码" Checked />
                                                    <asp:RadioButton ID="RadioButton4" runat="server" Font-Size="10pt" 
                                                        GroupName="group1" Text="客户条码" />
                                                    <asp:TextBox ID="txtID" 
                                                        runat="server" Width=250px MaxLength="50"></asp:TextBox>    &nbsp;&nbsp;&nbsp;    <asp:Button ID="btnQuery" OnClick="btnQuery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                                                                                
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                         <table class=table90 width=100%>            
             <tr>
                <td align="left" class="style7" >
                    产品序列号：<asp:Label ID="lblID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style7" >
                    产品条码：<asp:Label ID="lblSN" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    品名：<asp:Label ID="lblModelName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    代号：<asp:Label ID="lblCode" runat="server"></asp:Label>
                </td>
            </tr>
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
                            Width="100%" EmptyDataText="没有相关记录！" 
                           onrowdatabound="GridView1_RowDataBound">
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
                               <asp:TemplateField HeaderText="作业员">
                                   <ItemTemplate>
                                       <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="工位安排">
                                   <ItemTemplate>
                                       <asp:Label ID="lblFindTime" runat="server" Text='<%# Bind("dispatch") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>  
                               <asp:TemplateField HeaderText="开始时间">
                                   <ItemTemplate>
                                       <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("begintime") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>  
                               <asp:TemplateField HeaderText="结束时间">
                                   <ItemTemplate>
                                       <asp:Label ID="lblType" runat="server" Text='<%# Bind("endtime") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                               <asp:BoundField  HeaderText="耗时(分钟)" DataFormatString="{0:f2}" DataField="usetime"/>
                                <asp:TemplateField HeaderText="作业结果">
                                   <ItemTemplate>
                                       <asp:Label ID="lblrepairer" runat="server" Text='<%# Bind("resultstr") %>' Width=100></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                                <asp:TemplateField HeaderText="作业记录">
                                   <ItemTemplate>
                                       <asp:Label ID="lblrepairTime" runat="server" Text='<%# Bind("data") %>' Width=100></asp:Label>
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