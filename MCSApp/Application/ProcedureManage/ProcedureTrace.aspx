<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcedureTrace.aspx.cs" Inherits="MCSApp.Application.ProcedureManage.ProcedureTrace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        document.onkeydown=function()
        {
          if(event.keyCode==13)
          {document.getElementById("btnAddRcd").click();}
        }//IE only
      function pageLoad() {
      }
    var modeltype;
    function showDetailData(DataID)
    {
         x_open('记录表', '../QueryManage/DataDetail.aspx?DataID='+DataID,800,300); 
    }	
    </script>
        <link href="../../css/style.css" type="text/css" rel="stylesheet">
        <script src="../../scripts/CustomWin.js"></script>
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
                            <b>流程追溯</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
<table width="100%"  class=table90>
                                            <tbody>
                                                <tr>
                                                   <td>                                        
                                                       扫描序列号或条码：<asp:TextBox ID="txtID" 
                                                        runat="server" Width=300px MaxLength="50"></asp:TextBox>

                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddRcd" OnClick="btnAddRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="查找产品" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
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
                                    <table border=0 width=100%>
                                        <tr>
                                            <td width=50% valign=top>
                                            产品结构表
                                                <asp:TreeView ID="TreeStructure" runat="server" ExpandDepth="4" 
                                                    ImageSet="Arrows" ShowLines="True" 
                                                    onselectednodechanged="TreeStructure_SelectedNodeChanged">
                                                    <SelectedNodeStyle BackColor="Silver" />
                                                </asp:TreeView>
                                            </td>
                                            <td valign="top" align=left width=50% >
                                                档案信息
                                                <asp:Panel ID="Panel1" runat="server" Visible="False">
         
        <table class=table90 width="100%">
            <tr>
                <td align="left" class="style2" >
                    序列号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    品号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblModelID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style2">
                    条码号</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblSN" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    名称</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style2">
                    建档时间</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblFoundTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    状态</td>
                <td align="left" class="style3">
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style2">
                    备注</td>
                <td align="left" colspan="3">
                    <asp:Label ID="lblReMark" runat="server"></asp:Label>
                </td>
            </tr>
        </table></asp:Panel> 
                        <asp:Panel ID="Panel2" runat="server" Visible="False">
         
        <table class=table90 width=100%>
            <tr>
                <td align="left" class="style7" >
                    序列号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    &nbsp;</td>
                <td align="left" class="style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    品号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMMID" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    名称</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblMName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style7">
                    批次号</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblBatch" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    供应商</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style7">
                    建档时间</td>
                <td align="left" class="style5">
                    <asp:Label ID="lblMFoundTime" runat="server"></asp:Label>
                </td>
                <td align="left" class="style1">
                    生产员工工号</td>
                <td align="left" class="style6">
                    <asp:Label ID="lblEID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr  >
                <td align="left" class="style7">
                    备注</td>
                <td align="left" colspan="3">
                    <asp:Label ID="lblMremark" runat="server"></asp:Label>
                </td>
            </tr>
        </table></asp:Panel> 
                                        </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%" valign="top" width="50%">
                                                流程历史信息</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%" valign="top" width="50%">
                                                <asp:GridView ID="GridView1" runat="server" 
                                                    AutoGenerateColumns="False" EmptyDataText="没有相关记录！" PageSize="20" 
                                                    Width="100%" onrowdatabound="GridView1_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="序号">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text="<%# Container.DataItemIndex + 1%> "></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30px" />
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="process" HeaderText="工序名" />
                                                        <asp:BoundField DataField="Name" HeaderText="作业员" />
                                                        <asp:BoundField DataField="Dispatch" HeaderText="工位安排" />
                                                        <asp:BoundField DataField="begintime" HeaderText="开始时间" HtmlEncode=false DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                                        <asp:BoundField DataField="endtime" HeaderText="结束时间"   HtmlEncode=false DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
                                                        <asp:BoundField DataField="resultstr" HeaderText="作业结果" />
                                                        <asp:BoundField DataField="data" HeaderText="作业记录" />                                                        
                                                    </Columns>
                                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>

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