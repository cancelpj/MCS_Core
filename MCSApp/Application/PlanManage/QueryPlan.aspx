<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryPlan.aspx.cs" Inherits="MCSApp.Application.PlanManage.QueryPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <script type="text/javascript">
        var Request = {
            QueryString: function(item) {
                var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
                return svalue ? svalue[1] : svalue;
            }
        }       //通过从地址栏获得Request传参
            
      function setContolValue() 
      {
          var vRdoIDs=document.getElementsByName('eids');
          var i=0;
          if (Request.QueryString("GroupID") == null) {
              for (i = 0; i < vRdoIDs.length; i++) {
                  if (vRdoIDs[i].checked) {
                      window.parent.SelcontrolName.value = vRdoIDs[i].value;
                      window.parent.closeit()//关闭窗体；
                      break;
                  }
              }
          }
      }
    
    </script>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
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
                            <b>生产计划列表</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90>
                                        <tbody>
                                            <tr>
                                                <td  align=left nowrap>
                                                    <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" runat="server"
                                                        Text="生产计划号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="产品品号" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnOK" runat="server" BackColor="Transparent" Visible="false"  
                                                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                        Text="新增" ValidationGroup="group1" Width="70px" />
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
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                       AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging1" 
                       PageSize="10" EmptyDataText="没有相关记录！" >
                    <Columns>
                        <asp:TemplateField HeaderText="生产计划号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtid" runat="server" Text='<%# Bind("ID") %>' Width="100px" 
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorid" runat="server" ControlToValidate="txtid"
                                    ErrorMessage="生产计划号不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <input name='eids' ondblclick='setContolValue()' type=radio value='<%# Eval("ID") %>'   validationgroup="1" />
                                <asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="产品品号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("ModelID") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="产品品号不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("ModelID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="产品名称">
                            <ItemTemplate>
                                <asp:Label ID="lblModelID_name" runat="server" Text='<%# getNameByModelID(DataBinder.Eval(Container.DataItem, "ModelID").ToString()) %>' Width="60px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="客户订单号" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblorderID" runat="server" Text='<%# Bind("orderID") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="数量">
                            <ItemTemplate>
                                <asp:Label ID="lbloutPut" runat="server" Text='<%# Bind("outPut") %>' Width="100px"></asp:Label>
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
