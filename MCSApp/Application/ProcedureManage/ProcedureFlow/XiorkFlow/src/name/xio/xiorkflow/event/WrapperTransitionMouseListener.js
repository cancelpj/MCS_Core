
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function WrapperTransitionMouseListener(wrapper) {
    this.wrapper = wrapper;
}
WrapperTransitionMouseListener.prototype = new MouseListener();

//
WrapperTransitionMouseListener.prototype.onMouseDown = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    if (state == StateMonitor.TRANSITION) {
        var transitionMonitor = this.wrapper.getTransitionMonitor();
        if (transitionMonitor.getFromMetaNodeModel() !== null) {
            this.newLine(e);
        }
    }
};
WrapperTransitionMouseListener.prototype.onMouseMove = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    if (state == StateMonitor.TRANSITION) {
        var transitionMonitor = this.wrapper.getTransitionMonitor();
        if (transitionMonitor.getFromMetaNodeModel() !== null) {
            if (this.lineView) {
                var point = Toolkit.getContainerCoord(e, this.wrapper.getViewer());
                var fromPoint = this.lineView.getFromPoint();
                if (point.getX() < fromPoint.getX()) {
                    point.setX(point.getX() + 10);
                }
                this.lineView.setTo(point);
            }
        }
    }
};
WrapperTransitionMouseListener.prototype.onMouseUp = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    if (state == StateMonitor.TRANSITION) {
        this.removeLine();
    }
};

//
WrapperTransitionMouseListener.prototype.newLine = function (e) {
    if (!this.lineView) {
        this.lineView = new LineView();
        this.wrapper.getViewer().add(this.lineView);
    }
    var point = Toolkit.getContainerCoord(e, this.wrapper.getViewer());
    this.lineView.setFrom(point);
    this.lineView.setTo(point);
};
WrapperTransitionMouseListener.prototype.removeLine = function () {
    if (!this.lineView) {
        return;
    }
    this.lineView.listenerProxy.clear();
    this.wrapper.getViewer().remove(this.lineView);
    this.lineView = null;
};

