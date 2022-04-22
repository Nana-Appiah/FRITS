<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="RecordReview.aspx.vb" Inherits="FRITS.RecordReview" %>

<%@ Register Src="~/Resources/uControls/ucAsyncFileUploadControl.ascx" TagPrefix="uc1" TagName="ucAsyncFileUploadControl" %>
<%@ Register Src="~/Resources/uControls/ucFileListControl.ascx" TagPrefix="uc1" TagName="ucFileListControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function cancelObservation(msg) {
            return getConfirmation(msg)
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">




    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Incident Reviews</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">


                            <tbody>

                                <tr>
                                    <td colspan="2">
                                        <asp:Panel runat="server" GroupingText="Branch">

                                            <table width="100%" cellspacing="5" cellpadding="0">
                                                <tr>
                                                    <td>Branch :
                                                        <asp:DropDownList DataValueField="Code" CssClass="DropDownList" AutoPostBack="true" OnSelectedIndexChanged="cboReviewBranch_SelectedIndexChanged" DataTextField="Name" Width="220px" runat="server" ID="cboReviewBranch"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>

                                        </asp:Panel>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="justify" valign="top" style="width: 35%">
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
                                                                <asp:Panel runat="server" GroupingText="Filter Option">

                                                                    <table width="100%" cellspacing="5" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="3">From :&nbsp
                                                                            <asp:TextBox runat="server" ID="txtFilterReviewFrom" Width="115px" Enabled="false" ValidationGroup="valFilterReview" CssClass="TextBox"></asp:TextBox>
                                                                                <asp:ImageButton runat="server" ID="ibtnFilterFrom" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                                <ajaxToolkit:CalendarExtender ID="ajtCalFilterFrom" runat="server" PopupButtonID="ibtnFilterFrom" Format="yyyy-MM-dd" TargetControlID="txtFilterReviewFrom" />

                                                                                To : &nbsp
                                                                             <asp:TextBox runat="server" ID="txtFilterReviewTo" Width="115px" Enabled="false" ValidationGroup="valFilterReview" CssClass="TextBox"></asp:TextBox>
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
                                                                                <asp:Button Width="150px" runat="server" ID="btnApplyReviewFilter" CssClass="Button1" Text="Apply Filter" />
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
                                                        <td>
                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>    
                                                        <td>Search :
                                                            <asp:DropDownList runat="server" ID="cboSearchOption" CssClass="DropDownList">
                                                                <asp:ListItem Value="0">By Code</asp:ListItem>
                                                                <asp:ListItem Value="1">By Description</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox runat="server" ID="txtSearchReview" CssClass="TextBox" Width="120px"></asp:TextBox>
                                                            <asp:Button CssClass="Button1" runat="server" OnClick="btnSearchReview_Click" ID="btnSearchReview" Height="21px" Width="65px" Text="Search" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="width:100% !important">


                                                            <ajaxToolkit:TabContainer runat="server" Height="100%" Width="100%">

                                                                <ajaxToolkit:TabPanel runat="server" HeaderText="Un-Submitted">

                                                                    <ContentTemplate>

                                                                        <asp:GridView ID="grdReviews" AutoGenerateSelectButton="false" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId"
                                                                            AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewPageIndexChanging"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdReview_RowDataBound"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <SelectedRowStyle BackColor="#A1DCF2" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Code" ItemStyle-Width="15%" DataField="ReviewCode"></asp:BoundField>
                                                                                <asp:BoundField HeaderText="Description" ItemStyle-Width="50%" DataField="ReviewName"></asp:BoundField>
                                                                                <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton
                                                                                            CssClass="gridEditButton"
                                                                                            ID="lblRecordFindings"
                                                                                            OnClick="lblRecordFindings_Click"
                                                                                            CausesValidation="false"
                                                                                            Font-Bold="true"
                                                                                            runat="server" ToolTip="Record Findings">Record Findings
                                                                                        </asp:LinkButton>
                                                                                        &nbsp;
                                                                            <asp:LinkButton
                                                                                CssClass="gridEditButton"
                                                                                ID="lblSubmitReview"
                                                                                OnClick="lblSubmitReview_Click"
                                                                                CausesValidation="false" ForeColor="Green"
                                                                                 Font-Bold="true"
                                                                                Visible='<%# IIf(Eval("IsSubmitted"), False, True).ToString() %>'
                                                                                runat="server" ToolTip="Submit Review To Supervisor">Submit
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

                                                                <ajaxToolkit:TabPanel runat="server" HeaderText="Submitted">

                                                                    <ContentTemplate>



                                                                           <asp:GridView ID="grdSubmittedReviews" AutoGenerateSelectButton="false" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId"
                                                                            AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnSubmittedReviewsPageIndexChanging"
                                                                            SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdReview_RowDataBound"
                                                                            FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                            <SelectedRowStyle BackColor="#A1DCF2" />
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Code" ItemStyle-Width="15%" DataField="ReviewCode"></asp:BoundField>
                                                                                <asp:BoundField HeaderText="Description" ItemStyle-Width="50%" DataField="ReviewName"></asp:BoundField>
                                                                                <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px"> 
                                                                                     <ItemTemplate>
                                                                                        <asp:LinkButton
                                                                                            CssClass="gridEditButton"
                                                                                            ID="lblUnSubmitReview"
                                                                                            OnClick="lblUnSubmitReview_Click"
                                                                                            CausesValidation="false" ForeColor="Red"
                                                                                            Visible='<%# IIf(Eval("IsSubmitted"), True, False).ToString() %>'
                                                                                            runat="server" ToolTip="Submit Review To Supervisor">Un-Submit
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
                                    <td align="justify" valign="top" style="width: 65%">

                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                            <div class="header">
                                                <asp:Label runat="server" ID="findingTitle" Text="REVIEW FINDINGS"></asp:Label>
                                            </div>
                                            <div class="body" style="line-height: 18px !important; margin-top: 15px;">


                                                <asp:Label runat="server" CssClass="pageTitle" ID="lblSelectedReview" Text="No Selected Review Found!"></asp:Label>


                                                <table width="100%" cellspacing="5" cellpadding="0" style="align-self: center; align-content: flex-start; align-items: flex-start; margin: auto !important">

                                                    <tr>
                                                        <td>
                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>

                                                        <td style="min-width: 100%;">

                                                            <asp:MultiView runat="server" ID="mvFindings" ActiveViewIndex="0">

                                                                <asp:View runat="server" ID="vFindingList">

                                                                    <table width="100%" cellspacing="5" cellpadding="0">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <asp:Button runat="server" ID="btnAddNewFinding" Enabled='false'  Visible="false" OnClick="btnAddNewFinding_Click" Width="180px" CssClass="Button" Text="Add Finding" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>

                                                                                    <asp:DataList Width="100%" DataKeyField="FindingId" runat="server" ID="dlFindingSummary" CellSpacing="3" RepeatLayout="Table" RepeatColumns="1" OnItemCommand="dlFindingSummary_ItemCommand" ShowFooter="false">

                                                                                        <FooterTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td valign="middle" align="center" style="width: 100%">
                                                                                                        <asp:Label runat="server" Font-Bold="true" ForeColor="Navy" Width="100%" Text="No Finding Record(s) Found!"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </FooterTemplate>

                                                                                        <ItemTemplate>
                                                                                            <table width="100%" cellpadding="3" cellspacing="3" style="border-bottom: 1px solid #05075f">
                                                                                                <tr>
                                                                                                    <td style="width: 85%">
                                                                                                        <table cellpadding="2" cellspacing="2" class="table">
                                                                                                            <tr>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Comment/Finding No
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Font-Bold="true" Text='<%# Eval("FindingNo") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td></td>

                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Review Area:
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" Font-Bold="true" ForeColor="#003399" Text='<%# Eval("Description") %>'></asp:Label>
                                                                                                                </td>

                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Risk Level
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Text='<%# Eval("RiskLevelName") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Risk Category
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Text='<%# Eval("RiskCategoryName") %>'></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Risk Sub-Category
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Text='<%# Eval("RiskSubCategory") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Management Awareness
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Text='<%# Eval("ManagementAwareness") %>'></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;">Total Observations
                                                                                                                </td>
                                                                                                                <td valign="top" align="center">:</td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Text='<%# Eval("ObservationCount") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                                <td valign="top" align="left" style="width: 125px; color: #c80d0d; font-style: italic; font-weight: bold;"></td>
                                                                                                                <td></td>
                                                                                                                <td></td>
                                                                                                            </tr>



                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td valign="middle" style="border-left: 1px #808080 solid;">

                                                                                                        <table width="100%" cellspacing="8" cellpadding="5">
                                                                                                            <tr>
                                                                                                                <td valign="middle">
                                                                                                                    <asp:LinkButton runat="server" ID="lbtnViewFinding" Text="View/Edit" CssClass="gridEditButton" CommandName="ViewFinding" CommandArgument='<%# Eval("FindingNo") %>' ToolTip="View Finding" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="middle">
                                                                                                                    <asp:LinkButton runat="server" CommandName="RemoveFinding" ClientIDMode="AutoID" ID="lbtnRemoveFinding" Text="Remove" ForeColor="Red" CommandArgument='<%# Eval("FindingNo") %>' Font-Bold="true" ToolTip="Remove Finding" Font-Italic="true"></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="middle">
                                                                                                                    <asp:LinkButton runat="server" CommandName="UploadFindingFile" ClientIDMode="AutoID" ID="lblUploadFindingFile" Text="Upload File" ForeColor="#009999" Font-Bold="true" Visible='<%# Show_Upload_Button(Eval("FindingId")) %>' CommandArgument='<%# Eval("FindingId") %>' ToolTip="Upload File" Font-Italic="true"></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td valign="middle">
                                                                                                                    <asp:LinkButton runat="server" ID="lbtnViewFiles" ClientIDMode="AutoID" CommandName="ViewFindingFiles" Text="View Files" ForeColor="#660066" Font-Bold="true" Visible='<%# Show_Upload_Button(Eval("FindingId")) %>' CommandArgument='<%# Eval("FindingId") %>' ToolTip="View Finding Related Files" Font-Italic="true"></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                        </table>

                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>

                                                                                    </asp:DataList>

                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>

                                                                </asp:View>

                                                                <asp:View runat="server" ID="vAddEditFinding">

                                                                    <table width="100%" cellspacing="5" cellpadding="0">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Finding No</td>
                                                                                <td valign="top">:</td>
                                                                                <td colspan="5" align="left" style="text-align: left;">
                                                                                    <asp:TextBox ReadOnly="true" Text='<%# _findingRecord.FindingNo %>' Enabled="false" Width="230px" ValidationGroup="vgFinding" runat="server" ID="txtFindingNo" TextMode="SingleLine" CssClass="TextBox"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Review Area</td>
                                                                                <td valign="top">:</td>
                                                                                <td colspan="5">
                                                                                    <asp:TextBox ValidationGroup="vgFinding" Text='<%# _findingRecord.Description %>' runat="server" Width="100%" ID="txtFndReviewArea" TextMode="SingleLine" Rows="5" CssClass="TextBox"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Risk Level</td>
                                                                                <td valign="top">:</td>
                                                                                <td align="left">
                                                                                    <asp:DropDownList Width="230px" DataTextField="Description" DataValueField="RiskLevelId" ValidationGroup="vgFinding" runat="server" ID="cboRiskLevel"></asp:DropDownList>

                                                                                </td>
                                                                                <td style="width: 25px"></td>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Risk Category</td>
                                                                                <td valign="top">:</td>
                                                                                <td>
                                                                                    <asp:DropDownList DataTextField="RiskCategoryDesc" DataValueField="RiskCategoryId" ValidationGroup="vgFinding" Width="230px" runat="server" ID="cboRiskCategory"></asp:DropDownList></td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Risk Sub-Category</td>
                                                                                <td valign="top">:</td>
                                                                                <td align="left">

                                                                                    <ajaxToolkit:ComboBox ID="cboRiskSubCategory" ValidationGroup="vgFinding" Width="200px" runat="server" AutoCompleteMode="Suggest" DataTextField="Value" DataValueField="Key" AppendDataBoundItems="true"></ajaxToolkit:ComboBox>
                                                                                </td>
                                                                                <td style="width: 25px"></td>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Management Awareness</td>
                                                                                <td valign="top">:</td>
                                                                                <td>

                                                                                    <ajaxToolkit:ComboBox DataTextField="Value" DataValueField="Key" ID="cboManageAwareness" ValidationGroup="vgFinding" Width="200px" runat="server" AutoCompleteMode="Suggest" AppendDataBoundItems="true"></ajaxToolkit:ComboBox>

                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="7"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" valign="top" style="width: 110px; color: #05075f">Observations</td>
                                                                                <td valign="top">:</td>
                                                                                <td colspan="5">

                                                                                    <asp:Panel runat="server" Width="100%" CssClass="modalPopup1" BorderWidth="1px" BorderColor="Navy" BorderStyle="Dotted" ID="pnlFndObservation">
                                                                                        <div class="header red">
                                                                                            <asp:Label runat="server" ID="lblObservationPanelTitle" Text="Observation List"></asp:Label>
                                                                                        </div>

                                                                                        <div class="body">

                                                                                            <table width="100%" cellspacing="5" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td>

                                                                                                        <asp:MultiView runat="server" ID="mlvFndObservation" ActiveViewIndex="1">

                                                                                                            <asp:View runat="server" ID="vFndObservationList">

                                                                                                                <table width="100%" cellspacing="2" cellpadding="0">
                                                                                                                    <tr>
                                                                                                                        <td align="left">
                                                                                                                            <asp:Label Style="text-align: left !important" runat="server" Font-Size="Small" ForeColor="Navy" Text="List Of Observation(s)"></asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="right">
                                                                                                                            <asp:Button Width="150px" runat="server" ID="btnAddFndObservation" OnClick="btnAddFndObservation_Click" CssClass="Button1" Height="21px" Text="Add Observation" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td colspan="2">
                                                                                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>

                                                                                                                <asp:DataList DataKeyField="ObservationId" runat="server" ID="dtlFndObservationList" OnItemCommand="dtlFndObservationList_ItemCommand">


                                                                                                                    <ItemTemplate>
                                                                                                                        <table runat="server" cellpadding="3" cellspacing="5" style="border-bottom: 1px solid #05075f; text-align: justify;">
                                                                                                                            <tr>
                                                                                                                                <td style="width: 85%;">
                                                                                                                                    <table width="100%">
                                                                                                                                        <tr>
                                                                                                                                            <td valign="top" style="width: 15%; color: #05075f; font-style: italic; font-weight: bold;">Description:
                                                                                                                                            </td>
                                                                                                                                            <td style="width: 85%">
                                                                                                                                                <asp:Label runat="server" ID="lblFndObsRecTitle" Text='<%# Eval("Description")  %>'></asp:Label>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td valign="top" style="width: 15%; font-style: italic; font-weight: bold; color: #05075f;">Root Cause:
                                                                                                                                            </td>
                                                                                                                                            <td style="width: 85%">
                                                                                                                                                <asp:Label runat="server" ID="Label1" Text='<%# Eval("RootCauseAnalysis")  %>'></asp:Label>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td valign="top" style="width: 15%; font-style: italic; font-weight: bold; color: #05075f;">Implication:
                                                                                                                                            </td>
                                                                                                                                            <td style="width: 85%">
                                                                                                                                                <asp:Label runat="server" ID="Label2" Text='<%# Eval("Implication")  %>'></asp:Label>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td valign="top" style="width: 15%; font-style: italic; font-weight: bold; color: #05075f;">Assumptions:
                                                                                                                                            </td>
                                                                                                                                            <td style="width: 85%">
                                                                                                                                                <asp:Label runat="server" ID="Label3" Text='<%# Eval("Assumptions")  %>'></asp:Label>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td valign="top" style="width: 15%; font-style: italic; font-weight: bold; color: #05075f;">Mitigating Control:
                                                                                                                                            </td>
                                                                                                                                            <td style="width: 85%">
                                                                                                                                                <asp:Label runat="server" ID="Label4" Text='<%# Eval("MitigatingControl")  %>'></asp:Label>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </td>

                                                                                                                                <td valign="top" style="width: 15%; border-left: 1px #808080 solid;">

                                                                                                                                    <table width="100%">
                                                                                                                                        <tr>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <asp:LinkButton runat="server" ID="lbtnFndViewObservation" Text="View/Edit" CssClass="gridEditButton" CommandArgument='<%# Eval("ObservationNo") %>' CommandName="ViewObservation" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <asp:LinkButton runat="server" CommandName="RemoveObservation" ID="lblFndRemoveObservation" CommandArgument='<%# Eval("ObservationNo") %>' Text="Remove" ForeColor="Red" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td valign="middle">

                                                                                                                                                <asp:LinkButton runat="server" CommandName="UploadObservationFile" ID="lblUploadObservationFile" CommandArgument='<%# Eval("ObservationNo") %>' Text="Upload File" Visible='<%# Show_Upload_Button(Eval("ObservationId")) %>' ForeColor="#009999" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>

                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:DataList>

                                                                                                            </asp:View>

                                                                                                            <asp:View runat="server" ID="vFndAddObservation">

                                                                                                                <table width="100%" cellspacing="5" cellpadding="0">

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" title="Description of Control Weakness (New or Re-Open)" style="width: 110px; color: #05075f">Observation No</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ReadOnly="True" Enabled="False" runat="server" Width="230px" Height="100%" ID="txtFndObservationNo" Text='<%# _observationRecord.ObservationNo %>' CssClass="TextBox"></asp:TextBox>
                                                                                                                        </td>


                                                                                                                        <td align="left" valign="top" title="Observation Date" style="width: 110px; color: #05075f">Observation Date</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5" align="left" style="text-align: left">
                                                                                                                            <asp:TextBox runat="server" ID="txtFndObservationDate" Width="130px" Enabled="false" ValidationGroup="vgValObservation" Text='<%# _observationRecord.ObservationDate %>' CssClass="TextBox"></asp:TextBox>
                                                                                                                            <asp:ImageButton runat="server" ID="ibtnFndObsDate" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                                                                            <ajaxToolkit:CalendarExtender ID="calFndObservationDate" runat="server" PopupButtonID="ibtnFndObsDate" Format="yyyy-MM-dd" TargetControlID="txtFndObservationDate" />

                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" title="Description of Control Weakness (New or Re-Open)" style="width: 110px; color: #05075f">Description</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">
                                                                                                                            <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtFndObservationDescription" TextMode="MultiLine" Text='<%# _observationRecord.Description %>' Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Root Cause Analysis</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">
                                                                                                                            <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtFndObservationRootCauseAnalysis" TextMode="MultiLine" Text='<%# _observationRecord.RootCauseAnalysis %>' Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f"></td>
                                                                                                                        <td valign="top"></td>
                                                                                                                        <td colspan="5">

                                                                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtFndObservationRootCauseAnalysis" ValidationGroup="vgValObservation" ID="RequiredFieldValidator1" Text="Implication field is required!"></asp:RequiredFieldValidator>

                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Implication</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">
                                                                                                                            <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtFndObservationImplication" TextMode="MultiLine" Text='<%# _observationRecord.Implication %>' ValidationGroup="vgValObservation" Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f"></td>
                                                                                                                        <td valign="top"></td>
                                                                                                                        <td colspan="5">

                                                                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtFndObservationImplication" ValidationGroup="vgValObservation" ID="valtxtFndObservationImplication" Text="Implication field is required!"></asp:RequiredFieldValidator>

                                                                                                                        </td>
                                                                                                                    </tr>


                                                                                                                    
                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Assumptions</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">
                                                                                                                            <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtFndObservationAssumptions" TextMode="MultiLine" Text='<%# _observationRecord.Assumptions %>' ValidationGroup="vgValObservation" Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Mitigating Control</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">
                                                                                                                            <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtFndObservationMitigationControl" TextMode="MultiLine" Text='<%# _observationRecord.MitigatingControl %>' ValidationGroup="vgValObservation" Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>



                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Current Status</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5" align="left" style="text-align: left">
                                                                                                                            <asp:DropDownList Width="230px" DataTextField="Description" DataValueField="ObservationStatusId" runat="server" ValidationGroup="vgValObservation" ID="cboFndObservationStatus"></asp:DropDownList>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td align="left" valign="top" style="width: 110px; color: #05075f">Recommendation(s)</td>
                                                                                                                        <td valign="top">:</td>
                                                                                                                        <td colspan="5">

                                                                                                                            <asp:MultiView runat="server" ID="mlvFndRecomms" ActiveViewIndex="1">
                                                                                                                                <asp:View runat="server" ID="vFndRecommsList">

                                                                                                                                    <table style="border: 1px #808080 solid" width="100%" cellspacing="5" cellpadding="0">
                                                                                                                                        <tr>
                                                                                                                                            <td align="left">
                                                                                                                                                <asp:Label Style="text-align: left !important" runat="server" Font-Size="Small" ForeColor="Navy" Text="List Of Recommendation(s)"></asp:Label>
                                                                                                                                            </td>
                                                                                                                                            <td align="right">
                                                                                                                                                <asp:Button Width="150px" runat="server" ID="btnAddFndRecomm" OnClick="btnAddFndRecomm_Click" CssClass="Button1" Height="21px" Text="Add Recommendation" />
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td colspan="2">
                                                                                                                                                <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td colspan="2">
                                                                                                                                                <asp:DataList runat="server" DataKeyField="RecommendationId" ID="dtlFndRecommList" RepeatColumns="1" CellSpacing="5" CellPadding="3" OnItemCommand="dtlFndRecommList_ItemCommand" AlternatingItemStyle-VerticalAlign="Top" AlternatingItemStyle-HorizontalAlign="Justify" RepeatLayout="Table" Width="100%">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        <table width="100%" cellpadding="1" cellspacing="2" style="border-bottom: 1px solid #05075f">
                                                                                                                                                            <tr>
                                                                                                                                                                <td style="width: 95%" align="left" valign="top">
                                                                                                                                                                    <asp:Label runat="server" Width="100%" ForeColor="#207526" Text='<%# Eval("Description") %>'></asp:Label>
                                                                                                                                                                    <p runat="server"></p>
                                                                                                                                                                </td>
                                                                                                                                                                <td align="right" valign="top" style="border-left: 1px #808080 solid;">
                                                                                                                                                                    <asp:LinkButton Font-Bold="true" Font-Italic="true" Text="Remove" CssClass="gridDeleteButton" runat="server" ForeColor="DarkRed" ToolTip="Remove Recommendation" ID="lbtnRemoveRecommendation" CommandName="RemoveItem"></asp:LinkButton>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                </asp:DataList>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>

                                                                                                                                </asp:View>
                                                                                                                                <asp:View runat="server" ID="vFndAddRecomms">

                                                                                                                                    <asp:Panel Width="100%" CssClass="modalPopup" runat="server" ID="pnlFndRecomms">

                                                                                                                                        <div class="header">
                                                                                                                                            <asp:Label runat="server" ID="lblRecommendationPanelTitle" Text="List of Observation Recommendations"></asp:Label>
                                                                                                                                        </div>

                                                                                                                                        <div class="body">

                                                                                                                                            <table width="100%" cellspacing="5" cellpadding="0">
                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <asp:TextBox runat="server" Width="98%" Height="100%" ID="txtRecommendation" ValidationGroup="vgValRecomm" TextMode="MultiLine" Rows="5" CssClass="MultiLineTextBox"></asp:TextBox>
                                                                                                                                                    </td>
                                                                                                                                                </tr>

                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtRecommendation" ValidationGroup="vgValRecomm" ID="valtxtRecommendation" Text="Recommendation field is required!"></asp:RequiredFieldValidator>
                                                                                                                                                    </td>
                                                                                                                                                </tr>


                                                                                                                                                <tr>
                                                                                                                                                    <td align="right">
                                                                                                                                                        <asp:Button runat="server" CssClass="Button1" ID="btnSaveFindingRecomms" CausesValidation="true" OnClick="btnSaveFindingRecomms_Click" ValidationGroup="vgValRecomm" Text="Save Recommendation" />
                                                                                                                                                        <asp:Button runat="server" CssClass="Button1" ID="btnCancelFindingRecomms" CausesValidation="false" OnClick="btnCancelFindingRecomms_Click" Text="Cancel" />
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>


                                                                                                                                        </div>
                                                                                                                                    </asp:Panel>

                                                                                                                                </asp:View>

                                                                                                                            </asp:MultiView>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td colspan="8">
                                                                                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td colspan="8" align="center">
                                                                                                                            <asp:Button runat="server" CssClass="Button1" Width="120px" Text="Save Observation" ID="btnFndSaveObservation" ValidationGroup="vgValObservation" OnClick="btnFndSaveObservation_Click" />
                                                                                                                            <asp:Button runat="server" CssClass="Button1" Width="90px" Text="Cancel" OnClick="btnFndCancelObservation_Click" CausesValidation="false"  ID="btnFndCancelObservation" />
                                                                                                                        </td>

                                                                                                                    </tr>

                                                                                                                </table>


                                                                                                            </asp:View>

                                                                                                        </asp:MultiView>


                                                                                                    </td>
                                                                                                </tr>

                                                                                            </table>
                                                                                        </div>



                                                                                    </asp:Panel>

                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="7">
                                                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                </td>
                                                                            </tr>


                                                                            <tr>
                                                                                <td colspan="7" align="center">
                                                                                    <asp:Button runat="server" CssClass="Button" Width="120px" OnClick="btnSaveFinding_Click" Text="Save Finding" ID="btnSaveFinding" />
                                                                                    <asp:Button runat="server" CssClass="Button" OnClick="btnCancelFinding_Click" Width="120px" Text="Cancel" ID="btnCancelFinding" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>

                                                                </asp:View>

                                                            </asp:MultiView>

                                                        </td>
                                                    </tr>

                                                </table>


                                            </div>

                                            <div class="footer">

                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button runat="server" ID="btnCommintChanges" Visible="false" CssClass="Button1" Width="150px" Text="Commit Changes" ToolTip="Save / Commit Changes To Finding(s)" OnClick="btnCommintChanges_Click" />

                                                            <asp:Button runat="server" ID="btnCancelReview" Visible="false" OnClick="btnCancelReview_Click" CssClass="Button1" Width="150px" Text="Cancel Review" ToolTip="Cancel/Close Review Without Saving Chagnes" />
                                                        </td>
                                                    </tr>
                                                </table>


                                            </div>

                                        </asp:Panel>

                                    </td>
                                </tr>


                            </tbody>
                        </table>
                    </td>
                </tr>

            </table>





            <asp:Label runat="server" Style="visibility: hidden;" ID="hndBnt" Text="Show Popup"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpUploadFile" runat="server" ClientIDMode="Static"
                PopupControlID="pnlUploadFile" TargetControlID="hndBnt"
                CancelControlID="btnCancelUpload" BehaviorID="mpbUploadFile"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlUploadFile" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" ID="lblFileUploadTitle" Text="File Upload"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <uc1:ucAsyncFileUploadControl runat="server" ClientIDMode="AutoID" ID="ucUploadFile" />
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="footer right">
                    <table width="100%">
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="btnCancelUpload" CssClass="Button" Width="100px" runat="server" Text="Cancel / Close" />
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>



            <asp:Label runat="server" Style="visibility: hidden;" ID="lbtnShowUploadFiles" Text="View Files"></asp:Label>

            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpShowUploadedFiles" ClientIDMode="AutoID"
                PopupControlID="pnlShowFiles" TargetControlID="lbtnShowUploadFiles"
                CancelControlID="btnClsShowFiles" DropShadow="true">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="pnlShowFiles" runat="server" CssClass="modalPopup" ClientIDMode="AutoID" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" ID="lblViewFilesTitle" Text=""></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <td>
                                            <uc1:ucFileListControl style="width: 100% !important;" ClientIDMode="AutoID" runat="server" ID="ucViewFiles" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="footer right">
                    <table width="100%">
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="btnClsShowFiles" CssClass="Button" Width="100px" runat="server" Text="Close" />
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>






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
