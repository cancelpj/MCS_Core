<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="MCSApp.WebForm2" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';

        var change = function(value) {
            return String.format(template, (value > 0) ? 'green' : 'red', value);
        }

        var pctChange = function(value) {
            return String.format(template, (value > 0) ? 'green' : 'red', value + '%');
        }
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" StateProvider="None" />

  <ext:Store ID="Store1" runat="server" >
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="company" />
                        <ext:RecordField Name="price" Type="Float" />
                        <ext:RecordField Name="change" Type="Float" />
                        <ext:RecordField Name="pctChange" Type="Float" />
                        <ext:RecordField Name="lastChange" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>
        
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Button" />
        
        <ext:GridPanel 
            runat="server" 
            StoreID="Store1" 
            StripeRows="true"
            Title="Array Grid" 
            Width="600" 
            Height="290"
            AutoExpandColumn="Company">
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column ColumnID="Company" Header="Company" Width="160" Sortable="true" DataIndex="company" />
                    <ext:Column Header="Price" Width="75" Sortable="true" DataIndex="price">
                        <Renderer Format="UsMoney" />
                    </ext:Column>
                    <ext:Column Header="Change" Width="75" Sortable="true" DataIndex="change">
                        <Renderer Fn="change" />
                    </ext:Column>
                    <ext:Column Header="Change" Width="75" Sortable="true" DataIndex="pctChange">
                        <Renderer Fn="pctChange" />
                    </ext:Column>
                    <ext:Column Header="Last Updated" Width="85" Sortable="true" DataIndex="lastChange">
                        <Renderer Fn="Ext.util.Format.dateRenderer('G:i:s')" />
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" />
            </SelectionModel>
            <LoadMask ShowMask="true" />
            <BottomBar>
                <ext:PagingToolBar runat="server" PageSize="10" StoreID="Store1" />
            </BottomBar>
        </ext:GridPanel>
    
    </form>
</body>
</html>
