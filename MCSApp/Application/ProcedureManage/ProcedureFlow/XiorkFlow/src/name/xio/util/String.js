
/**
 * <p>Title: 扩展String</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
String.prototype.contains = function (A) {
    return (this.indexOf(A) > -1);
};
String.prototype.equals = function () {
    for (var i = 0; i < arguments.length; i++) {
        if (this == arguments[i]) {
            return true;
        }
    }
    return false;
};
String.prototype.startsWith = function (A) {
    return (this.substr(0, A.length) == A);
};
String.prototype.endsWith = function (A, B) {
    var C = this.length;
    var D = A.length;
    if (D > C) {
        return false;
    }
    if (B) {
        var E = new RegExp(A + "$", "i");
        return E.test(this);
    } else {
        return (D == 0 || this.substr(C - D, D) == A);
    }
};
String.prototype.remove = function (A, B) {
    var s = "";
    if (A > 0) {
        s = this.substring(0, A);
    }
    if (A + B < this.length) {
        s += this.substring(A + B, this.length);
    }
    return s;
};
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};
String.prototype.ltrim = function () {
    return this.replace(/^\s*/g, "");
};
String.prototype.rtrim = function () {
    return this.replace(/\s*$/g, "");
};
String.prototype.replaceNewLineChars = function (A) {
    return this.replace(/\n/g, A);
};

