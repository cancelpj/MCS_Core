
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Dimension(width, height) {
    this.width;
    this.height;
    if (width) {
        this.width = width;
    }
    if (height) {
        this.height = height;
    }
}
Dimension.prototype.toString = function () {
    return "[Dimension]";
};
Dimension.prototype.getHeight = function () {
    return this.height;
};
Dimension.prototype.getWidth = function () {
    return this.width;
};
Dimension.prototype.getSize = function () {
    return this;
};
Dimension.prototype.setHeight = function (height) {
    if (height) {
        this.height = height;
    }
};
Dimension.prototype.setWidth = function (width) {
    if (width) {
        this.width = width;
    }
};
Dimension.prototype.setSize = function (width, height) {
    if (width) {
        this.width = width;
    }
    if (height) {
        this.height = height;
    }
};
Dimension.prototype.toString = function () {
    return "[width=" + this.width + ",height=" + this.height + "]";
};

