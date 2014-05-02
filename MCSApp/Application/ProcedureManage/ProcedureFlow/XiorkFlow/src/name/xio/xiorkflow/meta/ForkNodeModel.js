
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ForkNodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.FROMS_MAX = MetaNodeModel.NUM_NOT_LIMIT;
    this.setText("\u5206\u652F\u8282\u70B9");

    //
    this.setSize(new Dimension(80, 60));
}
ForkNodeModel.prototype = new MetaNodeModel();

//
ForkNodeModel.prototype.toString = function () {
	//分支
    return "[\u5206\u652f:" + this.getText() + "]";
};

//
ForkNodeModel.prototype.type = MetaNodeModel.TYPE_FORK_NODE;

