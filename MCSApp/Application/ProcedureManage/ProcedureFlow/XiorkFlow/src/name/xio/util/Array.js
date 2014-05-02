
/**
 * <p>Title: 扩展Array</p>
 * <p>Description: </p>
 * @copyright 
 * @author amen
 */

//
/**
 *
 */
Array.prototype.setArray = function (array) {
    this.length = array.length;
    for (var i = 0; i < array.length; i++) {
        this[i] = array[i];
    }
};

//
/**
 * Appends the specified element into the end of this array.
 * @param obj the element to be appended to this array.
 * @return return the position of the appended element.
 */
Array.prototype.add = function (obj) {
    var i = this.length;
    this[i] = obj;
    return i;
};

//
/**
 * Returns the element at the specified position in this array.
 * @param index index of element to return.
 * @return the element at the specified position in this array.
 */
Array.prototype.get = function (index) {
    if ((index < this.length) && (index >= 0)) {
        return this[index];
    }
    return false;
};

//
Array.prototype.indexOf = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == obj) {
            return i;
        }
    }
    return -1;
};

//
/**
 * Removes the specified element from this array,if it is present.
 * @ param obj element to be removed.
 */
Array.prototype.remove = function (obj) {
    var tempList = new Array();
    for (var i = 0, ti = 0; i < this.length; i++) {
        if (this[i] != obj) {
            tempList[ti] = this[i];
            ti++;
        }
    }
    this.setArray(tempList);
};

//
/**
 * Removes the element at the specified position in this array.
 * @param index the index of the element to removed.
 */
Array.prototype.removeByIndex = function (index) {
    var tempList = Array();
    for (var i = 0, a = 0; i < this.length; i++) {
        if (i != index) {
            tempList[a] = this[i];
            a++;
        }
    }
    this.setArray(tempList);
};

//
/**
 * Returns the number of elements in this array.
 * @return  the number of elements in this array.
 */
Array.prototype.size = function () {
    return this.length;
};

//
/**
 * Removes all of the elements from this array.
 */
Array.prototype.clear = function () {
    for (var i = 0, a = 0; i < this.length; i++) {
        this[i] = null;
    }
    this.length = 0;
};

//
Array.prototype.clone = function () {
    var cloneList = Array();
    for (var i = 0, a = 0; i < this.length; i++) {
        cloneList.add(this[i]);
    }
    return cloneList;
};

