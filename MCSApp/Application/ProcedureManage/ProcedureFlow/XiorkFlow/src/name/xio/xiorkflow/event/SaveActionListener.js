
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
 var wholeName="";
function SaveActionListener(xiorkFlow) {
    this.xiorkFlow = xiorkFlow;
}
SaveActionListener.prototype.actionPerformed = function (obj) {
    var wrapper = this.xiorkFlow.getWrapper();
    var toolbar = this.xiorkFlow.getToolBar();
    var name = wrapper.getModel().getName();
    if(name)
    {
        wholeName=wrapper.getModel().getName();//设置全局名称
    }
    else
    {
        wrapper.getModel().setName(wholeName)//取全局名称用
    }

//    if (!name) {
//    	//请输入您将工作流程图保存成的名字？
//        name = prompt("\u8bf7\u8f93\u5165\u5de5\u4f5c\u6d41\u7a0b\u56fe\u7684\u540d\u5b57\uff1f", "");
//        if (name != null && name != "") {
//            var addProcess = new AddProcess(wrapper, toolbar, this.xiorkFlow.getProcessList());
//            addProcess.addProcess(name);
//        }
//    } else {
//        var updateProcess = new UpdateProcess(wrapper, toolbar);
//        updateProcess.updateProcess();
//    }


        var updateProcess = new UpdateProcess(wrapper, toolbar);
        updateProcess.updateProcess();

};

