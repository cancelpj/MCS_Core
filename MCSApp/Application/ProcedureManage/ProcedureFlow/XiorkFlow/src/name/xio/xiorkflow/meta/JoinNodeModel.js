
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function JoinNodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.TOS_MAX = MetaNodeModel.NUM_NOT_LIMIT;
    this.setText("\u6C47\u805A\u8282\u70B9");

    //
    this.setSize(new Dimension(80, 60));
}
JoinNodeModel.prototype = new MetaNodeModel();

//
JoinNodeModel.prototype.toString = function () {
	//汇聚
    return "[\u6c47\u805a:" + this.getText() + "]";
};

//
JoinNodeModel.prototype.type = MetaNodeModel.TYPE_JOIN_NODE;

