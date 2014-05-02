<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataDetail.aspx.cs" Inherits="MCSApp.Application.QueryManage.DataDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>工序记录数据</title>
    <link href="../../css/style.css" type="text/css" rel="stylesheet">
    <script src="../../scripts/CustomWin.js" language=javascript></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
               <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" Width="100%" 
                       PageSize="20" EmptyDataText="没有相关记录！" 
                       onrowdatabound="GridView1_RowDataBound" >
                    <Columns>                                                                                                                                                           
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
    </div>
    </form>
</body>
</html>
