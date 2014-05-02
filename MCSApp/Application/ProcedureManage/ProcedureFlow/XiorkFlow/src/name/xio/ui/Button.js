
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Button(image, text) {
    this.base = Component;
    this.base(Toolkit.newTable());

    //
    this.ui.cellPadding = 0;
    this.ui.cellSpacing = 0;
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");

    //
    var row = this.ui.insertRow(-1);

    //
    if (image) {
        var imageIcon = Toolkit.newImage();
        imageIcon.src = image;
        imageIcon.className = "NAME_XIO_UI_BUTTON_IMGCELL";
        this.imgCell = row.insertCell(-1);
        this.imgCell.appendChild(imageIcon);
        this.imgCell.valign = "middle";
        this.imgCell.align = "center";
    } else {
        this.imgCell = null;
        if (!text) {
            text = "&nbsp;";
        }
    }

    //
    if (text) {
        this.txtCell = row.insertCell(-1);
        this.txtCell.innerHTML = text;
        this.txtCell.valign = "middle";
        this.txtCell.align = "center";
        this.txtCell.className = "NAME_XIO_UI_BUTTON_TXTCELL";
        if (!image) {
            this.txtCell.style.paddingLeft = "5px";
            this.txtCell.style.paddingRight = "5px";
        }
    } else {
        this.txtCell = null;
    }

    //
    this.addMouseListener(new ButtonMouseListener(this));

    //
    this.setModel(new ButtonModel());

    //
    this.actionListeners = new Array();
}
Button.prototype = new Component();
Button.prototype.toString = function () {
    return "[Component,Button]";
};
Button.prototype.setModel = function (model) {
    if (!model) {
        return;
    }
    this.model = model;
    this.model.addObserver(this);
};
Button.prototype.getModel = function () {
    return this.model;
};
Button.prototype.getText = function () {
    if (this.txtCell) {
        return this.txtCell.innerHTML;
    } else {
        return null;
    }
};
Button.prototype.setText = function (text) {
    if (this.txtCell) {
        this.txtCell.innerHTML = text;
    }
};
Button.prototype.doClick = function () {
    this.model.setPressed(true);
    for (var i = 0; i < this.actionListeners.size(); i++) {
        this.actionListeners.get(i).actionPerformed(this);
    }
    this.model.setPressed(false);
};

//
Button.prototype.addActionListener = function (actionListener) {
    this.actionListeners.add(actionListener);
};
Button.prototype.removeActionListener = function (actionListener) {
    this.actionListeners.remove(actionListener);
};
Button.prototype.clearActionListeners = function (actionListener) {
    this.actionListeners.clear();
};
Button.prototype.getActionListeners = function () {
    return this.actionListeners;
};

//
Button.prototype.update = function (observable, arg) {
    this._update();
};
Button.prototype._update = function () {
    if (this.model.isPressed()) {
        if (this.model.isEnabled()) {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_PRESSED");
        } else {
            this.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_PRESSED NAME_XIO_UI_DISENABLED");
        }
    } else {
        if (this.model.isEnabled()) {
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
function ButtonMouseListener(button) {
    this.button = button;
}
ButtonMouseListener.prototype = new MouseListener();
ButtonMouseListener.prototype.onClick = function () {
    if (!this.button.getModel().isEnabled()) {
        return;
    }
    this.button.doClick();
};
ButtonMouseListener.prototype.onMouseOver = function () {
    if (!this.button.getModel().isEnabled()) {
        return;
    }
    this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_OVER");
};
ButtonMouseListener.prototype.onMouseDown = function () {
    if (!this.button.getModel().isEnabled()) {
        return;
    }
    this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON NAME_XIO_UI_BUTTON_PRESSED");
};
ButtonMouseListener.prototype.onMouseOut = function () {
    if (!this.button.getModel().isEnabled()) {
        return;
    }
    this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");
};
ButtonMouseListener.prototype.onMouseUp = function () {
    if (!this.button.getModel().isEnabled()) {
        return;
    }
    this.button.setClassName("NAME_XIO_UI_FONT NAME_XIO_UI_BUTTON");
};

