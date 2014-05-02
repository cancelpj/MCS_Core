
/**
 * XiorkFlow工作空间，报考建立工作空间所需要的资源。
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */

//
/**
 * XiorkFlow的工作空间
 */
function XiorkFlowWorkSpace() {
}

//
XiorkFlowWorkSpace.BASE_PATH = "/mcsapp/Application/ProcedureManage/ProcedureFlow/";//XiorkFlow
//XiorkFlowWorkSpace.BASE_PATH = "/Application/ProcedureManage/ProcedureFlow/";//XiorkFlow

//
XiorkFlowWorkSpace.XIORK_FLOW_PATH = XiorkFlowWorkSpace.BASE_PATH + "XiorkFlow/";

//
XiorkFlowWorkSpace.DEFAULT_PROCESS_NAME = "default";
//
XiorkFlowWorkSpace.EVENTTIME='&et='+Math.random();
//
XiorkFlowWorkSpace.URL_ADD_PROCESS = XiorkFlowWorkSpace.BASE_PATH + "processes/DealWithXMLData.aspx?op=add"+XiorkFlowWorkSpace.EVENTTIME;
XiorkFlowWorkSpace.URL_DELETE_PROCESS = XiorkFlowWorkSpace.BASE_PATH + "processes/DealWithXMLData.aspx?op=delete"+XiorkFlowWorkSpace.EVENTTIME;
XiorkFlowWorkSpace.URL_GET_PROCESS = XiorkFlowWorkSpace.BASE_PATH + "processes/DealWithXMLData.aspx?op=get"+XiorkFlowWorkSpace.EVENTTIME;
XiorkFlowWorkSpace.URL_LIST_PROCESS = XiorkFlowWorkSpace.BASE_PATH + "processes/DealWithXMLData.aspx?op=list"+XiorkFlowWorkSpace.EVENTTIME;
XiorkFlowWorkSpace.URL_UPDATE_PROCESS = XiorkFlowWorkSpace.BASE_PATH + "processes/DealWithXMLData.aspx?op=update"+XiorkFlowWorkSpace.EVENTTIME;

//
XiorkFlowWorkSpace.STATUS_NULL = -1;
XiorkFlowWorkSpace.STATUS_SUCCESS = 0;
XiorkFlowWorkSpace.STATUS_FAIL = 1;
XiorkFlowWorkSpace.STATUS_FILE_EXIST = 3;
XiorkFlowWorkSpace.STATUS_FILE_NOT_FOUND = 5;
XiorkFlowWorkSpace.STATUS_XML_PARSER_ERROR = 7;
XiorkFlowWorkSpace.STATUS_IO_ERROR = 9;

XiorkFlowWorkSpace.STATUS_MultiStartNode=20;
XiorkFlowWorkSpace.STATUS_MultiEndNode=21;

XiorkFlowWorkSpace.STATUS_NoProcess =22;
XiorkFlowWorkSpace.STATUS_MultiToNode=23;
XiorkFlowWorkSpace.STATUS_MultiFromNode =24;
XiorkFlowWorkSpace.STATUS_NoCloseProcess=25;
XiorkFlowWorkSpace.STATUS_HASLOOP=26;
XiorkFlowWorkSpace.STATUS_MultiSamePName=27;
XiorkFlowWorkSpace.STATUS_MultiSameRange=28;

//
XiorkFlowWorkSpace.ID = 1;

//
XiorkFlowWorkSpace.META_NODE_MOVED_STEP_TIME = 100;
XiorkFlowWorkSpace.META_NODE_MOVED_STEP = 1;

//
XiorkFlowWorkSpace.META_NODE_MAX = 2000;//节点数目

//
XiorkFlowWorkSpace.META_NODE_MIN_WIDTH = 80;
XiorkFlowWorkSpace.META_NODE_MIN_HEIGHT = 30;

//
/**
 * 建立工作空间
 */
XiorkFlowWorkSpace.build = function () {
 	//引入所需要的资源，资源加载顺序不能更改

	//name.xio.util
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/Message.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/Array.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/String.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/List.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/Observable.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/util/Observer.js");

	//name.xio.geom
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/geom/Point.js");

	//name.xio.html
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/html/Toolkit.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/html/Browser.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/html/Cursor.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/html/MouseEvent.js");

	//name.xio.xml
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xml/XMLDocument.js");

	//name.xio.ajax
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ajax/Ajax.js");

	//name.xio.ui.event
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/event/KeyListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/event/MouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/event/MouseWheelListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/event/ContextMenuListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/event/ListenerProxy.js");

	//name.xio.ui
    BuildLibrary.loadCSS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ui.css");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Dimension.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Component.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Button.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ButtonModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ToggleButton.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ToggleButtonModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ButtonGroup.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ToolBar.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Panel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/ScrollPanel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Label.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/ui/Frame.js");

	//name.xio.geom.ui
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/geom/ui/GeometryCanvas.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/geom/ui/LineView.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/geom/ui/LineTextView.js");

    //name.xio.xiorkflow.meta.event
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/MetaNodeMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/MetaNodeTextMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/MetaNodeTextKeyListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/TransitionMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/MetaNodeResizeMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/TransitionTextMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/event/TransitionTextKeyListener.js");

    //name.xio.xiorkflow.meta
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/DragablePanel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/MetaModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/MetaNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/MetaNode.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/StartNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/StartNode.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/EndNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/EndNode.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/NodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/Node.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/ForkNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/ForkNode.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/JoinNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/JoinNode.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/TransitionCompass.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/TransitionModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/Transition.js");
    
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/EmptyMetaNode.js");//men add      
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/EmptyNodeModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/meta/EmptyNode.js");//men add
  
    //name.xio.xiorkflow.event
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/WrapperMetaMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/WrapperSelectMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/MetaMoveMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/MetaMoveKeyListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/WrapperTransitionMouseListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/DeleteMetaActionListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/SaveActionListener.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/event/HelpActionListener.js");

    //name.xio.xiorkflow.process
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/process/AddProcess.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/process/GetProcess.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/process/UpdateProcess.js");

    //name.xio.xiorkflow
    BuildLibrary.loadCSS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/xiorkflow.css");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowToolBar.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/StateMonitor.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/TransitionMonitor.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowViewer.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowTableViewer.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowXMLViewer.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/StatusLabel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowModel.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowModelConverter.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlow.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowWrapper.js");
    BuildLibrary.loadJS(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "src/name/xio/xiorkflow/XiorkFlowViewPattern.js");
};

//
/**
 * 资源加载
 */
function BuildLibrary() {
}
BuildLibrary.loadJS = function (url, charset) {
    if (!charset) {
        charset = "UTF-8";
    }
    var charsetProperty = " charset=\"" + charset + "\" ";
    document.write("<script type=\"text/javascript\" src=\"" + url + "\" onerror=\"alert('Error loading ' + this.src);\"" + charsetProperty + "></script>");
};
BuildLibrary.loadCSS = function (url, charset) {
    if (!charset) {
        charset = "UTF-8";
    }
    var charsetProperty = " charset=\"" + charset + "\" ";
    document.write("<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\" onerror=\"alert('Error loading ' + this.src);\"" + charsetProperty + "/>");
};

