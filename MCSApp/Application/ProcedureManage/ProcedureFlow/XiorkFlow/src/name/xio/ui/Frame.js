
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Frame(ui) {
    this.base = Component;
    if (ui) {
        this.base(ui);
    } else {
        this.base(Toolkit.newLayer());
    }

    //
    this.contentTableUI = Toolkit.newTable();
    this.ui.appendChild(this.contentTableUI);
    this.contentTableUI.cellPadding = 0;
    this.contentTableUI.cellSpacing = 0;
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_FRAME");
    this.contentTableUI.className = "NAME_XIO_UI_FONT NAME_XIO_UI_FRAME";
}
Frame.prototype = new Component();
Frame.prototype.toString = function () {
    return "[Component,Frame]";
};
/**
 * 添加组件。
 * 默认使用行布局，每添加一个组件，该组件作为一行添加，默认行高为被添加组件的高。
 */
Frame.prototype.add = function (component) {
    var componentUI = component.getUI ? component.getUI() : component;
    var height = componentUI.height ? componentUI.height : componentUI.style.height;
    var row = this.contentTableUI.insertRow(-1);
    var cell = row.insertCell(-1);
    if (height) {
        cell.height = height;
    }
    cell.appendChild(componentUI);
    return row;
};

