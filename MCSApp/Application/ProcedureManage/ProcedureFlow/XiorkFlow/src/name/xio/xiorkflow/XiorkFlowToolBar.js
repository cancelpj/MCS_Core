
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function XiorkFlowToolBar(xiorkFlow) {
    this.base = ToolBar;
    this.base();

    //
    this.xiorkFlow = xiorkFlow;

    //
    this.addSeparator();

    //
    this.saveButton = new Button(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/save.gif", "\u4fdd\u5b58");
    //保存
    this.saveButton.setToolTipText("\u4fdd\u5b58");
    this.saveButton.addActionListener(new SaveActionListener(this.xiorkFlow));
    this.add(this.saveButton);

    //
    this.nodeButtonGroup = new ButtonGroup();

    //
    this.addSeparator();

    //
    this.selectButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/select.gif", "", true);
    //选择
    this.selectButton.setToolTipText("\u9009\u62e9");
    this.add(this.selectButton);
    this.nodeButtonGroup.add(this.selectButton);
    this.selectButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_SELECT;

    //
    this.addSeparator();

    //
    this.startButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/start.gif");
    //流程开始节点
    this.startButton.setToolTipText("\u6D41\u7A0B\u5F00\u59CB\u8282\u70B9");
    this.add(this.startButton);
    this.nodeButtonGroup.add(this.startButton);
    this.startButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_START_NODE;

    //
    this.endButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/end.gif", "");
    //流程结束节点
    this.endButton.setToolTipText("\u6D41\u7A0B\u7ED3\u675F\u8282\u70B9");
    this.add(this.endButton);
    this.nodeButtonGroup.add(this.endButton);
    this.endButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_END_NODE;

    //
    this.addSeparator();

    //
    this.nodeButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/node.gif");
    //工序节点
    this.nodeButton.setToolTipText("\u5DE5\u5E8F\u8282\u70B9");
    this.add(this.nodeButton);
    this.nodeButtonGroup.add(this.nodeButton);
    this.nodeButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_NODE;

    //
    this.forkButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/fork.gif");
    //发散工序节点
    this.forkButton.setToolTipText("\u53D1\u6563\u5DE5\u5E8F\u8282\u70B9");
    this.add(this.forkButton);
    this.nodeButtonGroup.add(this.forkButton);
    this.forkButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_FORK_NODE;

    //
    this.joinButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/join.gif");
    //汇聚工序节点
    this.joinButton.setToolTipText("\u6C47\u805A\u5DE5\u5E8F\u8282\u70B9");
    this.add(this.joinButton);
    this.nodeButtonGroup.add(this.joinButton);
    this.joinButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_JOIN_NODE;
    //
      
    //
    this.emptyButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/empty.gif");//空节点图标
    //空节点
    this.emptyButton.setToolTipText("\u7A7A\u8282\u70B9");
    this.add(this.emptyButton);
    this.nodeButtonGroup.add(this.emptyButton);
    this.emptyButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_EMPTY_NODE;
    //
    this.addSeparator();

    //
    this.transitionButton = new ToggleButton(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/transition.gif");
    //连接线
    this.transitionButton.setToolTipText("\u8FDE\u63A5\u7EBF");
    this.add(this.transitionButton);
    this.nodeButtonGroup.add(this.transitionButton);
    this.transitionButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_TRANSITION;

    //
    this.addSeparator();

    //
    this.deleteButton = new Button(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/delete.gif");
    //删除
    this.deleteButton.setToolTipText("\u5220\u9664");
    this.deleteButton.addActionListener(new DeleteMetaActionListener(this.xiorkFlow));
    this.add(this.deleteButton);

    //
    this.addSeparator();

    //
    this.viewerPatternButtonGroup = new ButtonGroup();

    //design
    var designButton = new ToggleButton("", "\u8bbe\u8ba1", true);
    //设计视图
    designButton.setToolTipText("\u8bbe\u8ba1\u89c6\u56fe");
    this.add(designButton);
    this.viewerPatternButtonGroup.add(designButton);
    designButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_DESIGN;

    //table
    var tableButton = new ToggleButton("", "\u8868\u683c", true);
    //表格视图
    tableButton.setToolTipText("\u8868\u683c\u89c6\u56fe");
    this.add(tableButton);
    this.viewerPatternButtonGroup.add(tableButton);
    tableButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_TABLE;

    //混合显示
    var mixButton = new ToggleButton("", "\u6df7\u5408\u663e\u793a", true);
    //混合视图
    mixButton.setToolTipText("\u6df7\u5408\u89c6\u56fe");
    this.add(mixButton);
    this.viewerPatternButtonGroup.add(mixButton);
    mixButton.getModel().name = XiorkFlowToolBar.BUTTON_NAME_MIX;

    //
    //this.addSeparator();

    //
    //var helpButton = new Button(XiorkFlowWorkSpace.XIORK_FLOW_PATH + "images/xiorkflow/help.gif", "\u5e2e\u52a9");
    //helpButton.addActionListener(new HelpActionListener());
    //帮助
    //helpButton.setToolTipText("\u5e2e\u52a9");
    //this.add(helpButton);
}
XiorkFlowToolBar.prototype = new ToolBar();
XiorkFlowToolBar.prototype.getNodeButtonGroup = function () {
    return this.nodeButtonGroup;
};
XiorkFlowToolBar.prototype.setDesignButtonEnable = function (b) {
    var buttons = this.nodeButtonGroup.getButtons();
    for (var i = 0; i < buttons.size(); i++) {
        buttons.get(i).getModel().setEnabled(b);
    }
    this.deleteButton.getModel().setEnabled(b);
};
XiorkFlowToolBar.prototype.setButtonEnable = function (b) {
    var buttons = this.nodeButtonGroup.getButtons();
    for (var i = 0; i < buttons.size(); i++) {
        buttons.get(i).getModel().setEnabled(b);
    }
    var viewPatternbuttons = this.viewerPatternButtonGroup.getButtons();
    for (var i = 0; i < viewPatternbuttons.size(); i++) {
        viewPatternbuttons.get(i).getModel().setEnabled(b);
    }
    this.deleteButton.getModel().setEnabled(b);
    this.saveButton.getModel().setEnabled(b);
};
XiorkFlowToolBar.prototype.getViewerPatternButtonGroup = function () {
    return this.viewerPatternButtonGroup;
};

//
XiorkFlowToolBar.prototype.update = function (observable, arg) {
    if (arg instanceof Array) {
        if (arg.size() == 2) {
            var property = arg[0];
            var state = arg[1];
            if (property == StateMonitor.OPERATION_STATE_RESET) {
                switch (state) {
                  case StateMonitor.SELECT:
                    this.selectButton.getModel().setPressed(true);
                    break;
                  case StateMonitor.NODE:
                    this.nodeButton.getModel().setPressed(true);
                    break;
                  case StateMonitor.FORK_NODE:
                    this.forkNodeButton.getModel().setPressed(true);
                    break;
                  case StateMonitor.JOIN_NODE:
                    this.joinNode.getModel().setPressed(true);
                    break;
                  case StateMonitor.START_NODE:
                    this.startNodeButton.getModel().setPressed(true);
                    break;
                  case StateMonitor.END_NODE:
                    this.endNodeButton.getModel().setPressed(true);
                    break;                  
                    case StateMonitor.EMPTY_NODE:
                    this.endNodeButton.getModel().setPressed(true);//men add 
                    break;
                  case StateMonitor.TRANSITION:
                    this.transitionButton.getModel().setPressed(true);
                    break;
                  default:
                    break;
                }
            }
        }
    }
};

//
XiorkFlowToolBar.BUTTON_NAME_SELECT = "BUTTON_NAME_SELECT";
XiorkFlowToolBar.BUTTON_NAME_START_NODE = "BUTTON_NAME_START_NODE";
XiorkFlowToolBar.BUTTON_NAME_END_NODE = "BUTTON_NAME_END_NODE";
XiorkFlowToolBar.BUTTON_NAME_FORK_NODE = "BUTTON_NAME_FORK_NODE";
XiorkFlowToolBar.BUTTON_NAME_JOIN_NODE = "BUTTON_NAME_JOIN_NODE";
XiorkFlowToolBar.BUTTON_NAME_EMPTY_NODE = "BUTTON_NAME_EMPTY_NODE";
XiorkFlowToolBar.BUTTON_NAME_NODE = "BUTTON_NAME_NODE";
XiorkFlowToolBar.BUTTON_NAME_TRANSITION = "BUTTON_NAME_TRANSITION";

//
XiorkFlowToolBar.BUTTON_NAME_DESIGN = "BUTTON_NAME_DESIGN";
XiorkFlowToolBar.BUTTON_NAME_TABLE = "BUTTON_NAME_TABLE";
XiorkFlowToolBar.BUTTON_NAME_MIX = "BUTTON_NAME_MIX";

