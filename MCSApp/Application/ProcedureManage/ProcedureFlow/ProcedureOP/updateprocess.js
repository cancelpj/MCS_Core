
//

XiorkFlowWorkSpace.build();
var xiorkFlow;
var readonly = true;
//function click(e) {
//    if (document.all) 
//    { 
//       if (event.button==2) //右健
//       { 
////             var metaNodeModels = xiorkFlow.getWrapper().getModel().getMetaNodeModels();//元结点模型集
////             for (var i = 0; i < metaNodeModels.size(); i++) 
////             {
////               var metaNodeModel = metaNodeModels.get(i);//元结点模型
////		       //metaNodeModel.setSelected('True');//可以通过得到元节点模型来设置选中与否的属性
////                alert(metaNodeModel.getID());
////             }
//             var selectedNodes=xiorkFlow.getWrapper().getModel().getSelectedMetaNodeModels();
//             if(selectedNodes.size()>0)
//             {
//                 var selectedNode=selectedNodes[0];
//                 alert(selectedNode.getID());
//                 //tsaiannie(selectedNode.getID());   
//             }
//             
//             oncontextmenu = "return false;";
//       }
//    } 
//}

//document.onmousedown=click;
//document.oncontextmenu = new Function("return false;") 
//
function init() {
	//
     xiorkFlow= new XiorkFlow(Toolkit.getElementByID("designer"));

	//
    document.body.onbeforeunload = function () {
        if (xiorkFlow.getWrapper().isChanged()) {
    	    //您对工作流程图的编辑尚未保存，离开该页面将退出系统!
            window.event.returnValue = "\u60a8\u5bf9\u5de5\u4f5c\u6d41\u7a0b\u56fe\u7684\u7f16\u8f91\u5c1a\u672a\u4fdd\u5b58\uff0c\u79bb\u5f00\u8be5\u9875\u9762\u5c06\u9000\u51fa\u7cfb\u7edf!";
        }
    };

    //
    xiorkFlow.getToolBar().setButtonEnable(false);

	//
    var searchStr = window.location.search;
    var params = searchStr.substring(searchStr.indexOf("?") + 1);
    var paramsArray = params.split("&");

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
    if (name) {
        var getProcess = new GetProcess(xiorkFlow.getWrapper(), xiorkFlow.getTableViewer(), xiorkFlow.getToolBar());
        getProcess.getProcess(name);
        
    } else {
        //您所要编辑的工作流程图名字为空，无法进行编辑。系统自动转成添加模式。
        alert("\u60a8\u6240\u8981\u7f16\u8f91\u7684\u5de5\u4f5c\u6d41\u7a0b\u56fe\u540d\u5b57\u4e3a\u7a7a\uff0c\u65e0\u6cd5\u8fdb\u884c\u7f16\u8f91\u3002\u7cfb\u7edf\u81ea\u52a8\u8f6c\u6210\u6dfb\u52a0\u6a21\u5f0f\u3002");
        document.title = "\u589e\u52a0";
        xiorkFlow.getToolBar().setButtonEnable(true);
    }
}


