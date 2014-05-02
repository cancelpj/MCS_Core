
//
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function MetaNodeTextMouseListener(metaNode, wrapper) {
    this.metaNode = metaNode;
    this.wrapper = wrapper;
}
MetaNodeTextMouseListener.prototype = new MouseListener();
MetaNodeTextMouseListener.prototype.onDblClick = function (e) {
    var state = this.wrapper.getStateMonitor().getState();
    if (state != StateMonitor.SELECT) {
        return;
    }
    this.wrapper.getModel().clearSelectedMetaNodeModels();
    this.metaNode.startEdit();
    //men add
    //this.metaNode.rangestartEdit();;
};

