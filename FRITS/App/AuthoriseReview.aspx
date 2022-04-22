<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="AuthoriseReview.aspx.vb" Inherits="FRITS.AuthoriseReview" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Authorise Reviews</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">

                            <tbody>

                                <tr>
                                    <td align="justify" valign="top" style="width: 100%">

                                        <table width="100%" cellspacing="5" cellpadding="0" style="align-self: center; margin: auto !important">

                                            <tr>
                                                <td runat="server" id="rwSearch">Search :
                                                            <asp:TextBox runat="server" ID="txtSearchReview" CssClass="TextBox" Width="150px"></asp:TextBox>
                                                    <asp:Button CssClass="Button1" runat="server" ID="btnSearchReview" Height="21px" Width="65px" Text="Search" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="text-align: center;">

                                                    <asp:GridView ID="grdReviews" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="gridview resizable" AllowSorting="True" DataKeyNames="ReviewId"
                                                        AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewPageIndexChanging"
                                                        SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc"
                                                        FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                        <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                        <SelectedRowStyle BackColor="#A1DCF2" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Code" ItemStyle-Width="15%" DataField="ReviewCode"></asp:BoundField>
                                                            <asp:BoundField HeaderText="Description" ItemStyle-Width="50%" DataField="Description"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                <ItemTemplate>

                                                                    <asp:LinkButton
                                                                        CssClass="gridEditButton"
                                                                        ID="lbtnAuthoriseReview"
                                                                        OnClick="lbtnAuthoriseReview_Click"
                                                                        CausesValidation="false"
                                                                        runat="server" ToolTip="Authorise Review"> Authorise
                                                                    </asp:LinkButton>

                                                                    <asp:LinkButton
                                                                        CssClass="gridDeleteButton"
                                                                        ID="lblDelReview"
                                                                        CausesValidation="false"
                                                                        runat="server" ToolTip="Delete Review">Delete
                                                                    </asp:LinkButton>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <span class="gridviewNoRecord">No record(s) found!</span>
                                                        </EmptyDataTemplate>
                                                        <PagerStyle BackColor="White" ForeColor="#0033cc" />
                                                        <FooterStyle CssClass="footer"></FooterStyle>
                                                        <SortedAscendingHeaderStyle CssClass="sortedasc"></SortedAscendingHeaderStyle>
                                                        <SortedDescendingHeaderStyle CssClass="sorteddesc"></SortedDescendingHeaderStyle>
                                                    </asp:GridView>
                                                </td>

                                            </tr>

                                        </table>

                                    </td>
                                </tr>


                            </tbody>
                        </table>
                    </td>
                </tr>

            </table>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="prgLoadingProgress" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div class="upLoading">
                <div class="upAlign" style="align-content: center; align-self: center; align-items: center">

                    <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="Middle" ImageUrl="~/Images/loading5.gif" />

                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



</asp:Content>
