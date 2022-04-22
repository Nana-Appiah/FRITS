<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="ViewReviews.aspx.vb" Inherits="FRITS.ViewReviews" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Manage Reviews</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                            <tbody>                               

                                <tr>
                                    <td align="justify" valign="top" style="width: 100%">


                                        <table width="100%" cellspacing="5" cellpadding="0" style="align-self: center; margin: auto !important">

                                            <tr>
                                                <td style="align-items: flex-start; align-content: flex-start;">

                                                    <asp:Panel ID="cpFilter" runat="server" CssClass="cpHeader">
                                                        <asp:Label CssClass="pageTitle" ID="lblFilterLable" Text="" runat="server" />
                                                    </asp:Panel>
                                                    <asp:Panel ID="cpFilberBody" runat="server" CssClass="cpBody">
                                                        <br />
                                                        <asp:Panel ID="pnlFilterPanel" runat="server" GroupingText="Filter Option">

                                                            <table width="100%" cellspacing="5" cellpadding="0">

                                                                <tr>
                                                                    <td colspan="3">From :&nbsp
                                                                            <asp:TextBox runat="server" ID="txtFilterReviewFrom" Width="90px" Enabled="false" ValidationGroup="valFilterReview" CssClass="TextBox"></asp:TextBox>
                                                                        <asp:ImageButton runat="server" ID="ibtnFilterFrom" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                        <ajaxToolkit:CalendarExtender ID="ajtCalFilterFrom" runat="server" PopupButtonID="ibtnFilterFrom" Format="yyyy-MM-dd" TargetControlID="txtFilterReviewFrom" />

                                                                        To : &nbsp
                                                                             <asp:TextBox runat="server" ID="txtFilterReviewTo" Width="90px" Enabled="false" ValidationGroup="valFilterReview" CssClass="TextBox"></asp:TextBox>
                                                                        <asp:ImageButton runat="server" ID="ibtnFilterTo" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                        <ajaxToolkit:CalendarExtender ID="ajtCalFilterTo" runat="server" PopupButtonID="ibtnFilterTo" Format="yyyy-MM-dd" TargetControlID="txtFilterReviewTo" />

                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="3">
                                                                        <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td colspan="3" align="right">
                                                                        <asp:Button Width="90px" runat="server" ID="btnApplyReviewFilter" CssClass="Button1" Text="Apply" />
                                                                    </td>
                                                                </tr>

                                                            </table>

                                                        </asp:Panel>
                                                    </asp:Panel>
                                                    <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="cpFilberBody" CollapseControlID="cpFilter"
                                                        ExpandControlID="cpFilter" Collapsed="true" TextLabelID="lblFilterLable" CollapsedText="Filter Reviews"
                                                        ExpandedText="Filter Reviews" CollapsedSize="0"
                                                        ScrollContents="false"></ajaxToolkit:CollapsiblePanelExtender>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td runat="server" id="rwLine">
                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                    <br />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td runat="server" id="rwSearch">
                                                    Branch :
                                                        <asp:DropDownList DataValueField="Code" AutoPostBack="true" OnSelectedIndexChanged="cboReviewBranch_SelectedIndexChanged" DataTextField="Name" Width="210px" runat="server" ID="cboReviewBranch"></asp:DropDownList>
                                                    &nbsp;
                                                    Search :
                                                    <asp:DropDownList runat="server" ID="cboSearchOption" CssClass="DropDownList">
                                                                <asp:ListItem Value="0">By Code</asp:ListItem>
                                                                <asp:ListItem Value="1">By Description</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox runat="server" ID="txtSearchReview" CssClass="TextBox" Width="150px"></asp:TextBox>
                                                    <asp:Button CssClass="Button1" runat="server" ID="btnSearchReview" OnClick="btnSearchReview_Click" Height="21px" Width="65px" Text="Search" /> 
                                                </td>
                                            </tr>

                                        </table>

                                    </td>

                                </tr>

                                <tr>

                                    <td align="justify" valign="top" style="width: 100%">


                                        <asp:GridView ID="grdReviewAssignmentDetails" AutoGenerateSelectButton="false" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId"
                                            AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewAssignmentDetailsPageIndexChanging"
                                            SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc"
                                            FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                            <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#A1DCF2" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Review Code" ItemStyle-Width="60px" DataField="ReviewCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="Description" ItemStyle-Width="250px" DataField="ReviewName"></asp:BoundField>
                                                <asp:BoundField HeaderText="Reviewed By" ItemStyle-Width="250px" DataField="EmployeeName"></asp:BoundField>
                                                <asp:BoundField HeaderText="Branch" ItemStyle-Width="180px" DataField="BranchName"></asp:BoundField>
                                                <asp:BoundField HeaderText="From" ItemStyle-Width="80px" DataFormatString="{0:dd-MMM-yyyy}" DataField="ReviewFrom"></asp:BoundField>
                                                <asp:BoundField HeaderText="To" ItemStyle-Width="80px" DataFormatString="{0:dd-MMM-yyyy}" DataField="ReviewTo"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# IIf(Eval("IsSubmitted"), "Submitted", "Open") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton
                                                            CssClass="gridEditButton"
                                                            ID="btnViewReview"
                                                            OnClick="btnViewReview_Click"
                                                            CausesValidation="false"
                                                            runat="server" ToolTip="View"> View
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


