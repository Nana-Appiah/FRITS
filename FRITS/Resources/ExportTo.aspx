<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExportTo.aspx.vb" Inherits="FRITS.ExportTo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Export To</title>

    <script language="javascript" src="Resources/Scripts/global.js"></script>

    <script type="text/javascript">
		<!--
		
		 //-->
    </script>

</head>
<body onload="parent.window.document.title='Export To';">
    <form id="frm" runat="server">
    <div>
        <div style="padding-right: 15px; padding-left: 15px; padding-bottom: 15px; padding-top: 15px;">
            <table cellspacing="0" cellpadding="3" border="0">
                <tr>
                    <td nowrap height="25">
                        File Name
                    </td>
                    <td nowrap align="center" height="25">
                        :
                    </td>
                    <td nowrap height="25">
                        <asp:TextBox ID="txtName" runat="server" Width="230px" CssClass="TextBox"></asp:TextBox>.xls
                    </td>
                </tr>
                <tr>
                    <td nowrap height="25">
                        Location
                    </td>
                    <td nowrap align="center" height="25">
                        :
                    </td>
                    <td nowrap height="25">
                        <asp:TextBox ID="txtLocation" runat="server" Width="250px" CssClass="TextBox">C:\</asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <div align="center" style="display: none;">
                <asp:Button ID="btnSubmit" runat="server" Text="Continue" ToolTip="Continue" CssClass="Button" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
