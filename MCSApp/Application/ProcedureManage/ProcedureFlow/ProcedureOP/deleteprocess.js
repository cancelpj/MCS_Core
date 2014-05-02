
//
function deleteInit() {
	//
    var searchStr = window.location.search;
    var params = searchStr.substring(searchStr.indexOf("?") + 1);
    var paramsArray = params.split("&");
    var name = null;
    for (var i = 0; i < paramsArray.size(); i++) {
        var tempStr = paramsArray.get(i);
        var key = tempStr.substring(0, tempStr.indexOf("=")).toLowerCase();
        var value = tempStr.substring(tempStr.indexOf("=") + 1);
        if (key == "name") {
            name = value;
            break;
        }
    }
    deleteProcess(name);
}

//
function deleteProcess(name) {
    //
    if (name) {
    } else {
        //您所要删除的工作流程图名字为空，无法进行删除。
        alert("\u60a8\u6240\u8981\u5220\u9664\u7684\u5de5\u4f5c\u6d41\u7a0b\u56fe\u540d\u5b57\u4e3a\u7a7a\uff0c\u65e0\u6cd5\u8fdb\u884c\u5220\u9664\u3002");
    }

    //工作流程图删除后将无法恢复，您是否继续进行？
    if (window.confirm("\u5de5\u4f5c\u6d41\u7a0b\u56fe\u5220\u9664\u540e\u5c06\u65e0\u6cd5\u6062\u590d\uff0c\u60a8\u662f\u5426\u7ee7\u7eed\u8fdb\u884c\uff1f")) {
        var deleteProcessAjax = new DeleteProcessAjax();
        deleteProcessAjax.deleteProcess(name);
    }
}

//
/**
 * 删除工作流程图
 */
function DeleteProcessAjax() {
    this.base = Ajax;
    this.base();
}
DeleteProcessAjax.prototype = new Ajax();
DeleteProcessAjax.prototype.deleteProcess = function (name) {
    var url = XiorkFlowWorkSpace.URL_DELETE_PROCESS;
    var method = "POST";
    var params = "name=" + name;
    this.loadXMLHttpRequest(url, method, params);
};
DeleteProcessAjax.prototype.processXMLHttpRequest = function (httpRequest) {
    var doc = httpRequest.responseXML;
    if (!doc) {
    	//操作结束，未知服务器处理结果！
        alert("\u64cd\u4f5c\u7ed3\u675f\uff0c\u672a\u77e5\u670d\u52a1\u5668\u5904\u7406\u7ed3\u679c\uff01");
        //
        if (refreshProcessList) {
            refreshProcessList();
        }
        return false;
    }

    //
    var responseNode = doc.getElementsByTagName("Response")[0];
    var statusValue = eval(responseNode.getAttribute("status"));
    var alertStr = "";
    switch (statusValue) {
      case XiorkFlowWorkSpace.STATUS_SUCCESS:
      	//删除成功。
        alertStr = "\u5220\u9664\u6210\u529f\u3002";
        //
        if (refreshProcessList) {
            refreshProcessList();
        }
        break;
      case XiorkFlowWorkSpace.STATUS_FAIL:
      	//删除失败。
        alertStr = "\u5220\u9664\u5931\u8d25\u3002";
        break;
      case XiorkFlowWorkSpace.STATUS_FILE_NOT_FOUND:
      	//删除失败，文件未找到。
        alertStr = "\u5220\u9664\u5931\u8d25\uff0c\u6587\u4ef6\u672a\u627e\u5230\u3002";
        if (refreshProcessList) {
            refreshProcessList();
        }
        break;
      default:
        //删除失败，未知错误。
        alertStr = "\u5220\u9664\u5931\u8d25\uff0c\u672a\u77e5\u9519\u8bef\u3002";
    	//
        if (refreshProcessList) {
            refreshProcessList();
        }
        break;
    }
    alert(alertStr);
};

