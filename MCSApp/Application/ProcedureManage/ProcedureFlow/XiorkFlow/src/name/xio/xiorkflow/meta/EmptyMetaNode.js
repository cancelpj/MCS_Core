
/**
 * <p>Title: MetaNode</p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function EmptyMetaNode(model, img, wrapper) {
    this.base = Panel;
    this.base(Toolkit.newLayer());
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_XIORKFLOW_METANODE");

    //
    this.wrapper = wrapper;

    //bound rectangle
    var rectangleUrl = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/rectangle.gif";
    //lefttop
    this.lefttopRetangle = new Component(Toolkit.newImage());
    this.lefttopRetangle.getUI().src = rectangleUrl;
    this.lefttopRetangle.setLeft("-5px");
    this.lefttopRetangle.setTop("-5px");
    this.lefttopRetangle.setPosition("absolute");
    this.add(this.lefttopRetangle);
    //righttop
    this.righttopRetangle = new Component(Toolkit.newImage());
    this.righttopRetangle.getUI().src = rectangleUrl;
    this.righttopRetangle.setRight("-5px");
    this.righttopRetangle.setTop("-5px");
    this.righttopRetangle.setPosition("absolute");
    this.add(this.righttopRetangle);
    //leftbottom
    this.leftbottomRetangle = new Component(Toolkit.newImage());
    this.leftbottomRetangle.getUI().src = rectangleUrl;
    this.leftbottomRetangle.setLeft("-5px");
    this.leftbottomRetangle.setBottom("-5px");
    this.leftbottomRetangle.setPosition("absolute");
    this.add(this.leftbottomRetangle);
    //rightbottom
    this.rightbottomRetangle = new Component(Toolkit.newImage());
    this.rightbottomRetangle.getUI().src = rectangleUrl;
    this.rightbottomRetangle.setRight("-5px");
    this.rightbottomRetangle.setBottom("-5px");
    this.rightbottomRetangle.setPosition("absolute");
    this.add(this.rightbottomRetangle);
    this.rightbottomRetangle.setCursor(Cursor.RESIZE_SE);

    //
    this.table = Toolkit.newTable();
    this.table.width = "100%";
    this.table.height = "100%";//¾¡Á¿µÄÐ¡
    this.table.cellPadding = 0;
    this.table.cellSpacing = 0;
    
    this.add(this.table);

    //
    var titleRow = this.table.insertRow(-1);
    titleRow.className = "TITLE";
    
    //
    var titleImgCell = titleRow.insertCell(-1);
    titleImgCell.align = "center";
    titleImgCell.valign = "middle";
    if (!img) {
        img = XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/empty.gif";
    }
    var titleImg = Toolkit.newLayer();
    titleImg.className = "IMG";
    titleImg.style.background = "url(' " + img + "')"; 
    titleImgCell.appendChild(titleImg);
    
    //
    /*this.titleTxtCell = titleRow.insertCell(-1);
    this.titleTxtCell.align = "center";
    this.titleTxtCell.valign = "middle";
    this.titleTxtCell.className = "TXT";
    
    //
    this.titleInputCell = titleRow.insertCell(-1);
    this.titleInputCell.align = "left";
    this.titleInputCell.valign = "middle";
    this.titleInput = Toolkit.newElement("<input type=\"text\">");
    this.titleInput.style.display = "none";
    var _MetaNode = this;
    this.titleInput.onchange = function () {
        //_MetaNode.stopEdit();
    };
    this.titleInput.onblur = function () {
        //_MetaNode.stopEdit();
    };
    this.titleInputCell.appendChild(this.titleInput);*/
   
    //
    this.setModel(model);
    this.rightbottomRetangle.addMouseListener(new MetaNodeResizeMouseListener(this.rightbottomRetangle, model, this.wrapper));
}
MetaNode.prototype = new Panel();

//
MetaNode.prototype.setModel = function (model) {
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
    this._updateBoundRectangle();

};
MetaNode.prototype.getModel = function () {
    return this.model;
};

//men add
//
MetaNode.prototype.startEdit = function () {
    this.titleTxtCell.style.display = "none";
    this.titleInput.style.display = "";
    this.titleInputCell.style.display = "";
    this.titleInput.focus();
    //this.getModel().setEditing(true);

    this.getModel().setEditing(true);    
};
MetaNode.prototype.stopEdit = function () {
    this.titleTxtCell.style.display = "";
    this.titleInput.style.display = "none";
    this.titleInputCell.style.display = "none";
    this.getModel().setText(this.titleInput.value);
    this.getModel().setEditing(false);    

};

//
MetaNode.prototype._updatePosition = function () {
    var point = this.model.getPosition();
    this.setLeft(point.getX() + "px");
    this.setTop(point.getY() + "px");
};
MetaNode.prototype._updateSize = function () {
    var size = this.model.getSize();
    this.setWidth(size.getWidth() + "px");
    this.setHeight(size.getHeight() + "px");//
};
MetaNode.prototype._updateText = function () {
    var text = this.model.getText();
    this.titleInput.value = text;  
    this.titleTxtCell.innerText = text;
};
MetaNode.prototype._updateBoundRectangle = function () {
    if (this.model.isSelected()) {
        this.lefttopRetangle.setClassName("BOUND_RECTANGLE");
        this.righttopRetangle.setClassName("BOUND_RECTANGLE");
        this.leftbottomRetangle.setClassName("BOUND_RECTANGLE");
        this.rightbottomRetangle.setClassName("BOUND_RECTANGLE");
    } else {
        this.lefttopRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");
        this.righttopRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");
        this.leftbottomRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");
        this.rightbottomRetangle.setClassName("BOUND_RECTANGLE_UNSELECTED");

        this.stopEdit();        
    }
};

//
MetaNode.prototype.update = function (observable, arg) {
    this.wrapper.setChanged(true);
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
      case MetaModel.SELECTED_CHANGED:
        this._updateBoundRectangle();
        break;
      case MetaModel.SUICIDE:
        this._suicide();
        break;
      default:
        break;
    }
};

//
MetaNode.prototype._suicide = function () {
    this.listenerProxy.clear();
    if (!this.wrapper) {
        return;
    }
    this.wrapper.removeMetaNode(this);
};

