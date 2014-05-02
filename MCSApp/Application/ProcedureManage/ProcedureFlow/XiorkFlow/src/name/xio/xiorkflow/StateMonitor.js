
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function StateMonitor() {
    this.base = Observable;
    this.base();
    this.setState(StateMonitor.SELECT);
}
StateMonitor.prototype = new Observable();
StateMonitor.prototype.setState = function (state) {
    this.state = state;
    this.notifyObservers(StateMonitor.OPERATION_STATE_CHANGED);
};
StateMonitor.prototype.getState = function (state) {
    return this.state;
};

//
StateMonitor.prototype.resetState = function (state) {
    this.notifyObservers([StateMonitor.OPERATION_STATE_RESET, state]);
};

//
StateMonitor.OPERATION_STATE_CHANGED = "OPERATION_STATE_CHANGED";
StateMonitor.OPERATION_STATE_RESET = "OPERATION_STATE_RESET";

//
StateMonitor.SELECT = 1;
StateMonitor.START_NODE = 3;
StateMonitor.END_NODE = 5;
StateMonitor.FORK_NODE = 7;
StateMonitor.JOIN_NODE = 9;
StateMonitor.NODE = 11;
StateMonitor.TRANSITION = 13;
StateMonitor.EMPTY_NODE = 99;//men add
