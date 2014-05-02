
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function MetaNodeMouseListener(metaNodeModel, wrapper) {
    this.metaNodeModel = metaNodeModel;
    this.wrapper = wrapper;
}
MetaNodeMouseListener.prototype = new MouseListener();
MetaNodeMouseListener.prototype.onMouseDown = function (e) {
    this.moved = false;
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    this.down = true;

    //
    var state = this.wrapper.getStateMonitor().getState();
    switch (state) {
      case StateMonitor.SELECT:
        if (this.metaNodeModel.isEditing()) {
            return;
        }
        var selectedMetaNodeModels = this.wrapper.getModel().getSelectedMetaNodeModels();
        var selectedTransitionModels = this.wrapper.getModel().getSelectedTransitionModels();
        if ((selectedTransitionModels.size() <= 1) && (selectedTransitionModels.size() <= 1) && (!e.ctrlKey)) {
            this.wrapper.getModel().clearSelectedMetaNodeModels();
            this.wrapper.getModel().clearSelectedTransitionModels();
            this.wrapper.getModel().addSelectedMetaNodeModel(this.metaNodeModel);
            //双击节点，可以编辑节点名称；鼠标在节点右下角的点上托拽可以编辑节点尺寸
            this.wrapper.setStatusInfo("\u53cc\u51fb\u8282\u70b9\uff0c\u53ef\u4ee5\u7f16\u8f91\u8282\u70b9\u540d\u79f0\uff1b\u9f20\u6807\u5728\u8282\u70b9\u53f3\u4e0b\u89d2\u7684\u70b9\u4e0a\u6258\u62fd\u53ef\u4ee5\u7f16\u8f91\u8282\u70b9\u5c3a\u5bf8");
        }
        break;
      case StateMonitor.TRANSITION:
        var transitionMonitor = this.wrapper.getTransitionMonitor();
        if (this.metaNodeModel.isNewFromAvailable()) {
            transitionMonitor.setFromMetaNodeModel(this.metaNodeModel);
            //可以从 ×× 建立连接
            this.wrapper.setStatusInfo("\u53ef\u4ee5\u4ece" + this.metaNodeModel + "\u5efa\u7acb\u8fde\u63a5");
        } else {
        	//无法从 ×× 建立连接
            this.wrapper.setStatusInfo("\u65e0\u6cd5\u4ece" + this.metaNodeModel + "\u5efa\u7acb\u8fde\u63a5");
        }
        break;
    }
};
MetaNodeMouseListener.prototype.onMouseMove = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    if (this.down) {
        this.moved = true;
    }
};
MetaNodeMouseListener.prototype.onMouseOver = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    switch (state) {
      case StateMonitor.TRANSITION:
        var transitionMonitor = this.wrapper.getTransitionMonitor();
        var fromMetaNodeModel = transitionMonitor.getFromMetaNodeModel();
        if (fromMetaNodeModel) {
            if ((this.metaNodeModel.isNewToAvailable()) && (fromMetaNodeModel != this.metaNodeModel)) {
                transitionMonitor.setToMetaNodeModel(this.metaNodeModel);
            	//可以到 ×× 建立连接
                this.wrapper.setStatusInfo("\u53ef\u4ee5\u5230" + this.metaNodeModel + "\u5efa\u7acb\u8fde\u63a5");
            } else {
        		//无法到 ×× 建立连接
                this.wrapper.setStatusInfo("\u65e0\u6cd5\u5230" + this.metaNodeModel + "\u5efa\u7acb\u8fde\u63a5");
            }
        }
        break;
    }
};
MetaNodeMouseListener.prototype.onMouseOut = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    switch (state) {
      case StateMonitor.TRANSITION:
        var transitionMonitor = this.wrapper.getTransitionMonitor();
        if (transitionMonitor.getFromMetaNodeModel() !== null) {
            transitionMonitor.setToMetaNodeModel(null);
        	//未找到连接结束点
            this.wrapper.setStatusInfo("\u672a\u627e\u5230\u8fde\u63a5\u7ed3\u675f\u70b9");
        }
        break;
    }
};
MetaNodeMouseListener.prototype.onMouseUp = function (e) {
    this.down = false;
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }

    //
    var state = this.wrapper.getStateMonitor().getState();
    switch (state) {
      case StateMonitor.SELECT:
        if (this.moved) {
            return;
        }
        if (e.ctrlKey) {
            if (this.metaNodeModel.isSelected()) {
                this.wrapper.getModel().removeSelectedMetaNodeModel(this.metaNodeModel);
            } else {
                this.wrapper.getModel().addSelectedMetaNodeModel(this.metaNodeModel);
            }
            //您正在多选节点，按下键盘的方向键，或者托拽鼠标，可以移动选择节点的位置;按下删除按钮或者按下Delete键盘按键，可以删除选择的元
            this.wrapper.setStatusInfo("\u60a8\u6b63\u5728\u591a\u9009\u8282\u70b9\uff0c\u6309\u4e0b\u952e\u76d8\u7684\u65b9\u5411\u952e\uff0c\u6216\u8005\u6258\u62fd\u9f20\u6807\uff0c\u53ef\u4ee5\u79fb\u52a8\u9009\u62e9\u8282\u70b9\u7684\u4f4d\u7f6e;\u6309\u4e0b\u5220\u9664\u6309\u94ae\u6216\u8005\u6309\u4e0bDelete\u952e\u76d8\u6309\u952e\uff0c\u53ef\u4ee5\u5220\u9664\u9009\u62e9\u7684\u5143");
        } else {
            this.wrapper.getModel().clearSelectedMetaNodeModels();
            this.wrapper.getModel().clearSelectedTransitionModels();
            this.wrapper.getModel().addSelectedMetaNodeModel(this.metaNodeModel);
            //双击节点，可以编辑节点名称；鼠标在节点右下角的点上托拽可以编辑节点尺寸
            this.wrapper.setStatusInfo("\u53cc\u51fb\u8282\u70b9\uff0c\u53ef\u4ee5\u7f16\u8f91\u8282\u70b9\u540d\u79f0\uff1b\u9f20\u6807\u5728\u8282\u70b9\u53f3\u4e0b\u89d2\u7684\u70b9\u4e0a\u6258\u62fd\u53ef\u4ee5\u7f16\u8f91\u8282\u70b9\u5c3a\u5bf8");
        }
        break;
      case StateMonitor.TRANSITION:
        var transitionMonitor = this.wrapper.getTransitionMonitor();

    	//
        var fromMetaNodeModel = transitionMonitor.getFromMetaNodeModel();
        var toMetaNodeModel = this.metaNodeModel;
        if ((this.metaNodeModel.isNewToAvailable()) && (fromMetaNodeModel) && (fromMetaNodeModel != toMetaNodeModel)) {
            var transitionModel = new TransitionModel(fromMetaNodeModel, toMetaNodeModel);
            this.wrapper.getModel().addTransitionModel(transitionModel);
            //从 ×× 到 ×× 建立连接成功
            this.wrapper.setStatusInfo("\u4ece" + fromMetaNodeModel + "\u5230" + toMetaNodeModel + "\u5efa\u7acb\u8fde\u63a5\u6210\u529f");
        } else {
            //添加连接失败
            this.wrapper.setStatusInfo("\u6dfb\u52a0\u8fde\u63a5\u5931\u8d25");
        }

    	//
        transitionMonitor.reset();
        break;
    }
};

