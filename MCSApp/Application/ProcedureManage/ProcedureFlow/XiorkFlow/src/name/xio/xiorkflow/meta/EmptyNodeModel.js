
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function EmptyNodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.FROMS_MAX = MetaNodeModel.NUM_NOT_LIMIT;
    this.TOS_MAX = MetaNodeModel.NUM_NOT_LIMIT;
    
    this.setText("");//空节点不添加文本

    //
    this.setSize(new Dimension(15, 15));
}
EmptyNodeModel.prototype = new MetaNodeModel();

//
EmptyNodeModel.prototype.toString = function () {
	//空节点
    return "[EmptyNode:" + this.getText() + "]";
};

//
EmptyNodeModel.prototype.type = MetaNodeModel.TYPE_EMPTY_NODE;//men add

