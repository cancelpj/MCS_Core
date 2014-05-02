
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ToolBar() {
    this.base = Component;
    this.base(Toolkit.newLayer());
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_TOOLBAR");

    //
    this.toolbarTable = Toolkit.newTable();
    this.ui.appendChild(this.toolbarTable);
    this.toolbarTable.cellPadding = 0;
    this.toolbarTable.cellSpacing = 0;

    //
    this.containerRow = this.toolbarTable.insertRow(-1);
}
ToolBar.prototype = new Component();
ToolBar.prototype.toString = function () {
    return "[Component,ToolBar]";
};
ToolBar.prototype.addSeparator = function () {
    this.add(new ToolBarSeparator());
};
ToolBar.prototype.add = function (item) {
    this.addItem(item);
};
ToolBar.prototype.addItem = function (item) {
    var itemCell = this.containerRow.insertCell(-1);
    if (item.getUI) {
        itemCell.appendChild(item.getUI());
    } else {
        itemCell.appendChild(item);
    }
};

//
/**
 *
 */
function ToolBarSeparator() {
    this.base = Component;
    this.base(Toolkit.newLayer());

    //
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_TOOLBAR_SEPARATOR");
}
ToolBarSeparator.prototype = new Component();

