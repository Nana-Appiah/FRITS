<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master"
    CodeBehind="MyProfile.aspx.vb" Inherits="FRITS.MyProfile" Title="PAN-AFRICAN FRITS | My Profile" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">
        <tr>
            <td style="width: 100%" valign="top">
                <div class="NavHeader" style="width: 180px">
                    My Profile
                </div>
                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                    <tr>
                        <td style="width: 100%; height: 200px; vertical-align: top;" nowrap="nowrap">
                             <div style="padding-right: 5px; padding-left: 5px; padding-bottom: 10px; padding-top: 10px">
                                <div id="Toolbar" align="right" runat="server">
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="center" width="100%" height="25">
                                                <ComponentArt:ToolBar ID="tbrMain" CssClass="h_toolbar" ImagesBaseUrl="images/" DefaultItemCssClass="item"
                                                    DefaultItemHoverCssClass="itemHover" DefaultItemActiveCssClass="itemActive" DefaultItemTextImageRelation="ImageOnly"
                                                    DefaultItemImageHeight="16" DefaultItemImageWidth="16" ItemSpacing="1" Orientation="Horizontal"
                                                    AutoPostBackOnSelect="true" runat="server">
                                                    <Items>
                                                        <ComponentArt:ToolBarItem ItemType="Command" Text="Save" Value="Save" ToolTip="Save"
                                                            TextImageRelation="TextOnly" ImageUrl="saveitem.gif" />
                                                        <ComponentArt:ToolBarItem ItemType="Separator" ImageUrl="h_break.gif" ImageHeight="16"
                                                            ImageWidth="2" Visible="false" />
                                                        <ComponentArt:ToolBarItem ToolTip="Help" ImageUrl="help.gif" />
                                                    </Items>
                                                </ComponentArt:ToolBar>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
                                        <fieldset>
                                            <legend>General</legend>
                                            <br />
                                            <table cellspacing="3" cellpadding="0" border="0" width="100%" align="center">
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        User Id
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:Label ID="lblUserId" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        Full Name
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:Label ID="lblFullName" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        Email Address
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:TextBox ID="txtEmailAddress" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        Membership
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:Literal ID="ltlMembership" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </fieldset>
                                        <br />
                                        <fieldset>
                                            <legend>
                                                <asp:CheckBox ID="chkChangePassword" Text="Password" ToolTip="Change Password" AutoPostBack="true"
                                                    runat="server" /></legend>
                                            <br />
                                            <table cellspacing="3" cellpadding="0" border="0">
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        New Password
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:TextBox ID="txtNewPassword" Width="180px" CssClass="TextBox" Enabled="false"
                                                            TextMode="Password" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 150px;" nowrap="nowrap">
                                                        Confirm New Password
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" align="center" nowrap="nowrap">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap="nowrap">
                                                        <asp:TextBox ID="txtConfirmPassword" Width="180px" CssClass="TextBox" Enabled="false"
                                                            TextMode="Password" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </fieldset>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger EventName="ItemCommand" ControlID="tbrMain" />
                                        <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnSubmit" />
                                        <asp:AsyncPostBackTrigger EventName="CheckedChanged" ControlID="chkChangePassword" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                                    <ProgressTemplate>
                                        <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0">
                                            <tr>
                                                <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                                                    <br />
                                                    <br />
                                                    <img src="Images/spinner.gif" border="0" />
                                                    <br />
                                                    <br />
                                                    Please wait...
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <br />
                                <br />
                                <div align="center" style="display: none;">
                                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="Images/save2.png" />
                                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="Images/cancel.png" OnClientClick="window.location.href = 'Default.aspx';" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="height: 50px" />
            </td>
        </tr>
    </table>
</asp:Content>
