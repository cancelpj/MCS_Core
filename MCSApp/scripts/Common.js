<!--਍ഀ
function login(loginSuccess,IsLogout)਍ഀ
{਍ഀ
	if(loginSuccess)਍ഀ
		window.open('Main/Main.aspx','_self','top=0,left=0,width='+(window.screen.width)+',height='+(window.screen.height-170)+',toolbar=yes,menubar=yes,scrollbars=auto,location=yes,resizable=yes')//,'','resizable=yes''height=100,width=400,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no'਍ഀ
	else਍ഀ
	{਍ഀ
		if(IsLogout)਍ഀ
		{਍ഀ
			alert("登录失败！\n帐号与密码਍㥎䵓Ƒ⋿⤀㬀ഀ
਍ऀऀऀ爀攀琀甀爀渀㬀ഀ
਍ऀऀ紀ഀ
਍ऀ紀ഀ
਍紀ഀ
਍昀甀渀挀琀椀漀渀 挀栀攀挀欀䰀漀最椀渀⠀氀漀最椀渀匀甀挀挀攀猀猀⤀ഀ
਍笀ഀ
਍ऀ椀昀⠀氀漀最椀渀匀甀挀挀攀猀猀⤀ഀ
਍ऀऀ愀氀攀爀琀⠀∀笀啶ㅟ╙ƍ峿渀က๓왎ś൸上匹配！");਍ഀ
	਍ഀ
	if(Form1.TextBoxUserName!=null)਍ഀ
		Form1.TextBoxUserName.focus();਍ഀ
}਍ഀ
function winRefresh(loginSuccess,IsLogout,Isremoveal)਍ഀ
{਍ഀ
    if(loginSuccess)਍ഀ
    {਍ഀ
    //alert(Isremoveal);਍ഀ
       if (Isremoveal)਍ഀ
        {win=parent.parent.window;਍ഀ
        //alert(win.document.body.innerHTML);਍ഀ
        win.head.location='../Main/TopPage.aspx';਍ഀ
        //清理原主窗口里的窗体       ਍ഀ
         parent.win.removeall();਍ഀ
         win.fraMain.location='../Main/main.htm';਍ഀ
        //win.refresh();//alert(win.close());਍ഀ
        }਍ഀ
        else਍ഀ
          parent.win.removewin(parent.win.currentwin);਍ഀ
    }਍ഀ
	else਍ഀ
	{਍ഀ
		if(!IsLogout)਍ഀ
		{਍ഀ
			alert("登录失败！\n帐号与密码਍㥎䵓Ƒ⋿⤀㬀ഀ
਍ऀऀऀ爀攀琀甀爀渀㬀ഀ
਍ऀऀ紀ഀ
਍ऀ紀ഀ
਍紀ഀ
਍昀甀渀挀琀椀漀渀 匀攀氀圀椀渀伀瀀攀渀⠀吀愀戀氀攀一愀洀攀Ⰰ䘀椀攀氀搀一愀洀攀Ⰰ圀栀攀爀攀匀琀爀Ⰰ眀椀渀一愀洀攀⤀ഀ
਍笀ഀ
਍⼀⼀⠀蹵ൎ上面的url每次都਍౶౔෿上晓得为何相同਍赎け൒朊务器端取数据਍ഀ
    var popObject=new Date();਍ഀ
    var popTime=popObject.getTime();//取得自1970年1月1日逝去的毫秒数 ਍ഀ
਍ഀ
    //带中文的字符最好要加密escpae否则在传输过程中