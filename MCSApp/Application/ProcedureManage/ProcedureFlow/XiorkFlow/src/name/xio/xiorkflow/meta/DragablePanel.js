
/**
 * <p>Title: DragablePanel</p>
 * <p>Description: 使用左坐标、上坐标定位。</p>
 * <p>Copyright: </p>
 * @author amen
 */
function DragablePanel(ui, container) {
    this.base = Panel;
    this.base(ui);
    this.setPosition("absolute");
    this.setCursor(Cursor.MOVE);
    this.container = container;
    this.addMouseListener(new DragablePanelMouseListener(this, container));
}
DragablePanel.prototype = new Panel();
DragablePanel.prototype.toString = function () {
    return "[Component,Panel,DragablePanel]";
};

//
/**
 *
 */
function DragablePanelMouseListener(source, container) {
    this.source = source;
    this.container = container;
}
DragablePanelMouseListener.prototype = new MouseListener();
DragablePanelMouseListener.prototype.onMouseDown = function (e) {
    var theButton = e.button;
    if (theButton == Browser.BUTTON_LEFT) {
        this.container.setDragSource(this.source);
        this.container.setDragCoord(Toolkit.getContainerCoord(e, this.container));
    }
};

