<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MCSApp._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<SCRIPT language="javascript">

function disableselect(e){
return false
}

function reEnable(){
return true
}

//if IE4+
//document.onselectstart=new Function ("return false")

//if NS6
if (window.sidebar){
document.onmousedown=disableselect
document.onclick=reEnable
}
</SCRIPT>

<style type=text/css>TD IMG {
	DISPLAY: block
}
BODY {
	MARGIN: 0px; BACKGROUND-COLOR: #f7f7f7
}
.STYLE1 {
	FONT-SIZE: 12px; COLOR: #333333; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 14px; BACKGROUND-COLOR: #edf6ff; BORDER-BOTTOM-STYLE: none
}
.STYLE2 {
	FONT-WEIGHT: bold; FONT-SIZE: 12px; COLOR: #ffffff
}
BODY {
	FONT-SIZE: 12px; COLOR: #ffffff
}
TD {
	FONT-SIZE: 12px; COLOR: #ffffff
}
TH {
	FONT-SIZE: 12px; COLOR: #ffffff
}
A:link {
	COLOR: #ffffff; TEXT-DECORATION: none
}
A:visited {
	COLOR: #ffffff; TEXT-DECORATION: none
}
A:hover {
	COLOR: #006699; TEXT-DECORATION: underline
}
A:active {
	COLOR: #006699; TEXT-DECORATION: none
}
</STYLE>

<SCRIPT src="images/md5.js"></SCRIPT>

<SCRIPT>
function IsDigit(cCheck)
	{
	return (('0'<=cCheck) && (cCheck<='9'));
	}

function IsAlpha(cCheck)
	{
         return ((('a'<=cCheck) && (cCheck<='z')) ||(cCheck!='_') || (('A'<=cCheck) && (cCheck<='Z')))
	}

function IsaNull(cCheck)
	{
	return(cCheck != " ")
	}

function checkform()
{
    try{
        var userObj=document.getElementById('name'); 
        var passwordObj=document.getElementById('password');
        var user=userObj.value;        
        var passwordvalue=passwordObj.value;   
                     
        if(user==''||passwordvalue=="") 
        {   
            alert("用户名或密码不能这空！");
            userObj.focus();
            return;
        }
        

        var re=/^\d{1}.*/;

//        if(re.test(user)){
//            alert("用户名不能以数字开头！");
//            userObj.focus();
//            return;
//        }

        re=/[\+|\-|\\|\/||&|!|~|@|#|\$|%| |\^|\*|\(|\)|=|\?|´|"|<|>|\.|,|:|;|\]|\[|\{|\}|\|]+/;
            if(re.test(user)){
            alert("您输入用户名含有非法字符！\n用户名为不以数字开头的字母、数字和下划线组成的名称，\n如：test12_");
            userObj.focus();
            return;
        }

    }catch(exception){}
document.forms[0].submit();
}

</SCRIPT>

<STYLE type=text/css>TD IMG {
	DISPLAY: block
}
BODY {
	MARGIN: 0px; BACKGROUND-COLOR: #f1f1f1
}
.STYLE1 {
	FONT-SIZE: 12px; COLOR: #333333; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 14px; BACKGROUND-COLOR: #f0fbff; BORDER-BOTTOM-STYLE: none
}
.STYLE2 {
	FONT-SIZE: 12px; COLOR: #ffffff
}
A:link {
	COLOR: #ffffff
}
A:visited {
	COLOR: #ffffff
}
A:hover {
	COLOR: #990000
}
A:active {
	COLOR: #990000
}
</STYLE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>生产控制系统</title>
</head>
<BODY>
    <form id="form2" runat="server">

<TABLE cellSpacing=0 cellPadding=0 width=780 align=center border=0>
  <TBODY>
  <TR height="82" width="780">
    <TD ></TD>
    <TD><IMG height=82 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD><IMG id=index_r2_c1 height=50 alt="" 
      src="images/index_r2_c1.jpg" width=780 border=0 
      name=index_r2_c1></TD>
    <TD><IMG height=50 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD><IMG id=index_r3_c1 height=71 alt="" 
      src="images/index_r3_c1.jpg" width=780 border=0 
      name=index_r3_c1></TD>
    <TD><IMG height=71 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD><IMG id=index_r4_c1 height=33 alt="" 
      src="images/index_r4_c1.jpg" width=780 border=0 
      name=index_r4_c1></TD>
    <TD><IMG height=33 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD background=images/index_r5_c1.gif height=74>
      <TABLE height=72 cellSpacing=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD width="47%">&nbsp;</TD>
          <TD width="20%">
            <TABLE cellSpacing=0 width="100%" border=0>
              <TBODY>
              <TR>
                <TD><IMG height=6 src="images/spacer.gif" 
                width=1></TD></TR>
              <TR>
                <TD>
                <asp:TextBox class=STYLE1 maxLength=16 size=24 name=name id="txtname" runat="server"></asp:TextBox>
                </TD></TR>
              <TR>
                <TD><IMG height=15 src="images/spacer.gif" 
                width=1>
                  </TD></TR>
              <TR>
                <TD>
                  <asp:TextBox class=STYLE1 maxLength=16 size=24 name=password id="txtpassword" runat="server" TextMode=Password></asp:TextBox>
                  </TD></TR>
              <TR>
                <TD><IMG height=6 src="images/spacer.gif" 
                width=1></TD></TR>
                
                
                </TBODY></TABLE>
                
          </TD>
          <TD width="2%">&nbsp;</TD>
          <TD width="8%">
              <asp:ImageButton ID="IBtn" ImageUrl="~/images/spacer.gif" runat="server" 
                  Height="50px" onclick="IBtn_Click" Width="60px" />
            </TD>
          <TD width="23%">&nbsp;</TD></TR></TBODY></TABLE></TD>
    <TD><IMG height=74 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD style="background-image:url('images/index_r8_c1.jpg')" align="center">
        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtname" Display="None" ErrorMessage="工号不能为空！" 
            SetFocusOnError="True"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txtpassword" Display="None" ErrorMessage="密码不能为空！" 
            SetFocusOnError="True"></asp:RequiredFieldValidator>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
            DisplayMode="List" />
      </TD>
    <TD><IMG height=57 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD><IMG id=index_r9_c1 height=55 alt="" 
      src="images/index_r9_c1.jpg" width=780 useMap=#index_r9_c1Map 
      border=0 name=index_r9_c1></TD>
    <TD><IMG height=55 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
  <TR>
    <TD><IMG id=index_r10_c1 height=63 alt="" 
      src="images/index_r10_c1.jpg" width=780 useMap=#index_r10_c1Map 
      border=0 name=index_r10_c1></TD>
    <TD><IMG height=63 alt="" src="images/spacer.gif" width=1 
      border=0></TD></TR>
       </TBODY></TABLE>
    </form>
</BODY></HTML>
