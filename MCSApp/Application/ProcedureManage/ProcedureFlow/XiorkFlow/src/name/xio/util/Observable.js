
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Observable() {
    this.observers = new Array();
}
Observable.prototype.addObserver = function (observer) {
    if (this.observers.indexOf(observer) < 0) {
        this.observers.add(observer);
    }
};
Observable.prototype.countObservers = function () {
    return this.observers.size();
};
Observable.prototype.removeObserver = function (observer) {
    this.observers.remove(observer);
};
Observable.prototype.removeObservers = function () {
    this.observers.clear();
};
Observable.prototype.notifyObservers = function (arg) {
    for (var i = 0; i < this.observers.size(); i++) {
        this.observers.get(i).update(this, arg);
    }
};

