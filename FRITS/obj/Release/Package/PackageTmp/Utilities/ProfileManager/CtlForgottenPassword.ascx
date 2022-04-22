<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtlForgottenPassword.ascx.vb"
    Inherits="FRITS.CtlForgottenPassword" %>
<asp:ScriptManager ID="ScriptManager" runat="server" />
<br />
<b>Enter the email address associated with your account.</b><br />
<br />
<table cellspacing="5" cellpadding="3" border="0">
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
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td align="left">
            <asp:Button ID="btnSubmit" runat="server" Text="Send Password" OnClientClick="this.disabled = true;ShowProcessing();__doPostBack('ctl04$btnSubmit','');"
                ToolTip="Send Password" CssClass="Button" />
        </td>
        <td align="left">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                ShowMessageBox="True" ShowSummary="False" />
        </td>
    </tr>
</table>
<div align="center" id="processing" style="display: none;">
    <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0" style="display: block;">
        <tr>
            <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                <img alt="" src="Images/spinner.gif" border="0" />
                <br />
                <br />
                Please wait...
            </td>
        </tr>
    </table>
</div>
<br />
<br />
Remember your password? <a href="Login.aspx">Click here</a> to login. 