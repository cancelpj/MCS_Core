<!--
//可以打包为js文件;
var x0=0,y0=0,x1=0,y1=0;
var offx=6,offy=6;
var moveable=false;
var hover='orange',normal='#336699';//color;
var index=10000;//z-index;
//开始拖动;
function startDrag(obj)
{
if(event.button==1)
{
//锁定标题栏;
obj.setCapture();
//定义对象;
var win = obj.parentNode;
var sha = win.nextSibling;
//记录鼠标和层位置;
x0 = event.clientX;
y0 = event.clientY;
x1 = parseInt(win.style.left);
y1 = parseInt(win.style.top);
//记录颜色;
normal = obj.style.backgroundColor;
//改变风格;
obj.style.backgroundColor = hover;
win.style.borderColor = hover;
obj.nextSibling.style.color = hover;
sha.style.left = x1 + offx;
sha.style.top = y1 + offy;
moveable = true;
}
}
//拖动;
function drag(obj)
{
if(moveable)
{
var win = obj.parentNode;
var sha = win.nextSibling;
win.style.left = x1 + event.clientX - x0;
win.style.top = y1 + event.clientY - y0;
sha.style.left = parseInt(win.style.left) + offx;
sha.style.top = parseInt(win.style.top) + offy;
}
}
//停止拖动;
function stopDrag(obj)
{
if(moveable)
{
var win = obj.parentNode;
var sha = win.nextSibling;
var msg = obj.nextSibling;
win.style.borderColor     = normal;
obj.style.backgroundColor = normal;
msg.style.color           = normal;
sha.style.left = obj.parentNode.style.left;
sha.style.top = obj.parentNode.style.top;
obj.releaseCapture();
moveable = false;
}
}
//获得焦点;
function getFocus(obj)
{
if(obj.style.zIndex!=index)
{
index = index + 2;
var idx = index;
obj.style.zIndex=idx;
obj.nextSibling.style.zIndex=idx-1;
}
}
//最小化;
function min(obj)
{
var win = obj.parentNode.parentNode;
var sha = win.nextSibling;
var tit = obj.parentNode;
var msg = tit.nextSibling;
var flg = msg.style.display=="none";
if(flg)
{
win.style.height = parseInt(msg.style.height) + parseInt(tit.style.height) + 2*2;
sha.style.height = win.style.height;
msg.style.display = "block";
obj.innerHTML = "0";
}
else
{
win.style.height = parseInt(tit.style.height) + 2*2;
sha.style.height = win.style.height;
obj.innerHTML = "2";
msg.style.display = "none";
}
}
//创建一个对象;
function xWin(id,w,h,l,t,tit,msg)
{
index = index+2;
this.id      = id;
this.width   = w;
this.height = h;
this.left    = l;
this.top     = t;
this.zIndex = index;
this.title   = tit;
if(typeof msg=="object"){
this.message = msg.innerHTML;
msg.parentNode.removeChild(msg);
}
else
this.message = msg;
this.obj     = null;
this.bulid   = bulid;
this.bulid();
}
//初始化;
function bulid()
{
var str = ""
+ "<div id=xMsg" + this.id + " "
+ "style='"
+ "z-index:" + this.zIndex + ";"
+ "width:" + this.width + ";"
+ "height:" + this.height + ";"
+ "left:" + this.left + ";"
+ "top:" + this.top + ";"
+ "background-color:" + normal + ";"
+ "color:" + normal + ";"
+ "font-size:8pt;"
+ "font-family:Tahoma;"
+ "position:absolute;"
+ "cursor:default;"
+ "border:2px solid " + normal + ";"
+ "' "
+ "onmousedown='getFocus(this)'>"
+ "<div "
+ "style=' align=center"
+ "background-color:" + normal + ";"
+ "width:" + (this.width-2*2) + ";"
+ "height:20;"
+ "color:white;"
+ "' "
+ "onmousedown='startDrag(this)' "
+ "onmouseup='stopDrag(this)' "
+ "onmousemove='drag(this)' "
+ "ondblclick1='min(this.childNodes[1])'"
+ ">"
+ "<table border=0 width=100%><tr><td><span style='width:" + (this.width-2*12-4) + ";padding-left:3px;'>" + this.title + "</span></td><td align=right>"
+ "<span style=' width:12;border-width:0px;color:white;font-family:webdings;' onclick='ShowHide(\""+this.id+"\",null)'>r</span></td></tr></table>"
+ "</div>"
+ "<div style='"
+ "width:"+(this.width-2*2-2)+"; "
+ "height:" + (this.height-20-8) + ";"
+ "background-color:white;"
+ "line-height:14px;"
+ "word-break:break-all;"
+ "padding:3px;"
+ "'><iframe src=EmployeeForQuery.aspx width=100% height=100% frameborder=0 scrolling=auto></iframe></div>"
+ "</div>"
+ "<div id=xMsg" + this.id + "bg style='"
+ "width:" + this.width + ";"
+ "height:" + this.height + ";"
+ "top:" + this.top + ";"
+ "left:" + this.left + ";"
+ "z-index:" + (this.zIndex-1) + ";"
+ "position:absolute;"
+ "background-color:black;"
+ "filter:alpha(opacity=40);"
+ "'></div>";
//document.body.insertAdjacentHTML("beforeEnd",str);

var test=document.getElementById("test");
test.insertAdjacentHTML("beforeEnd",str);

}
//显示隐藏窗口
function ShowHide(id,dis){
var bdisplay = (dis==null)?((document.getElementById("xMsg"+id).style.display=="")?"none":""):dis
document.getElementById("xMsg"+id).style.display = bdisplay;
document.getElementById("xMsg"+id+"bg").style.display = bdisplay;
}



function initialize()
{
var b = new xWin("1",800,600,50,15,"弹出窗口",document.getElementById("div1"));
ShowHide("1","none");//隐藏窗口1

var show=""
if(show!=""){
   ShowHide('1',"block");
}
} 
window.onload = initialize;
//-->