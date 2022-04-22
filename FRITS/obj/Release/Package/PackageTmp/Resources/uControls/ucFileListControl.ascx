<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucFileListControl.ascx.vb" Inherits="FRITS.ucFileListControl" %>

<asp:DataList DataKeyField="ReviewFileId" Width="100%" runat="server" ID="dtlFileList" OnItemCommand="dtlFileList_ItemCommand">
    <ItemTemplate>
        <table  runat="server" cellpadding="3" cellspacing="5" style="width:100%; border-bottom: 1px solid #05075f; text-align: justify;">
            <tr>
                <td style="width:75%">                   
                        <%# Eval("Description") %>                
                </td>
                <td style="width: 25%">
                    <%-- <asp:LinkButton ID = "lnkOpen" CommandName="OpenFile" Text = "Open"  runat = "server" />--%>
                    <asp:LinkButton ID="lnkDownload" CommandName="DownloadFile" ForeColor="Green" Text="Download" runat="server"></asp:LinkButton>
                    &nbsp;
                <asp:LinkButton ID="lnkDelete" CommandName="DeleteFile" ForeColor="Red" Text="Delete" runat="server" />
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:DataList>


