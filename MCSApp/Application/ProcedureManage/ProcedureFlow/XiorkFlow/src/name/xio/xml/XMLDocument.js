
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XMLDocument() {
}
XMLDocument.newDomcument = function () {
    if (window.ActiveXObject) {
        return new ActiveXObject("Microsoft.XMLDOM");
    } else {
        //alert("Your browser cannot handle this script");
    }
    //TODO firefox ...
};

