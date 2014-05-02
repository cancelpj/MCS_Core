
/**
 * <p>Title: StartNode</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function StartNode(model, wrapper) {
    this.base = MetaNode;
    var imageUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/start.gif";
    this.base(model, imageUrl, wrapper);
}
StartNode.prototype = new MetaNode();

