<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForgottenPassword.aspx.vb"
    Inherits="FRITS.ForgottenPassword" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgotten Password</title>

    <script type="text/javascript" src="../../Resources/Scripts/global.js"></script>

    <script type="text/javascript">

        function SetFocus() {
            document.frm.txtEmail.focus();
        }
		
    </script>

</head>
<body class="dialog" onload="parent.window.document.title='Forgotten Password';SetFocus();">
    <form id="frm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div style="padding-right: 15px; padding-left: 15px; padding-bottom: 35px; padding-top: 35px;">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" align="center"
            border="0">
            <tr>
                <td align="center" nowrap>
                    <p>
                        Please enter the email address you provided when your profile was being created.</p>
                    <asp:UpdatePanel ID="UpdatePanel" runat="server">
                        <ContentTemplate>
                            <table cellspacing="0" cellpadding="3" border="0" align="center">
                                <tr>
                                    <td nowrap="nowrap" height="25">
                                        Email Address
                                    </td>
                                    <td nowrap="nowrap" height="25">
                                        :
                                    </td>
                                    <td nowrap="nowrap" height="25">
                                        <asp:TextBox ID="txtEmail" TabIndex="0" runat="server" CssClass="TextBox" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                        <ProgressTemplate>
                            <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0">
                                <tr>
                                    <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                                        <img alt="" src="../../Images/spinner.gif" border="0" />
                                    </td>
                                </tr>
                            </table>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <br />
                    <div align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Send Password" OnClientClick="this.disabled = true;__doPostBack('btnSubmit','');" ToolTip="Send Password"
                            CssClass="Button" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClientClick="parent.close();"
                            CssClass="Button" Visible="false" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
