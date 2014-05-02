
/**
 * <p>Title: EndNode</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function EndNode(model, wrapper) {
    this.base = MetaNode;
    var imageUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/end.gif";
    this.base(model, imageUrl, wrapper);
}
EndNode.prototype = new MetaNode();

