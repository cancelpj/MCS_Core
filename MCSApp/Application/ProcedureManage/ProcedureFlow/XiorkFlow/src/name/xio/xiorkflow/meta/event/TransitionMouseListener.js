
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function TransitionMouseListener(transitionModel, wrapper) {
    this.transitionModel = transitionModel;
    this.wrapper = wrapper;
}
TransitionMouseListener.prototype = new MouseListener();

//
TransitionMouseListener.prototype.onMouseDown = function (e) {
    this.moved = false;
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    this.down = true;

    //
    var state = this.wrapper.getStateMonitor().getState();
    switch (state) {
      case StateMonitor.SELECT:
        var selectedMetaNodeModels = this.wrapper.getModel().getSelectedMetaNodeModels();
        var selectedTransitionModels = this.wrapper.getModel().getSelectedTransitionModels();
        if ((selectedTransitionModels.size() <= 1) && (selectedMetaNodeModels.size() <= 1) && (!e.ctrlKey)) {
            this.wrapper.getModel().clearSelectedMetaNodeModels();
            this.wrapper.getModel().clearSelectedTransitionModels();
            this.wrapper.getModel().addSelectedTransitionModel(this.transitionModel);
            //双击连接线，可以编辑连接名称
            this.wrapper.setStatusInfo("\u53cc\u51fb\u8fde\u63a5\u7ebf\uff0c\u53ef\u4ee5\u7f16\u8f91\u8fde\u63a5\u540d\u79f0");
        }
        break;
      case StateMonitor.TRANSITION:
        break;
    }
};
TransitionMouseListener.prototype.onMouseMove = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    if (this.down) {
        this.moved = true;
    }
};
TransitionMouseListener.prototype.onMouseUp = function (e) {
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
            if (this.transitionModel.isSelected()) {
                this.wrapper.getModel().removeSelectedTransitionModel(this.transitionModel);
            } else {
                this.wrapper.getModel().addSelectedTransitionModel(this.transitionModel);
            }
        } else {
            this.wrapper.getModel().clearSelectedMetaNodeModels();
            this.wrapper.getModel().clearSelectedTransitionModels();
            this.wrapper.getModel().addSelectedTransitionModel(this.transitionModel);
            //双击连接线，可以编辑连接名称
            this.wrapper.setStatusInfo("\u53cc\u51fb\u8fde\u63a5\u7ebf\uff0c\u53ef\u4ee5\u7f16\u8f91\u8fde\u63a5\u540d\u79f0");
        }
        break;
    }
};

