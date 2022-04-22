<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dialog.aspx.vb" Inherits="FRITS._Dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frm" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;
        table-layout: fixed">
        <tr>
            <td valign="top">
                <iframe name="ContentWrapperFrame" style="width: 100%; height: 100%;" scrolling="auto"
                    src="<%=DefaultPage%>" class="ContentWrapperFrame" frameborder="0"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
