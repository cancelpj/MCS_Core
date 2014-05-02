
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */

//工作流程列表
var processList;

//
function openAddProcess() {
    window.showModalDialog("addprocess.html", processList, "dialogWidth:800px;dialogHeight:600px;center:yes;help:no;scroll:no;status:no;resizable:yes;");
}

//
function viewProcess(name) {
    window.showModalDialog("viewprocess.html?name=" + name, processList, "dialogWidth:800px;dialogHeight:600px;center:yes;help:no;scroll:no;status:no;resizable:yes;");
}

//
function updateProcess(name) {
    window.showModalDialog("updateprocess.html?name=" + name, processList, "dialogWidth:800px;dialogHeight:600px;center:yes;help:no;scroll:no;status:no;resizable:yes;");
}

//
function deleteProcess(name) {
    deleteProcess(name);
}

//
function init() {
    var processListTable = Toolkit.getElementByID("processlist");

    //
    processList = new ProcessList(processListTable);
    processList.refreshProcessList();

    //
    processListTable.refresh = function () {
        refreshProcessList();
    };
}

//
function refreshProcessList() {
    if (processList) {
        processList.refreshProcessList();
    }
}

//
/**
 * 工作流列表
 */
function ProcessList(processListTable) {
    this.processListTable = processListTable;
    this.processListAjax = new ProcessListAjax(this);
}
ProcessList.prototype.resetProcessBody = function () {
    if (!this.processListTable) {
        return;
    }
    Toolkit.clearElement(this.processListTable);

    //
    var row = this.processListTable.insertRow();

    //工作流程图xml文件名
    var nameCell = row.insertCell();
    nameCell.className = "head name";
    nameCell.innerText = "\u5de5\u4f5c\u6d41\u7a0b\u56fexml\u6587\u4ef6\u540d";
    //
    var viewCell = row.insertCell();
    viewCell.className = "head view";
    viewCell.innerText = " ";
    //
    var delCell = row.insertCell();
    delCell.className = "head del";
    delCell.innerText = " ";
    //
    var editCell = row.insertCell();
    editCell.className = "head edit";
    editCell.innerText = " ";

    //
    var defaultRow = this.processListTable.insertRow();
    var defaultProcessName = "default";
    //工作流程图范例
    ProcessList._insertProcessRow(defaultRow, "\u5de5\u4f5c\u6d41\u7a0b\u56fe\u8303\u4f8b", defaultProcessName, true, false, false);
};
ProcessList.prototype.refreshProcessList = function () {
    this.resetProcessBody();

    //
    this.processListAjax.refresh();
};
ProcessList.prototype.insertProcessRow = function (processName) {
    if (!processName) {
        return;
    }
    if (processName.equals(XiorkFlowWorkSpace.DEFAULT_PROCESS_NAME)) {
        return;
    }
    var processRow = this.processListTable.insertRow();
    ProcessList._insertProcessRow(processRow, processName, processName);
};
ProcessList._insertProcessRow = function (row, text, processName, viewable, delalbe, editable) {
    processName = processName ? processName : " ";
    viewable = viewable == null ? true : viewable;
    delalbe = delalbe == null ? true : delalbe;
    editable = editable == null ? true : editable;

    //
    var processNameCell = row.insertCell();
    processNameCell.className = "body name";
    processNameCell.innerText = text;

    //
    var viewCell = row.insertCell();
    viewCell.className = "body view";
    viewCell.align = "center";
    if (viewable) {
        var viewA = Toolkit.newElement("a");
        viewA.href = "viewprocess.html?name=" + processName;
        //浏览
        viewA.innerText = "\u6d4f\u89c8";
        viewCell.appendChild(viewA);

        //
        viewA.onclick = function () {
            viewProcess(processName);
            return false;
        };
    } else {
        viewCell.innerText = " ";
    }

    //
    var delCell = row.insertCell();
    delCell.className = "body del";
    delCell.align = "center";
    if (delalbe) {
        var delA = Toolkit.newElement("a");
        delA.href = "deleteprocess.html?name=" + processName;
        //删除
        delA.innerText = "\u5220\u9664";
        delCell.appendChild(delA);

        //
        delA.onclick = function () {
            deleteProcess(processName);
            return false;
        };
    } else {
        delCell.innerText = " ";
    }

    //
    var editCell = row.insertCell();
    editCell.className = "body edit";
    editCell.align = "center";
    if (editable) {
        var editA = Toolkit.newElement("a");
        editA.href = "updateprocess.html?name=" + processName;
        //编辑
        editA.innerText = "\u7f16\u8f91";
        editCell.appendChild(editA);

        //
        editA.onclick = function () {
            updateProcess(processName);
            return false;
        };
    } else {
        editCell.innerText = " ";
    }
};

//
/**
 * 工作流列表的ajax对象
 */
function ProcessListAjax(processList) {
    this.base = Ajax;
    this.base();
    this.processList = processList;
}
ProcessListAjax.prototype = new Ajax();
ProcessListAjax.prototype.refresh = function () {
    var url = XiorkFlowWorkSpace.URL_LIST_PROCESS;
    var method = "GET";
    var params = null;
    this.loadXMLHttpRequest(url, method, params);
};
ProcessListAjax.prototype.processXMLHttpRequest = function (httpRequest) {
    var doc = httpRequest.responseXML;
    var fileNodes = doc.getElementsByTagName("File");
    for (var i = 0; i < fileNodes.length; i++) {
        var fileNode = fileNodes[i];
        var processName = fileNode.getAttribute("name");
        this.processList.insertProcessRow(processName);
    }
};

