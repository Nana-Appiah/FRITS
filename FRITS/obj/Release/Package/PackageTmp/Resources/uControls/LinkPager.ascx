<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LinkPager.ascx.vb" Inherits="FRITS.LinkPager" %>
<div style="font-size: 8pt; font-family: Verdana; width: 100%">
    <table cellspacing="3" cellpadding="1" align="left" style="text-align: left; float: left; width: 100%">
        <tbody>
            <tr>
                <td align="left" style="width: 220px; margin-right:20px">
                    <div id="left1" style="float: left;">
                        <span>Display </span>
                        <asp:DropDownList ID="ddlPageSize"
                            runat="server"
                            AutoPostBack="true" CssClass="DropDownList"
                            OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                            <asp:ListItem Text="1"  Value="1"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="25" Selected="True" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50"  Value="50"></asp:ListItem>
                            <asp:ListItem Text="75"  Value="75"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="150" Value="150"></asp:ListItem>
                            <asp:ListItem Text="500" Value="500"></asp:ListItem>
                             <asp:ListItem Text="1000" Value="1000"></asp:ListItem>
                             <asp:ListItem Text="1500" Value="1500"></asp:ListItem>
                        </asp:DropDownList>
                        <span>records per page</span>
                          
                    </div>
                </td>                            
                <td align="left" style="width:260px"> 
                        <span>Page</span>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <span>of</span>
                        <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                    <span> (</span>
                        <asp:Label ID="lblTotalItems" runat="server"></asp:Label>
                    <span> Items)</span>                    
                </td>                               
                <td align="right" style="width:auto">
                    <div runat="server" id="right" style="width:auto;">
                        <span>Pages : </span>[
                        <asp:Repeater ID="rptPages" runat="server" OnItemDataBound="rptPages_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPageNumbers" runat="server" OnClick="lnkPageNumbers_Click"></asp:LinkButton>
                                <asp:Label ID="lblPageNumbers" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:Repeater>]
                    </div>
                </td>
                <td align="right" style="width:290px">
                    <table cellspacing="3" cellpadding="1" align="right" style="text-align: right; float: right; width: auto">
                        <tbody>
                            <tr>
                                <td align="right" style="text-align: right;">
                                    <asp:LinkButton ID="lnkFirstPage" runat="server" Text="<< First" CssClass="pagerFirst" OnClick="lnkGOFPage_Click"></asp:LinkButton>
                                    <asp:Label ID="lblFirstPage" runat="server" CssClass="pagerFirst" Text="<< First"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkPreviousPage" runat="server" Text="< Previous" CssClass="pagerPrevious" OnClick="lnkGOFPage_Click"></asp:LinkButton>
                                    <asp:Label ID="lblPreviousPage" runat="server" CssClass="pagerPrevious" Text="< Previous"></asp:Label>
                                </td>

                                <td>
                                    <asp:LinkButton ID="lnkNextPage" CssClass="pagerNext" runat="server" Text="Next >" OnClick="lnkGOFPage_Click"></asp:LinkButton>
                                    <asp:Label ID="lblNextPage" runat="server" CssClass="pagerNext" Text="Next >"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLast" runat="server" Text="Last >>" OnClick="lnkGOFPage_Click"></asp:LinkButton>
                                    <asp:Label ID="lblLastPage" runat="server" CssClass="pagerLast" Text="Last >>"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>


</div>
