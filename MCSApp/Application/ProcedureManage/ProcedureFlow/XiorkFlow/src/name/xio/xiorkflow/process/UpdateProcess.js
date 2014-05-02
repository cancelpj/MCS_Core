
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function UpdateProcess(wrapper, toolbar) {
    this.base = Ajax;
    this.base();
    this.wrapper = wrapper;
    this.toolbar = toolbar;
}
UpdateProcess.prototype = new Ajax();
UpdateProcess.prototype.setButtonEnable = function (b) {
    if (this.toolbar) {
        this.toolbar.setButtonEnable(b);
    }
};
UpdateProcess.prototype.updateProcess = function () {
    var model = this.wrapper.getModel();
    var name = model.getName();
    if (!name) {
        this.name = null;
    	//工作流程图名字为空！
        alert("\u5de5\u4f5c\u6d41\u7a0b\u56fe\u540d\u5b57\u4e3a\u7a7a\uff01");
        return false;
    }

    //
    this.setButtonEnable(false);

    //
    var doc = XiorkFlowModelConverter.convertModelToXML(model);
    if (!doc) {
    	//将工作流程图转化成xml时出错！
        alert("\u5c06\u5de5\u4f5c\u6d41\u7a0b\u56fe\u8f6c\u5316\u6210xml\u65f6\u51fa\u9519\uff01");
        this.setButtonEnable(true);
        return false;
    }

    //
    var url = XiorkFlowWorkSpace.URL_UPDATE_PROCESS;
    var method = "POST";
    var params = "name=" + name;
    params += "&xml=" + doc.xml;

    //
    this.loadXMLHttpRequest(url, method, params);
};
UpdateProcess.prototype.onReadyStateChange = function (httpRequest) {
    if (httpRequest.readyState == 4) {
        if (httpRequest.status == 200) {
            this.processXMLHttpRequest(httpRequest);
        } else {
        	//处理过程出现错误！
            alert("\u5904\u7406\u8fc7\u7a0b\u51fa\u73b0\u9519\u8bef\uff01");
            this.setButtonEnable(true);
        }
    }
};
UpdateProcess.prototype.processXMLHttpRequest = function (httpRequest) {
    var doc = httpRequest.responseXML;
    if (!doc) {
    	//操作结束，未知服务器处理结果！
        alert("\u64cd\u4f5c\u7ed3\u675f\uff0c\u672a\u77e5\u670d\u52a1\u5668\u5904\u7406\u7ed3\u679c\uff01");
        this.setButtonEnable(true);
        return false;
    }

    //
    var responseNode = doc.getElementsByTagName("Response")[0];
    var EventNode=doc.getElementsByTagName("Response")[0];
    var EventName="";
    if(EventNode)
    {
       EventName = '"'+EventNode.getAttribute("name")+'"';//eval()        
    }
    var statusValue = eval(responseNode.getAttribute("status"));
    var alertStr = "";
    switch (statusValue) {
      case XiorkFlowWorkSpace.STATUS_SUCCESS:
      	//更新成功。
        alertStr = "\u66f4\u65b0\u6210\u529f\u3002";
        this.wrapper.getModel().setName(this.name);
        this.wrapper.setChanged(false);
        break;
      case XiorkFlowWorkSpace.STATUS_FAIL:
      	//更新失败。
        alertStr = "\u66f4\u65b0\u5931\u8d25\u3002";
        break;
      case XiorkFlowWorkSpace.STATUS_XML_PARSER_ERROR:
      	//更新失败，xml解析出错。
        alertStr = "\u66f4\u65b0\u5931\u8d25\uff0cxml\u89e3\u6790\u51fa\u9519\u3002";
        break;
      case XiorkFlowWorkSpace.STATUS_FILE_NOT_FOUND:
      	//更新失败，文件未找到。系统自动转成添加模式。
        document.title = "\u589e\u52a0";
        alertStr = "\u66f4\u65b0\u5931\u8d25\uff0c\u6587\u4ef6\u672a\u627e\u5230\u3002\u7cfb\u7edf\u81ea\u52a8\u8f6c\u6210\u6dfb\u52a0\u6a21\u5f0f\u3002";
        break;
      case XiorkFlowWorkSpace.STATUS_IO_ERROR:
      	//更新失败，IO错误。
        alertStr = "\u66f4\u65b0\u5931\u8d25\uff0cIO\u9519\u8bef\u3002";
        break;
        
      case XiorkFlowWorkSpace.STATUS_MultiStartNode:
      	//更新失败，开始节点过多，应为1个。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u5F00\u59CB\u8282\u70B9\u8FC7\u591A\uFF0C\u5E94\u4E3A\u0031\u4E2A\u3002";
        break; 
        
      case XiorkFlowWorkSpace.STATUS_MultiEndNode:
      	//更新失败，结束节点过多，应为1个。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u7ED3\u675F\u8282\u70B9\u8FC7\u591A\uFF0C\u5E94\u4E3A\u0031\u4E2A\u3002";
        break;
         
      case XiorkFlowWorkSpace.STATUS_NoProcess:
      	//更新失败，不能只有开始或结束工序，应有中间工序
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u4E0D\u80FD\u53EA\u6709\u5F00\u59CB\u4E0E\u7ED3\u675F\u5DE5\u5E8F\uFF0C\u5E94\u6709\u4E2D\u95F4\u5DE5\u5E8F\u3002\u68C0\u67E5\u5DE5\u5E8F"+EventName+"\u65F6\u53D1\u751F\u9519\u8BEF\u0021";
        break;
      case XiorkFlowWorkSpace.STATUS_MultiToNode:
      	//更新失败，目的工序节点数目超过20。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u76EE\u7684\u5DE5\u5E8F\u8282\u70B9\u6570\u76EE\u8D85\u8FC7\u0032\u0030\u3002\u68C0\u67E5\u5DE5\u5E8F"+EventName+"\u65F6\u53D1\u751F\u9519\u8BEF\u0021";
        break;
      case XiorkFlowWorkSpace.STATUS_MultiFromNode:
      	//更新失败，源工序节点数目超过20。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u6E90\u5DE5\u5E8F\u8282\u70B9\u6570\u76EE\u8D85\u8FC7\u0032\u0030\u3002\u68C0\u67E5\u5DE5\u5E8F"+EventName+"\u65F6\u53D1\u751F\u9519\u8BEF\u0021";
        break;
      case XiorkFlowWorkSpace.STATUS_NoCloseProcess:
      	//更新失败，有向图中存在孤立的工序节点。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u6709\u5411\u56FE\u4E2D\u5B58\u5728\u5B64\u7ACB\u7684\u5DE5\u5E8F\u8282\u70B9\u3002\u68C0\u67E5\u5DE5\u5E8F"+EventName+"\u65F6\u53D1\u751F\u9519\u8BEF\u0021";
        break;
      case XiorkFlowWorkSpace.STATUS_HASLOOP:
      	//更新失败，有向图中存在闭合环路。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u6709\u5411\u56FE\u4E2D\u5B58\u5728\u95ED\u5408\u73AF\u8DEF\u3002";
        break; 
      case XiorkFlowWorkSpace.STATUS_MultiSamePName:
      	//更新失败，有向图中工序名称重复。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u6709\u5411\u56FE\u4E2D\u5DE5\u5E8F\u540D\u79F0\u91CD\u590D\u3002";
        break;         
      case XiorkFlowWorkSpace.STATUS_MultiSameRange:
      	//更新失败，有向图中工序类别重复。
        alertStr = "\u66F4\u65B0\u5931\u8D25\uFF0C\u6709\u5411\u56FE\u4E2D\u5DE5\u5E8F\u7C7B\u522B\u91CD\u590D\u3002";
        break;         
        
                    
      default:
        //更新失败，未知错误。
        alertStr = "\u66f4\u65b0\u5931\u8d25\uff0c\u672a\u77e5\u9519\u8bef\u3002";
        break;
    }
    this.setButtonEnable(true);
    alert(alertStr);
    
};


