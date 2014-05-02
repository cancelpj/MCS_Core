
var oPopup = window.createPopup();
/*****************************************************
取指定对象的x坐标
*****************************************************/
function getx(e){
  var l=e.offsetLeft;
  while(e=e.offsetParent){
    l+=e.offsetLeft;
    }
  return(l);
  }
/*****************************************************
取指定对象的y坐标
*****************************************************/
function gety(e){
  var t=e.offsetTop;
  while(e=e.offsetParent){
    t+=e.offsetTop;
    }
  return(t);
  }
/*****************************************************
显示popup窗体
c:窗体内容obj
x:相对o的x坐标距离
y:相对o的y坐标距离
w:宽度
h:高度，如果设置为0则自行获取默认高度
o:相对于何obj
*****************************************************/
function showpopup(c,x,y,w,h,o)
{
	oPopup.document.body.innerHTML = c.innerHTML;
	oPopup.document.createStyleSheet(document.styleSheets[0].href );
	var popupBody = oPopup.document.body;
	oPopup.show(0, 0, w, 0);
	h=h==0?popupBody.scrollHeight:h;
	oPopup.hide();
	oPopup.show(x, y, w, h, o);
}
/*****************************************************
提取xml根节点
*****************************************************/
function getxmldoc(url)
{
    var oXMLDoc = new ActiveXObject('MSXML');
    oXMLDoc.url = url;
	var ooRoot=oXMLDoc.root;
	return ooRoot;
}
/*****************************************************
所有gif图片鼠标经过效果，鼠标经过后显示的图片文件名在原图片名后加_over
*****************************************************/
function imgover(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG") //不是img对象则退出
		return false;
	var r, re;                    // 声明变量。
	var ss = obj.src;
	re = /.gif$\b/i;             // 创建正则表达式模式。
	r = ss.replace(re, "_over.gif");    //加上_over
	obj.src=r;
	obj.behave='over';
}
/*****************************************************
所有gif图片鼠标按下效果，鼠标按下后显示的图片文件名在原图片名后加_down
*****************************************************/
function imgdown(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(obj.tagName!="IMG") //不是img对象则退出
		return false;
	var r, re;                    // 声明变量。
	var ss = obj.src;
	if(obj.behave=='over')
	{
		re = /_over.gif$\b/i;             
		r = ss.replace(re, "_down.gif"); 
	}
	if(obj.behave=='')
	{
		re = /.gif$\b/i;             // 创建正则表达式模式。
		r = ss.replace(re, "_down.gif");    //加上_down
	}
	obj.src=r;
	obj.behave='down';
}
/*****************************************************
所有gif图片鼠标按下效果，鼠标经过后显示的图片文件名在原图片名后加_over
*****************************************************/
function imgup(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG") //不是img对象则退出
		return false;
	var r, re;                    // 声明变量。
	var ss = obj.src;
	re = /_down.gif$\b/i;             // 创建正则表达式模式。
	r = ss.replace(re, "_over.gif");   
	obj.src=r;
	obj.behave='over';
}
/*****************************************************
所有gif图片鼠标移出效果，鼠标经过后显示的图片文件名在原图片名后去掉_over
*****************************************************/
function imgout(obj)
{
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG")	//不是img对象则退出
		return false;
	var r, re;                    // 声明变量。
	var r = ss = obj.src;
	if(obj.behave=='over')
	{
		re = /_over.gif$\b/i;             
		r = ss.replace(re, ".gif");    
	}
	if(obj.behave=='down')
	{
		re = /_down.gif$\b/i;             
		r = ss.replace(re, ".gif"); 
	}
	obj.src=r;           
	obj.behave='';
}
/*****************************************************
所有样式表鼠标经过效果，鼠标经过后原样式表在后加over
*****************************************************/
function classover(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	if(obj.behave=='over')return;
	var ss = obj.className;
	var r = ss+"_over";    //加上over
	obj.className=r;
	obj.behave='over';
}
/*****************************************************
所有样式表鼠标移出效果，鼠标移出后原样式表在后去掉over
*****************************************************/
function classout(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	var r, re;                    // 声明变量。
	var ss = obj.className;
	if(obj.behave=='over')
	{
		re = /_over$\b/i;             // 创建正则表达式模式。
		r = ss.replace(re, "");    
	}
	if(obj.behave=='down')
	{
		re = /_down$\b/i;             // 创建正则表达式模式。
		r = ss.replace(re, "");    
	}
	obj.className=r;              
	obj.behave='';
}
/*****************************************************
所有样式表鼠标按下效果，鼠标按下后原样式表在后加上down
*****************************************************/
function classdown(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	var r, re;                    // 声明变量。
	var ss = obj.className;
	re = /_over$\b/i;             // 创建正则表达式模式。
	r = ss.replace(re, "_down");    
	obj.className=r;              
	obj.behave='down';
}
/*****************************************************
所有样式表鼠标释放效果，鼠标释放后原样式表在后
*****************************************************/
function classup(obj)
{
	if(obj.locked == "true") return; //如果对象被锁定，则不触发事件
	if(typeof(obj)!="object")return false;
	var r, re;                    // 声明变量。
	var ss = obj.className;
	re = /_down$\b/i;             // 创建正则表达式模式。
	r = ss.replace(re, "_over");   
	obj.className=r;
	obj.behave='over';
}
/*****************************************************
检查日期正确性
*****************************************************/
function chkDateTime(str){
	var reg = /^(\d{1,4})-(\d{1,2})-(\d{1,2})$/; 
	var r = str.match(reg); 
	if(r==null)return false; 
	var d= new Date(r[1], --r[2],r[3]); 
	if(d.getFullYear()!=r[1])return false;
	if(d.getMonth()!=r[2])return false;
	if(d.getDate()!=r[3])return false;
	return true;
}
