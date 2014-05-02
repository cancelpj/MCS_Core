
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function StartNodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.TOS_MAX = 0;
    this.setText("\u5F00\u59CB\u8282\u70B9");

    //
    this.setSize(new Dimension(80, 60));
}
StartNodeModel.prototype = new MetaNodeModel();

//
StartNodeModel.prototype.toString = function () {
	//开始
    return "[\u5f00\u59cb:" + this.getText() + "]";
};

//
StartNodeModel.prototype.type = MetaNodeModel.TYPE_START_NODE;

