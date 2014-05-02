﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComponentArchives.aspx.cs" Inherits="MCSApp.Application.ProductManage.ComponentArchives" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>部件档案</title>    
    <script type="text/javascript" src="../../scripts/JSDate.js"></script>
    <script type="text/javascript">
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
                            <b>部件档案</b>
                        </td>
                        <td align="right">
                            </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                        <table width="100%"  class=table90 >
                                        <tbody>
                                            <tr>
                                                <td  align=left>
                                                    <span style="font-size: 10pt">部件序列号</span><asp:TextBox ID="txtID" 
                                                        runat="server" Width=300px MaxLength="50"></asp:TextBox>

                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAddRcd" OnClick="btnAddRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="添加记录" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
                                                        BackColor="Transparent" Width="70px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnSaveRcd" OnClick="btnSaveRcd_Click" runat="server" ValidationGroup="group1"
                                                        Text="保存" Height="20px" Font-Size="10pt" BorderWidth="1px" BorderColor="Gray"
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
                       <asp:GridView ID="GridView1" runat="server" AllowPaging="False" 
                           AutoGenerateColumns="False" PageSize="20" 
                            Width="100%" EmptyDataText="没有相关记录！" AutoGenerateDeleteButton="True" 
    onrowdeleting="GridView1_RowDeleting" DataKeyNames="ID">
                           <Columns>
                               <asp:BoundField DataField="ID" HeaderText="部件序列号" />
                               <asp:BoundField DataField="ModelID" HeaderText="品号" />
                               <asp:BoundField DataField="PlanID" HeaderText="生产计划号" />
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:TextBox runat='server' ID='txtremark' Width='300px' 
                                       Text='<%# Eval("remark") %>' MaxLength=50></asp:TextBox>
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
