<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Pager.ascx.vb" Inherits="FRITS.Pager" %>
<div style="font-size:8pt; font-family:Verdana; margin-right:15px;">
      <div id="left" style="float:left;">
      <span>Display </span>
      <asp:DropDownList ID="ddlPageSize"
          runat="server"
          AutoPostBack="true" CssClass="DropDownList"
          OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
          <asp:ListItem Text="1" Value="1"></asp:ListItem>
          <asp:ListItem Text="5" Value="5"></asp:ListItem>
          <asp:ListItem Text="10" Value="10"></asp:ListItem>
          <asp:ListItem Text="20" Value="20"></asp:ListItem>
          <asp:ListItem Text="25" Selected="True" Value="25"></asp:ListItem>
          <asp:ListItem Text="50" Value="50"></asp:ListItem>
          <asp:ListItem Text="75" Value="75"></asp:ListItem>
          <asp:ListItem Text="100" Value="100"></asp:ListItem>
          <asp:ListItem Text="150" Value="150"></asp:ListItem>
          <asp:ListItem Text="200" Value="200"></asp:ListItem>
      </asp:DropDownList>
      <span> records per Page</span>
   </div>
    <div id="right" style="float:right;">
      <span>Showing Page </span>
      <asp:DropDownList ID="ddlPageNumber" runat="server"
         AutoPostBack="true" CssClass="DropDownList"
         OnSelectedIndexChanged="ddlPageNumber_SelectedIndexChanged">        
      </asp:DropDownList>
      <span> of</span>
      <asp:Label ID="lblShowRecords" Visible="true" ForeColor="Navy" runat="server"></asp:Label>
      <span> Pages</span>
   </div>
    
</div>