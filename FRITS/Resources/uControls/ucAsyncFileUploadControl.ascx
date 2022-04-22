<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucAsyncFileUploadControl.ascx.vb" Inherits="FRITS.ucAsyncFileUploadControl" %>


<script type="text/javascript">
    function uploadComplete(sender, args) {
        if (parseInt(args.get_length) <= 0)
            return;

        $get("<%=lblMesg.ClientID%>").innerHTML = "File Successfully Selected For Upload";
    }
 
    function uploadError(sender) {
        $get("<%=lblMesg.ClientID%>").innerHTML = "File upload failed.";
    }

    function validateUpload(sender) {
        var _fd = document.getElementById('<%= txtFileDescription.ClientID %>').value;
        if (_fd) {
            return true;
        }
        alert("Please provide file name to continue!");
        return false;
    }
</script>

<table style="width:100%" cellpadding="5" >

    <tr>
        <td>
            File Description : &nbsp;<asp:TextBox runat="server" Width="100%" CssClass="TextBox" TextMode="SingleLine" id="txtFileDescription"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <ajaxToolkit:AsyncFileUpload  ID="fuUploadAsync" OnClientUploadComplete="uploadComplete" OnClientUploadError="uploadError"  runat="server" Width="480px" UploaderStyle="Modern" CompleteBackColor = "White" UploadingBackColor="#CCFFFF"  ThrobberID="imgLoader" OnUploadedComplete = "FileUploadComplete" BackColor="#ffffff" />
            <asp:Image ID="imgLoader" runat="server" ImageUrl="~/Images/loading1.gif"/>
        </td>        
    </tr>   
    <tr>
        <td>
            <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>
        </td>
    </tr>

     <tr>
        <td align="left" >
            <asp:Button runat="server" Width="100px" OnClick="btnUploadFile_Click" OnClientClick="return validateUpload(this);" ID="btnUploadFile" ClientIDMode="AutoID" CssClass="Button1" Text="Upload File" />
        </td>
    </tr>
</table>

