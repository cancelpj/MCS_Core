<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcedureScan.aspx.cs"
    Inherits="MCSApp.Application.ProcedureManage.ProcedureScan" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>条码扫描</title>
        <script type="text/javascript" src="../../scripts/JSDate.js"></script>
        <script src="../../scripts/CustomWin.js"></script>
    <script type="text/javascript">
    function focus()
    {
      document.getElementById('Text1').focus();
    }
    </script>
    
    <style type="text/css">
    .lab:{
            color :Blue;
        }
    </style>
    

</head>
<body onload="focus()">
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server"  StateProvider="None" />
    
    
    <table style="font-size:12px;"><tr>
        
    <td>条码：<ext:Hidden ID="Hidden_s" runat="server" Text="s">
        </ext:Hidden></td>
    <td>
    <input id="Text1" name='Text1' type="text" onblur="this.focus();" onkeyup="if(this.value.length==20) Coolite.AjaxMethods.test1(this.value,{success: function(result){Text1.value='';Text1.focus();}, failure: function(errorMsg){ Ext.Msg.alert('<font color=red>服务器通信错误</font>', errorMsg+'<br>网络中断、接口服务器未运行均可产生类似错误，请刷新网页重试。');}});" />
    </td>
    
    <td>班组：</td>
    <td style="color:Blue"><ext:Label ID="Label1" runat="server" Text="待定"></ext:Label><ext:Hidden ID="Hidden1" runat="server" Text="待定"></ext:Hidden></td>
    <td>&nbsp; 计划：</td>
    <td style="color:Blue"><ext:Label ID="Label2" runat="server" Text="待定"></ext:Label><ext:Hidden ID="Hidden2" runat="server" Text="待定"></ext:Hidden></td>
    <td>&nbsp; 工序：</td>
    <td style="color:Blue">
        <ext:ComboBox ID="ComboBox1" runat="server" Width="55px">
           <SelectedItem Text="待定" Value="待定" />        
            <Items>
            <ext:ListItem Text="待定" Value="待定" />            
            </Items>
        </ext:ComboBox>
    
    </td>
        <td>&nbsp; 流向：</td>
    <td style="color:Blue">
           <ext:ComboBox ID="ComboBox2" runat="server" Width="55px">
           <SelectedItem Text="待定" Value="待定" />
            <Items>
            <ext:ListItem Text="待定" Value="待定" />
            <ext:ListItem Text="入" Value="入" />
            <ext:ListItem Text="出" Value="出" />
            </Items>
        </ext:ComboBox>
    
    </td>
    </tr></table>
    
&nbsp;
        <ext:Store ID="Store2" runat="server" OnRefreshData="MyData_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="id" />
                        <ext:RecordField Name="barcode" Type="String" />
                        <ext:RecordField Name="planId" Type="String" />
                        <ext:RecordField Name="modelId" Type="String" />
                        <ext:RecordField Name="process" Type="String" />
                        <ext:RecordField Name="user" Type="String" />
                        
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>
        
        <ext:GridPanel ID="GridPanel2" 
            runat="server" 
            StoreID="Store2" 
            StripeRows="true"
            Title="已扫描的记录" 
            Width="600" 
            Height="400"
            AutoExpandColumn="Id">
            <ColumnModel ID="ColumnModel2" runat="server">
                <Columns>
                    <ext:Column ColumnID="Id" Header="编号" Width="20" Sortable="true" DataIndex="id" />
                    <ext:Column Header="条码" Width="160" Sortable="true" DataIndex="barcode">
                        
                    </ext:Column>
                    <ext:Column Header="计划号" Width="75" Sortable="true" DataIndex="planId">
                        
                    </ext:Column>
                    <ext:Column Header="品号" Width="75" Sortable="true" DataIndex="modelId">
                        
                    </ext:Column>
                    <ext:Column Header="工序" Width="75" Sortable="true" DataIndex="process">
                        
                    </ext:Column>
                    <ext:Column Header="用户" Sortable="true" DataIndex="user">
                        
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" />
            </SelectionModel>
            <LoadMask ShowMask="true" />
            <BottomBar>
            <ext:StatusBar ID="StatusBar1" runat="server" ></ext:StatusBar>
                <%--<ext:PagingToolBar ID="PagingToolBar2" runat="server" PageSize="35" StoreID="Store2" />--%>
            </BottomBar>
        </ext:GridPanel>

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    </form>
</body>
</html>
