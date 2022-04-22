window.onload = function () {
	if (document.readyState === 'complete') {
		if (window.showModalDialog == undefined) {
			window.showModalDialog = function (url, mixedVar, features) {
				window.hasOpenWindow = true;
				if (mixedVar) var mixedVar = mixedVar;
				if (features) var features = features.replace(/(dialog)|(px)/ig, "").replace(/;/g, ',').replace(/\:/g, "=");
				window.myNewWindow = window.open(url, "_blank", features);
			}
		}
	}
}

function window_onload()
{
    if (window.screen.height != 786 && window.screen.width != 1024) {
	    alert('This application is best viewed at a screen resolution of 1024X786');
	}
}

function LaunchHomepage()
{
    window.location.href = applicationPath + '/Default.aspx';
}

function LaunchMyProfile()
{
    window.location.href = applicationPath + '/MyProfile.aspx';
}

function LaunchHelp()
{
    alert('Not Available.');
}

function LaunchAbout()
{
    var url = base64encode('Help/About.aspx'); 
	window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:300px;dialogWidth:450px;center:yes;resizable:no;scroll:no;status:no;help:no');
}	

function LaunchProfileManager()
{
	//var url = base64encode(applicationPath + '/Utilities/ProfileManager/Default.aspx');
	//window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:465px;dialogWidth:800px;center:yes;resizable:no;scroll:no;status:no;help:no');

	ShowDialog('/Utilities/ProfileManager/Default.aspx', 465, 800);
}	

function LaunchForgottenPassword()
{
    var url = base64encode(applicationPath + '/Utilities/ProfileManager/ForgottenPassword.aspx');
    
	window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:200px;dialogWidth:550px;center:yes;resizable:no;scroll:no;status:no;help:no');
}

function LaunchChangePassword()
{
    var url = base64encode(applicationPath + '/Utilities/ProfileManager/ChangePassword.aspx'); 
    
    window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:200px;dialogWidth:300px;center:yes;resizable:no;scroll:no;status:no;help:no');
}

function LaunchExpiredPage()
{
	var newWindow = window.open('','_self');
		
	newWindow.document.writeln('<html><head><title><\/title>\n');
	newWindow.document.writeln('<body bgcolor=#ffffff">');
	newWindow.document.writeln("	<table width=98% cellpading=0 cellspacing=0 bgcolor=#ffffff><tr><td align=right><font face=arial size=2><b>Expired</b></font></td></tr></table>");
	newWindow.document.writeln('<\/body><\/html>');

	window.location.href = "Blank.html";

}

function OpenDialog(url,height,width)
{
    // we must encode url and decode it in iframe as source url
    var newurl = base64encode(url); 
    
    //window.open(applicationPath + '/Resources/Dialog.aspx?url=' + newurl, "dialog", "width=" + width + ", height=" + height + ", resizable=no, scrollbars=yes, menubar=no, toolbar=no");
    window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + newurl, '', 'dialogHeight:' + height + 'px;dialogWidth:' + width + 'px;center:yes;resizable:yes;scroll:yes;status:no;help:no');

}

function Reload()
{
	window.document.location.href = window.document.location.href;
}

function LogOut()
{
	if(confirm('Are you sure you want to log out of the system?'))
	{
		window.location.href = applicationPath + '/Login.aspx?fn=logout';
	}
}

function ShowClsBtn()
{
    top.window.document.all.clsBtn.style.display = 'block';
}
	
function mouseover(item)
{
	switch (item)
	{
		case 'hidetoc' :
			window.status = 'Hide Navigation Bar';
		break;
	}
}

function mouseout(item)
{
	switch (item)
	{
		case 'hidetoc' :
			window.status = '';
		break;
	}
}	

function PopUpDialog(url,hgt,wth)
{
	var x = showModalDialog(url,window,'status:no;resize:yes;dialogHeight:'+hgt+';dialogWidth:'+wth+';help:no');
	if (x == "" || x == null || x == "undefined")
	{ return; }
		
	GoToURL(x);
}

function PopUpDialog(url,name,hgt,wth)
{
	var x = showModalDialog(url,name,'status:no;resize:yes;dialogHeight:'+hgt+';dialogWidth:'+wth+';help:no');
	if (x == "" || x == null || x == "undefined")
	{ return; }
		
	GoToURL(x);
}

function GoToURL(str)
{
	if (str == "" || str == null || str == "undefined")
	{	return;	}
	window.fraContent.location.href = str;
}

function toggleElement(eName,eHeader,eUrl)
{
	document.getElementById(eHeader).innerText = eName;
	
	if (!eUrl=='')
	{
		window.fraContent.location.href = applicationPath + '/'+ eUrl;
	}
}

function ShowProcessing()
{							
	document.getElementById('processing').style.display = '';
}

function ShowUploading()
{							
	document.getElementById('uploading').style.display = '';
}

function OpenDetailWin(url)
{
	var win = window.open(url,'Details','height=450 width=325','_blank');
	win.focus();
}

var base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var base64DecodeChars = new Array(
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,
    52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,
    -1,  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14,
    15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,
    -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1);

function base64encode(str) {
    var out, i, len;
    var c1, c2, c3;

    len = str.length;
    i = 0;
    out = "";
    while(i < len) {
	c1 = str.charCodeAt(i++) & 0xff;
	if(i == len)
	{
	    out += base64EncodeChars.charAt(c1 >> 2);
	    out += base64EncodeChars.charAt((c1 & 0x3) << 4);
	    out += "==";
	    break;
	}
	c2 = str.charCodeAt(i++);
	if(i == len)
	{
	    out += base64EncodeChars.charAt(c1 >> 2);
	    out += base64EncodeChars.charAt(((c1 & 0x3)<< 4) | ((c2 & 0xF0) >> 4));
	    out += base64EncodeChars.charAt((c2 & 0xF) << 2);
	    out += "=";
	    break;
	}
	c3 = str.charCodeAt(i++);
	out += base64EncodeChars.charAt(c1 >> 2);
	out += base64EncodeChars.charAt(((c1 & 0x3)<< 4) | ((c2 & 0xF0) >> 4));
	out += base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >>6));
	out += base64EncodeChars.charAt(c3 & 0x3F);
    }
    return out;
}

function base64decode(str) {
    var c1, c2, c3, c4;
    var i, len, out;

    len = str.length;
    i = 0;
    out = "";
    while(i < len) {
	/* c1 */
	do {
	    c1 = base64DecodeChars[str.charCodeAt(i++) & 0xff];
	} while(i < len && c1 == -1);
	if(c1 == -1)
	    break;

	/* c2 */
	do {
	    c2 = base64DecodeChars[str.charCodeAt(i++) & 0xff];
	} while(i < len && c2 == -1);
	if(c2 == -1)
	    break;

	out += String.fromCharCode((c1 << 2) | ((c2 & 0x30) >> 4));

	/* c3 */
	do {
	    c3 = str.charCodeAt(i++) & 0xff;
	    if(c3 == 61)
		return out;
	    c3 = base64DecodeChars[c3];
	} while(i < len && c3 == -1);
	if(c3 == -1)
	    break;

	out += String.fromCharCode(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));

	/* c4 */
	do {
	    c4 = str.charCodeAt(i++) & 0xff;
	    if(c4 == 61)
		return out;
	    c4 = base64DecodeChars[c4];
	} while(i < len && c4 == -1);
	if(c4 == -1)
	    break;
	out += String.fromCharCode(((c3 & 0x03) << 6) | c4);
    }
    return out;
}

function emailCheck (emailStr) {

	/* The following variable tells the rest of the function whether or not
	to verify that the address ends in a two-letter country or well-known
	TLD.  1 means check it, 0 means don't. */

	var checkTLD=1;

	/* The following is the list of known TLDs that an e-mail address must end with. */

	var knownDomsPat=/^(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum)$/;

	/* The following pattern is used to check if the entered e-mail address
	fits the user@domain format.  It also is used to separate the username
	from the domain. */

	var emailPat=/^(.+)@(.+)$/;

	/* The following string represents the pattern for matching all special
	characters.  We don't want to allow special characters in the address. 
	These characters include ( ) < > @ , ; : \ " . [ ] */

	var specialChars="\\(\\)><@,;:\\\\\\\"\\.\\[\\]";

	/* The following string represents the range of characters allowed in a 
	username or domainname.  It really states which chars aren't allowed.*/

	var validChars="\[^\\s" + specialChars + "\]";

	/* The following pattern applies if the "user" is a quoted string (in
	which case, there are no rules about which characters are allowed
	and which aren't; anything goes).  E.g. "jiminy cricket"@disney.com
	is a legal e-mail address. */

	var quotedUser="(\"[^\"]*\")";

	/* The following pattern applies for domains that are IP addresses,
	rather than symbolic names.  E.g. joe@[123.124.233.4] is a legal
	e-mail address. NOTE: The square brackets are required. */

	var ipDomainPat=/^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/;

	/* The following string represents an atom (basically a series of non-special characters.) */

	var atom=validChars + '+';

	/* The following string represents one word in the typical username.
	For example, in john.doe@somewhere.com, john and doe are words.
	Basically, a word is either an atom or quoted string. */

	var word="(" + atom + "|" + quotedUser + ")";

	// The following pattern describes the structure of the user

	var userPat=new RegExp("^" + word + "(\\." + word + ")*$");

	/* The following pattern describes the structure of a normal symbolic
	domain, as opposed to ipDomainPat, shown above. */

	var domainPat=new RegExp("^" + atom + "(\\." + atom +")*$");

	/* Finally, let's start trying to figure out if the supplied address is valid. */

	/* Begin with the coarse pattern to simply break up user@domain into
	different pieces that are easy to analyze. */

	var matchArray=emailStr.match(emailPat);

	if (matchArray==null) {

	/* Too many/few @'s or something; basically, this address doesn't
	even fit the general mould of a valid e-mail address. */

	alert("Email address seems incorrect (check @ and .'s)");
	return false;
	}
	var user=matchArray[1];
	var domain=matchArray[2];

	// Start by receiveg that only basic ASCII characters are in the strings (0-127).

	for (i=0; i<user.length; i++) {
	if (user.charCodeAt(i)>127) {
	alert("Ths username contains invalid characters.");
	return false;
	}
	}
	for (i=0; i<domain.length; i++) {
	if (domain.charCodeAt(i)>127) {
	alert("Ths domain name contains invalid characters.");
	return false;
	}
	}

	// See if "user" is valid 

	if (user.match(userPat)==null) {

	// user is not valid

	alert("The username doesn't seem to be valid.");
	return false;
	}

	/* if the e-mail address is at an IP address (as opposed to a symbolic
	host name) make sure the IP address is valid. */

	var IPArray=domain.match(ipDomainPat);
	if (IPArray!=null) {

	// this is an IP address

	for (var i=1;i<=4;i++) {
	if (IPArray[i]>255) {
	alert("Destination IP address is invalid!");
	return false;
	}
	}
	return true;
	}

	// Domain is symbolic name.  Check if it's valid.

	var atomPat=new RegExp("^" + atom + "$");
	var domArr=domain.split(".");
	var len=domArr.length;
	for (i=0;i<len;i++) {
	if (domArr[i].search(atomPat)==-1) {
	alert("The domain name does not seem to be valid.");
	return false;
	}
	}

	/* domain name seems valid, but now make sure that it ends in a
	known top-level domain (like com, edu, gov) or a two-letter word,
	representing country (uk, nl), and that there's a hostname preceding 
	the domain or country. */

	if (checkTLD && domArr[domArr.length-1].length!=2 && 
	domArr[domArr.length-1].search(knownDomsPat)==-1) {
	alert("The address must end in a well-known domain or two letter " + "country.");
	return false;
	}

	// Make sure there's a host name preceding the domain.

	if (len<2) {
	alert("This address is missing a hostname!");
	return false;
	}

	// If we've gotten this far, everything's valid!
	return true;
}

/***** ADDED BY CYO*/

function ShowDialog(url, height, width) {

	
	var loadurl = base64encode(url);
	var _appUrl = location.href.replace(location.pathname, '') + url    

	var rslt = window.showModalDialog(_appUrl + '/Resources/Dialog.aspx?url=' + loadurl, '', 'dialogHeight:' + height + 'px;dialogWidth:' + width + 'px;center:yes;resizable:yes;scroll:yes;status:no;help:no');

		return true;		       
}




function refreshParent() {
    __doPostBack('', '');
      // window.close();
    return true;
}

function ConfirmAction(msg) {    
    if (confirm(msg + "?") == true)
        return true;
    else
        return false;    
}

function SetEnd(TB) {
    if (TB.createTextRange) {
        var FieldRange = TB.createTextRange();
        FieldRange.moveStart('character', TB.value.length);
        FieldRange.collapse();
        FieldRange.select();
    }
}

function toastify(type, msg, title, position, showclosebutton) {

    if (position == null || position == '') {
        toastr.options.positionClass = 'toast-bottom-right';
    }
    else {
        toastr.options.positionClass = position;
    }
    if (showclosebutton == null || showclosebutton == '' || showclosebutton == 'true') {
        toastr.options.closeButton = 'true';
    }

    switch (type) {
        case 'success': toastr.success(msg, title);
            break;
        case 'info': toastr.info(msg, title);
            break;
        case 'warning': toastr.warning(msg, title);
            break;
        case 'error': toastr.error(msg, title);
            break;
    }

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "4500",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    
   
    
    //toastr.clear();
}

/*$(document).ready(function () {
//    function doAjaxCall(e, param) {
//        //e.preventDefault();      
//        jQuery.ajax({
//            url: url,
//            type: "POST",
//            dataType: "json",
//            data: param,
//            contentType: "application/json; charset=utf-8",
//            success: function (data) {
//                alert(JSON.stringify(data));
//            },            
//            failure: function(response) {
//                alert(response.d);
//            });
//    }
//})*/

