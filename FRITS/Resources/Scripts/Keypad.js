function On(iKey)
{			
	if(isNaN(iKey))
	{
		document.getElementById(iKey.name).style.color='red';			
	}
	else
	{
		document.getElementById(iKey).style.color='red';
	}
}
function Off(iKey)
{	
	if(isNaN(iKey))
	{
		document.getElementById(iKey.name).style.color='white';			
	}
	else
	{
		document.getElementById(iKey).style.color='white';
	}
}

var m_arrDesc	= new Array();	
var m_arrChar	= new Array();
var m_arrValues = new Array();

m_arrDesc[0]='Exclamation';
m_arrDesc[1]='AtTheRate';
m_arrDesc[2]='Hash';
m_arrDesc[3]='Dollar';
m_arrDesc[4]='Percentage';
m_arrDesc[5]='Ampersand';
m_arrDesc[6]='Exponential';
m_arrDesc[7]='Asterisk';
m_arrDesc[8]='OpenBracket';
m_arrDesc[9]='CloseBracket';
m_arrDesc[10]='Underscore';
m_arrDesc[11]='Plus';

m_arrDesc[22]='Minus';
m_arrDesc[23]='Equal';

m_arrDesc[34]='OpenBrace';
m_arrDesc[35]='CloseBrace';

m_arrDesc[45]='OpenSquare';
m_arrDesc[46]='CloseSquare';
m_arrDesc[47]='Colan';

m_arrDesc[55]='Semicolan';
m_arrDesc[56]='LessThan';
m_arrDesc[57]='Comma';
m_arrDesc[58]='GreaterThan';
m_arrDesc[59]='Dot';

m_arrDesc[60]='QuestionMark';
m_arrDesc[61]='ForwardSlash';
m_arrDesc[62]='Tilt';
m_arrDesc[63]='GraveAccent';

m_arrChar[0]='!';
m_arrChar[1]='@';
m_arrChar[2]='#';
m_arrChar[3]='$';
m_arrChar[4]='%';
m_arrChar[5]='^';
m_arrChar[6]='&';
m_arrChar[7]='*';
m_arrChar[8]='(';
m_arrChar[9]=')';
m_arrChar[10]='_';
m_arrChar[11]='+';	

m_arrChar[12]='1';
m_arrChar[13]='2';
m_arrChar[14]='3';
m_arrChar[15]='4';
m_arrChar[16]='5';
m_arrChar[17]='6';
m_arrChar[18]='7';
m_arrChar[19]='8';
m_arrChar[20]='9';
m_arrChar[21]='0';
m_arrChar[22]='-';
m_arrChar[23]='=';	

m_arrChar[24]='q';
m_arrChar[25]='w';
m_arrChar[26]='e';
m_arrChar[27]='r';
m_arrChar[28]='t';
m_arrChar[29]='y';
m_arrChar[30]='u';
m_arrChar[31]='i';
m_arrChar[32]='o';
m_arrChar[33]='p';
m_arrChar[34]='{';
m_arrChar[35]='}';

m_arrChar[36]='a';
m_arrChar[37]='s';
m_arrChar[38]='d';
m_arrChar[39]='f';
m_arrChar[40]='g';
m_arrChar[41]='h';
m_arrChar[42]='j';
m_arrChar[43]='k';
m_arrChar[44]='l';
m_arrChar[45]='[';
m_arrChar[46]=']';
m_arrChar[47]=':';
	
m_arrChar[48]='z';	
m_arrChar[49]='x';
m_arrChar[50]='c';
m_arrChar[51]='v';
m_arrChar[52]='b';
m_arrChar[53]='n';
m_arrChar[54]='m';
m_arrChar[55]=';';
m_arrChar[56]='<';
m_arrChar[57]=',';
m_arrChar[58]='>';
m_arrChar[59]='.';

m_arrChar[60]='?';
m_arrChar[61]='/';
m_arrChar[62]='~';
m_arrChar[63]='`';
m_arrChar[64]='Clear';
m_arrChar[65]='CapsLock';
m_arrChar[66]='Enter';
m_arrChar[67]='Close';
			
for(i=0;i<=67;i++)
{
	if( (i >= 0 && i <= 11) || (i >= 22 && i <= 23) || (i >= 34 && i <= 35) || (i >= 45 && i <= 47) || (i >= 55 && i <= 63))
	{
		m_arrValues[i] = "<input type=button onMouseOver='On(" + m_arrDesc[i] + ");' onMouseOut='Off(" + m_arrDesc[i] + ");' class='Keys' name='" + m_arrDesc[i] + "' onclick='javascript:GetKey(" + m_arrDesc[i] + ");' value='" + m_arrChar[i] + "'>";
	}
	else if(i == 66)
	{
		m_arrValues[i] = "<input type=button onMouseOver='On(" + m_arrChar[i] + ");' onMouseOut='Off(" + m_arrChar[i] + ");' class='Keys' name='" + m_arrChar[i] + "' onclick='return Test();' value='" + m_arrChar[i] + "'><input type=hidden id='hidden_enter' name='hidden_enter'>";
	}
	else if(i == 67)
	{
		m_arrValues[i] = "<table height=10 width='18' cellspacing=0 cellpadding=0><tr><td class='Close'><a href='#' style='font-family:arial;font-weight:bold;color:black;text-decoration:none' title='Close' onclick='javascript:GetKey(-1);'>X</a></td><td width=2></td></tr></table>";
	}
	else
	{
		m_arrValues[i] = "<input type=button onMouseOver=On(" + m_arrChar[i] + "); onMouseOut=Off(" + m_arrChar[i] + "); class='Keys' name='" + m_arrChar[i] + "' onclick=javascript:GetKey(" + m_arrChar[i] + "); value='" + m_arrChar[i] + "'>";
	}
		
}

var bCAPS = false;

function SetKeyPosition()
{
	var i;
	var j;
			
	for(i=0;i<=67;i++)
	{	
		Position = eval("pos" + i);
		Position.innerHTML = m_arrValues[i];			
	}
	
	var l_int_Width		= Math.round(Math.random() * 150);
	TDKeyBoard.width	= l_int_Width;
	bCAPS				= false;
}

function GetKey(iKey)
{
	if(isNaN(iKey))
	{
		iKey = iKey.value;
	}
	
	if (iKey == -1)
	{
		document.getElementById('RowKeyboard').style.display = 'none';						
		return;
	}
	
	if(iKey == -2)
	{
		
		document.getElementById('hidden_enter').value = 'hidden_enter';
		//alert(document.getElementById('hidden_enter').value);
		//document.all("Example").value=document.getElementById('hidden_enter').value;
		//alert(document.all("Example").value);
		document.forms[0].submit();
	}
			
	if (iKey == 'Clear')
	{
		obj.value = '';			
		return;
	}
	
	if (iKey == 'CapsLock')
	{
		document.getElementById(iKey).value = "CAPSLOCK";
		bCAPS = true;
		ChangeLetterCase('U');
		return;
	}
	
	if (iKey == 'CAPSLOCK')
	{
		document.getElementById(iKey).value = "CapsLock";
		ChangeLetterCase('L');			
		bCAPS = false;
		return;
	}
	
	if (iKey == 'Upper')
	{
		obj.value = obj.value.toUpperCase();
		return;
	}
	
	if (iKey == 'Lower')
	{
		obj.value = obj.value.toLowerCase();
		return;
	}
	
	if (isNaN(iKey))
	{
		if (bCAPS)
			iKey = iKey.toUpperCase();
		else
			iKey = iKey.toLowerCase();
	}
	
	obj.value+= iKey;
}

function ChangeLetterCase(Case)
{
	for(i=24;i<=54;i++)
	{
		if( (i >= 24 && i <= 33) || (i >= 36 && i <= 44) || (i >= 48 && i <= 54) )
		{
			if(Case == 'U')
			{
				document.all(m_arrChar[i]).value = document.all(m_arrChar[i]).value.toUpperCase();
			}
			else
			{
				document.all(m_arrChar[i]).value = document.all(m_arrChar[i]).value.toLowerCase();
			}
		}
	}		
}