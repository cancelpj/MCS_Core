<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallWebService.aspx.cs"
    Inherits="MCSApp.WebForm3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WebService调试用例</title>
</head>

<script type="text/javascript" language="javascript">
    var iCallID;    //呼叫WebService队列号
    function init() {
        iMCS.useService("http://localhost/iMCS/Security.asmx?wsdl", "Security");
        iCallID = iMCS.Security.callService("HelloWorld");
    }
    function onWSresult() {
        if ((event.result.error) && (iCallID == event.result.id)) {     //先根据队列号确实是否对应的返回值
            var xfaultcode = event.result.errorDetail.code;
            var xfaultstring = event.result.errorDetail.string;
            var xfaultsoap = event.result.errorDetail.raw;

            // Add code to output error information here
        }
        else {
            alert("The method returned the result : " + event.result.value);
        }
    }
</script>

<body onload="init()">
    <div id="iMCS" style="behavior: url(scripts/webservice.htc)" onresult="onWSresult()">
    </div>
</body>
</html>
