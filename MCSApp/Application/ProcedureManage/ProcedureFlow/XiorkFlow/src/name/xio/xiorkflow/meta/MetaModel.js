
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function MetaModel() {
    this.base = Observable;
    this.base();
    this.setSelected(false);
    this.setText("");
    this.setRange("\u672A\u5B9A\u4E49\u7C7B\u522B");//δ�������
}
MetaModel.prototype = new Observable();
MetaModel.prototype.setSelected = function (selected) {
    if (selected == null) {
        return;
    }
    if (this.selected == selected) {
        return;
    }
    this.selected = selected;
    this.notifyObservers(MetaModel.SELECTED_CHANGED);
};
MetaModel.prototype.isSelected = function () {
    return this.selected;
};
MetaModel.prototype.suicide = function () {
    this.notifyObservers(MetaModel.SUICIDE);
};

//
MetaModel.prototype.getID = function () {
    return this.id;
};
MetaModel.prototype.setID = function (id) {
    this.id = id;
};

//��
MetaModel.prototype.getRange = function () {
    return this.range;
};
MetaModel.prototype.setRange = function (range) {
    var regEx = /\</g;
    range = range.replace(regEx, "\uff1c");
    regEx = /\>/g;
    range = range.replace(regEx, "\uff1e");
    this.range = range;
    this.notifyObservers(MetaModel.TEXT_CHANGED);
};

//
MetaModel.prototype.setText = function (text) {
    var regEx = /\</g;
    text = text.replace(regEx, "\uff1c");
    regEx = /\>/g;
    text = text.replace(regEx, "\uff1e");
    this.text = text;
    this.notifyObservers(MetaModel.TEXT_CHANGED);
};
MetaModel.prototype.getText = function () {
    return this.text;
};

//men add
//ͬʱ�����ı�������
MetaModel.prototype.setTextnRange = function (text,range) {
    var regEx = /\</g;
    text = text.replace(regEx, "\uff1c");
    range = range.replace(regEx, "\uff1c");
    
    regEx = /\>/g;
    text = text.replace(regEx, "\uff1e");
    range = range.replace(regEx, "\uff1e");
    
    this.text = text;
    this.range = range;
    this.notifyObservers(MetaModel.TEXT_CHANGED);
};

//
MetaModel.prototype.setEditing = function (editing) {
    this.editing = editing;
};
MetaModel.prototype.isEditing = function () {
    return this.editing;
};

//
MetaModel.SELECTED_CHANGED = "SELECTED_CHANGED";
MetaModel.SUICIDE = "META_SUICIDE";
MetaModel.TEXT_CHANGED = "TEXT_CHANGED";

//
MetaModel.TYPE_META = "XIORKFLOW_META";
MetaModel.prototype.type = MetaModel.TYPE_META;

