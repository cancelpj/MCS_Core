
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XiorkFlowXMLViewer() {
    this.base = ScrollPanel;
    this.base();

    //
    this.setClassName("NAME_XIO_UI_FONT NAME_XIO_XIORKFLOW_XML_VIEWER");

    //
    this.pre = Toolkit.newElement("pre");
    this.getUI().appendChild(this.pre);
}
XiorkFlowXMLViewer.prototype = new ScrollPanel();
XiorkFlowXMLViewer.prototype.refresh = function (model) {
    this.pre.innerText = "";

	//
    var doc = XiorkFlowModelConverter.convertModelToXML(model);

    //
    this.pre.innerText = doc.xml;
};

