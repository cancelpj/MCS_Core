		var dateField=null;
		var keepState=false;
		var formatArray=new Array("gb2312","日","一","二","三","四","五","六","年","月","昨天","今天","请输入正确日期格式的数据！\n例：2005-01-01");
	
		function selectDate(obj){
			dateField=obj;			
			dateFieldInit(obj.value);
			var obj1=document.all("__DateTimeBoxDiv");
			
			var obj2=obj1.children[0];
			//obj1.style.xpos = window.event.x;
			//obj1.style.ypos = window.event.y;
			obj1.style.xpos = window.event.x+document.body.scrollLeft-10;
			obj1.style.ypos = window.event.y+document.body.scrollTop-10;			
			if((parseInt(window.event.x)+parseInt(obj2.width))>=parseInt(document.body.offsetWidth)){
				//obj1.style.xpos=(parseInt(document.body.scrollWidth)-parseInt(obj2.width));
			}
			if((parseInt(window.event.y)+parseInt(obj2.height))>=parseInt(document.body.offsetHeight)){
				//obj1.style.ypos=(parseInt(document.body.offsetHeight)-parseInt(obj2.height)+parseInt(document.body.scrollTop));
			}
			obj1.style.left = obj1.style.xpos;
			obj1.style.top = obj1.style.ypos;
			document.all("__DateTimeBoxDiv").style.display="";
		}
		
		function __DateTimeBoxSizeTo(x,y){
			document.all("__DateTimeIFrame").style.pixelWidth=x;
			document.all("__DateTimeIFrame").style.pixelHeight=y;
		}
		
		function __DateTimeBoxAllHidden(){
			document.all("__DateTimeBoxDiv").style.display="none";
		}
		
		function __DateTimeBoxDivMouseOut(){
			if(!keepState)
			{
				__DateTimeBoxAllHidden();
            }
			else
			{
				//do nothing
				keepState=false;
		    }
			
		}
		
		function __DateTimeBoxInit(){
			var str="";
			str+="<span id=\"__DateTimeBoxDiv\" onmouseout=\"__DateTimeBoxDivMouseOut();\"  style=\"display:none; position:absolute; left:; top:; width:; Height:; z-index:998\" class=\"\">";
			str+="<iframe id=\"__DateTimeIFrame\" name=\"__DateTimeIFrame\" src=\"about:blank\" frameborder=\"0\" width=\"240\" height=\"185\" scrolling=\"no\"></iframe>";
			str+="</span>";
			//prompt("",str);
			document.write(str);
		}
		
		function __DateBoxOnBlur(obj,canNull){
		if(canNull=='true'&&obj.value=='')
		{ 
		    return;  
		}
			if(!__DateBoxCheckDate(obj.value)){
				obj.value=obj.defaultValue.toString();
				alert(formatArray[12]);
			}else{
				var month=obj.value.split("-")[1];
				var day=obj.value.split("-")[2];
				if(month.length<2 || day.length<2){
					if(month.length<2)month="0"+month;
					if(day.length<2)day="0"+day;
					obj.value=obj.value.split("-")[0]+"-"+month+"-"+day;
				}
				
				
			}
		}
		
		function __DateBoxCheckDate(bdate){
			if (bdate.length == 0) return true;
			var re=/^(([1-2]\d{3})\-(0?[1|3|5|7|8]|12|10)\-([1-2]?[0-9]|0[1-9]|30|31)|([1-2]\d{3})\-(0?[4|6|9]|11)\-([1-2]?[0-9]|0[1-9]|30)|([1-2]\d{3})\-(0?[2])\-([1-2]?[0-9]|0[1-9]))$/;
			if (re.test(bdate)){
				if ((parseInt(bdate.split("-")[1])==2)&&(parseInt(bdate.split("-")[2])==29)){
					if (!(parseInt(bdate.split("-")[0])%4==0)&&(!parseInt(bdate.split("-")[0])%10==0)|(parseInt(bdate.split("-")[0])%40==0)){
					return false;
					}
				}
			}
			return re.test(bdate);
		}
		
		function dateFieldInit(dates){
		    var bgcolor="#EBEAEF";
			var str="<html>";
			str+="<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset="+formatArray[0]+"\">";
			str+="<style type=\"text/css\"><!--";
			str+=".DateTimeBoxTextStyle { font-family: \"Arial\"; font-size: 9pt; color:\"black\";background-color: \""+bgcolor+"\";line-height: 12pt; text-decoration: none; cursor: hand}";
			str+=".TDStyle { font-family: \"Arial\"; font-size: 9pt; color:\"black\";background-color: \""+bgcolor+"\";line-height: 12pt; text-decoration: none; cursor: default}";
			str+=".DateTimeBoxArrowStyle { font-family: \"Arial\"; font-size: 6pt; color:\"blue\"; text-decoration: none; cursor: hand}";
			str+=".DateTimeBoxTextMouseOverStyle { font-family: \"Arial\"; font-size: 9pt; line-height: 12pt; cursor: hand; text-decoration: none;border-right-width: 1px;border-bottom-width: 1px;border-left-width: 1px;border-top-style: solid;border-right-style: solid;border-bottom-style: solid;border-left-style: solid;border-top-color: #000000;border-right-color: #000000;border-bottom-color: #000000;border-left-color: #000000;background-color: \"pink\";}";
			str+=".DateTimeBoxTextMouseOutStyle { font-family: \"Arial\"; font-size: 9pt; line-height: 12pt; cursor: hand; text-decoration: none;border-right-width: 1px;border-bottom-width: 1px;border-left-width: 1px;border-top-style: solid;border-right-style: solid;border-bottom-style: solid;border-left-style: solid;border-top-color: ;border-right-color: ;border-bottom-color: ;border-left-color: ;background-color: \""+bgcolor+"\";}";
			str+="--></style>";
			str+="</head>";
			str+="<body leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"  oncontextmenu=\"window.event.returnValue=false\" onkeypress=\"window.event.returnValue=false\" onkeydown=\"window.event.returnValue=false\" onkeyup=\"window.event.returnValue=false\" ondragstart=\"window.event.returnValue=false\" onselectstart=\"event.returnValue=false\" >";
			str+="<table id=\"__DateBoxDateTable\" width=\"100%\" height=\"100%\" border=\"1\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" bgcolor=\""+bgcolor+"\"><tr ><td valign=\"top\">";
			str+="<table width=\"100%\" border=\"1\" bordercolor=\"#DDDDDD\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" bgcolor=\""+bgcolor+"\">";
			str+="<tr>";
			str+="<td align=\"center\" width=\"15\" bgcolor=\"#DDDDDD\" class=\"DateTimeBoxTextStyle\" onClick=\"__DateBoxDateChangeYear(-1);return false;\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\">";
			str+="<b>&lt;&lt;</b></td><td align=\"center\" width=\"15\" bgcolor=\"#DDDDDD\" class=\"DateTimeBoxTextStyle\" onClick=\"__DateBoxDateChangeMonth(-1);return false;\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\">";
			str+="<b>&lt;</b></td><td align=\"center\" bgcolor=\"#DDDDDD\" class=\"TDStyle\">";
			str+="<input id=\"__DateBoxDateNowDate\" value=\"\" size=\"2\" readonly onmouseover=\"this.style.cursor='default';\" style=\"background-color:'"+bgcolor+"';border-style:none\">";
			str+="<select name=\"__selMonth\" id=\"__selMonth\" onChange=\"__DateBoxDateChangeMonth(+1,this.options[this.selectedIndex].value-1);parent.keepState=true;\" onFocus=\"parent.keepState=true;\" onBlur=\"parent.keepState=false;\">"
			str+="<option value=\"1\">1月</option>"
			str+="<option value=\"2\">2月</option>"
			str+="<option value=\"3\">3月</option>"
			str+="<option value=\"4\">4月</option>"
			str+="<option value=\"5\">5月</option>"
			str+="<option value=\"6\">6月</option>"
			str+="<option value=\"7\">7月</option>"
			str+="<option value=\"8\">8月</option>"
			str+="<option value=\"9\">9月</option>"
			str+="<option value=\"10\">10月</option>"
			str+="<option value=\"11\">11月</option>"
			str+="<option value=\"12\">12月</option>"
			str+="</select>"
			str+="</td><td align=\"center\" width=\"15\" bgcolor=\"#DDDDDD\" class=\"DateTimeBoxTextStyle\" onClick=\"__DateBoxDateChangeMonth(+1);return false;\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\">";
			str+="<b>&gt;</b></td><td align=\"center\" width=\"15\" bgcolor=\"#DDDDDD\" class=\"DateTimeBoxTextStyle\" onClick=\"__DateBoxDateChangeYear(+1);return false;\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\">";
			str+="<b>&gt;&gt;</b></td><td align=\"center\" width=\"30\" bgcolor=\"#DDDDDD\" class=\"DateTimeBoxTextStyle\" onclick=\"__DateTimeBoxClose();\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\">";
			str+="<b>关闭</b></td></tr></table>";
			
			str+="<table width=\"100%\" border=\"1\" bordercolor=\"#FFFFFF\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" bgcolor=\"#FFFFFF\" >";			
			
			str+="<tr align=\"center\">";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#FF3333\">"+formatArray[1]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#666666\">"+formatArray[2]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#666666\">"+formatArray[3]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#666666\">"+formatArray[4]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#666666\">"+formatArray[5]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#666666\">"+formatArray[6]+"</font></b></td>";
			str+="<td width=\"25\" class=\"DateTimeBoxTextStyle\"><b><font color=\"#3333FF\">"+formatArray[7]+"</font></b></td>";
			str+="</tr>";
			
			str+="<tr align=\"center\" bgcolor=\""+bgcolor+"\"><td colspan=\"7\" height=\"1\"></td></tr><tr align=\"center\">";
			for(var lp=0;lp<42;lp++){
				str+="<td id=\"__DateBoxDateItem"+(lp+1)+"\" width=\"25\" style=\"cursor:hand;\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\" onClick=\"__DateBoxDateSelected(this);\" class=\"DateTimeBoxTextStyle\"></td>";
				if((lp+1)%7==0&&lp<42){
				str+="</tr><tr align=\"center\">";
				}
			}
			str+="</tr></table></td></tr><tr><td valign=\"bottom\">";
			str+="<table width=\"100%\" border=\"1\" bordercolor=\"#FFFFFF\" cellspacing=\"0\" cellpadding=\"0\"><tr>";
			str+="<td width=\"50%\" align=\"center\" class=\"DateTimeBoxTextStyle\" bgcolor=\"#FFFFFF\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\" style=\"cursor:hand;\" onClick=\"__DateBoxDateIsYesD();\">"+formatArray[10]+"</td>";
			str+="<td align=\"center\" class=\"DateTimeBoxTextStyle\" bgcolor=\"#FFFFFF\" onmouseover=\"__DateBoxDateOnMouseOver(this);\" onmouseout=\"__DateBoxDateOnMouseOut(this);\" style=\"cursor:hand;\" onClick=\"__DateBoxDateIsNow();\">"+formatArray[11]+"</td>";					
			str+="</tr></table></td></tr></table>";
			str+="</body>";
			str+="</html>";
			
			str+="<Script>";
			str+="var __DateTimeNowDateTime=new Date();";
			str+="var __DateBoxDateMonthArray=[31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];";						
			str+="var formatArray=new Array(\"gb2312\",\"日\",\"一\",\"二\",\"三\",\"四\",\"五\",\"六\",\"年\",\"月\",\"昨天\",\"今天\",\"请输入正确日期格式的数据！\\n例：2005-01-01\");";
			
			str+="function __DateBoxDateFlash(strNowDate){";
			str+=" var indate=strNowDate.split(\'-\')[2];";			
			str+="__DateTimeNowDateTime=new Date();";			
			str+="if(strNowDate!=\"\"){";
			str+="if(strNowDate.indexOf(\"-\")>0){";
			
			str+="__DateTimeNowDateTime=new Date(strNowDate.split(\"-\")[0],strNowDate.split(\"-\")[1]-1,strNowDate.split(\"-\")[2]);";
			str+="}}";
					
			str+="var nowYear=__DateTimeNowDateTime.getYear();";
			str+="if(nowYear<100){";
			str+="nowYear+=1900;";
			str+="}";
			str+="var nowMonth=__DateTimeNowDateTime.getMonth();";
			
			str+="var firstDay=new Date(nowYear,nowMonth,1);";
			str+="var nowDay=firstDay.getDay();";
			str+="if((nowYear%4==0&&nowYear%100!=0)||(nowYear%400==0)){";
			str+="__DateBoxDateMonthArray[1]=29;";
			str+="}else{";
			str+="__DateBoxDateMonthArray[1]=28;";
			str+="}";
			str+="for(var i=1;;i++){";
			str+="if(document.all(\"__DateBoxDateItem\"+i)){";
			str+="document.all(\"__DateBoxDateItem\"+i).innerHTML=\"\";";
			str+="}else{";
			str+="break;";
			str+="}}";			
			str+="for(var i=0;i<=__DateBoxDateMonthArray[nowMonth]+nowDay;i++){";
			str+="if(i>nowDay){";
			str+="if((i-nowDay)<10){";
			str+="document.all(\"__DateBoxDateItem\"+i).innerHTML=(i-nowDay==indate)?(\"<font color='red' ><strong>\"+\"0\"+(i-nowDay).toString()+\"</strong></font>\"):\"0\"+(i-nowDay);";
			str+="}else{ ";
			str+="document.all(\"__DateBoxDateItem\"+i).innerHTML=(i-nowDay==indate)?(\"<font color='red' ><strong>\"+(i-nowDay).toString()+\"</strong></font>\"):(i-nowDay);";
			str+="}}}";	
			
			str+="document.all(\"__DateBoxDateNowDate\").value=nowYear+formatArray[8];";
			str+="document.all(\"__selMonth\").selectedIndex=(nowMonth)";
			str+="}";
			
			str+="function __DateBoxDateChangeYear(_iYearNumber){";
			str+="__DateTimeNowDateTime.setYear(__DateTimeNowDateTime.getYear()+_iYearNumber);";
			str+="var nowYear=__DateTimeNowDateTime.getYear();";
			str+="var nowMonth=__DateTimeNowDateTime.getMonth()+1;";
			str+="var nowDate=__DateTimeNowDateTime.getDate();";
			str+="if(nowYear<1000){";
			str+="nowYear+=1900;";
			str+="}";
			str+="__DateBoxDateFlash(nowYear+\"-\"+nowMonth+\"-\"+nowDate);";
			str+="}";
			
			str+="function __DateBoxDateChangeMonth(_iMonthNumber,mark){ ";
			str+="if((mark!=null&&mark!='')||mark==0){";
			str+=" __DateTimeNowDateTime=new Date(__DateTimeNowDateTime.getYear(),mark,__DateTimeNowDateTime.getDate());";
			str+="mark=\"\";}";
			str+="else{";
			str+="__DateTimeNowDateTime=new Date(__DateTimeNowDateTime.getYear(),__DateTimeNowDateTime.getMonth()+(_iMonthNumber),__DateTimeNowDateTime.getDate());";
			str+="setMonthValue(__DateTimeNowDateTime.getMonth());";
			str+="}";
			str+="function setMonthValue(value){";
			str+="var selMonth=document.getElementById(\"__selMonth\");";
			str+="selMonth.selectedIndex=value;}"
			str+="var nowYear=__DateTimeNowDateTime.getYear();";
			str+="var nowMonth=__DateTimeNowDateTime.getMonth()+1;";
			str+="var nowDate=__DateTimeNowDateTime.getDate();";
			
			str+="if(nowYear<1000){";
			str+="nowYear+=1900;";
			str+="}";
			str+="__DateBoxDateFlash(nowYear+\"-\"+nowMonth+\"-\"+nowDate);";
			str+="}";
			str+="function __DateTimeBoxClose(){";
			str+="window.frames.parent.__DateTimeBoxDivMouseOut();";
			str+="}";
			
			str+="function __DateBoxDateOnMouseOver(obj){";
			str+="if(obj.innerText!=\"\"){";
			str+="obj.className=\"DateTimeBoxTextMouseOverStyle\";";
			str+="}";
			str+="}";
			
			str+="function __DateBoxDateOnMouseOut(obj){";
			str+="obj.className=\"DateTimeBoxTextMouseOutStyle\";";
			str+="}";
			
			str+="function __DateBoxDateIsYesD(){";
			str+="var today=new Date();";
			str+="var yesterd=new Date(today.getTime()-24*60*60*1000);";
			str+="var yesYear=yesterd.getYear();";
			str+="var yesMonth=yesterd.getMonth()+1;";
			str+="var yesDate=yesterd.getDate();";
			str+="if(yesYear<1000){";
			str+="yesYear+=1900;";
			str+="}";
			str+="if(yesMonth<10){";
			str+="yesMonth=\"0\"+yesMonth.toString();";
			str+="}";
			str+="if(yesDate<10){";
			str+="yesDate=\"0\"+yesDate.toString();";
			str+="}";
			str+="window.frames.parent.__DateTimeBoxFlashString(yesYear+\"-\"+yesMonth+\"-\"+yesDate);";
			str+="}";
			
			str+="function __DateBoxDateIsNow(){";
			str+="__DateTimeNowDateTime=new Date();";
			str+="var nowYear=__DateTimeNowDateTime.getYear();";
			str+="var nowMonth=__DateTimeNowDateTime.getMonth()+1;";
			str+="var nowDate=__DateTimeNowDateTime.getDate();";
			str+="if(nowYear<1000){";
			str+="nowYear+=1900;";
			str+="}";
			str+="if(nowMonth<10){";
			str+="nowMonth=\"0\"+nowMonth.toString();";
			str+="}";
			str+="if(nowDate<10){";
			str+="nowDate=\"0\"+nowDate.toString();";
			str+="}";
			str+="window.frames.parent.__DateTimeBoxFlashString(nowYear+\"-\"+nowMonth+\"-\"+nowDate);";
			str+="}";
			str+="function __DateBoxDateSelected(obj){";
			str+="if(obj.innerText!=\"\"){";
			str+="var nowYear=__DateTimeNowDateTime.getYear();";
			str+="var nowMonth=__DateTimeNowDateTime.getMonth()+1;";
			str+="if(nowYear<1000){";
			str+="nowYear+=1900;";
			str+="}";
			str+="if(nowMonth<10){";
			str+="nowMonth=\"0\"+nowMonth.toString();";
			str+="}";
			str+="window.frames.parent.__DateTimeBoxFlashString(nowYear+\"-\"+nowMonth+\"-\"+obj.innerText);";
			str+="}";
			str+="}";
			str+="<\/Script>";
			//prompt("",str);
			var d=new Date();
			var yy=d.getYear();
			var mm=d.getMonth()+1;
			var dd=d.getDate();
			var datestr=yy+"-"+mm+"-"+dd;
			if(dates=='')
			{
			    dates=datestr;//如果框中字符为空
			}
			var BoxWin=window.open("","__DateTimeIFrame");
			BoxWin.document.clear();
			BoxWin.document.open();
			BoxWin.document.write(str);
			

			
			BoxWin.document.write("<Script Language=\"JavaScript\">__DateBoxDateFlash('"+dates+"');</Script>");
			BoxWin.document.close();
		}
		
			
		function __DateTimeBoxFlashString(str){
			if(dateField){
				dateField.value=str;
				__DateTimeBoxAllHidden();
			}
		}

	function checkpressnum(){
		if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57))|| (window.event.keyCode == 13))){
			window.alert("您的输入不正确，请输入数字");
			window.event.keyCode = 0;
			return false;
			}
		return true;
	}
	function checkpress()
	{

		if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57))|| (window.event.keyCode == 46))){
			window.alert("您的输入不正确，请输入数字");
			window.event.keyCode = 0;
			return false;
			}
		return true;
    }
	function checkratio(selobj,defaultvalue){	
		if((selobj.value=="")||(selobj.value>100)){	
			window.alert("您的输入不正确，请输入0－100之间的数");
			selobj.value=defaultvalue;
		return false;
		}
		return true;
	}
	function checkrate(selobj,defaultvalue){	
	 if(isNaN(selobj.value))
	  {
		  alert('输入的字符不是小数');
		  selobj.value=defaultvalue;
		  return false;
	  }
		if((selobj.value=="")||(selobj.value>1)){	
			window.alert("您的输入不正确，请输入0－1之间的数");
			selobj.value=defaultvalue;
		return false;
		}
		return true;
	}

	function checkdata(selobj,defaultvalue){	
		if(selobj.value==""){	
			window.alert("不能为空，请输入数值");
			selobj.value=defaultvalue;
		return false;
		}
		return true;
	}	


	function daysofmonth(yearobj,monthobj){
		var yeartemp=yearobj.value;
		var monthtemp=monthobj.value;
		
		if(monthtemp==2){
			if((yeartemp%4 == 0) && (yeartemp%100 != 0) || (yeartemp%400 == 0)){
				return 29;
			}else{
				return 28;
			}
		}else if(monthtemp==4||monthtemp==6||monthtemp==9||monthtemp==11){
			return 30;			
		}else{
			return 31;
		}
		
	}
	
	function adddayoption(yearobj,monthobj,dayobj){
		var numofdays=daysofmonth(yearobj,monthobj);
		if((dayobj)!=null){
			var ln = dayobj.options.length;
			var diff=ln-numofdays;
			if(diff>0){
				while(diff--)
					dayobj.options[--ln] = null;
					dayobj.value=ln;
			}	else if(diff<0){
				for(var i=0;i<-diff;i++){
					dayobj.add(new Option(ln+i+1,ln+i+1));
				}
			}
		}
		return true;
	}
	
	function initdate(year,month,date){
		document.form1.select1.value=year;
		document.form1.select2.value=month;		
		adddayoption(document.form1.select1,document.form1.select2,document.form1.select3);
		if((document.form1.select3)!=null)
		document.form1.select3.value=date;
		return true;
		
	}
	
	function initdate2(endyear,endmonth,endday,startyear,startmonth,startday){
		document.form1.select1.value=endyear;
		document.form1.select2.value=endmonth;			
		adddayoption(document.form1.select1,document.form1.select2,document.form1.select3);
		if((document.form1.select3)!=null)
		document.form1.select3.value=endday;
				
		document.form1.select4.value=startyear;
		document.form1.select5.value=startmonth;			
		adddayoption(document.form1.select4,document.form1.select5,document.form1.select6);
		if((document.form1.select6)!=null)
		document.form1.select6.value=startday;
		return true;
		
	}
	
	
	function checkdate(startd,endd,targerurl){
		var sd1=new String(startd);
		var sd2=new String(endd);
		var today=(new Date()).getTime();
		var dt1=(new Date(sd1.replace(/-/g,"\/"))).getTime();
		var dt2=(new Date(sd2.replace(/-/g,"\/"))).getTime();
		if(today<dt2){
			alert("结束日期大于当前日期");
			return false;
		}
		if(dt2<dt1){
			alert("结束日期小于开始日期");
			return false;
		}
		location.href=targerurl+"?startdate="+startd+"&enddate="+endd;
		return true;
	}
	


	function MM_preloadImages() { //v3.0
	  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
	    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
	    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
	}
	
	function MM_swapImgRestore() { //v3.0
	  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
	}
	
	function MM_findObj(n, d) { //v4.01
	  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
	    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
	  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
	  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
	  if(!x && d.getElementById) x=d.getElementById(n); return x;
	}
	
	function MM_swapImage() { //v3.0
	  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
	   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
	}
	
function showDialog(pageName,view)
{
	if(view==null)
		view='dialogWidth:350px;dialogHeight:150px';
	return window.showModalDialog(pageName,'','status:no;help:no;'+view);
}
