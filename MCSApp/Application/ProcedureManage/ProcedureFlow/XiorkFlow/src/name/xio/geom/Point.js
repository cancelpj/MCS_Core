
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Point(x, y) {
    this.setX(x);
    this.setY(y);
}
Point.prototype.clone = function () {
    return new Point(this.x, this.y);
};
Point.prototype.toString = function () {
    return "Point[" + this.getX() + "," + this.getY() + "]";
};
Point.prototype.setX = function (x) {
    this.x = x;
};
Point.prototype.getX = function () {
    return this.x;
};
Point.prototype.setY = function (y) {
    this.y = y;
};
Point.prototype.getY = function () {
    return this.y;
};
Point.prototype.distance = function (oPoint) {
    return Math.sqrt(Math.pow(this.getX() - oPoint.getX(), 2) + Math.pow(this.getY() - oPoint.getY(), 2));
};

