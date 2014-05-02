
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function LineTextView() {
    this.base = LineView;
    this.base();
    this.setPosition("absolute");
    this.setLeft("0px");
    this.setTop("0px");

    //
    this.path = Toolkit.newElement("<v:path textpathok='true'/>");
    this.add(this.path);

    //
    this.textPath = Toolkit.newElement("<v:textpath on='true' string=''/>");
    this.add(this.textPath);
}
LineTextView.prototype = new LineView();

//
LineTextView.prototype.setText = function (text) {
    text = text ? text : "";
    this.textPath.string = text;
};
LineTextView.prototype.getText = function () {
    return this.textPath.string;
};

//
LineTextView.prototype.setFrom = function (point) {
    if (!point) {
        return;
    }
    this.fromPoint = point;
    this._updateDirection();
};
LineTextView.prototype.setTo = function (point) {
    if (!point) {
        return;
    }
    this.toPoint = point;
    this._updateDirection();
};
LineTextView.prototype._updateDirection = function () {
    if (!this.fromPoint) {
        return;
    }
    if (!this.toPoint) {
        return;
    }

    //
    if (this.fromPoint.getX() == this.toPoint.getX()) {
        this.fromPoint.setX(this.fromPoint.getX() - 1);
    }
    if (this.fromPoint.getY() == this.toPoint.getY()) {
        this.fromPoint.setY(this.fromPoint.getY() - 1);
    }

    //
    if (this.fromPoint.getX() > this.toPoint.getX()) {
        this.getUI().from = this.toPoint.getX() + "," + this.toPoint.getY();
        this.getUI().to = this.fromPoint.getX() + "," + this.fromPoint.getY();
    } else {
        this.getUI().from = this.fromPoint.getX() + "," + this.fromPoint.getY();
        this.getUI().to = this.toPoint.getX() + "," + this.toPoint.getY();
    }
};

