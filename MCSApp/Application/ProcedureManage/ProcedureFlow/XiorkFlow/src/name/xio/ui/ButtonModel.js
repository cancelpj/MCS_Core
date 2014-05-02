
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ButtonModel(enabled, pressed) {
    this.base = Observable;
    this.base();

	//
    if (enabled != null) {
        this.setEnabled(enabled);
    } else {
        this.setEnabled(true);
    }
    if (pressed != null) {
        this.setPressed(pressed);
    } else {
        this.setPressed(false);
    }
}
ButtonModel.prototype = new Observable();
ButtonModel.prototype.setEnabled = function (enabled) {
    if (this.enabled == enabled) {
        return;
    }
    this.enabled = enabled;
    this.notifyObservers(ButtonModel.ENABLED_CHANGED_PROPERTY);
};
ButtonModel.prototype.isEnabled = function () {
    return this.enabled;
};
ButtonModel.prototype.setPressed = function (pressed) {
    if ((this.pressed == pressed) || (!this.isEnabled())) {
        return;
    }
    this.pressed = pressed;
    this.notifyObservers(ButtonModel.PRESSED_CHANGED_PROPERTY);
};
ButtonModel.prototype.isPressed = function () {
    return this.pressed;
};

//静态变量
ButtonModel.ENABLED_CHANGED_PROPERTY = "BUTTON_ENABLED_CHANGED";
ButtonModel.PRESSED_CHANGED_PROPERTY = "BUTTON_PRESSED_CHANGED";

