
//
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function MetaMoveMouseListener(wrapper) {
    this.wrapper = wrapper;
    this.lastCoord = null;
}
MetaMoveMouseListener.prototype = new MouseListener();
MetaMoveMouseListener.prototype.onMouseDown = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }

    //
    this.lastCoord = Toolkit.getContainerCoord(e, this.wrapper.getViewer());
    this.lastTime = new Date();
};
MetaMoveMouseListener.prototype.onMouseMove = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }

    //
    if (this.lastCoord) {
        var curTime = new Date();
        if ((curTime - this.lastTime) > XiorkFlowWorkSpace.META_NODE_MOVED_STEP_TIME) {
            this.move(e);
            this.lastTime = curTime;
        }
    }
};
MetaMoveMouseListener.prototype.onMouseUp = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }

    //
    if (this.lastCoord) {
        this.move(e);
    }

    //
    this.lastCoord = null;
};
MetaMoveMouseListener.prototype.move = function (e) {
    var curCoord = Toolkit.getContainerCoord(e, this.wrapper.getViewer());

    //
    var metaNodeModels = this.wrapper.getModel().getSelectedMetaNodeModels();
    XiorkFlowModel.moveMetaNodeModelBy(metaNodeModels, curCoord.getX() - this.lastCoord.getX(), curCoord.getY() - this.lastCoord.getY());

    //TODO transition

    //
    this.lastCoord = curCoord;
};

