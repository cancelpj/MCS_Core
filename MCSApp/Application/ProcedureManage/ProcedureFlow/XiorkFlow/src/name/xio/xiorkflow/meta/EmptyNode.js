
/**
 * <p>Title: EmptyNode</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function EmptyNode(model, wrapper) {
    this.base = EmptyMetaNode;
    var imageUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/empty.gif";//将来换成空节点图标
    this.base(model, imageUrl, wrapper);
}
EmptyNode.prototype = new MetaNode();//这里保持 MetaNode

