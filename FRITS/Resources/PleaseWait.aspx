<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PleaseWait.aspx.vb" Inherits="FRITS.PleaseWait" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Please Wait</title>

    <script language="javascript" src="Resources/Scripts/global.js"></script>

</head>
<body onload="parent.window.document.title='Processing';">
    <form id="frm" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" border="0" height="100%" width="100%" align="center">
            <tr>
                <td style="padding-bottom: 5px; padding-top: 10px" align="center">
                    Process in progress....<br>
                    <span id="uploading" style="display: block" name="uploading">
                        <img src="../images/UPload_00.gif" border="0" alt="">
                    </span>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
