
/**
 * <p>Title:  </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XiorkFlowModelConverter() {
}

//
XiorkFlowModelConverter.convertModelToXML = function (model) {
    var doc = XMLDocument.newDomcument();

	//root
    var workflowProcessNode = doc.createElement(XiorkFlowModelConverter.NODE_ROOT);
    doc.documentElement = workflowProcessNode;//创建根结点

    //
    var activitiesNode = doc.createElement(XiorkFlowModelConverter.NODE_ACTIVITIES);
    workflowProcessNode.appendChild(activitiesNode);//行为结点集

    //metaNodes
    var metaNodeModels = model.getMetaNodeModels();//元结点模型集
    for (var i = 0; i < metaNodeModels.size(); i++) {
        var metaNodeModel = metaNodeModels.get(i);//元结点模型
        var activitieNode = XiorkFlowModelConverter.convertMetaNodeModelToXML(metaNodeModel, doc);//转化元结点模型到xml
        activitiesNode.appendChild(activitieNode);
    }
    
    //
    var transitionsNode = doc.createElement(XiorkFlowModelConverter.NODE_TRANSITIONS);
    workflowProcessNode.appendChild(transitionsNode);

    //
    var transitionModels = model.getTransitionModels();
    for (var i = 0; i < transitionModels.size(); i++) {
        var transitionModel = transitionModels.get(i);
        var transitionNode = XiorkFlowModelConverter.convertTransitionModelToXML(transitionModel, doc);
        transitionsNode.appendChild(transitionNode);
    }

    //
    return doc;
};
 //转化元结点模型到xml
XiorkFlowModelConverter.convertMetaNodeModelToXML = function (metaNodeModel, doc) {
    var activitieNode = doc.createElement(XiorkFlowModelConverter.NODE_ACTIVITIE);//创建行为结点

    //                                                                    元结点模型之ID      
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_ID, metaNodeModel.getID());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_TYPE, metaNodeModel.type);
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_NAME, metaNodeModel.getText());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_X_COORD, metaNodeModel.getPosition().getX());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_Y_COORD, metaNodeModel.getPosition().getY());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_WIDTH, metaNodeModel.getSize().getWidth());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_HEIGHT, metaNodeModel.getSize().getHeight());
    activitieNode.setAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_RANGE, metaNodeModel.getRange());//补    

    //
    return activitieNode;
};
//转化过渡结点模型到xml
XiorkFlowModelConverter.convertTransitionModelToXML = function (transitionModel, doc) {
    var transitionNode = doc.createElement(XiorkFlowModelConverter.NODE_TRANSITION);

    //
    transitionNode.setAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_ID, transitionModel.getID());
    transitionNode.setAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_NAME, transitionModel.getText());
    transitionNode.setAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_FROM, transitionModel.getFromMetaNodeModel().getID());
    transitionNode.setAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_TO, transitionModel.getToMetaNodeModel().getID());

    //
    return transitionNode;
};

//转化元节点到模型
XiorkFlowModelConverter.convertXMLToModel = function (doc, initModel) {
    if (!doc) {
        return null;
    }
    var model = initModel;
    if (!model) {
        model = new XiorkFlowModel();
    }

    //
    var activitieNodes = doc.getElementsByTagName("Activitie");
    for (var i = 0; i < activitieNodes.length; i++) {
        var activitieNode = activitieNodes[i];
        var metaNodeModel = XiorkFlowModelConverter.convertXMLToMetaNodeModel(activitieNode);
        model.addMetaNodeModel(metaNodeModel);
    }

    //
    var transitionNodes = doc.getElementsByTagName("Transition");
    for (var i = 0; i < transitionNodes.length; i++) {
        var transitionNode = transitionNodes[i];
        var transitionModel = XiorkFlowModelConverter.convertXMLToTransitionModel(transitionNode, model);
        model.addTransitionModel(transitionModel);
    }

    //
    return model;
};
XiorkFlowModelConverter.convertXMLToMetaNodeModel = function (node) {
    var metaNodeModel = null;

	//
    var type = node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_TYPE);
    switch (type) {
      case MetaNodeModel.TYPE_NODE:
        metaNodeModel = new NodeModel();
        break;
      case MetaNodeModel.TYPE_START_NODE:
        metaNodeModel = new StartNodeModel();
        break;
      case MetaNodeModel.TYPE_END_NODE:
        metaNodeModel = new EndNodeModel();
        break;
      case MetaNodeModel.TYPE_FORK_NODE:
        metaNodeModel = new ForkNodeModel();
        break;
      case MetaNodeModel.TYPE_JOIN_NODE:
        metaNodeModel = new JoinNodeModel();
        break;
      case MetaNodeModel.TYPE_EMPTY_NODE:
        metaNodeModel = new EmptyNodeModel();//men add
        break;
    }
    if (!metaNodeModel) {
        return null;
    }

    //
    var id = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_ID));
    metaNodeModel.setID(id);
    
    //
    var name = node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_NAME);
    metaNodeModel.setText(name);
    
    //
    var range = node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_RANGE);
    metaNodeModel.setRange(range);//补
    //
    var xCoordinate = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_X_COORD));
    var yCoordinate = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_Y_COORD));
    metaNodeModel.setPosition(new Point(xCoordinate, yCoordinate));

    //
    var width = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_WIDTH));
    var height = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_ACTIVITIE_HEIGHT));
    metaNodeModel.setSize(new Dimension(width, height));

    //
    return metaNodeModel;
};
XiorkFlowModelConverter.convertXMLToTransitionModel = function (node, model) {
    var fromID = node.getAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_FROM);
    fromMetaNodeModel = XiorkFlowModelConverter.getMetaNodeModel(model, fromID);
    var toID = node.getAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_TO);
    toMetaNodeModel = XiorkFlowModelConverter.getMetaNodeModel(model, toID);

    //
    var id = eval(node.getAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_ID));

    //
    var name = node.getAttribute(XiorkFlowModelConverter.ATTR_TRANSITION_NAME);
    name = name ? name : "";

    //
    var transitionModel = new TransitionModel(fromMetaNodeModel, toMetaNodeModel, id);

	//
    transitionModel.setText(name);

    //
    return transitionModel;
};
XiorkFlowModelConverter.getMetaNodeModel = function (model, id) {
    var metaNodeModels = model.getMetaNodeModels();
    for (var i = 0; i < metaNodeModels.size(); i++) {
        var metaNodeModel = metaNodeModels.get(i);
        if (metaNodeModel.getID() == id) {
            return metaNodeModel;
        }
    }
};

//static
XiorkFlowModelConverter.NODE_ROOT = "ProcedureflowProcess";//工序流程

//
XiorkFlowModelConverter.NODE_ACTIVITIES = "Activities";
XiorkFlowModelConverter.NODE_ACTIVITIE = "Activitie";
XiorkFlowModelConverter.ATTR_ACTIVITIE_ID = "id";
XiorkFlowModelConverter.ATTR_ACTIVITIE_TYPE = "type";
XiorkFlowModelConverter.ATTR_ACTIVITIE_NAME = "name";
XiorkFlowModelConverter.ATTR_ACTIVITIE_X_COORD = "xCoordinate";
XiorkFlowModelConverter.ATTR_ACTIVITIE_Y_COORD = "yCoordinate";
XiorkFlowModelConverter.ATTR_ACTIVITIE_WIDTH = "width";
XiorkFlowModelConverter.ATTR_ACTIVITIE_HEIGHT = "height";
XiorkFlowModelConverter.ATTR_ACTIVITIE_RANGE = "range";//补一个属性

//
XiorkFlowModelConverter.NODE_TRANSITIONS = "Transitions";
XiorkFlowModelConverter.NODE_TRANSITION = "Transition";
XiorkFlowModelConverter.ATTR_TRANSITION_ID = "id";
XiorkFlowModelConverter.ATTR_TRANSITION_NAME = "name";
XiorkFlowModelConverter.ATTR_TRANSITION_FROM = "from";
XiorkFlowModelConverter.ATTR_TRANSITION_TO = "to";

