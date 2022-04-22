<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master"
    CodeBehind="AccessDenied.aspx.vb" Inherits="FRITS.AccessDenied" Title="PAN-AFRICAN FRITS | Access Denied" %>

<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">
        <tr>
            <td style="width: 5px" nowrap="nowrap">
            </td>
            <td style="width: 100%" valign="top">
                <div class="NavHeader" style="width: 180px">
                    Access Denied
                </div>
                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                    <tr>
                        <td style="width: 100%; height: 200px; vertical-align: top;" nowrap="nowrap">
                            <div style="padding-right: 5px; padding-left: 5px; padding-bottom: 10px; padding-top: 10px">
                                You do not have the permission to access this page or section of this application.
                                <br />
                                <br />
                                Please contact <strong>system</strong> administrator.
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
