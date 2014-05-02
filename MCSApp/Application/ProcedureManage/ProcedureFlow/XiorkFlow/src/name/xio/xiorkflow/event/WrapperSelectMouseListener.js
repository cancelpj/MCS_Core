
//
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function WrapperSelectMouseListener(wrapper) {
    this.wrapper = wrapper;
}
WrapperSelectMouseListener.prototype = new MouseListener();
WrapperSelectMouseListener.prototype.onMouseDown = function (e) {
    this.moved = false;
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    this.down = true;
};
WrapperSelectMouseListener.prototype.onMouseMove = function (e) {
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    if (this.down) {
        this.moved = true;
    }
};
WrapperSelectMouseListener.prototype.onMouseUp = function (e) {
    this.down = false;
    if (e.button != MouseEvent.BUTTON_LEFT) {
        return;
    }
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }
    if (this.moved) {
        return;
    }

    //
    var viewer = this.wrapper.getViewer();
    if (viewer.getUI() == e.srcElement) {
        this.wrapper.getModel().clearSelectedMetaNodeModels();
        this.wrapper.getModel().clearSelectedTransitionModels();
    }
};

