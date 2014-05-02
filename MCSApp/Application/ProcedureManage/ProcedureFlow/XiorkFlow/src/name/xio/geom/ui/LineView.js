
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function LineView() {
    this.base = Component;
    this.base(Toolkit.newElement("v:line"));
    this.setPosition("absolute");
    this.setLeft("0px");
    this.setTop("0px");
}
LineView.prototype = new Component();

//
LineView.prototype.setFrom = function (point) {
    if (!point) {
        return;
    }
    this.fromPoint = point;
    this.getUI().from = point.getX() + "," + point.getY();
};
LineView.prototype.setTo = function (point) {
    if (!point) {
        return;
    }
    this.toPoint = point;
    this.getUI().to = point.getX() + "," + point.getY();
};

//
LineView.prototype.getFromPoint = function () {
    return this.fromPoint;
};
LineView.prototype.getToPoint = function () {
    return this.toPoint;
};

