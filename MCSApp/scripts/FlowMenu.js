਍ഀ
var expandState = 0;਍ഀ
function moveAlong(layerName, paceLeft, paceTop, fromLeft, fromTop){਍ഀ
	clearTimeout(eval(layerName).timer)਍ഀ
	if(eval(layerName).curLeft != fromLeft){਍ഀ
		if((Math.max(eval(layerName).curLeft, fromLeft) - Math.min(eval(layerName).curLeft, fromLeft)) < paceLeft){eval(layerName).curLeft = fromLeft}਍ഀ
		else if(eval(layerName).curLeft < fromLeft){eval(layerName).curLeft = eval(layerName).curLeft + paceLeft}਍ഀ
			else if(eval(layerName).curLeft > fromLeft){eval(layerName).curLeft = eval(layerName).curLeft - paceLeft}਍ഀ
		if(ie){document.all[layerName].style.left = eval(layerName).curLeft}਍ഀ
		if(ns){document[layerName].left = eval(layerName).curLeft}਍ഀ
	}਍ഀ
	if(eval(layerName).curTop != fromTop){਍ഀ
   if((Math.max(eval(layerName).curTop, fromTop) - Math.min(eval(layerName).curTop, fromTop)) < paceTop){eval(layerName).curTop = fromTop}਍ഀ
		else if(eval(layerName).curTop < fromTop){eval(layerName).curTop = eval(layerName).curTop + paceTop}਍ഀ
			else if(eval(layerName).curTop > fromTop){eval(layerName).curTop = eval(layerName).curTop - paceTop}਍ഀ
		if(ie){document.all[layerName].style.top = eval(layerName).curTop}਍ഀ
		if(ns){document[layerName].top = eval(layerName).curTop}਍ഀ
	}਍ഀ
	eval(layerName).timer=setTimeout('moveAlong("'+layerName+'",'+paceLeft+','+paceTop+','+fromLeft+','+fromTop+')',30)਍ഀ
}਍ഀ
਍ഀ
function setPace(layerName, fromLeft, fromTop, motionSpeed){਍ഀ
	eval(layerName).gapLeft = (Math.max(eval(layerName).curLeft, fromLeft) - Math.min(eval(layerName).curLeft, fromLeft))/motionSpeed਍ഀ
	eval(layerName).gapTop = (Math.max(eval(layerName).curTop, fromTop) - Math.min(eval(layerName).curTop, fromTop))/motionSpeed਍ഀ
	moveAlong(layerName, eval(layerName).gapLeft, eval(layerName).gapTop, fromLeft, fromTop)਍ഀ
}਍ഀ
function FixY(){਍ഀ
	if(ie){sidemenu.style.top = document.body.scrollTop+58}//调节距页顶的高度਍ഀ
	if(ns){sidemenu.top = window.pageYOffset+58}਍ഀ
}਍ഀ
਍ഀ
function expand()਍ഀ
{਍ഀ
  if(expandState == 0)਍ഀ
  {਍ഀ
    setPace('master', 0, 10, 5); ਍ഀ
    if(ie)਍ഀ
    {਍ഀ
      document.menutop.src = '../images/menui.gif' ਍ഀ
    }; ਍ഀ
    expandState = 1;਍ഀ
  }਍ഀ
  else਍ഀ
  {਍ഀ
    setPace('master', -200, 10, 5); ਍ഀ
    if(ie)਍ഀ
    {਍ഀ
      document.menutop.src='../images/menuo.gif'਍ഀ
    }; ਍ഀ
    expandState = 0;਍ഀ
  }਍ഀ
}਍ഀ
਍ഀ
document.write("<style type=text/css>#master {LEFT: -200px; POSITION: absolute; TOP: 100px; VISIBILITY: visible; Z-INDEX: 999}</style>")਍ഀ
document.write("<table id=master width='218' border='0' cellspacing='0' cellpadding='0'><tr><td><img border=0 height=6 src='../images/menutop.gif'  width=200></td><td rowspan='2' valign='top'><img id=menu onMouseOver=javascript:expand() border=0 height=70 name=menutop src='../images/menuo.gif' width=18></td></tr>");਍ഀ
document.write("<tr><td valign='top'><table width='100%' border='0' cellspacing='2' cellpadding='0'><tr><td height='430' valign='top'><table width=100% height='100%' border=0 cellpadding=1 cellspacing=4 bordercolor='purple' bgcolor=#00ced1 style=FILTER: alpha(opacity=90)><tr>");਍ഀ
document.write("<td align='center' bordercolor='#ecf6f5'>关口列表</td></tr><tr><td valign='top' height='100%' bordercolor='#ecf6f5'>");਍ഀ
document.write("<iframe width='100%' height='100%' src='FlowTree.aspx' frameborder=0></iframe></td></tr></table></td></tr></table></td></tr></table>");਍ഀ
਍ഀ
var ie = document.all ? 1 : 0਍ഀ
var ns = document.layers ? 1 : 0਍ഀ
var master = new Object('element')਍ഀ
master.curLeft = -200;   ਍ഀ
master.curTop = 10;਍ഀ
master.gapLeft = 0;      ਍ഀ
master.gapTop = 0;਍ഀ
master.timer = null;਍ഀ
if(ie){var sidemenu = document.all.master;}਍ഀ
if(ns){var sidemenu = document.master;}਍ഀ
setInterval('FixY()',100);਍ഀ
਍ഀ
