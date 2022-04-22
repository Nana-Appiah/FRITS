<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="Default.aspx.vb" Inherits="FRITS._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">
        <tr>
            <td style="width: 100%" valign="top">
                <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                    <asp:Label runat="server" ID="lblAddFixedAssetItemTitle">Dashboard</asp:Label>
                </div>
                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                    <tbody>
                       
                        <tr>
                            <td>
                                <div class="lblnotifTitle" style="width:200px;">
                                    <p>Summaries</p>
                                </div>
                                <asp:Panel ID="Panel1" CssClass="dashPanel" runat="server">
                                    <table cellpadding="5" cellspacing="0" width="100%">
                                        <tbody>
                                            <tr>
                                                                                             
                                                <td align="right">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <div class="lblnotifTitle1">
                                                                        <p>Asset Summary</p>
                                                                    </div>
                                                                    <asp:Panel ClientIDMode="Static" ID="Panel4" ScrollBars="Auto" CssClass="dashPanel1" runat="server">

                                                                        <asp:GridView ID="grdAssetSummary" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="FixedAssetId"
                                                                            AllowPaging="false"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc"
                                                                            SortedDescendingHeaderStyle-CssClass=" sorteddesc"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="16" ShowHeader="true" ShowHeaderWhenEmpty="true" PageSize="50" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Asset Name" ItemStyle-Width="300px" DataField="FixedAssetName"></asp:BoundField>
                                                                                <asp:BoundField HeaderText="Total Items" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" DataField="ItmCount"></asp:BoundField>                                                                                
                                                                            </Columns>                                                                            
                                                                            <FooterStyle CssClass="footer"></FooterStyle>
                                                                            <SortedAscendingHeaderStyle CssClass="sortedasc"></SortedAscendingHeaderStyle>
                                                                            <SortedDescendingHeaderStyle CssClass="sorteddesc"></SortedDescendingHeaderStyle>
                                                                        </asp:GridView>

                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <div class="lblnotifTitle1">
                                                                        <p>Status Summary</p>
                                                                    </div>
                                                                    <asp:Panel ClientIDMode="Static" ID="Panel2" ScrollBars="Auto" CssClass="dashPanel1" runat="server">
                                                                        <asp:GridView ID="grdStatusSummary" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="AssetStatusId"
                                                                            AllowPaging="true"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc"
                                                                            SortedDescendingHeaderStyle-CssClass="sorteddesc"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="16" ShowHeader="true" ShowHeaderWhenEmpty="true" PageSize="50" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Status Name" ItemStyle-Width="300px" DataField="StatusName"></asp:BoundField>
                                                                                <asp:BoundField HeaderText="Total Assets" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" DataField="ItmCount"></asp:BoundField>                                                                                
                                                                            </Columns>                                                                            
                                                                            <FooterStyle CssClass="footer"></FooterStyle>
                                                                            <SortedAscendingHeaderStyle CssClass="sortedasc"></SortedAscendingHeaderStyle>
                                                                            <SortedDescendingHeaderStyle CssClass="sorteddesc"></SortedDescendingHeaderStyle>
                                                                            <PagerStyle ForeColor="Navy" />
                                                                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <div class="lblnotifTitle1">
                                                                        <p>Condition Summary</p>
                                                                    </div>
                                                                    <asp:Panel ClientIDMode="Static" ID="Panel3" ScrollBars="Auto" CssClass="dashPanel1" runat="server">

                                                                        <asp:GridView ID="grdConditionSummary" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="AssetConditionId"
                                                                            AllowPaging="false"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc"
                                                                            SortedDescendingHeaderStyle-CssClass="sorteddesc"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="16" ShowHeader="true" ShowHeaderWhenEmpty="true" PageSize="50" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Condition Name" ItemStyle-Width="300px" DataField="ConditionName"></asp:BoundField>
                                                                                <asp:BoundField HeaderText="Total Assets" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" DataField="ItmCount"></asp:BoundField>                                                                                
                                                                            </Columns>                                                                            
                                                                            <FooterStyle CssClass="footer"></FooterStyle>
                                                                            <SortedAscendingHeaderStyle CssClass="sortedasc"></SortedAscendingHeaderStyle>
                                                                            <SortedDescendingHeaderStyle CssClass="sorteddesc"></SortedDescendingHeaderStyle>
                                                                        </asp:GridView>

                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                 

                                </asp:Panel>
                                
                            </td>
                            <td style="width: 300px" align="right">
                                <div id="noticeBoard" class="lblnotifTitle">
                                    <p>Notifications</p>
                                </div>
                                <asp:Panel ClientIDMode="Static" ID="pnlNotifications" ScrollBars="Auto" CssClass="dashPanel" runat="server">

                                    <asp:DataList ID="dlNofications" runat="server" DataKeyField="Indx" RepeatColumns="1" CellSpacing="1" CellPadding="5"  RepeatLayout="Table">
                                        <ItemTemplate>
                                            <table class="table" cellspacing="2px" cellpadding="1">
                                                <tr>
                                                    <th style="color:#ff0000; margin:3px;" colspan="2">
                                                        <b><%# Eval("Title") %></b>
                                                    </th>
                                                </tr>                                                
                                                <tr>
                                                    <td>
                                                        <table cellspacing="1" cellpadding="1">
                                                            <tbody >
                                                                <tr>
                                                                    <td align="left"><b>Total Items :</b></td>
                                                                    <td align="left"><%# Eval("NotifCount")%></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="justify" colspan="2" style="line-height:17px; font-style:italic">
                                                                        <%# Eval("NotifDetail")%>
                                                                    </td>
                                                                </tr>                                                                
                                                            </tbody>                                                            
                                                        </table>
                                                        <hr class="panLine1" />                                                       
                                                    </td>
                                                </tr>                                                
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
