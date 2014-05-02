
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ToggleButtonModel(enabled, pressed, pressed) {
    this.base = ButtonModel;
    this.base(enabled, pressed);

	//
    if (pressed != null) {
        this.setPressed(pressed);
    } else {
        this.setPressed(false);
    }

    //
    this.buttonGroup = null;
}
ToggleButtonModel.prototype = new ButtonModel();
ToggleButtonModel.prototype.setPressed = function (pressed) {
    var group = this.getGroup();
    if (group != null) {
        group.setPressed(this, pressed);
        pressed = group.isPressed(this);
    }
    if (this.isPressed() == pressed) {
        return;
    }
    this.pressed = pressed;
    this.notifyObservers(ToggleButtonModel.PRESSED_CHANGED_PROPERTY);
};
ToggleButtonModel.prototype.isPressed = function () {
    return this.pressed;
};
ToggleButtonModel.prototype.setGroup = function (buttonGroup) {
    this.buttonGroup = buttonGroup;
};
ToggleButtonModel.prototype.getGroup = function () {
    return this.buttonGroup;
};

//静态变量
ToggleButtonModel.PRESSED_CHANGED_PROPERTY = "TOGGLE_BUTTON_PRESSED_CHANGED";

