
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function TransitionModel(fromMetaNodeModel, toMetaNodeModel, id) {
    this.base = MetaModel;
    this.base();

    //
    if (id) {
        this.setID(id);
    }

    //
    this.setMetaNodeModel(fromMetaNodeModel, toMetaNodeModel);

    //
    this._check();
}
TransitionModel.prototype = new MetaModel();

//
TransitionModel.prototype._check = function () {
    if (this.isValidity()) {
        return;
    }
    if (this.getFromMetaNodeModel()) {
        this.getFromMetaNodeModel().removeFrom(this);
    }
    if (this.getToMetaNodeModel()) {
        this.getToMetaNodeModel().removeTo(this);
    }
};

//
TransitionModel.prototype.isValidity = function () {
    if (!this.getFromMetaNodeModel()) {
        return false;
    }
    if (!this.getFromMetaNodeModel().isFromValidity()) {
        return false;
    }
    if (!this.getToMetaNodeModel()) {
        return false;
    }
    if (!this.getToMetaNodeModel().isToValidity()) {
        return false;
    }
    return true;
};

//
TransitionModel.prototype.setMetaNodeModel = function (fromMetaNodeModel, toMetaNodeModel) {
    if (this.fromMetaNodeModel) {
        this.fromMetaNodeModel.removeFrom(this);
    }
    if (this.toMetaNodeModel) {
        this.toMetaNodeModel.removeTo(this);
    }
    this.fromMetaNodeModel = fromMetaNodeModel;
    this.toMetaNodeModel = toMetaNodeModel;
    this.fromMetaNodeModel.addFrom(this);
    this.toMetaNodeModel.addTo(this);
};
TransitionModel.prototype.getFromMetaNodeModel = function () {
    return this.fromMetaNodeModel;
};
TransitionModel.prototype.getToMetaNodeModel = function () {
    return this.toMetaNodeModel;
};

//
TransitionModel.prototype._updateOffset = function () {
    if (!this.getFromMetaNodeModel()) {
        return false;
    }
    if (!this.getToMetaNodeModel()) {
        return false;
    }

    //
    var fromOffset = this.getFromOffset();
    if (fromOffset) {
        var size = this.getFromMetaNodeModel().getSize();
        var width = size.getWidth();
        var height = size.getHeight();
        var xOffset = fromOffset.getX();
        var yOffset = fromOffset.getY();
        if (xOffset < 0) {
            xOffset = 0;
        }
        if (xOffset > width) {
            xOffset = width;
        }
        if (yOffset < 0) {
            yOffset = 0;
        }
        if (yOffset > height) {
            yOffset = height;
        }
        if ((xOffset != toOffset.getX()) || (yOffset != toOffset.getY())) {
            fromOffset.setXY(xOffset, yOffset);
        }
    }

    //
    var toOffset = this.getToOffset();
    if (toOffset) {
        var size = this.getToMetaNodeModel().getSize();
        var width = size.getWidth();
        var height = size.getHeight();
        var xOffset = toOffset.getX();
        var yOffset = toOffset.getY();
        if (xOffset < 0) {
            xOffset = 0;
        }
        if (xOffset > width) {
            xOffset = width;
        }
        if (yOffset < 0) {
            yOffset = 0;
        }
        if (yOffset > height) {
            yOffset = height;
        }
        if ((xOffset != toOffset.getX()) || (yOffset != toOffset.getY())) {
            toOffset.setXY(xOffset, yOffset);
        }
    }
};

//
TransitionModel.prototype.setFromOffset = function (offset) {
    if (offset == null) {
        return;
    }
    this.fromOffset = offset;
    this.notifyObservers(TransitionModel.TRANSITION_POSITION_CHANGED);
};
TransitionModel.prototype.getFromOffset = function () {
    return this.fromOffset;
};
TransitionModel.prototype.setToOffset = function (offset) {
    if (offset == null) {
        return;
    }
    this.toOffset = offset;
    this.notifyObservers(TransitionModel.TRANSITION_POSITION_CHANGED);
};
TransitionModel.prototype.getToOffset = function () {
    return this.toOffset;
};

//
TransitionModel.prototype.update = function (observable, arg) {
    switch (arg) {
      case MetaNodeModel.POSITION_CHANGED:
        this.notifyObservers(TransitionModel.TRANSITION_POSITION_CHANGED);
        break;
      case MetaNodeModel.SIZE_CHANGED:
        this._updateOffset();
        break;
    }
};

//
TransitionModel.prototype.suicide = function () {
    if (this.getFromMetaNodeModel()) {
        this.getFromMetaNodeModel().removeFrom(this);
    }
    if (this.getToMetaNodeModel()) {
        this.getToMetaNodeModel().removeTo(this);
    }
    this.notifyObservers(TransitionModel.SUICIDE);
};

//
TransitionModel.SUICIDE = "TRANSITION_SUICIDE";

//
TransitionModel.TYPE_TRANSITION = "XIORKFLOW_TRANSITION";
TransitionModel.prototype.type = TransitionModel.TYPE_TRANSITION;

//
TransitionModel.TRANSITION_POSITION_CHANGED = "TRANSITION_POSITION_CHANGED";

