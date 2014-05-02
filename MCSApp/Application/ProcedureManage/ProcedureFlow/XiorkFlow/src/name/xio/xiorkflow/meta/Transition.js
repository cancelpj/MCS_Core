
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Transition(model, wrapper) {
    this.base = LineView;
    this.base();

    //
    var stroke = Toolkit.newElement("v:stroke");
    stroke.EndArrow = "Classic";
    this.add(stroke);

    //
    //bound rectangle
    var rectangleUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/rectangle.gif";
    //
    this.fromRetangle = new Component(Toolkit.newImage());
    this.fromRetangle.getUI().src = rectangleUrl;
    this.fromRetangle.setLeft("-3px");
    this.fromRetangle.setTop("-3px");
    this.fromRetangle.setPosition("absolute");
    this.add(this.fromRetangle);
    //
    this.toRetangle = new Component(Toolkit.newImage());
    this.toRetangle.getUI().src = rectangleUrl;
    this.toRetangle.setLeft("-3px");
    this.toRetangle.setTop("-3px");
    this.toRetangle.setPosition("absolute");
    this.add(this.toRetangle);

    //
    this.textInput = Toolkit.newElement("<input type=\"text\">");
    this.textInput.style.display = "none";
    this.add(this.textInput);
    var _Transition = this;
    this.textInput.onchange = function () {
        _Transition.stopEdit();
    };
    this.textInput.onblur = function () {
        _Transition.stopEdit();
    };

    //linetext
    this.lineText = new LineTextView();

    //
    this.setModel(model);
    this.wrapper = wrapper;
}
Transition.prototype = new LineView();

//
Transition.prototype.setModel = function (model) {
    if (!model) {
        return;
    }
    this.model = model;
    this.model.addObserver(this);
    this._updatePoints();
    this._updateBoundRectangle();
    this._updateText();
};
Transition.prototype.getModel = function () {
    return this.model;
};

//
Transition.prototype.getLineText = function () {
    return this.lineText;
};

//
Transition.prototype.startEdit = function () {
    this.textInput.style.display = "";
    this.textInput.focus();
    this.getModel().setEditing(true);
};
Transition.prototype.stopEdit = function () {
    this.textInput.style.display = "none";
    this.getModel().setText(this.textInput.value);
    this.getModel().setEditing(false);
};

//
Transition.prototype._updateText = function () {
    var text = this.model.getText();
    this.textInput.value = text;
    this.lineText.setText(text);
};

//
Transition.prototype._updatePoints = function () {
    var fromMetaNodeModel = this.model.getFromMetaNodeModel();
    var toMetaNodeModel = this.model.getToMetaNodeModel();
    if (!fromMetaNodeModel) {
        return;
    }
    if (!toMetaNodeModel) {
        return;
    }
    var fromOffset = this.getModel().getFromOffset();
    var toOffset = this.getModel().getToOffset();
    if ((!fromOffset) || (!toOffset)) {
        var offset = TransitionCompass.getOffset(fromMetaNodeModel, toMetaNodeModel);
        if (!offset) {
            return;
        }
        if (!fromOffset) {
            fromOffset = offset[0];
        }
        if (!toOffset) {
            toOffset = offset[1];
        }
    }

    //
    var from = TransitionCompass.convertOffsetToPoint(fromMetaNodeModel, fromOffset);
    var to = TransitionCompass.convertOffsetToPoint(toMetaNodeModel, toOffset);

    //
    this.setFrom(from);
    this.setTo(to);

    //linetext point
    this.lineText.setFrom(from);
    this.lineText.setTo(to);

    //
    var minX = Math.min(from.getX(), to.getX());
    var minY = Math.min(from.getY(), to.getY());
    this.fromRetangle.setLeft((from.getX() - minX) + "px");
    this.fromRetangle.setTop((from.getY() - minY) + "px");
    this.toRetangle.setLeft((to.getX() - minX) + "px");
    this.toRetangle.setTop((to.getY() - minY) + "px");
};

//
Transition.prototype._updateBoundRectangle = function () {
    if (this.model.isSelected()) {
        this.fromRetangle.setClassName("BOUND_RECTANGLE");
        this.toRetangle.setClassName("BOUND_RECTANGLE");
    } else {
        this.fromRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");
        this.toRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");
    }
};

//
Transition.prototype._suicide = function () {
    this.listenerProxy.clear();
    if (!this.wrapper) {
        return;
    }
    this.wrapper.removeTransition(this);
};

//
Transition.prototype.update = function (observable, arg) {
    this.wrapper.setChanged(true);
    switch (arg) {
      case TransitionModel.TRANSITION_POSITION_CHANGED:
        this._updatePoints();
        break;
      case MetaModel.SELECTED_CHANGED:
        this._updateBoundRectangle();
        break;
      case TransitionModel.SUICIDE:
        this._suicide();
      case MetaModel.TEXT_CHANGED:
        this._updateText();
        break;
        break;
    }
};

