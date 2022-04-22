<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChangePassword.aspx.vb"
    Inherits="FRITS.ChangePassword" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>

    <script type="text/javascript" src="../../Resources/Scripts/global.js"></script>

    <script type="text/javascript">

    </script>

</head>
<body class="dialog" onload="parent.window.document.title='Change Password';">
    <form id="frm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div style="padding-right: 15px; padding-left: 15px; padding-bottom: 25px; padding-top: 25px;">
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <table cellspacing="0" cellpadding="3" border="0" width="100%" align="center">
                    <tr>
                        <td nowrap height="25">
                            Old Password
                        </td>
                        <td nowrap height="25">
                            :
                        </td>
                        <td nowrap width="100%" height="25">
                            <asp:TextBox ID="txtOldPassword" runat="server" MaxLength="8" TextMode="Password"
                                CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap height="25">
                            New Password
                        </td>
                        <td nowrap height="25">
                            :
                        </td>
                        <td nowrap width="100%" height="25">
                            <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="8" TextMode="Password"
                                CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap height="25">
                            Confirm Password
                        </td>
                        <td nowrap height="25">
                            :
                        </td>
                        <td nowrap width="100%" height="25">
                            <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="8" TextMode="Password"
                                CssClass="TextBox"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnSubmit" />
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
        <div align="center" style="display: none;">
            <asp:Button ID="btnSubmit" runat="server" Text="Generate" ToolTip="Generate" CssClass="Button" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClientClick="parent.close();"
                CssClass="Button" />
        </div>
    </div>
    </form>
</body>
</html>
