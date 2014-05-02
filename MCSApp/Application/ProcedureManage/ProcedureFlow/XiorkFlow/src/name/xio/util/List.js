
/**
 * An ordered collection. The user can insert element,access element,remove 
 * element,etc.
 * It is zero based.
 * @copyright 
 * @author amen
 */
function List() {
    this.listData = new Array();
}

//
/**
 * Appends the specified element into the end of this list.
 * @param obj the element to be appended to this list.
 * @return return the position of the appended element.
 */
List.prototype.add = function (obj) {
    this.listData[this.listData.length] = obj;
    return (this.listData.length - 1);
};

//
/**
 * Returns the element at the specified position in this list.
 * @param index index of element to return.
 * @return the element at the specified position in this list.
 */
List.prototype.get = function (index) {
    if ((index < this.listData.length) && (index >= 0)) {
        return this.listData[index];
    }
    return false;
};

//
/**
 * Removes the specified element from this list,if it is present.
 * @ param obj element to be removed.
 */
List.prototype.remove = function (obj) {
    var tempList = new Array();
    for (var i = 0, ti = 0; i < this.listData.length; i++) {
        if (this.listData[i] != obj) {
            tempList[ti] = this.listData[i];
            ti++;
        }
    }
    this.listData = tempList;
};

//
/**
 * Removes the element at the specified position in this list.
 * @param index the index of the element to removed.
 */
List.prototype.removeByIndex = function (index) {
    var tempList = Array();
    for (var i = 0, a = 0; i < this.listData.length; i++) {
        if (i != index) {
            tempList[a] = this.listData[i];
            a++;
        }
    }
    this.listData = tempList;
};

//
/**
 * Returns the number of elements in this list.
 * @return  the number of elements in this list.
 */
List.prototype.size = function () {
    return this.listData.length;
};

//
/**
 * Removes all of the elements from this list.
 */
List.prototype.clear = function () {
    for (var i = 0, a = 0; i < this.listData.length; i++) {
        this.listData[i] = null;
    }
    this.listData.length = 0;
};

