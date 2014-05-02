
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ButtonGroup() {
    this.base = Observable;
    this.base();
    this.buttons = new Array();
    this.pressedButtonModel = null;
}
ButtonGroup.prototype = new Observable();
ButtonGroup.prototype.add = function (button) {
    if (!button) {
        return;
    }
    this.buttons.add(button);
    var model = button.getModel ? button.getModel() : button;
    if (model.isPressed()) {
        if (this.pressedButtonModel == null) {
            this.pressedButtonModel = model;
        } else {
            model.setPressed(false);
        }
    }
    button.getModel().setGroup(this);
};
ButtonGroup.prototype.getButtonCount = function () {
    if (this.buttons == null) {
        return 0;
    } else {
        return this.buttons.size();
    }
};
ButtonGroup.prototype.getButtons = function () {
    return this.buttons;
};
ButtonGroup.prototype.getPressedButtonModel = function () {
    return this.pressedButtonModel;
};
/**
 * 判断按钮是否选择状态
 * @param button name.xio.ui.ToggleButton或者name.xio.ui.ToggleButtonModel
 */
ButtonGroup.prototype.isPressed = function (button) {
    var model = button.getModel ? button.getModel() : button;
    return model == this.pressedButtonModel;
};
ButtonGroup.prototype.remove = function (button) {
    if (button == null) {
        return;
    }
    this.buttons.remove(button);
    if (button.getModel() == this.pressedButtonModel) {
        this.pressedButtonModel = null;
    }
    button.getModel().setGroup(null);
};
ButtonGroup.prototype.setPressed = function (button, pressed) {
    var model = button.getModel ? button.getModel() : button;
    if ((pressed) && (model != null) && (model != this.pressedButtonModel)) {
        var oldPressedButtonModel = this.pressedButtonModel;
        this.pressedButtonModel = model;
        if (oldPressedButtonModel != null) {
            oldPressedButtonModel.setPressed(false);
        }
        model.setPressed(true);
    }
    this.notifyObservers(ButtonGroup.PRESSED_CHANGED);
};

//
ButtonGroup.PRESSED_CHANGED = "BUTTON_GROUP_PRESSED_CHANGED";

