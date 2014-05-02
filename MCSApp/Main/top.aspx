<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="MCSApp.Main.top" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<HEAD>
<TITLE> MCStop </TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<link href="style.css" rel="stylesheet" type="text/css">
<script src="../scripts/CustomWin.js" language=javascript></script>
<SCRIPT LANGUAGE="JavaScript">
<!--
	var strCols_Current = top.fstMain.cols;
	function controltoolbar()
	{
		if(top.fstMain.cols==strCols_Current)
		{
			top.fstMain.cols="0,*";
		}
		else{
			top.fstMain.cols=strCols_Current;
		}
	}
	  top.fstMain.cols = strCols_Current;

	function button(text,event,img,button)
	{
		var output="";
		output+='<TABLE height="30" border=0 cellspacing=0 cellpadding=0 class="button" event="'+event+'"';
		if(button!='')
			output+=' button="'+button+'"';
		output+='>';
		output+='	 <TR>';
		output+='		<TD><IMG SRC="../images/button_left.gif" WIDTH="3" HEIGHT="30" BORDER=0 ALT=""></TD>';
		output+='		<TD class="buttonline">';
		if(img!='')
			output+='	<IMG SRC="'+img+'" BORDER=0 ALT="" hspace="1" align="absmiddle">';
		if(text!='')
			output+='	'+text+'&nbsp;';
		output+='	</TD>';
		output+='		<TD><IMG SRC="../images/button_right.gif" WIDTH="3" HEIGHT="30" BORDER=0 ALT=""></TD>';
		output+='	 </TR>';
		output+='</TABLE>';
		document.write(output);
	}
//<img  src="../images/fingu.jpg"/>-->
</SCRIPT>
</HEAD>

<BODY leftmargin="0" topmargin="0" oncontextmenu="return false" onselectstart="return false" ondragstart="return false">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"></asp:UpdatePanel> 
       
<TABLE width="100%" border=0 cellspacing=0 cellpadding=0>
  <TR>
		<TD  style="background-color:lightcyan;">
<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 
                height=51 width="100%" classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 
                VIEWASTEXT>
	<PARAM NAME="_cx" VALUE="12039">
	<PARAM NAME="_cy" VALUE="1720">
	<PARAM NAME="FlashVars" VALUE="">
	<PARAM NAME="Movie" VALUE="../images/fingu.swf">
	<PARAM NAME="Src" VALUE="../images/fingu.swf">
	<PARAM NAME="WMode" VALUE="Transparent">
	<PARAM NAME="Play" VALUE="-1">
	<PARAM NAME="Loop" VALUE="-1">
	<PARAM NAME="Quality" VALUE="High">
	<PARAM NAME="SAlign" VALUE="">
	<PARAM NAME="Menu" VALUE="-1">
	<PARAM NAME="Base" VALUE="">
	<PARAM NAME="AllowScriptAccess" VALUE="always">
	<PARAM NAME="Scale" VALUE="ShowAll">
	<PARAM NAME="DeviceFont" VALUE="0">
	<PARAM NAME="EmbedMovie" VALUE="0">
	<PARAM NAME="BGColor" VALUE="">
	<PARAM NAME="SWRemote" VALUE="">
	<PARAM NAME="MovieData" VALUE="">
	<PARAM NAME="SeamlessTabbing" VALUE="1">
	<embed src="../images/fingu.swf"  quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" ></embed>
</OBJECT>
            </TD>
  </TR>
  <TR height="10">
		<TD >
			<TABLE height="100%" width="100%" border=0 cellspacing=0 cellpadding=0>
			  <TR>
					<TD width=210 align=center style="background-color:lightcyan"><a href='#' onclick='controltoolbar()' style="color: #0000FF">展开/关闭菜单</a></TD>
					<TD align=right style="background-color:lightcyan">
					    <asp:Label ID="lblUserName" runat="server" ForeColor="Red"></asp:Label>
                        <asp:LinkButton ID="lbtQuit" runat="server" onclick="lbtQuit_Click" 
                            ForeColor="Blue">[ 安全退出 ]&nbsp;</asp:LinkButton>
					    <asp:LinkButton ID="lbtChangePsw" runat="server" ForeColor="Blue"><A href="../Application/EmployeeManage/ChangePassword.aspx" target='fraMain' style="color:Blue">[ 修改密码 ]</A></asp:LinkButton>
					</TD>
			  </TR>
			</TABLE>
		</TD>
  </TR>
	<TR height=2>
		<TD background="../images/seperate_line_bg.gif"><IMG SRC="../images/seperate_line_bg.gif" WIDTH="1" HEIGHT="2" BORDER=0 ALT=""></TD>
  </TR>
</TABLE>
    </form>
</BODY>
</HTML>
