
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XiorkFlowTableViewer(model, ui) {
    this.base = ScrollPanel;
    this.base(ui);

    //
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_XIORKFLOW_TALBE_VIEWER");

    //
    this.table = Toolkit.newTable();
    this.table.className = "NAME_XIO_XIORKFLOW_TABLE";
    this.add(this.table);

    //
    this.rows = new Array();

    //
    this.setModel(model);
}
XiorkFlowTableViewer.prototype = new ScrollPanel();
XiorkFlowTableViewer.prototype.setModel = function (model) {
    if (!model) {
        return;
    }
    if (this.model == model) {
        return;
    }
    if (this.model) {
        this.model.removeObserver(this);
    }
    this.model = model;
    this.model.addObserver(this);

    //
    this.init();
};
XiorkFlowTableViewer.prototype.getModel = function (model) {
    return this.model;
};
XiorkFlowTableViewer.prototype.update = function (observable, arg) {
    if (arg instanceof Array) {
        if (arg.size() == 2) {
            var property = arg[0];
            var model = arg[1];
            switch (property) {
              case XiorkFlowModel.META_NODE_MODEL_ADD:
                this.addMetaNodeModel(model);
                break;
              default:
                break;
            }
        }
    }
};

//
XiorkFlowTableViewer.prototype.init = function () {
	//clear
    this.clearTable();

    //
    var headRow = this.addRow();
    headRow.className = "head";

    //id
    var idHeadCell = headRow.insertCell();
    idHeadCell.innerText = "ID";
    idHeadCell.width = "40px";

    //name
    var nameHeadCell = headRow.insertCell();
    nameHeadCell.innerText = "NAME";

    //type
    var typeHeadCell = headRow.insertCell();
    typeHeadCell.innerText = "TYPE";
    typeHeadCell.width = "80px";

    //x
    var xHeadCell = headRow.insertCell();
    xHeadCell.innerText = "X";
    xHeadCell.width = "40px";

    //y
    var yHeadCell = headRow.insertCell();
    yHeadCell.innerText = "Y";
    yHeadCell.width = "40px";

    //width
    var widthHeadCell = headRow.insertCell();
    widthHeadCell.innerText = "WIDTH";
    widthHeadCell.width = "40px";

    //height
    var idHeadCell = headRow.insertCell();
    idHeadCell.innerText = "HEIGHT";
    idHeadCell.width = "40px";

    //from
    var fromHeadCell = headRow.insertCell();
    fromHeadCell.innerText = "FROM";
    fromHeadCell.width = "100px";

    //to
    var toHeadCell = headRow.insertCell();
    toHeadCell.innerText = "TO";
    toHeadCell.width = "100px";

    //
    if (!this.model) {
        return;
    }

    //
    var metaNodeModels = this.model.getMetaNodeModels();
    for (var size = 0; size < metaNodeModels.size(); size++) {
        var model = metaNodeModels.get(size);
        this.addMetaNodeModel(model);
    }
};
XiorkFlowTableViewer.prototype.addMetaNodeModel = function (metaNodeModel) {
    if (!metaNodeModel) {
        return;
    }
    var row = this.addRow();
    row.className = "body";
    var metaNodeRow = new MetaNodeRow(this, row, metaNodeModel);
};
XiorkFlowTableViewer.prototype.addRow = function () {
    var row = this.table.insertRow();
    this.rows.add(row);
    return row;
};
XiorkFlowTableViewer.prototype.clearTable = function () {
    Toolkit.clearElement(this.table);
    this.rows.clear();
};
XiorkFlowTableViewer.prototype.removeRow = function (row) {
    if (!row) {
        return;
    }
    for (var i = 0; i < this.rows.size(); i++) {
        var tempRow = this.rows.get(i);
        if (tempRow == row) {
            this.table.deleteRow(i);
            this.rows.remove(row);
            return;
        }
    }
};

//
/**
 *
 */
function MetaNodeRow(tableViewer, row, model) {
    this.tableViewer = tableViewer;
    this.row = row;

    //id
    var idCell = this.row.insertCell();

    //name
    this.nameCell = this.row.insertCell();

    //type
    this.typeCell = this.row.insertCell();

    //x
    this.xCell = this.row.insertCell();

    //y
    this.yCell = this.row.insertCell();

    //width
    this.widthCell = this.row.insertCell();

    //height
    this.idCell = this.row.insertCell();

    //from
    this.fromCell = this.row.insertCell();

    //to
    this.toCell = this.row.insertCell();

    //
    this.setModel(model);
    idCell.innerText = this.model.getID();
    this.typeCell.innerText = this.model.type;
}
MetaNodeRow.prototype = new Object();
MetaNodeRow.prototype.setModel = function (model) {
    if (this.model == model) {
        return;
    }
    if (this.model) {
        this.model.removeObserver(this);
    }
    this.model = model;
    this.model.addObserver(this);

    //
    this._updatePosition();
    this._updateSize();
    this._updateText();
    this._updateFrom();
    this._updateTo();
};
MetaNodeRow.prototype._updatePosition = function () {
    this.xCell.innerText = this.model.getPosition().getX();
    this.yCell.innerText = this.model.getPosition().getY();
};
MetaNodeRow.prototype._updateSize = function () {
    this.widthCell.innerText = this.model.getSize().getWidth();
    this.idCell.innerText = this.model.getSize().getHeight();
};
MetaNodeRow.prototype._updateText = function () {
    this.nameCell.innerText = this.model.getText();
};
MetaNodeRow.prototype._updateFrom = function () {
    var tos = this.model.getTos();
    var tosStr = " ";
    var tosSize = tos.size() - 1;
    for (var i = 0; i < tosSize; i++) {
        var fromMetaNodeModel = tos.get(i).getFromMetaNodeModel();
        if (fromMetaNodeModel) {
            tosStr += fromMetaNodeModel.getID() + ",";
        }
    }
    if (tosSize >= 0) {
        var fromMetaNodeModel = tos.get(tosSize).getFromMetaNodeModel();
        if (fromMetaNodeModel) {
            tosStr += fromMetaNodeModel.getID();
        }
    }
    this.fromCell.innerText = tosStr;
};
MetaNodeRow.prototype._updateTo = function () {
    var froms = this.model.getFroms();
    var fromsStr = " ";
    var fromsSize = froms.size() - 1;
    for (var i = 0; i < fromsSize; i++) {
        var toMetaNodeModel = froms.get(i).getToMetaNodeModel();
        if (toMetaNodeModel) {
            fromsStr += toMetaNodeModel.getID() + ",";
        }
    }
    if (fromsSize >= 0) {
        var toMetaNodeModel = froms.get(fromsSize).getToMetaNodeModel();
        if (toMetaNodeModel) {
            fromsStr += toMetaNodeModel.getID();
        }
    }
    this.toCell.innerText = fromsStr;
};
MetaNodeRow.prototype._suicide = function () {
    this.tableViewer.removeRow(this.row);
    this.row = null;
};
MetaNodeRow.prototype.update = function (observable, arg) {
    switch (arg) {
      case MetaNodeModel.POSITION_CHANGED:
        this._updatePosition();
        break;
      case MetaNodeModel.SIZE_CHANGED:
        this._updateSize();
        break;
      case MetaModel.TEXT_CHANGED:
        this._updateText();
        break;
      case MetaNodeModel.FROM_CHANGED:
        this._updateTo();
        break;
      case MetaNodeModel.TO_CHANGED:
        this._updateFrom();
        break;
      case MetaModel.SUICIDE:
        this._suicide();
        break;
      default:
        break;
    }
};

