
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function TransitionTextKeyListener(transition, wrapper) {
    this.transition = transition;
    this.wrapper = wrapper;
}
TransitionTextKeyListener.prototype = new KeyListener();
TransitionTextKeyListener.prototype.onKeyUp = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }
    var charCode = (e.charCode) ? e.charCode : e.keyCode;
    //enter key
    if (charCode == 13) {
        this.transition.stopEdit();
    }
};

