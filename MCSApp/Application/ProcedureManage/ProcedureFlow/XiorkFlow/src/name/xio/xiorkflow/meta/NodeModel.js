
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function NodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.setText("\u8282\u70B9");

    //
    this.setSize(new Dimension(80, 60));
}
NodeModel.prototype = new MetaNodeModel();

//
NodeModel.prototype.toString = function () {
	//节点
    return "[\u8282\u70b9:" + this.getText() + "]";
};

//
NodeModel.prototype.type = MetaNodeModel.TYPE_NODE;

