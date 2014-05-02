
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Component(ui) {
    if (ui) {
        if (ui.getUI) {
            this.ui = ui.getUI();
        } else {
            this.ui = ui;
        }
    } else {
        return false;
    }
    this.setClassName("NAME_XIO_UI_FONT");
    this.listenerProxy = new ListenerProxy(this.ui);
}
Component.prototype.getUI = function () {
    return this.ui;
};
Component.prototype.toString = function () {
    return "[Component]";
};

//
Component.prototype.setToolTipText = function (text) {
    this.ui.title = text;
};
Component.prototype.getToolTipText = function () {
    return this.ui.title;
};

//html element
Component.prototype.add = function (component) {
    if (component == null) {
        return;
    }
    if (component.getUI) {
        this.ui.appendChild(component.getUI());
        return;
    }
    this.ui.appendChild(component);
};
Component.prototype.remove = function (component) {
    if (component == null) {
        return;
    }
    if (component.getUI) {
        this.ui.removeChild(component.getUI());
        return;
    }
    this.ui.removeChild(component);
};
Component.prototype.insertBefore = function (component, referComponent) {
    if ((component == null) || (referComponent == null)) {
        return;
    }
    if ((component.getUI) && (referComponent.getUI)) {
        this.ui.insertBefore(component.getUI(), referComponent.getUI());
        return;
    }
    this.ui.insertBefore(component, referComponent);
};

//listener
Component.prototype.addMouseListener = function (mouseListener) {
    this.listenerProxy.addMouseListener(mouseListener);
};
Component.prototype.removeMouseListener = function (mouseListener) {
    this.listenerProxy.removeMouseListener(mouseListener);
};
Component.prototype.clearMouseListeners = function () {
    this.listenerProxy.clearMouseListeners();
};
Component.prototype.getMouseListeners = function () {
    return this.listenerProxy.getMouseListeners();
};
Component.prototype.addKeyListener = function (keyListener) {
    this.listenerProxy.addKeyListener(keyListener);
};
Component.prototype.removeKeyListener = function (keyListener) {
    this.listenerProxy.removeKeyListener(keyListener);
};
Component.prototype.clearKeyListeners = function () {
    this.listenerProxy.clearKeyListeners();
};
Component.prototype.getKeyListeners = function () {
    return this.listenerProxy.getKeyListeners();
};
Component.prototype.addMouseWheelListener = function (mouseWheelListener) {
    this.listenerProxy.addMouseWheelListener(mouseWheelListener);
};
Component.prototype.removeMouseWheelListener = function (mouseWheelListener) {
    this.listenerProxy.removeMouseWheelListener(mouseWheelListener);
};
Component.prototype.clearMouseWheelListeners = function () {
    this.listenerProxy.clearMouseWheelListeners();
};
Component.prototype.getMouseWheelListeners = function () {
    return this.listenerProxy.getMouseWheelListeners();
};
Component.prototype.addContextMenuListener = function (contextMenuListener) {
    this.listenerProxy.addContextMenuListener(contextMenuListener);
};
Component.prototype.removeContextMenuListener = function (contextMenuListener) {
    this.listenerProxy.removeContextMenuListener(contextMenuListener);
};
Component.prototype.clearContextMenuListeners = function () {
    this.listenerProxy.clearContextMenuListeners();
};
Component.prototype.getContextMenuListeners = function () {
    return this.listenerProxy.getContextMenuListeners();
};

//class
Component.prototype.setClassName = function (className) {
    this.ui.className = className;
};

//style property
Component.prototype.setOpacity = function (opacity) {
    this.ui.style.filter = "Alpha(opacity=" + opacity + ")";
};
Component.prototype.setBackgroundColor = function (backgroundColor) {
    this.ui.style.backgroundColor = backgroundColor;
};
Component.prototype.setColor = function (color) {
    this.ui.style.color = color;
};
Component.prototype.setInnerText = function (text) {
    this.ui.innerText = text;
};
Component.prototype.setTextAlign = function (textAlign) {
    this.ui.style.textAlign = textAlign;
};
Component.prototype.setFontSize = function (fontSize) {
    this.ui.style.fontSize = fontSize;
};
Component.prototype.setFontWeight = function (fontWeight) {
    this.ui.style.fontWeight = fontWeight;
};
Component.prototype.setDisplay = function (display) {
    this.ui.style.display = display;
};
//position
Component.prototype.setPosition = function (position) {
    this.ui.style.position = position;
};
Component.prototype.setLeft = function (left) {
    this.ui.style.left = left;
};
Component.prototype.setRight = function (right) {
    this.ui.style.right = right;
};
Component.prototype.setTop = function (top) {
    this.ui.style.top = top;
};
Component.prototype.setBottom = function (bottom) {
    this.ui.style.bottom = bottom;
};
Component.prototype.getLeft = function () {
    return this.ui.style.left;
};
Component.prototype.getRight = function () {
    return this.ui.style.right;
};
Component.prototype.getTop = function () {
    return this.ui.style.top;
};
Component.prototype.getBottom = function () {
    return this.ui.style.bottom;
};
//overflow
Component.prototype.setOverflow = function (overflow) {
    this.ui.style.overflow = overflow;
};
//cursor
Component.prototype.setCursor = function (cursor) {
    this.ui.style.cursor = cursor;
};
//size
Component.prototype.setWidth = function (width) {
    this.ui.style.width = width;
};
Component.prototype.setHeight = function (height) {
    this.ui.style.height = height;
};
Component.prototype.getWidth = function () {
    return this.ui.style.width;
};
Component.prototype.getHeight = function () {
    return this.ui.style.height;
};
//border
Component.prototype.setBorder = function (border) {
    this.ui.style.border = border;
};
//margin
Component.prototype.setMargin = function (margin) {
    this.ui.style.margin = margin;
};
Component.prototype.setMarginLeft = function (marginLeft) {
    this.ui.style.marginLeft = marginLeft;
};
Component.prototype.setMarginRight = function (marginRight) {
    this.ui.style.marginRight = marginRight;
};
Component.prototype.setMarginTop = function (marginTop) {
    this.ui.style.marginTop = marginTop;
};
Component.prototype.setMarginBottom = function (marginBottom) {
    this.ui.style.marginBottom = marginBottom;
};
//padding
Component.prototype.setPadding = function (padding) {
    this.ui.style.padding = padding;
};
Component.prototype.setPaddingLeft = function (paddingLeft) {
    this.ui.style.paddingLeft = paddingLeft;
};
Component.prototype.setPaddingRight = function (paddingRight) {
    this.ui.style.paddingRight = paddingRight;
};
Component.prototype.setPaddingTop = function (paddingTop) {
    this.ui.style.paddingTop = paddingTop;
};
Component.prototype.setPaddingBottom = function (paddingBottom) {
    this.ui.style.paddingBottom = paddingBottom;
};

