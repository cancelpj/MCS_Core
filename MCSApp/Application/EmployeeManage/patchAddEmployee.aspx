<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patchAddEmployee.aspx.cs" Inherits="MCSApp.Application.EmployeeManage.patchAddEmployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                            <b>员工列表</b>
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
                                                        Text="工号" GroupName="group1" Checked="True" Font-Size="10pt"></asp:RadioButton><asp:RadioButton
                                                            ID="RadioButton2" runat="server" Text="姓名" GroupName="group1" Font-Size="10pt">
                                                    </asp:RadioButton>&nbsp;
                                                    <asp:TextBox ID="txtcondition" runat="server" ValidationGroup="group1"
                                                        Width="120px" Font-Size="10pt"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnquery" OnClick="btnquery_Click" runat="server" ValidationGroup="group1"
                                                        Text="查询" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnOK" runat="server" BackColor="Transparent" 
                                                        BorderColor="Gray" BorderWidth="1px" Font-Size="10pt" Height="20px" 
                                                        OnClick="btnOK_Click" Text="确定" ValidationGroup="group1" Width="70px" />
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
                       PageSize="20" EmptyDataText="没有相关记录！" DataKeyNames="id" >
                    <Columns>
                        <asp:TemplateField HeaderText="工号">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtid" runat="server" Text='<%# Bind("ID") %>' Width="100px" 
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorid" runat="server" ControlToValidate="txtid"
                                    ErrorMessage="工号不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckid" runat="server"/><asp:Label ID="lblid" runat="server" Text='<%# Bind("ID") %>'></asp:Label>    
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="员工名称">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"
                                    Width="100px"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="员工名称不能为空!" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="隐列" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblp" runat="server" Text='<%# Bind("state") %>' Width="1px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtremark" runat="server" MaxLength="50" Text='<%# Bind("remark") %>'
                                    Width="300px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblremark" runat="server" Text='<%# Bind("remark") %>' Width="300px"></asp:Label>
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
