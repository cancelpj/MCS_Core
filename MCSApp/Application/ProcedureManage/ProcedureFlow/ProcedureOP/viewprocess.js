//
XiorkFlowWorkSpace.build();

//
function init() {
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

    //
    if (!name) {
    	//您所要浏览的工作流程图名字为空，无法浏览。
        alert("\u60a8\u6240\u8981\u6d4f\u89c8\u7684\u5de5\u4f5c\u6d41\u7a0b\u56fe\u540d\u5b57\u4e3a\u7a7a\uff0c\u65e0\u6cd5\u6d4f\u89c8\u3002");
        return false;
    }

    //
    var xiorkFlowViewPattern = new XiorkFlowViewPattern(Toolkit.getElementByID("designer"));
    xiorkFlowViewPattern.getWrapper().getModel().setEditable(false);
    var getProcess = new GetProcess(xiorkFlowViewPattern.getWrapper(), xiorkFlowViewPattern.getTableViewer(), xiorkFlowViewPattern.getToolBar());
    getProcess.getProcess(name);
    /*var model = new XiorkFlowModel();
    model.setEditable(false);
    var wrapper = new XiorkFlowWrapper(new Component(document.getElementById("designer")), model);
    var tableViewer = XiorkFlowTableViewer(model, document.getElementById("tableViewer"));
    var getProcess = new GetProcess(wrapper, tableViewer);
    getProcess.getProcess(name);*/
}

