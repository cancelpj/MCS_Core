
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ToggleButton(image, text, pressed) {
    this.base = Button;
    this.base(image, text);

    //
    this.setModel(new ToggleButtonModel(null, null, pressed));

    //
    this.clearMouseListeners();
    this.addMouseListener(new ToggleButtonMouseListener(this));
    this._update();
}
ToggleButton.prototype = new Button();
ToggleButton.prototype.toString = function () {
    return "[Component,Button,ToggleButton]";
};
ToggleButton.prototype.doClick = function () {
    var model = this.getModel();
    model.setPressed(!model.isPressed());
};

//
ToggleButton.prototype.update = function (observable, arg) {
    this._update();
};
ToggleButton.prototype._update = function () {
    var model = this.getModel();
    if (model.isPressed()) {
        if (model.isEnabled()) {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_TOGGLE_BUTTON_PRESSED");
        } else {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_PRESSED NAME_XIO_UI_DISENABLED");
        }
    } else {
        if (model.isEnabled()) {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");
        } else {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_DISENABLED");
        }
    }
};

//
/**
 *
 */
function ToggleButtonMouseListener(button) {
    this.button = button;
}
ToggleButtonMouseListener.prototype = new MouseListener();
ToggleButtonMouseListener.prototype.onClick = function () {
    var model = this.button.getModel();
    if (!model.isEnabled()) {
        return;
    }
    this.button.doClick();
};
ToggleButtonMouseListener.prototype.onMouseOver = function () {
    var model = this.button.getModel();
    if (!model.isEnabled()) {
        return;
    }
    if (model.isPressed()) {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_TOGGLE_BUTTON_PRESSED NAME_XIO_UI_BUTTON_OVER");
    } else {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_OVER");
    }
};
ToggleButtonMouseListener.prototype.onMouseDown = function () {
    var model = this.button.getModel();
    if (!model.isEnabled()) {
        return;
    }
    this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_TOGGLE_BUTTON_PRESSED");
};
ToggleButtonMouseListener.prototype.onMouseOut = function () {
    var model = this.button.getModel();
    if (!model.isEnabled()) {
        return;
    }
    if (model.isPressed()) {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_TOGGLE_BUTTON_PRESSED");
    } else {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");
    }
};
ToggleButtonMouseListener.prototype.onMouseUp = function () {
    var model = this.button.getModel();
    if (!model.isEnabled()) {
        return;
    }
    if (model.isPressed()) {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_TOGGLE_BUTTON_PRESSED");
    } else {
        this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");
    }
};

