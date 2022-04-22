<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout2.Master"
    CodeBehind="Settings.aspx.vb" Inherits="FRITS.Settings" Title="PAN-AFRICAN FRITS | Settings" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">
        <tr>
            <td style="width: 5px" nowrap="nowrap">
            </td>
            <td style="width: 100%" valign="top">
                <div class="NavHeader" style="width: 180px">
                    Settings
                </div>
                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                    <tr>
                        <td style="width: 100%; height: 200px; vertical-align: top;" nowrap="nowrap">
                           <div style="padding-right: 5px; padding-left: 5px; padding-bottom: 10px; padding-top: 10px">
                                <div id="Toolbar" align="right" runat="server">
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="center" width="100%" height="25">
                                                <ComponentArt:ToolBar ID="tbrMain" CssClass="h_toolbar" ImagesBaseUrl="../images/"
                                                    DefaultItemCssClass="item" DefaultItemHoverCssClass="itemHover" DefaultItemActiveCssClass="itemActive"
                                                    DefaultItemTextImageRelation="ImageOnly" DefaultItemImageHeight="16" DefaultItemImageWidth="16"
                                                    ItemSpacing="1" Orientation="Horizontal" runat="server" AutoPostBackOnSelect="true">
                                                    <Items>
                                                        <ComponentArt:ToolBarItem ItemType="Command" Text="Save" Value="Save" ToolTip="Save"
                                                            TextImageRelation="TextOnly" ImageUrl="saveitem.gif" AutoPostBackOnSelect="True" />
                                                        <ComponentArt:ToolBarItem ItemType="Separator" ImageUrl="h_break.gif" ImageHeight="16"
                                                            ImageWidth="2" Visible="false" AutoPostBackOnSelect="True" />
                                                        <ComponentArt:ToolBarItem ToolTip="Help" ImageUrl="help.gif" AutoPostBackOnSelect="True" />
                                                    </Items>
                                                </ComponentArt:ToolBar>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <fieldset>
                                            <legend>General</legend>
                                            <br />
                                            <table cellspacing="3" cellpadding="2" border="0" width="100%" align="center">
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        HTTP Server
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtHTTPServer" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        SMTP Server
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtSMTPServer" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Order Folder
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtOrderFolder" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Batch No.
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtBatchNo" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Batch Prefix
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtBatchPrefix" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </fieldset>
                                        <br />
                                        <br />
                                        <fieldset>
                                            <legend>Notification By Email</legend>
                                            <br />
                                            <table cellspacing="3" cellpadding="2" border="0" width="100%" align="center">
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Allow
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:CheckBox ID="chkAllowNotification" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Email Address
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtNotificationEmail" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px; width: 100px;" nowrap="nowrap">
                                                        Email Alias
                                                    </td>
                                                    <td style="height: 25px; width: 15px;" valign="center" nowrap align="center">
                                                        :
                                                    </td>
                                                    <td style="height: 25px" valign="center" nowrap align="left">
                                                        <asp:TextBox ID="txtNotificationEmailAlias" Width="300px" CssClass="TextBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </fieldset>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger EventName="ItemCommand" ControlID="tbrMain" />
                                        <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnSubmit" />
                                        <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnCancel" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                                    <ProgressTemplate>
                                        <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0">
                                            <tr>
                                                <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                                                    <img src="../Images/spinner.gif" border="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <br />
                                <br />
                                <div align="center" style="display: none;">
                                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="../Images/save2.png" />
                                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../Images/cancel.png" OnClientClick="window.location.href = '../Default.aspx';" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="height: 50px" />
    </div>
</asp:Content>
