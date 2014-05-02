
/**
 * <p>Title:JoinNode</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function JoinNode(model, wrapper) {
    this.base = MetaNode;
    var imageUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/join.gif";
    this.base(model, imageUrl, wrapper);
}
JoinNode.prototype = new MetaNode();

