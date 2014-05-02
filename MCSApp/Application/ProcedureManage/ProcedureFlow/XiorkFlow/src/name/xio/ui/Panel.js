
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Panel(ui) {
    this.base = Component;
    if (ui) {
        this.base(ui);
    } else {
        this.base(Toolkit.newLayer());
    }
}
Panel.prototype = new Component();
Panel.prototype.toString = function () {
    return "[Component,Panel]";
};

