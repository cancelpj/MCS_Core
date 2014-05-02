
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function MetaNodeResizeMouseListener(retangle, metaNodeModel, wrapper) {
    this.retangle = retangle;
    this.metaNodeModel = metaNodeModel;
    this.wrapper = wrapper;
}
MetaNodeResizeMouseListener.prototype = new MouseListener();
MetaNodeResizeMouseListener.prototype.onMouseDown = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }

    //
    this.metaNodeModel.setResizing(true);

    //
    this.wrapper.getModel().clearSelectedMetaNodeModels();
    this.wrapper.getModel().clearSelectedTransitionModels();

    //
    this.lastCoord = Toolkit.getContainerCoord(e, this.wrapper.getViewer());
    this.lastTime = new Date();

    //
    this.retangle.getUI().setCapture();
};
MetaNodeResizeMouseListener.prototype.onMouseMove = function (e) {
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
            this.resize(e);
            this.lastTime = curTime;
        }
    }
};
MetaNodeResizeMouseListener.prototype.onMouseUp = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }

    //
    if (this.lastCoord) {
        this.resize(e);
    }

    //
    this.lastCoord = null;
    this.retangle.getUI().releaseCapture();

    //
    this.metaNodeModel.setResizing(false);
};
MetaNodeResizeMouseListener.prototype.resize = function (e) {
    var curCoord = Toolkit.getContainerCoord(e, this.wrapper.getViewer());

    //
    var xOffset = curCoord.getX() - this.lastCoord.getX();
    var yOffset = curCoord.getY() - this.lastCoord.getY();
    var size = this.metaNodeModel.getSize();
    this.metaNodeModel.setSize(new Dimension(size.getWidth() + xOffset, size.getHeight() + yOffset));

    //
    this.lastCoord = curCoord;
};

