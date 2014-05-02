
/**
 * <p>Description: </p>
 * <p>Copyright: </p>
 * @author amen
 */
function Ajax() {
}
Ajax.prototype = new Object();
Ajax.prototype.loadXMLHttpRequest = function (url, method, params) {
    //alert('url:'+url);
    //alert('method:'+method); 
    //alert('params:'+params); 
    //url='processes/default.xml';
    //method='GET';
    //params=null;

    var httpRequest = Ajax.createHttpRequest();
    var ajax = this;
    httpRequest.onreadystatechange = function () {
        ajax.onReadyStateChange(httpRequest);
    };
    method = method.toUpperCase();
    if (method == "GET") {

        httpRequest.open(method, url + "?" + params, true);
        //httpRequest.open("GET", "../processes/getXMLData.aspx?id=1&et="+Math.random(), true);
        httpRequest.setRequestHeader("Content-Type", "text/html;charset=UTF-8");
        params = "";
    } else {
        httpRequest.open(method, url, true);
        httpRequest.setRequestHeader("Content-Type", "text/html;charset=UTF-8");
        //httpRequest.setRequestHeader("Method", "POST " + url + " HTTP/1.1");
        //httpRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    }

    //
    httpRequest.send(params);
};
Ajax.prototype.onReadyStateChange = function (httpRequest) {
    if (httpRequest.readyState == 4) {
        if (httpRequest.status == 200) {
            this.processXMLHttpRequest(httpRequest);
        }
    }
};
Ajax.prototype.processXMLHttpRequest = function (httpRequest) {
};
Ajax.createHttpRequest = function () {
	//初始化、指定处理函数、发送请求的函数
    var httpRequest = false;
	//开始初始化XMLHttpRequest对象
    if (window.XMLHttpRequest) { //Mozilla 浏览器
        httpRequest = new XMLHttpRequest();
        if (httpRequest.overrideMimeType) {//设置MiME类别
            httpRequest.overrideMimeType("text/xml");
        }
    } else {
        if (window.ActiveXObject) { //IE浏览器
            try {
                httpRequest = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    httpRequest = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                }
            }
        }
    }
    if (!httpRequest) { //异常，创建对象实例失败
        window.alert("\u4e0d\u80fd\u521b\u5efaXMLHttpRequest\u5bf9\u8c61\u5b9e\u4f8b.");
        return false;
    }
    return httpRequest;
};

//
Ajax.READY_STATE_UNINITIALIZED = 0;
Ajax.READY_STATE_LOADING = 1;
Ajax.READY_STATE_LOADED = 2;
Ajax.READY_STATE_INTERACTIVE = 3;
Ajax.READY_STATE_COMPLETE = 4;

//c http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
Ajax.HTTP_RESPONSE_STATUS_OK = 200;

