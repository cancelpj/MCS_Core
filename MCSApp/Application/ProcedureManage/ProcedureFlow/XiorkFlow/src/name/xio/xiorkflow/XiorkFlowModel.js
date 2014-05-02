
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XiorkFlowModel() {
    this.base = Observable;
    this.base();
    this.resetID();
    this.setName(null);

    //
    this.metaNodeModels = new Array();
    this.transitionModels = new Array();

    //
    this.selectedMetaNodeModels = new Array();
    this.selectedTransitionModels = new Array();

    //
    this.setEnable(true);
    this.setEditable(true);
}
XiorkFlowModel.prototype = new Observable();

//
XiorkFlowModel.prototype.addMetaNodeModel = function (metaNodeModel) {
	//null
    if (metaNodeModel == null) {
        return;
    }
    //exist
    if (this.metaNodeModels.indexOf(metaNodeModel) > -1) {
        return;
    }

	//
    var id = metaNodeModel.getID();
    if (id) {
        this.updateID(id);
    } else {
        metaNodeModel.setID(this.nextID());
    }

    //
    this.metaNodeModels.add(metaNodeModel);
    var args = [XiorkFlowModel.META_NODE_MODEL_ADD, metaNodeModel];
    this.notifyObservers(args);
};
XiorkFlowModel.prototype.removeMetaNodeModel = function (metaNodeModel) {
	//null
    if (metaNodeModel == null) {
        return;
    }

    //
    var fromTransitionModels = metaNodeModel.getFroms();
    for (var i = 0; i < fromTransitionModels.size(); i++) {
        this.removeTransitionModel(fromTransitionModels.get(i));
    }
    var toTransitionModels = metaNodeModel.getTos();
    for (var i = 0; i < toTransitionModels.size(); i++) {
        this.removeTransitionModel(toTransitionModels.get(i));
    }

    //
    this.metaNodeModels.remove(metaNodeModel);
    this.removeSelectedMetaNodeModel(metaNodeModel);

    //
    var args = [XiorkFlowModel.META_NODE_MODEL_REMOVE, metaNodeModel];
    this.notifyObservers(args);
};
XiorkFlowModel.prototype.getMetaNodeModels = function () {
    return this.metaNodeModels;
};

//
XiorkFlowModel.prototype.addTransitionModel = function (transitionModel) {
	//null
    if (transitionModel == null) {
        return;
    }
    //exist
    if (this.transitionModels.indexOf(transitionModel) > -1) {
        return;
    }

    //
    var id = transitionModel.getID();
    if (id) {
        this.updateID(id);
    } else {
        transitionModel.setID(this.nextID());
    }

    //
    this.transitionModels.add(transitionModel);
    var args = [XiorkFlowModel.TRANSITION_MODEL_ADD, transitionModel];
    this.notifyObservers(args);
};
XiorkFlowModel.prototype.removeTransitionModel = function (transitionModel) {
	//null
    if (transitionModel == null) {
        return;
    }

    //
    this.transitionModels.remove(transitionModel);
    this.removeSelectedTransitionModel(transitionModel);
    var args = [XiorkFlowModel.TRANSITION_MODEL_REMOVE, transitionModel];
    this.notifyObservers(args);
};
XiorkFlowModel.prototype.getTransitionModels = function () {
    return this.transitionModels;
};

//
XiorkFlowModel.prototype.addSelectedMetaNodeModel = function (metaNodeModel) {
	//null
    if (metaNodeModel == null) {
        return;
    }
    //exist
    if (this.selectedMetaNodeModels.indexOf(metaNodeModel) > -1) {
        return;
    }
    metaNodeModel.setSelected(true);
    this.selectedMetaNodeModels.add(metaNodeModel);
};
XiorkFlowModel.prototype.removeSelectedMetaNodeModel = function (metaNodeModel) {
	//null
    if (metaNodeModel == null) {
        return;
    }
    metaNodeModel.setSelected(false);
    this.selectedMetaNodeModels.remove(metaNodeModel);
};
XiorkFlowModel.prototype.getSelectedMetaNodeModels = function () {
    return this.selectedMetaNodeModels;
};
XiorkFlowModel.prototype.clearSelectedMetaNodeModels = function () {
    for (var i = 0; i < this.selectedMetaNodeModels.size(); i++) {
        this.selectedMetaNodeModels.get(i).setSelected(false);
    }
    return this.selectedMetaNodeModels.clear();
};

//
XiorkFlowModel.prototype.addSelectedTransitionModel = function (transitionModel) {
	//null
    if (transitionModel == null) {
        return;
    }
    //exist
    if (this.selectedTransitionModels.indexOf(transitionModel) > -1) {
        return;
    }
    transitionModel.setSelected(true);
    this.selectedTransitionModels.add(transitionModel);
};
XiorkFlowModel.prototype.removeSelectedTransitionModel = function (transitionModel) {
	//null
    if (transitionModel == null) {
        return;
    }
    transitionModel.setSelected(false);
    this.selectedTransitionModels.remove(transitionModel);
};
XiorkFlowModel.prototype.getSelectedTransitionModels = function () {
    return this.selectedTransitionModels;
};
XiorkFlowModel.prototype.clearSelectedTransitionModels = function () {
    for (var i = 0; i < this.selectedTransitionModels.size(); i++) {
        this.selectedTransitionModels.get(i).setSelected(false);
    }
    return this.selectedTransitionModels.clear();
};

//
XiorkFlowModel.prototype.deleteSelected = function () {
    var selectedMetaNodeModels = this.getSelectedMetaNodeModels().clone();
    var selectedTransitionModels = this.getSelectedTransitionModels().clone();
    if ((selectedMetaNodeModels.size() > 0) || (selectedTransitionModels.size() > 0)) {
        if (!window.confirm("\u5220\u9664\u540e\u5c06\u65e0\u6cd5\u56de\u590d\uff0c\u60a8\u786e\u5b9a\u5220\u9664\uff1f")) {
            return;
        }

        //
        for (var i = 0; i < selectedTransitionModels.size(); i++) {
            this.removeTransitionModel(selectedTransitionModels.get(i));
        }

        //
        for (var i = 0; i < selectedMetaNodeModels.size(); i++) {
            this.removeMetaNodeModel(selectedMetaNodeModels.get(i));
        }
    }
};


//
XiorkFlowModel.prototype.setEnable = function (enable) {
    this.enable = enable;
};
XiorkFlowModel.prototype.isEnable = function () {
    return this.enable;
};
XiorkFlowModel.prototype.setEditable = function (editable) {
    this.editable = editable;
};
XiorkFlowModel.prototype.isEditable = function () {
    return this.editable;
};

//
XiorkFlowModel.prototype.setName = function (name) {
    this.name = name;
};
XiorkFlowModel.prototype.getName = function () {
    return this.name;
};

//
XiorkFlowModel.prototype.resetID = function () {
    this.id = XiorkFlowWorkSpace.ID;
};
XiorkFlowModel.prototype.updateID = function (id) {
    if (id > this.id) {
        this.id = id;
    }
};
XiorkFlowModel.prototype.nextID = function () {
    return this.id++;
};

//
XiorkFlowModel.moveMetaNodeModelBy = function (metaNodeModels, x, y) {
    for (var i = 0; i < metaNodeModels.size(); i++) {
        var metaNodeModel = metaNodeModels.get(i);
        var pos = metaNodeModel.getPosition();
        if (((pos.getX() + x) < 0) || ((pos.getY() + y) < 0)) {
            return;
        }
    }
    for (var i = 0; i < metaNodeModels.size(); i++) {
        var metaNodeModel = metaNodeModels.get(i);
        var pos = metaNodeModel.getPosition();
        metaNodeModel.setPosition(new Point(pos.getX() + x, pos.getY() + y));
    }
};

//static
XiorkFlowModel.META_NODE_MODEL_ADD = "META_NODE_MODEL_ADD";
XiorkFlowModel.META_NODE_MODEL_REMOVE = "META_NODE_MODEL_REMOVE";
XiorkFlowModel.TRANSITION_MODEL_ADD = "TRANSITION_MODEL_ADD";
XiorkFlowModel.TRANSITION_MODEL_REMOVE = "TRANSITION_MODEL_REMOVE";

