<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtlLogin.ascx.vb" Inherits="FRITS.CtlLogin" %>
<table cellpadding="3" cellspacing="5">
    <tr>
        <td align="left">
            Login ID
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtLoginID" CssClass="TextBox" runat="server" Width="200"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginID"
                Display="Dynamic" ErrorMessage="Please enter Login ID">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="left">
            Password
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtPassword" CssClass="TextBox" TextMode="Password" runat="server"
                Width="200" onKeyPress="return disableCtrlKeyCombination(event);" onKeyDown="return disableCtrlKeyCombination(event);"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                Display="Dynamic" ErrorMessage="Please enter Password">*</asp:RequiredFieldValidator>
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
            <asp:Button ID="btnLogin" CssClass="Button" runat="server" Text="Login" />
        </td>
        <td align="left">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                ShowMessageBox="True" ShowSummary="False" />
        </td>
    </tr>
</table>
<br />
<br />
Forgotten your password or password expired? <a href="Login.aspx?pg=reset">Click here</a>
to reset it. 