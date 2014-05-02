
/**
 * <p>Title:</p>
 * <p>Description: </p>
 * @copyright 
 * @author amen
 */
var Message = new function () {
    this.outter = null;
};
Message.setOutter = function (outter) {
    this.outter = outter;
};
Message.println = function (text) {
    if (this.outter) {
        this.outter.innerHTML = this.outter.innerHTML + text + "<br>";
    }
};
Message.print = function (text) {
    if (this.outter) {
        this.outter.innerText = this.outter.innerText + text;
    }
};

