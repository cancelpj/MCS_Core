<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveOrClosePlan.aspx.cs" Inherits="MCSApp.Application.PlanManage.ActiveOrClosePlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
    <title>处理计划</title>
        <script type="text/javascript" src="../../scripts/JSDate.js"></script>
        <script src="../../scripts/CustomWin.js"></script>
    <script type="text/javascript">
    __DateTimeBoxInit();
      function pageLoad() 
      {
      }
	  function getPlanProcedure(productModel,planid,rblEnable)
      {
          x_open('计划流程列表', 'selectPlanProcedure.aspx?productModel='+productModel+"&planid="+planid+"&rblEnable="+rblEnable,800,600); 
      }
      function query()
      {
          document.getElementById('btnquery').click();
      }
    </script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
</head>
<body >
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
                            <b>查看生产计划</b>
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
                                                    <span style="font-size: 10pt"><span style="font-size: 10pt">
                                                    日期:<asp:CheckBox ID="ckCancelTime" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="ckCancelTime_CheckedChanged" />
                                                    从</span><input id="dt_begin_UseBox" maxlength="15" name="dt_begin_UseBox" onblur="__DateBoxOnBlur(this,'true');"
                                        ondblclick="selectDate(this);" style="text-align: center; width: 68px;" type="text"
                                        value='' runat="server" /><img onclick="selectDate(document.getElementById ('dt_begin_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />
                                    <span style="font-size: 10pt">至</span><input type="text" id="dt_end_UseBox" name="dt_end_UseBox" value='' style="text-align:center; width: 68px;" maxlength="15"  onDblClick="selectDate(this);" onBlur="__DateBoxOnBlur(this,'true');" runat="server" /><img onclick="selectDate(document.getElementById ('dt_end_UseBox'));"  align="middle" src="../../images/Calendar_scheduleHS.png" style="border-width:0px;cursor:hand;" />

                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    <br />
                                                    </span><asp:RadioButton ID="RadioButton1" runat="server"
                                                        Text="生产计划号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton ID="RadioButton2" runat="server"
                                                        Text="产品品号" GroupName="group1"  Font-Size="10pt"></asp:RadioButton><asp:RadioButton ID="RadioButton3" runat="server"
                                                        Text="客户订单号" GroupName="group1"  Font-Size="10pt"></asp:RadioButton>
                                                    &nbsp;计划单状态<asp:DropDownList ID="ddlPlanState" runat="server">
                                                        <asp:ListItem Value="0">----</asp:ListItem>
                                                        <asp:ListItem Value="1">初始</asp:ListItem>
                                                        <asp:ListItem Value="2">激活</asp:ListItem>
                                                        <asp:ListItem Value="3">关闭</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtcondition" 
                                                        runat="server" ValidationGroup="group1"
                                                        Width="306px" Font-Size="10pt"></asp:TextBox>&nbsp;
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
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                    ShowMessageBox="False" Font-Size="10pt" />
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="表格正处在编辑状态，请先保存或取消!" Visible="False"
                    Font-Size="10pt"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    DataKeyNames="id,state" 
                       AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" 
                       PageSize="15" EmptyDataText="没有相关记录！" 
                       onrowdatabound="GridView1_RowDataBound" 
                       onrowcommand="GridView1_RowCommand" onrowcreated="GridView1_RowCreated" >
                    <Columns>
                        <asp:TemplateField HeaderText="计划号">
                            <EditItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="60px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="60px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品号">
                            <EditItemTemplate>
                                <asp:Label ID="lblModelID" runat="server" Text='<%# Bind("ModelID") %>' Width="80px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModelID" runat="server" Text='<%# Bind("ModelID") %>' Width="80px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="产品名称">
                            <ItemTemplate>
                                <asp:Label ID="lblModelID_name" runat="server" Text='<%# getNameByModelID(DataBinder.Eval(Container.DataItem, "ModelID").ToString()) %>' Width="80px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="数量">
                            <EditItemTemplate>
                                <asp:Label ID="lblOutput" runat="server" Text='<%# Bind("Output") %>' Width="30px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOutput" runat="server" Text='<%# Bind("Output") %>' Width="30px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="客户订单号">
                            <EditItemTemplate>
                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>' Width="80px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderID" runat="server" Text='<%# Bind("OrderID") %>' Width="80px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>                         
                        <asp:TemplateField HeaderText="类型">
                            <EditItemTemplate>
                                <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("plantypestr") %>' Width="30px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("plantypestr") %>' Width="30px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="创建时间">
                            <EditItemTemplate>
                                <asp:Label ID="lblFoundTime" runat="server" Text='<%# Bind("FoundTime") %>' Width="100px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFoundTime" runat="server" Text='<%# Bind("FoundTime") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="关闭时间">
                            <EditItemTemplate>
                                <asp:Label ID="lblCloseTime" runat="server" Text='<%# Bind("CloseTime") %>' Width="190px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCloseTime" runat="server" Text='<%# Bind("CloseTime") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="计划流程">
                            <ItemTemplate>
                                <a href="#" onclick="getPlanProcedure('<%# Eval("Modelid") %>','<%# Eval("id") %>','false')">查看<img style="border: 0;cursor: hand" src="../../images/search.gif" /></a>&nbsp;
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="隐列" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblp" runat="server" Text='<%# Bind("state") %>' Width="1px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlState" runat="server" Width="30px">

                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblstatestr" runat="server" Text='<%# Bind("statestr") %>' Width="30px"></asp:Label>                                
                                <asp:LinkButton ID="lbActive" runat="server" ForeColor="Blue">[激活]</asp:LinkButton>
                                <asp:LinkButton ID="lbEdit" runat="server" ForeColor="Blue">[修改]</asp:LinkButton>
                                <asp:LinkButton ID="lbClose" runat="server" ForeColor="Blue" 
                                    CommandName="Close">[关闭]</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="备注">
                            <EditItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("remark") %>' Width="200px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("remark") %>' Width="200px"></asp:Label>
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
