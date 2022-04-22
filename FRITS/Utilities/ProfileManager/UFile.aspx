<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UFile.aspx.vb" Inherits="FRITS.UFile"
    Theme="ProfileManager" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role</title>

    <script type="text/javascript" src="../../Resources/Scripts/global.js"></script>

    <script type="text/javascript">
		<!--	

		//-->
    </script>

</head>
<body class="dialog">
    <form id="frm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContentPane" runat="server">
        <div id="Toolbar" style="border-right: silver 0px solid; padding-right: 5px; border-top: silver 0px solid;
            padding-left: 5px; padding-bottom: 0px; border-left: silver 0px solid; padding-top: 0px;
            border-bottom: silver 0px solid; background-color: #FFFFFF;" align="right" runat="server">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" width="100%" height="25">
                        <ComponentArt:ToolBar ID="tbrMain" CssClass="h_toolbar" ImagesBaseUrl="~/Images/"
                            DefaultItemCssClass="item" DefaultItemHoverCssClass="itemHover" DefaultItemActiveCssClass="itemActive"
                            DefaultItemCheckedCssClass="itemChecked" DefaultItemCheckedHoverCssClass="itemActive"
                            DefaultItemTextImageRelation="ImageOnly" DefaultItemImageHeight="16" DefaultItemImageWidth="16"
                            ItemSpacing="1" Orientation="Horizontal" runat="server" AutoPostBackOnSelect="true"
                            AutoPostBackOnCheckChanged="true">
                            <Items>
                                <ComponentArt:ToolBarItem ItemType="Separator" Text="Help" ToolTip="Help" TextImageRelation="ImageOnly"
                                    ImageUrl="h_break.gif" ImageHeight="16" ImageWidth="2" />
                                <ComponentArt:ToolBarItem ToolTip="Help" ImageUrl="help.gif" />
                            </Items>
                        </ComponentArt:ToolBar>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-right: 15px; padding-left: 15px; padding-bottom: 15px; padding-top: 0px;">
            <table height="100%" cellspacing="0" cellpadding="0" width="100%" align="center"
                border="0">
                <tr>
                    <td valign="top" height="100%">
                        <table cellpadding="0" cellspacing="3" border="0">
                            <tr>
                                <td nowrap height="25">
                                    File
                                </td>
                                <td nowrap width="10" height="25" align="center">
                                    :
                                </td>
                                <td nowrap width="100%" height="25">
                                    <asp:FileUpload ID="txtFile" SkinID="FileUpload" Width="400" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap height="25">
                                    &nbsp;
                                </td>
                                <td nowrap width="10" height="25" align="center">
                                    &nbsp;
                                </td>
                                <td nowrap width="100%" height="25">
                                    <asp:Button ID="btnImport" runat="server" CssClass="Button" Text="Import" />
                                </td>
                            </tr>
                        </table>
                        <div align="center" id="processing" style="display: none;">
                            <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0" style="display: block;">
                                <tr>
                                    <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                                        <br />
                                        <br />
                                        <img alt="" src="../Images/spinner.gif" border="0" />
                                        <br />
                                        <br />
                                        Please wait...
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    </form>
</body>
</html>
