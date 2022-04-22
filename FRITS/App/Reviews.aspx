<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="Reviews.aspx.vb" Inherits="FRITS.Reviews" %>

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
                                    <td align="justify" valign="top" style="width: 30%">
                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                            <div class="header">
                                                <asp:Label runat="server" ID="reviewTitle" Text="REVIEWS"></asp:Label>
                                            </div>
                                            <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
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
                                                        <td runat="server" id="rwSearch">Search :
                                                            <asp:DropDownList runat="server" ID="cboSearchOption" CssClass="DropDownList">
                                                                <asp:ListItem Value="0">By Code</asp:ListItem>
                                                                <asp:ListItem Value="1">By Description</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox runat="server" ID="txtSearchReview" CssClass="TextBox" Width="100px"></asp:TextBox>
                                                            <asp:Button CssClass="Button1" runat="server" OnClick="btnSearchReview_Click" ID="btnSearchReview" Height="21px" Width="65px" Text="Search" />  
                                                            <asp:Button Style="float: right;" OnClick="btnCreateReview_Click" Height="21px" runat="server" ID="btnCreateReview" CssClass="Button" Text="Create New" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="text-align: center;">


                                                            <asp:MultiView runat="server" ID="mlvReviews" ActiveViewIndex="0">

                                                                <asp:View runat="server" ID="vReviewList">


                                                                    <ajaxToolkit:TabContainer runat="server" Height="100%" Width="100%">

                                                                        <ajaxToolkit:TabPanel runat="server" HeaderText="Opened Reviews">

                                                                            <ContentTemplate>

                                                                                <asp:GridView ID="grdReviews" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                    CssClass="gridview resizable" AllowSorting="True" DataKeyNames="ReviewId" OnSelectedIndexChanged="grdReview_SelectedIndexChanged"
                                                                                    AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewPageIndexChanging"
                                                                                    SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdReview_RowDataBound"
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
                                                                                                    OnClick="lblAssignReview_Click"
                                                                                                    ID="lblAssignReview"
                                                                                                    ForeColor="#660066"
                                                                                                    CausesValidation="false"
                                                                                                    Font-Bold="true"
                                                                                                    runat="server" ToolTip="Assign To Employee">Assign
                                                                                                </asp:LinkButton>
                                                                                                &nbsp;
                                                                                    <asp:LinkButton
                                                                                        CssClass="gridEditButton"
                                                                                        ID="lblEditReview"
                                                                                        Font-Bold="true"
                                                                                        CausesValidation="false"
                                                                                        runat="server" ToolTip="Edit Review">Edit  &nbsp;
                                                                                    </asp:LinkButton>

                                                                                                <asp:LinkButton
                                                                                                    CssClass="gridDeleteButton"
                                                                                                    ID="lblDelReview"
                                                                                                    CausesValidation="false"
                                                                                                    Font-Bold="true"
                                                                                                    runat="server" ToolTip="Delete Review">Delete
                                                                                                </asp:LinkButton>
                                                                                                &nbsp;
                                                                                    <asp:LinkButton
                                                                                        CssClass="gridDeleteButton"
                                                                                        OnClick="lblCloseReview_Click"
                                                                                        ID="lblCloseReview"
                                                                                        ForeColor="#006600"
                                                                                        Font-Bold="true"
                                                                                        CausesValidation="false"
                                                                                        Visible='<%# IIf(Eval("IsClosed"), False, True).ToString() %>'
                                                                                        runat="server" ToolTip="Close">Close
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

                                                                            </ContentTemplate>

                                                                        </ajaxToolkit:TabPanel>

                                                                        <ajaxToolkit:TabPanel runat="server" HeaderText="Closed Reviews">

                                                                            <ContentTemplate>

                                                                                 <asp:GridView ID="grdClosedReviews" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                    CssClass="gridview resizable" AllowSorting="True" DataKeyNames="ReviewId" 
                                                                                    AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnClosedReviewsPageIndexChanging"
                                                                                    SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdReview_RowDataBound"
                                                                                    FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                                    <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                                    <SelectedRowStyle BackColor="#A1DCF2" />
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderText="Code" ItemStyle-Width="50px" DataField="ReviewCode"></asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Description" ItemStyle-Width="250px" DataField="Description"></asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Date Closed" ItemStyle-Width="80px" DataFormatString="{0:dd-MMM-yyyy}" DataField="ClosedDate"></asp:BoundField>                     
                                                                                         <asp:BoundField HeaderText="Closed By" ItemStyle-Width="150px" DataField="ClosedByName"></asp:BoundField>

                                                                                       <%-- <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                                            <ItemTemplate>

                                                                                    <asp:LinkButton
                                                                                        CssClass="gridDeleteButton"
                                                                                        OnClick="lblCloseReview_Click"
                                                                                        ID="lblCloseReview"
                                                                                        ForeColor="#006600"
                                                                                        Font-Bold="true"
                                                                                        CausesValidation="false"
                                                                                        Visible='<%# IIf(Eval("IsClosed"), False, True).ToString() %>'
                                                                                        runat="server" ToolTip="Close">Close
                                                                                    </asp:LinkButton>

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>--%>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <span class="gridviewNoRecord">No record(s) found!</span>
                                                                                    </EmptyDataTemplate>
                                                                                    <PagerStyle BackColor="White" ForeColor="#0033cc" />
                                                                                    <FooterStyle CssClass="footer"></FooterStyle>
                                                                                    <SortedAscendingHeaderStyle CssClass="sortedasc"></SortedAscendingHeaderStyle>
                                                                                    <SortedDescendingHeaderStyle CssClass="sorteddesc"></SortedDescendingHeaderStyle>
                                                                                </asp:GridView>









                                                                            </ContentTemplate>

                                                                        </ajaxToolkit:TabPanel>


                                                                    </ajaxToolkit:TabContainer>


                                                                </asp:View>


                                                                <asp:View ID="vAddEditReview" runat="server">

                                                                    <table style="width: 100%; text-align: center;">
                                                                        <tr>
                                                                            <td align="center">

                                                                                <asp:Panel Style="text-align: center !important;" Width="95%" CssClass="modalPopup" runat="server" ID="pnlAddEditReview">

                                                                                    <div id="pnlHeaderTitle" class="header">
                                                                                        <label>Add / Edit Review</label>
                                                                                    </div>

                                                                                    <div class="body">

                                                                                        <table style="text-align: center" width="90%" cellspacing="0" cellpadding="0">

                                                                                            <tr>
                                                                                                <td>Review Code</td>
                                                                                                <td style="width: 5px">:</td>
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" Enabled="false" Width="200px" ID="txtReviewCode" TextMode="SingleLine" Rows="5" CssClass="TextBox"></asp:TextBox>

                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td style="width: 5px"></td>
                                                                                                <td>
                                                                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtReviewCode" ValidationGroup="vgReview" ID="valReviewCode" Text="Code field is required!"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td>Description</td>
                                                                                                <td style="width: 5px">:</td>
                                                                                                <td style="align-content: center; align-items: center; text-align: center">
                                                                                                    <asp:TextBox runat="server" Width="200px" ID="txtReviewDescription" TextMode="SingleLine" Rows="5" CssClass="TextBox"></asp:TextBox>

                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td style="width: 5px"></td>
                                                                                                <td>
                                                                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtReviewDescription" ValidationGroup="vgReview" ID="valReviewDesc" Text="Description field is required!"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td colspan="2"></td>
                                                                                                <td style="align-content: center; align-items: center;">
                                                                                                    <asp:Button runat="server" ValidationGroup="vgReview" Width="100px" OnClick="btnSaveReview_Click" CssClass="Button1" ID="btnSaveReview" Text="Save Review" />
                                                                                                    <asp:Button runat="server" ValidationGroup="vgReview" Width="100px" OnClick="btnCancelReview_Click" CssClass="Button1" CausesValidation="false" ID="btnCancelReview" Text="Cancel" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                    </div>
                                                                                </asp:Panel>

                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </asp:View>

                                                            </asp:MultiView>

                                                        </td>

                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="footer">
                                            </div>

                                        </asp:Panel>
                                    </td>
                                    <td align="justify" valign="top" style="width: 70%">

                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                            <div class="header">
                                                <asp:Label runat="server" ID="findingTitle" Text="ASSIGNED TO"></asp:Label>
                                            </div>
                                            <div class="body" style="line-height: 18px !important; margin-top: 15px;">

                                                <asp:Label runat="server" CssClass="pageTitle" ID="lblSelectedReview"></asp:Label>

                                                <table width="100%" cellspacing="5" cellpadding="0" style="align-self: center; align-content: flex-start; align-items: flex-start; margin: auto !important">

                                                    <tr>
                                                        <td>
                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>

                                                        <td style="min-width: 100%;">


                                                            <ajaxToolkit:TabContainer runat="server" Height="100%" Width="100%">

                                                                <ajaxToolkit:TabPanel runat="server" HeaderText="Assignment Details">

                                                                    <ContentTemplate>

                                                                        <asp:GridView ID="grdReviewAssignmentDetails" AutoGenerateSelectButton="false" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId"
                                                                            AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewAssignmentDetailsPageIndexChanging"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdReviewAssignmentDetails_RowDataBound"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <SelectedRowStyle BackColor="#A1DCF2" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Employee Name" ItemStyle-Width="250px" DataField="EmployeeName"></asp:BoundField>
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

                                                                    </ContentTemplate>

                                                                </ajaxToolkit:TabPanel>


                                                            </ajaxToolkit:TabContainer>

                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                            <div class="footer">
                                            </div>

                                        </asp:Panel>

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
