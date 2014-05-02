
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function EndNodeModel() {
    this.base = MetaNodeModel;
    this.base();

    //
    this.FROMS_MAX = 0;
    this.setText("\u7ED3\u675F\u8282\u70B9");

    //
    this.setSize(new Dimension(80, 60));
}
EndNodeModel.prototype = new MetaNodeModel();

//
EndNodeModel.prototype.toString = function () {
	//结束
    return "[\u7ed3\u675f:" + this.getText() + "]";
};

//
EndNodeModel.prototype.type = MetaNodeModel.TYPE_END_NODE;

