
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function ListenerProxy(container, propagation) {
    if (container.getUI) {
        this.container = container.getUI();
    } else {
        this.container = container;
    }
    if (!this.container) {
        return;
    }
    if (propagation == null) {
        this.propagation = false;
    } else {
        this.propagation = propagation;
    }
    this.mouseListeners = new Array();
    this.keyListeners = new Array();
    this.mouseWheelListeners = new Array();
    this.contextMenuListeners = new Array();

    //
    this.setEnable(true);

    //
    var ListenerProxy = this;

	//
    _onClick = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onClick(e);
        }
        return this.propagation;
    };

	//
    _onDblClick = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onDblClick(e);
        }
        return this.propagation;
    };

	//
    _onMouseDown = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onMouseDown(e);
        }
        return this.propagation;
    };

	//
    _onMouseUp = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onMouseUp(e);
        }
        return this.propagation;
    };

	//
    _onMouseMove = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onMouseMove(e);
        }
        return this.propagation;
    };

	//
    _onMouseOver = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onMouseOver(e);
        }
        return this.propagation;
    };

	//
    _onMouseOut = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseListeners.size(); i++) {
            ListenerProxy.mouseListeners.get(i).onMouseOut(e);
        }
        return this.propagation;
    };

	//
    _onMouseWheel = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.mouseWheelListeners.size(); i++) {
            ListenerProxy.mouseWheelListeners.get(i).onMouseWheel(e);
        }
        return this.propagation;
    };

	//
    _onKeyDown = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        var charCode = (e.charCode) ? e.charCode : e.keyCode;
        for (var i = 0; i < ListenerProxy.keyListeners.size(); i++) {
            ListenerProxy.keyListeners.get(i).onKeyDown(e);
        }
        return this.propagation;
    };

	//
    _onKeyPress = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.keyListeners.size(); i++) {
            ListenerProxy.keyListeners.get(i).onKeyPress(e);
        }
        return this.propagation;
    };

	//
    _onKeyUp = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.keyListeners.size(); i++) {
            ListenerProxy.keyListeners.get(i).onKeyUp(e);
        }
        return this.propagation;
    };

    //
    _onContextMenu = function (e) {
        if (!ListenerProxy.isEnable()) {
            return;
        }
        e = (e) ? e : ((event) ? event : null);
        for (var i = 0; i < ListenerProxy.contextMenuListeners.size(); i++) {
            ListenerProxy.contextMenuListeners.get(i).onContextMenu(e);
        }
        return this.propagation;
    };

    //
    this.container.onclick = _onClick;
    this.container.ondblclick = _onDblClick;
    this.container.onmousedown = _onMouseDown;
    this.container.onmouseup = _onMouseUp;
    this.container.onmousemove = _onMouseMove;
    this.container.onmouseover = _onMouseOver;
    this.container.onmouseout = _onMouseOut;

    //
    this.container.onmousewheel = _onMouseWheel;

    //
    this.container.onkeydown = _onKeyDown;
    this.container.onkeypress = _onKeyPress;
    this.container.onkeyup = _onKeyUp;

    //
    this.container.oncontextmenu = _onContextMenu;
}

//
/**
 * 当容器被删除时调用，防止内存泄漏。
 */
ListenerProxy.prototype.clear = function () {
	//
    this.mouseListeners.clear();
    this.keyListeners.clear();
    this.mouseWheelListeners.clear();
    this.contextMenuListeners.clear();

    //
    this.container.onclick = null;
    this.container.ondblclick = null;
    this.container.onmousedown = null;
    this.container.onmouseup = null;
    this.container.onmousemove = null;
    this.container.onmouseover = null;
    this.container.onmouseout = null;

    //
    this.container.onmousewheel = null;

    //
    this.container.onkeydown = null;
    this.container.onkeypress = null;
    this.container.onkeyup = null;

    //
    this.container.oncontextmenu = null;
};

//
ListenerProxy.prototype.setEnable = function (enable) {
    this.enable = enable;
};
ListenerProxy.prototype.isEnable = function () {
    return this.enable;
};

//
ListenerProxy.prototype.addMouseListener = function (mouseListener) {
    this.mouseListeners.add(mouseListener);
};
ListenerProxy.prototype.removeMouseListener = function (mouseListener) {
    this.mouseListeners.remove(mouseListener);
};
ListenerProxy.prototype.clearMouseListeners = function (mouseListener) {
    this.mouseListeners.clear();
};
ListenerProxy.prototype.getMouseListeners = function () {
    return this.mouseListeners;
};

//
ListenerProxy.prototype.addKeyListener = function (keyListener) {
    this.keyListeners.add(keyListener);
};
ListenerProxy.prototype.removeKeyListener = function (keyListener) {
    this.keyListeners.remove(keyListener);
};
ListenerProxy.prototype.clearKeyListeners = function (keyListener) {
    this.keyListeners.clear();
};
ListenerProxy.prototype.getKeyListeners = function () {
    return this.keyListeners;
};

//
ListenerProxy.prototype.addMouseWheelListener = function (mouseWheelListener) {
    this.mouseWheelListeners.add(mouseWheelListener);
};
ListenerProxy.prototype.removeMouseWheelListener = function (mouseWheelListener) {
    this.mouseWheelListeners.remove(mouseWheelListener);
};
ListenerProxy.prototype.clearMouseWheelListeners = function (mouseWheelListener) {
    this.mouseWheelListeners.clear();
};
ListenerProxy.prototype.getMouseWheelListeners = function () {
    return this.mouseWheelListeners;
};

//
ListenerProxy.prototype.addContextMenuListener = function (contextMenuListener) {
    this.contextMenuListeners.add(contextMenuListener);
};
ListenerProxy.prototype.removeContextMenuListener = function (contextMenuListener) {
    this.contextMenuListeners.remove(contextMenuListener);
};
ListenerProxy.prototype.clearContextMenuListeners = function (contextMenuListener) {
    this.contextMenuListeners.clear();
};
ListenerProxy.prototype.getContextMenuListeners = function () {
    return this.contextMenuListeners;
};

