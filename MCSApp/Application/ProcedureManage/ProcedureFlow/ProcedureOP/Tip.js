    if ( had_tips != 1 ) { 
    var _ua = document.all ? ((navigator.userAgent.indexOf('MSIE 5') > 5) ? "ie5" : "ie4") : "foo"; 
    if (_ua != "foo" ) { 
    //if (typeof(_doc) == "undefined" || _doc == null) _doc = document; 
    var _tipx = 0; 
    var _tipy = 0; 
    var _tipOX = 10; 
    var _tipOY = 1; 
    var _tipWidth; 
    
    // Opacity=70 提示框背景透明度 
    //document.write('<DIV id="TIP__"  STYLE="filter:Alpha(Opacity=70,style=1,Finishopacity=80);position:absolute;display:none;z-index:100"></DIV>'); 
    
    document.onmousemove = tsai; 
    var _tip = document.getElementById('TIP__'); 
    } 
    } 
    var had_tips = 1; 
    function tsaiannie (sMsg, sTitle, iWidth, iHeight, iBorder, sTFC, sTBC, sMFC, sMBC) { 
    
    _tipWidth = 180; // 提示框的寬度 
    var _tipBorder = 2, // 提示框边框的寬度 
    _tipTFC = "#FFF8F0", 
    _tipTBC = "#ff8c00", // 提示框边框的顏色 
    _tipMFC = "#3F3F38", 
    _tipMBC = "ffffff"; // 提示框背景的顏色 
    
    if (_ua == "foo") return; 
    _tip.innerHTML = '<TABLE CELLPADDING="'+_tipBorder+'" CELLSPACING="0" WIDTH="'+_tipWidth+'" BORDER="0" BGCOLOR="'+_tipTBC+'"><TR><TD>' 
    + ((sTitle != null) ? '<TABLE CELLPADDING="0" CELLSPACING="0" WIDTH="100%" BORDER="0"><TR><TD STYLE="padding-top:2px;padding-bottom:2px;font:8pt Arial,Verdana,Tahoma;color:'+_tipTFC+'">'+sTitle+'</TD></TR></TABLE>' : '') 
    + '<TABLE CELLPADDING="2" CELLSPACING="0" WIDTH="100%" BORDER="0" BGCOLOR="'+_tipMBC+'"><TR><TD STYLE="color:'+_tipMFC+';font:normal 8pt Arial,Verdana,Tahoma">'+sMsg+'</TD></TR></TABLE></TD></TR></TABLE>'; 
    annie(); 
    _tip.style.display = ""; 
    } 
    
    function tsai() { 
    if (_ua == "ie4") { 
    _tipx = event.x; 
    _tipy = event.y; 
    } else if (_ua == "ie5") { 
    _tipx = (event.clientX + _tipOX + _tipWidth <= document.body.clientWidth) ? 
    (event.clientX + document.body.scrollLeft + _tipOX) : 
    (event.clientX + document.body.scrollLeft - _tipWidth - 5); 
    _tipy = event.clientY + document.body.scrollTop + _tipOY; 
    } else { 
    return; 
    } 
    if (_tip.style.display != "none") annie(); 
    } 
    
    function annie() { 
    _tip.style.pixelTop = _tipy; 
    _tip.style.pixelLeft = _tipx; 
    } 
    
    function tsai900403 () { 
    _tip.style.display = "none"; 
    } 
    document.write('<DIV></DIV>'); 