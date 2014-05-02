<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcedureList.aspx.cs"
    Inherits="MCSApp.Application.ProcedureManage.ProcedureFlow.ProcedureList" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工序流程</title>

    <script type="text/javascript" src="../../../scripts/JSDate.js"></script>

    <script type="text/javascript" src="../../../scripts/CustomWin1.js"></script>

    <script type="text/javascript">
        function pageLoad() {
        }
        function BorrowProcedure(ProcedureID) {
            x_open('借用流程', 'BorrowProcedure.aspx?ProcedureID=' + ProcedureID, 800, 300);
        }
        //
        function updateProcess(id, editState) {
            if (editState == 'false') {
                window.showModalDialog("ProcedureOP/viewprocess.html?name=" + id, '', "dialogWidth:800px;dialogHeight:600px;center:yes;help:no;scroll:no;status:no;resizable:yes;");
            }
            else {
                var result = window.showModalDialog("ProcedureOP/updateprocess.html?name=" + id, '', "dialogWidth:800px;dialogHeight:600px;center:yes;help:no;scroll:no;status:no;resizable:yes;");
                form1.btnquery.click();
            }
        }
        function GettestSoft(modelID) {
            x_open('测试软件维护', 'TestSoft.aspx?id=' + modelID, 800, 600);
        }
      
      
    </script>

    <link href="../../../css/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:ScriptManager ID="ScriptManager2" runat="server">
        </ext:ScriptManager>
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <table height="40%" cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
            <tr>
                <td width="5" height="5">
                    <img height="7" alt="" src="../../../images/corner_01.gif" width="5" />
                </td>
                <td>
                    <img height="7" alt="" src="../../../images/corner_02.gif" width="120" />
                </td>
                <td width="5" height="5">
                    <img height="7" alt="" src="../../../images/corner_03.gif" width="5" />
                </td>
            </tr>
            <tr>
                <td width="5">
                    <img height="7" alt="" src="../../../images/corner_04.gif" width="5" />
                </td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table class="tableFace" cellspacing="0" cellpadding="0" width="100%" align="center"
                                border="1">
                                <tr class="tableRow">
                                    <td align="center" width="20%" height="22">
                                        <b>工艺流程定义</b>
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2">
                                        <table width="100%" class="table90">
                                            <tbody>
                                                <tr>
                                                    <td align="left">
                                                        <span style="font-size: 10pt">查询条件</span>：<asp:RadioButton ID="RadioButton1" runat="server"
                                                            Checked="True" Font-Size="10pt" GroupName="group1" Text="品号" />
                                                        <asp:RadioButton ID="RadioButton2" runat="server" Font-Size="10pt" GroupName="group1"
                                                            Text="流程名称" />
                                                        &nbsp;
                                                        <asp:TextBox ID="txtcondition" runat="server" CssClass="input" Font-Size="10pt" ValidationGroup="group1"
                                                            Width="120px"></asp:TextBox>
                                                        &nbsp;
                                                        <asp:Button ID="btnquery" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" OnClick="btnquery_Click" Text="查询"
                                                            ValidationGroup="group1" Width="70px" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnNew" runat="server" BackColor="Transparent" BorderColor="Gray"
                                                            BorderWidth="1px" Font-Size="10pt" Height="20px" Text="新建流程" ValidationGroup="group1"
                                                            Width="70px" OnClick="lbtNew_Click" />
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
                            <asp:Label ID="lblAccMsg" runat="server" ForeColor="Red" Text="品号或名称已经存在，请更换后再提交！"
                                Visible="False" Font-Size="10pt"></asp:Label>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound"
                                CellPadding="1" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                PageSize="20" EmptyDataText="没有相关记录！" DataKeyNames="ID">
                                <Columns>
                                    <asp:TemplateField HeaderText="流程编号" Visible="false">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtID" runat="server" Text='<%# Bind("ID") %>' Width="100"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Width="100"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="流程名称">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' Width="160"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                Display="None" ErrorMessage="流程名称不能为空!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="160px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="产品品号">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtModelID" runat="server" Text='<%# Bind("ModelID") %>' Width="100"></asp:TextBox>
                                            <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store1" DisplayField="ID" ValueField="ID"
                                                SelectOnFocus="true" ForceSelection="true" Mode="Local">
                                                <SelectedItem Value='<%# DataBinder.Eval(Container.DataItem, "ModelID").ToString() %>' />
                                            </ext:ComboBox>
                                            <%--Value='<%# DataBinder.Eval(Container.DataItem, "ModelID").ToString() %>' Text='<%# DataBinder.Eval(Container.DataItem, "ModelID").ToString() %>'--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtModelID"
                                                Display="None" ErrorMessage="产品品号不能为空!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelID" runat="server" Text='<%# Bind("ModelID") %>' Width="100"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="流程状态">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstate" runat="server" Text='<%# Bind("State") %>' Width="100"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="测试软件">
                                        <ItemTemplate>
                                            <a href="#" onclick="GettestSoft('<%# Eval("ModelID") %>')">查看</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="查看流程">
                                        <EditItemTemplate>
                                            <a href="javascript:updateProcess('<%# Eval("ID") %>','<%# Eval("EditState") %>')">
                                            </a>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <a href="javascript:updateProcess('<%# Eval("ID") %>','<%# Eval("EditState") %>')"
                                                style="color: #0000FF">查看流程</a>
                                            <asp:LinkButton ID="lbBorrow" runat="server" ForeColor="Blue">[借用]</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="操作" ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/images/update.gif" ToolTip="更新" />
                                            &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.gif" ToolTip="取消" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                                ImageUrl="~/images/edit.gif" ToolTip="编辑" />
                                            &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                CommandName="Delete" ImageUrl="~/images/delete.gif" ToolTip="删除" />
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
                </td>
                <td width="5">
                    <img height="7" alt="" src="../../../images/corner_05.gif" width="5" />
                </td>
            </tr>
            <tr>
                <td width="5" height="5">
                    <img height="5" alt="" src="../../../images/corner_06.gif" width="5" />
                </td>
                <td>
                    <img height="5" alt="" src="../../../images/corner_07.gif" width="120" />
                </td>
                <td width="5" height="5">
                    <img height="5" alt="" src="../../../images/corner_08.gif" width="5" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
