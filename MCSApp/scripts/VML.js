  var __Level=0;਍ഀ
  var __ID=0;਍ഀ
  function RightMenu()਍ഀ
  {਍ഀ
    this.AddExtendMenu=AddExtendMenu;਍ഀ
    this.AddItem=AddItem;਍ഀ
    this.GetMenu=GetMenu;਍ഀ
    this.HideAll=HideAll;਍ഀ
    this.I_OnMouseOver=I_OnMouseOver;਍ഀ
    this.I_OnMouseOut=I_OnMouseOut;਍ഀ
    this.I_OnMouseUp=I_OnMouseUp;਍ഀ
    this.P_OnMouseOver=P_OnMouseOver;਍ഀ
    this.P_OnMouseOut=P_OnMouseOut;਍ഀ
਍ഀ
    ਍ഀ
    A_rbpm = new Array();਍ഀ
਍ഀ
    HTMLstr  = "";਍ഀ
    HTMLstr += "<!-- RightButton PopMenu -->\n";਍ഀ
    HTMLstr += "\n";਍ഀ
    HTMLstr += "<!-- PopMenu Starts -->\n";਍ഀ
    HTMLstr += "<div id='E_rbpm' class='rm_div'>\n";਍ഀ
                        // rbpm = right button pop menu਍ഀ
    HTMLstr += "<table width='100%' border='0' cellspacing='0'>\n";਍ഀ
    HTMLstr += "<tr><td height='100%' width='20' valign='middle'  bgcolor='#f0ffff' onclick=window.event.cancelBubble=true; style='font-size:x-small'>菜单\n";਍ഀ
    HTMLstr += "</td><td height='100%' width='120' style='padding: 1' valign='bottom'>\n";਍ഀ
    HTMLstr += "<table width='100%' border='0' cellspacing='0'>\n";਍ഀ
    HTMLstr += "<!-- Insert A Extend Menu or Item On Here For E_rbpm -->\n";਍ഀ
    HTMLstr += "</table></td></tr></table>\n";਍ഀ
    HTMLstr += "</div>\n";਍ഀ
    HTMLstr += "<!-- Insert A Extend_Menu Area on Here For E_rbpm -->";਍ഀ
    HTMLstr += "\n";਍ഀ
    HTMLstr += "<!-- PopMenu Ends -->\n";਍ഀ
  }਍ഀ
  function AddExtendMenu(id,img,wh,name,parent)਍ഀ
  {਍ഀ
    var TempStr = "";਍ഀ
਍ഀ
    eval("A_"+parent+".length++");਍ഀ
    eval("A_"+parent+"[A_"+parent+".length-1] = id");  // 将此项注册到父菜单项的ID数组中去਍ഀ
    TempStr += "<div id='E_"+id+"' class='rm_div'>\n";਍ഀ
    TempStr += "<table width='100%' border='0' cellspacing='0'>\n";਍ഀ
    TempStr += "<!-- Insert A Extend Menu or Item On Here For E_"+id+" -->";਍ഀ
    TempStr += "</table>\n";਍ഀ
    TempStr += "</div>\n";਍ഀ
    TempStr += "<!-- Insert A Extend_Menu Area on Here For E_"+id+" -->";਍ഀ
    TempStr += "<!-- Insert A Extend_Menu Area on Here For E_"+parent+" -->";਍ഀ
    HTMLstr = HTMLstr.replace("<!-- Insert A Extend_Menu Area on Here For E_"+parent+" -->",TempStr);਍ഀ
    ਍ഀ
    eval("A_"+id+" = new Array()");਍ഀ
    TempStr  = "";਍ഀ
    TempStr += "<!-- Extend Item : P_"+id+" -->\n";਍ഀ
    TempStr += "<tr id='P_"+id+"' class='out'";਍ഀ
    TempStr += " onmouseover='P_OnMouseOver(\""+id+"\",\""+parent+"\")'";਍ഀ
    TempStr += " onmouseout='P_OnMouseOut(\""+id+"\",\""+parent+"\")'";਍ഀ
    TempStr += " onmouseup=window.event.cancelBubble=true;";਍ഀ
    TempStr += " onclick=window.event.cancelBubble=true;";਍ഀ
    TempStr += "><td nowrap>";਍ഀ
    TempStr += "<font face='Wingdings' style='font-size:18px'>0</font> "+name+"  </td><td style='font-family: webdings; text-align: ;'>4";਍ഀ
    TempStr += "</td></tr>\n";਍ഀ
    TempStr += "<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->";਍ഀ
    HTMLstr = HTMLstr.replace("<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->",TempStr);਍ഀ
  }਍ഀ
  function AddItem(id,img,wh,name,parent,location)਍ഀ
  {਍ഀ
    var TempStr = "";਍ഀ
    var ItemStr = "<!-- ITEM : I_"+id+" -->";਍ഀ
    if(id == "sperator")਍ഀ
    {਍ഀ
      TempStr += ItemStr+"\n";਍ഀ
      TempStr += "<tr class='out' onclick='window.event.cancelBubble=true;' onmouseup='window.event.cancelBubble=true;'><td colspan='2' height='1'><hr class='sperator'></td></tr>";਍ഀ
      TempStr += "<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->";਍ഀ
      HTMLstr = HTMLstr.replace("<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->",TempStr);਍ഀ
      return;਍ഀ
    }਍ഀ
    if(HTMLstr.indexOf(ItemStr) != -1)਍ഀ
    {਍ഀ
      alert("I_"+id+"already exist!");਍ഀ
      return;਍ഀ
    }਍ഀ
    TempStr += ItemStr+"\n";਍ഀ
    TempStr += "<tr id='I_"+id+"' class='out'";਍ഀ
    TempStr += " onmouseover='I_OnMouseOver(\""+id+"\",\""+parent+"\")'";਍ഀ
    TempStr += " onmouseout='I_OnMouseOut(\""+id+"\")'";਍ഀ
    TempStr += " onclick='window.event.cancelBubble=true;'";਍ഀ
    if(location == null)਍ഀ
      TempStr += " onmouseup='I_OnMouseUp(\""+id+"\",\""+parent+"\",null)'";਍ഀ
//    else਍ഀ
//      TempStr += " onmouseup='I_OnMouseUp(\""+id+"\",\""+parent+"\",\""+escape(location)+"\")'";਍ഀ
    TempStr += "><td nowrap>";਍ഀ
    TempStr +="<font face='Wingdings' style='font-size:18px'>"+wh+"</font> "+ name+" ";//以Wingdings字体做为图片，要改成图片，请在这里更改਍ഀ
    TempStr += "</td><td></td></tr>\n";਍ഀ
    TempStr += "<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->";਍ഀ
    HTMLstr = HTMLstr.replace("<!-- Insert A Extend Menu or Item On Here For E_"+parent+" -->",TempStr);਍ഀ
  }਍ഀ
  function GetMenu()਍ഀ
  {਍ഀ
    return HTMLstr;਍ഀ
  }਍ഀ
  function I_OnMouseOver(id,parent)਍ഀ
  {਍ഀ
    var Item;਍ഀ
    if(parent != "rbpm")਍ഀ
    {਍ഀ
      var ParentItem;਍ഀ
      ParentItem = eval("P_"+parent);਍ഀ
      ParentItem.className="over";਍ഀ
    }਍ഀ
    Item = eval("I_"+id);਍ഀ
    Item.className="over";਍ഀ
    HideAll(parent,1);਍ഀ
  }਍ഀ
  function I_OnMouseOut(id)਍ഀ
  {਍ഀ
    var Item;਍ഀ
    Item = eval("I_"+id);਍ഀ
    Item.className="out";਍ഀ
  }਍ഀ
  function I_OnMouseUp(id,parent,location)਍ഀ
  {਍ഀ
    try਍ഀ
    {਍ഀ
        var ParentMenu;਍ഀ
        window.event.cancelBubble=true;਍ഀ
        OnClick();਍ഀ
        ParentMenu = eval("E_"+parent);਍ഀ
        ParentMenu.display="none";਍ഀ
        if(location == null)਍ഀ
          eval("Do_"+id+"()");਍ഀ
        else਍ഀ
        {਍ഀ
            var _win=window.parent.win;਍ഀ
            _win.addwin(location,id);    ਍ഀ
        }   ਍ഀ
    }਍ഀ
    catch(e)਍ഀ
    {਍ഀ
        alert("没有找到相应的可执行操作!正在建设中......");਍ഀ
        //return;਍ഀ
    }਍ഀ
਍ഀ
  }਍ഀ
  function P_OnMouseOver(id,parent)਍ഀ
  {਍ഀ
    var Item;਍ഀ
    var Extend;਍ഀ
    var Parent;਍ഀ
    if(parent != "rbpm")਍ഀ
    {਍ഀ
      var ParentItem;਍ഀ
      ParentItem = eval("P_"+parent);਍ഀ
      ParentItem.className="over";਍ഀ
    }਍ഀ
    HideAll(parent,1);਍ഀ
    Item = eval("P_"+id);਍ഀ
    Extend = eval("E_"+id);਍ഀ
    Parent = eval("E_"+parent);਍ഀ
    Item.className="over";਍ഀ
    Extend.style.display="block";਍ഀ
    Extend.style.posLeft=document.body.scrollLeft+Parent.offsetLeft+Parent.offsetWidth-4;਍ഀ
    if(Extend.style.posLeft+Extend.offsetWidth > document.body.scrollLeft+document.body.clientWidth)਍ഀ
        Extend.style.posLeft=Extend.style.posLeft-Parent.offsetWidth-Extend.offsetWidth+8;਍ഀ
    if(Extend.style.posLeft < 0) Extend.style.posLeft=document.body.scrollLeft+Parent.offsetLeft+Parent.offsetWidth;਍ഀ
    Extend.style.posTop=Parent.offsetTop+Item.offsetTop+1;਍ഀ
    if(Extend.style.posTop+Extend.offsetHeight > document.body.scrollTop+document.body.clientHeight)਍ഀ
      Extend.style.posTop=document.body.scrollTop+document.body.clientHeight-Extend.offsetHeight;਍ഀ
    if(Extend.style.posTop < 0) Extend.style.posTop=0;਍ഀ
  }਍ഀ
  function P_OnMouseOut(id,parent)਍ഀ
  {਍ഀ
  }਍ഀ
  function HideAll(id,flag)਍ഀ
  {਍ഀ
    var Area;਍ഀ
    var Temp;਍ഀ
    var i;਍ഀ
    if(!flag)਍ഀ
    {਍ഀ
      Temp = eval("E_"+id);਍ഀ
      Temp.style.display="none";਍ഀ
    }਍ഀ
    Area = eval("A_"+id);਍ഀ
    if(Area.length)਍ഀ
    {਍ഀ
      for(i=0; i < Area.length; i++)਍ഀ
      {਍ഀ
        HideAll(Area[i],0);਍ഀ
        Temp = eval("E_"+Area[i]);਍ഀ
        Temp.style.display="none";਍ഀ
        Temp = eval("P_"+Area[i]);਍ഀ
        Temp.className="out";਍ഀ
      }਍ഀ
    }਍ഀ
  }਍ഀ
਍ഀ
  document.onmouseup=OnMouseUp;਍ഀ
  document.onclick=OnClick;਍ഀ
  function OnMouseUp()਍ഀ
  {਍ഀ
    if(window.event.button == 2)਍ഀ
    {਍ഀ
      ਍ഀ
      var srcid=event.srcElement.id;਍ഀ
      var title=event.srcElement.title;਍ഀ
      __Level=srcid.split('_')[1];਍ഀ
      __ID=srcid.split('_')[2];਍ഀ
਍ഀ
      var PopMenu;਍ഀ
      PopMenu = eval("E_rbpm");਍ഀ
      HideAll("rbpm",0);਍ഀ
      PopMenu.style.display="block";਍ഀ
      PopMenu.style.posLeft=document.body.scrollLeft+window.event.clientX;਍ഀ
      PopMenu.style.posTop=document.body.scrollTop+window.event.clientY;਍ഀ
      if(PopMenu.style.posLeft+PopMenu.offsetWidth > document.body.scrollLeft+document.body.clientWidth)਍ഀ
        PopMenu.style.posLeft=document.body.scrollLeft+document.body.clientWidth-PopMenu.offsetWidth;਍ഀ
      if(PopMenu.style.posLeft < 0) PopMenu.style.posLeft=0;਍ഀ
      if(PopMenu.style.posTop+PopMenu.offsetHeight > document.body.scrollTop+document.body.clientHeight)਍ഀ
        PopMenu.style.posTop=document.body.scrollTop+document.body.clientHeight-PopMenu.offsetHeight;਍ഀ
      if(PopMenu.style.posTop < 0) PopMenu.style.posTop=0; ਍ഀ
                   ਍ഀ
      var z=document.getElementById('I_关口台帐');      ਍ഀ
      var a=document.getElementById('I_表计检测数据');਍ഀ
      var b=document.getElementById('I_互感器检测数据');਍ഀ
      var c=document.getElementById('I_计量绕组二次负荷校验数据');਍ഀ
      var d=document.getElementById('I_电压二次回路压降数据');       ਍ഀ
      var _win=window.parent.parent.win;਍ഀ
      if(__Level==2)਍ഀ
      { ਍ഀ
           z.style.display='';      ਍ഀ
           a.style.display='';਍ഀ
           b.style.display='';਍ഀ
           c.style.display='';਍ഀ
           d.style.display='';਍ഀ
           z.onmouseup=function(){_win.addwin('../ArchivesM/SwitchManager.aspx?ID='+__ID,title);};           ਍ഀ
           a.onmouseup=function(){_win.addwin('../checkdata/CheckAmmeter.aspx?ID='+__ID,title+'表计检测数据');};਍ഀ
           b.onmouseup=function(){_win.addwin('../checkdata/CheckInduction.aspx?swid='+__ID,title+'互感器检测数据');};਍ഀ
           c.onmouseup=function(){_win.addwin('../checkdata/CheckCable.aspx?ID='+__ID,title+'二次负荷数据');};਍ഀ
           d.onmouseup=function(){_win.addwin('../checkdata/CheckTwiceU.aspx?ID='+__ID,title+'二次回路压降数据');};਍ഀ
           ਍ഀ
      } ਍ഀ
      else਍ഀ
      {਍ഀ
           z.style.display='none';           ਍ഀ
           a.style.display='none';਍ഀ
           b.style.display='none';਍ഀ
           c.style.display='none';਍ഀ
           d.style.display='none';          ਍ഀ
      } ਍ഀ
    }਍ഀ
  }਍ഀ
  function getLevel()਍ഀ
  {਍ഀ
      return _Level;਍ഀ
  }਍ഀ
  function getID()਍ഀ
  {਍ഀ
      return __ID;਍ഀ
  }਍ഀ
  function OnClick()਍ഀ
  {਍ഀ
    HideAll("rbpm",0);਍ഀ
  }਍ഀ
  // Add Your Function on following਍ഀ
  function Do_viewcode(){window.location="view-source:"+window.location.href;}਍ഀ
  function Do_exit() {var _win=window.parent.parent.win;if(_win.currentwin.title!='首页') _win.removewinbytitle(_win.currentwin.title); }//_win.removewinbytitle(_win.title);਍ഀ
  ਍ഀ
  function Do_refresh() {window.location.reload();}਍ഀ
  function Do_enlarge() ਍ഀ
  {਍ഀ
      r_zoom(107);਍ഀ
  }਍ഀ
  function Do_enlittle() ਍ഀ
  {਍ഀ
      r_zoom(109);  ਍ഀ
  }਍ഀ
  function Do_default() ਍ഀ
  {਍ഀ
      r_zoom(13);  ਍ഀ
  }  ਍ഀ
  function Do_back() {history.back();}਍ഀ
  function Do_forward() {history.forward();}਍ഀ
  function Do_help(){alert("帮助")}਍ഀ
  ਍ഀ
　　var __i=0;਍ഀ
　　document.onmousewheel =gunlunsuofang;਍ഀ
　　document.onkeydown =r_zoom;਍ഀ
　　function r_zoom(obj)਍ഀ
　　{   var IEKey;਍ഀ
　　    if(obj!=null)਍ഀ
　　    {਍ഀ
            IEKey=obj;　        ਍ഀ
　　    }਍ഀ
　　    else਍ഀ
　　    {਍ഀ
　　        IEKey=event.keyCode;਍ഀ
　　    }　　        ਍ഀ
਍ഀ
        if (IEKey == 13) ਍ഀ
        { ਍ഀ
            document.body.style.zoom=1;਍ഀ
            __i=1;਍ഀ
            ਍ഀ
        } ਍ഀ
        ਍ഀ
        else if (IEKey == 107) ਍ഀ
        { ਍ഀ
            __i++; ਍ഀ
            document.body.style.zoom=1+__i/10; ਍ഀ
        } ਍ഀ
        else if (IEKey == 109) ਍ഀ
        { ਍ഀ
            __i--;਍ഀ
            document.body.style.zoom=1+__i/10;਍ഀ
        }　਍ഀ
　　    ਍ഀ
　　}਍ഀ
　　਍ഀ
    //ctrl+滚轮਍ഀ
    function gunlunsuofang()਍ഀ
    {਍ഀ
　　    if(!event.ctrlKey)//如果਍ॎ捣琀爀氀攀푐�ൖ
਍        笀ഀ
਍            爀攀琀甀爀渀 琀爀甀攀㬀ഀ
਍        紀  ഀ
਍        ⼀⼀瘀愀爀 稀漀漀洀㴀瀀愀爀猀攀䤀渀琀⠀搀漀挀甀洀攀渀琀⸀戀漀搀礀⸀猀琀礀氀攀⸀稀漀漀洀Ⰰ㄀　⤀簀簀㄀　　㬀ഀ
਍        开开椀⬀㴀攀瘀攀渀琀⸀眀栀攀攀氀䐀攀氀琀愀⼀㄀㈀　　㬀 ഀ
਍        搀漀挀甀洀攀渀琀⸀戀漀搀礀⸀猀琀礀氀攀⸀稀漀漀洀㴀㄀⬀开开椀⼀㄀　㬀ഀ
਍        爀攀琀甀爀渀 昀愀氀猀攀㬀 ഀ
਍    紀  ഀ
਍  ഀ
਍瘀愀爀 洀攀渀甀 㴀 渀攀眀 刀椀最栀琀䴀攀渀甀⠀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀猀ၓ≞Ⰰ∀猀琀愀爀琀开瀀爀漀∀Ⰰ∀㈀∀Ⰰ∀猀ၓ≞Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀栀ꆈ삋䭨灭湥≣Ⰰ∀猀琀愀爀琀开瀀爀漀∀Ⰰ∀㈀∀Ⰰ∀栀ꆈ삋䭨灭湥≣Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀鈀὎桡쁖䭨灭湥≣Ⰰ∀猀琀愀爀琀开瀀爀漀∀Ⰰ∀㈀∀Ⰰ∀鈀὎桡쁖䭨灭湥≣Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀ꄀ쾋햑쑾豾ⅎὫ瞍Ↄ豨炚湥≣Ⰰ∀猀琀愀爀琀开瀀爀漀∀Ⰰ∀㈀∀Ⰰ∀ꄀ쾋햑쑾豾ⅎὫ瞍Ↄ豨炚湥≣Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀㔀譵豓ⅎ�讍䵓炖湥≣Ⰰ∀猀琀愀爀琀开瀀爀漀∀Ⰰ∀㈀∀Ⰰ∀㔀譵豓ⅎ�讍䵓炖湥≣Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍ഀ
਍ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀猀瀀攀爀愀琀漀爀∀Ⰰ∀∀Ⰰ∀∀Ⰰ∀∀Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀攀渀氀愀爀最攀∀Ⰰ∀猀琀愀爀琀开氀漀最漀昀昀∀Ⰰ∀洀∀Ⰰ∀☀渀戀猀瀀㬀㸀❥⁙⠀⬀⤀∀Ⰰ∀爀戀瀀洀∀Ⰰ渀甀氀氀⤀㬀ഀ
਍洀攀渀甀⸀䄀搀搀䤀琀攀洀⠀∀攀渀氀椀琀琀氀攀∀Ⰰ∀猀琀愀爀琀开氀漀最漀昀昀∀Ⰰ∀眀∀Ⰰ∀☀渀戀猀瀀㬀☀渀戀猀瀀㬀☀渀戀猀瀀㬀⤀ཿ⁜⠀ഀ＊)","rbpm",null);਍ഀ
menu.AddItem("default","start_logoff","v","&nbsp;还原(Enter)","rbpm",null);਍ഀ
menu.AddItem("sperator","","","","rbpm",null);਍ഀ
਍ഀ
menu.AddItem("refresh","start_logoff","a","&nbsp;刷新","rbpm",null);਍ഀ
menu.AddItem("exit","start_shut","x","&nbsp;关闭","rbpm",null);਍ഀ
document.writeln(menu.GetMenu());਍ഀ
document.oncontextmenu=new Function('event.returnValue=false;'); //禁止右键਍ഀ
਍ഀ
//////////////////////////////////////图层托动///////////////////////////////////਍ഀ
਍ഀ
function drag(obj,event)਍ഀ
{਍ഀ
        var s = obj.style਍ഀ
        var b = document.body਍ഀ
        var x = event.clientX + b.scrollLeft - parseInt(s.left)਍ഀ
        var y = event.clientY + b.scrollTop - parseInt(s.top)਍ഀ
        var m = function()਍ഀ
        {਍ഀ
            s.left = event.clientX + b.scrollLeft - x਍ഀ
            s.top = event.clientY + b.scrollTop - y਍ഀ
        }਍ഀ
        if (window.captureEvents)਍ഀ
        {਍ഀ
            var u = function()਍ഀ
            {਍ഀ
                    window.onmouseup = null਍ഀ
                    window.onmousemove = null਍ഀ
                    window.releaseEvents(Event.MOUSEMOVE|Event.MOUSEUP)਍ഀ
            }਍ഀ
            window.onmouseup = u਍ഀ
            window.onmousemove = m਍ഀ
            window.captureEvents(Event.MOUSEMOVE|Event.MOUSEUP)਍ഀ
            event.stopPropagation()਍ഀ
        }਍ഀ
        else਍ഀ
        {਍ഀ
            var u = function()਍ഀ
            {਍ഀ
                    obj.onmouseup = null਍ഀ
                    obj.onmousemove = null਍ഀ
                    obj.releaseCapture()਍ഀ
            }਍ഀ
            obj.onmouseup = u਍ഀ
            obj.onmousemove = m਍ഀ
            obj.setCapture()਍ഀ
            event.cancelBubble = true਍ഀ
        }਍ഀ
}਍ഀ
  ਍ഀ
਍ഀ
function  setParentDiv()਍ഀ
{਍ഀ
    var div1=document.getElementById('div1');਍ഀ
//    div1.innerHTML+=unescape("<table width='100%' height='100%' border='1' >");਍ഀ
//    div1.innerHTML+=unescape("<tr><td bgcolor='#999999'>HI</td><td width=20>&nbsp;</td></tr><tr><td height='50' ></td><td><img src='../images/delete.gif' width='16' height='16' onmousedown=did('div1')></td></tr></table>");਍ഀ
਍ഀ
    div1.innerHTML+=unescape(document.getElementById("hdDetailInfo").value);਍ഀ
}਍ഀ
਍ഀ
var   ms=0;   ਍ഀ
function did(obj)਍ഀ
{   ਍ഀ
ms=obj;   ਍ഀ
event.srcElement.setCapture();   ਍ഀ
x=event.x;   ਍ഀ
y=event.y;   ਍ഀ
}   ਍ഀ
਍ഀ
function document.onmousemove()਍ഀ
{   ਍ഀ
if(ms)਍ഀ
{   ਍ഀ
    document.all(ms).style.pixelWidth=event.x-document.all(ms).style.pixelLeft;   ਍ഀ
    document.all(ms).style.pixelHeight=event.y-document.all(ms).style.pixelTop;   ਍ഀ
}   ਍ഀ
}   ਍ഀ
਍ഀ
function document.onmouseup()਍ഀ
{   ਍ഀ
 if(ms)਍ഀ
 {   ਍ഀ
    event.srcElement.releaseCapture();   ਍ഀ
    ms=0;   ਍ഀ
 }   ਍ഀ
}਍ഀ
