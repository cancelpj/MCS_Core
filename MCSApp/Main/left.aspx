<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="left.aspx.cs" Inherits="MCSApp.Main.left" %>

<%@ Register src="../Common/LeftMenuItem.ascx" tagname="LeftMenuItem" tagprefix="uc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<HEAD>
<TITLE>MCSleft</TITLE>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<link href="style.css" rel="stylesheet" type="text/css">
<style>
<!-- 
td{

}
.menubar{
	behavior:url(menubar.htc);
}
.barcontent{
	display:none;
}
.barcontent1{
	overflow: hidden;
	display:;
}
.bartitle
{
	background: url(../images/toolbar_bg.gif);
	background-repeat: repeat-x;
	height:25px;
	font-variant: middle;
	cursor:hand;
}
.datalist{
	color:white;
	line-height: 180%;
	padding-left:5pt;
}
.clstrc{
	display:none;
}
.clstrcshow{
	display:;
}
.clstr{
	color:white;
	cursor:hand;
}
-->
</style>
<SCRIPT LANGUAGE="JavaScript">
<!--
function init()
{
}

function RemoveAll()
{
	top.fraMain.win.removeall();
}

function AddWin(url, title)
{
	top.fraMain.AddWin(url, title);
}
function gettabWin()
{
var clkelement=event.srcElement;
//clkelement.target="tabWin";
clkelement.target="fraMain";
    //return "tabWin";
}
//-->
</SCRIPT>
</HEAD>

<BODY bgcolor="#6E8ADE" style="border-left:1pt solid white;border-bottom:1pt solid white;" leftmargin="0" topmargin="0" oncontextmenu="return false" onselectstart="return false" ondragstart="return false" scroll="no" onload="init()">
<form runat='server'>
<uc1:LeftMenuItem ID="LeftMenuItem1" runat="server" />
</form>
</BODY>
</HTML>

