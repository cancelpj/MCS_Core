
var oPopup = window.createPopup();
/*****************************************************
ȡָ�������x����
*****************************************************/
function getx(e){
  var l=e.offsetLeft;
  while(e=e.offsetParent){
    l+=e.offsetLeft;
    }
  return(l);
  }
/*****************************************************
ȡָ�������y����
*****************************************************/
function gety(e){
  var t=e.offsetTop;
  while(e=e.offsetParent){
    t+=e.offsetTop;
    }
  return(t);
  }
/*****************************************************
��ʾpopup����
c:��������obj
x:���o��x�������
y:���o��y�������
w:���
h:�߶ȣ��������Ϊ0�����л�ȡĬ�ϸ߶�
o:����ں�obj
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
��ȡxml���ڵ�
*****************************************************/
function getxmldoc(url)
{
    var oXMLDoc = new ActiveXObject('MSXML');
    oXMLDoc.url = url;
	var ooRoot=oXMLDoc.root;
	return ooRoot;
}
/*****************************************************
����gifͼƬ��꾭��Ч������꾭������ʾ��ͼƬ�ļ�����ԭͼƬ�����_over
*****************************************************/
function imgover(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG") //����img�������˳�
		return false;
	var r, re;                    // ����������
	var ss = obj.src;
	re = /.gif$\b/i;             // ����������ʽģʽ��
	r = ss.replace(re, "_over.gif");    //����_over
	obj.src=r;
	obj.behave='over';
}
/*****************************************************
����gifͼƬ��갴��Ч������갴�º���ʾ��ͼƬ�ļ�����ԭͼƬ�����_down
*****************************************************/
function imgdown(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(obj.tagName!="IMG") //����img�������˳�
		return false;
	var r, re;                    // ����������
	var ss = obj.src;
	if(obj.behave=='over')
	{
		re = /_over.gif$\b/i;             
		r = ss.replace(re, "_down.gif"); 
	}
	if(obj.behave=='')
	{
		re = /.gif$\b/i;             // ����������ʽģʽ��
		r = ss.replace(re, "_down.gif");    //����_down
	}
	obj.src=r;
	obj.behave='down';
}
/*****************************************************
����gifͼƬ��갴��Ч������꾭������ʾ��ͼƬ�ļ�����ԭͼƬ�����_over
*****************************************************/
function imgup(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG") //����img�������˳�
		return false;
	var r, re;                    // ����������
	var ss = obj.src;
	re = /_down.gif$\b/i;             // ����������ʽģʽ��
	r = ss.replace(re, "_over.gif");   
	obj.src=r;
	obj.behave='over';
}
/*****************************************************
����gifͼƬ����Ƴ�Ч������꾭������ʾ��ͼƬ�ļ�����ԭͼƬ����ȥ��_over
*****************************************************/
function imgout(obj)
{
	if(typeof(obj)!="object")return false;
	if(obj.tagName!="IMG")	//����img�������˳�
		return false;
	var r, re;                    // ����������
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
������ʽ����꾭��Ч������꾭����ԭ��ʽ���ں��over
*****************************************************/
function classover(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	if(obj.behave=='over')return;
	var ss = obj.className;
	var r = ss+"_over";    //����over
	obj.className=r;
	obj.behave='over';
}
/*****************************************************
������ʽ������Ƴ�Ч��������Ƴ���ԭ��ʽ���ں�ȥ��over
*****************************************************/
function classout(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	var r, re;                    // ����������
	var ss = obj.className;
	if(obj.behave=='over')
	{
		re = /_over$\b/i;             // ����������ʽģʽ��
		r = ss.replace(re, "");    
	}
	if(obj.behave=='down')
	{
		re = /_down$\b/i;             // ����������ʽģʽ��
		r = ss.replace(re, "");    
	}
	obj.className=r;              
	obj.behave='';
}
/*****************************************************
������ʽ����갴��Ч������갴�º�ԭ��ʽ���ں����down
*****************************************************/
function classdown(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	var r, re;                    // ����������
	var ss = obj.className;
	re = /_over$\b/i;             // ����������ʽģʽ��
	r = ss.replace(re, "_down");    
	obj.className=r;              
	obj.behave='down';
}
/*****************************************************
������ʽ������ͷ�Ч��������ͷź�ԭ��ʽ���ں�
*****************************************************/
function classup(obj)
{
	if(obj.locked == "true") return; //��������������򲻴����¼�
	if(typeof(obj)!="object")return false;
	var r, re;                    // ����������
	var ss = obj.className;
	re = /_down$\b/i;             // ����������ʽģʽ��
	r = ss.replace(re, "_over");   
	obj.className=r;
	obj.behave='over';
}
/*****************************************************
���������ȷ��
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
