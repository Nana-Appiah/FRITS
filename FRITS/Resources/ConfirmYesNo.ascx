<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ConfirmYesNo.ascx.vb" Inherits="FAMS.ConfirmYesNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:LinkButton CssClass="gridEditButton" ID="lbtnConfirmButton" runat="server" CommandName="Confirm" OnClick="lbtnConfirm_Click">Delete</asp:LinkButton>

<ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btnAssignAssets"></ajaxToolkit:ConfirmButtonExtender>
<ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lbtnConfirmButton" OkControlID="btnYes"
    CancelControlID="btnNo" BackgroundCssClass="modalBackground">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Confirmation
    </div>
    <div class="body">
        Do you authorize the movement of this asset to the stated destination?
    </div>
    <div class="footer" style="align-items:center;">
        <asp:Button ID="btnYes" runat="server" Text="Yes" />
        <asp:Button ID="btnNo" runat="server" Text="No" />
    </div>
</asp:Panel>
