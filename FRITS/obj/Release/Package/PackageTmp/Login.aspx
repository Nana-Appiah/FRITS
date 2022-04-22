<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="FRITS.Login" %>

<%@ Reference Control="Utilities\ProfileManager\CtlLogin.ascx" %>
<%@ Reference Control="Utilities\ProfileManager\CtlForgottenPassword.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PAN-AFRICAN FRITS | Login</title>
    <link href="images/favicon.ico" rel="shortcut icon" />

    <script type="text/javascript" src="Resources/Scripts/Global.js"  ></script>

    <script type="javascript" src="Resources/Scripts/Keypad.js"></script>

    <script type="text/javascript" language="JavaScript">

        function disableCtrlKeyCombination(e) {

            //list all CTRL + key combinations you want to disable 
            var forbiddenKeys = new Array('a', 'n', 'c', 'x', 'v', 'j');
            var key;
            var isCtrl;

            if (window.event) {
                key = window.event.keyCode;     //IE 
                if (window.event.ctrlKey)
                    isCtrl = true;
                else
                    isCtrl = false;
            }
            else {
                key = e.which;     //firefox 

                if (e.ctrlKey)
                    isCtrl = true;
                else
                    isCtrl = false;
            }

            //if ctrl is pressed check if other key is in forbidenKeys array 
            if (isCtrl) {
                for (i = 0; i < forbiddenkeys.length; i++) {
                    //case-insensitive comparation 
                    if (forbiddenKeys[i].toLowerCase() == String.fromCharCode(key).toLowerCase()) {
                        alert('Key combination CTRL + ' + String.fromCharCode(key) + ' has been disabled.');
                        return false;
                    }
                }
            }

            return true;
        } 
    </script>

</head>
<body class="login" oncontextmenu="return false;">
    <form id="frm" runat="server">
    <div>
        <table cellpadding="50" cellspacing="0" style="height: 500px" width="100%">
            <tr>
                <td align="center" valign="middle" nowrap>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border: thin 2 black; color: White; background-color: #005B82; height: 25px;
                                width: 300px">
                                <img alt="PASL Logo" src="Images/Pan_African_logo_517x258.jpg" />
                            </td>
                        </tr>
                       <tr>
                            <td style="border: thin 2 black; padding: 15px; background-color: white;" align="center">
                                <br />
                                <br />
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                <br />
                                <br />
                                For further assistance, please contact your system administrator.
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                    <div style="color: White">
                        <br />
                        Copyright © 2017 Pan-African Savings and Loans Company Limited. All Rights Reserved.
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
