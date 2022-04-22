<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="UpdateReviewRecord.aspx.vb" Inherits="FRITS.UpdateReviewRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">      

        function cancelObservation(msg) {
            return getConfirmation(msg)
        }
        let _arrUserList = [];

        function showActionPlanModalPopup() {
            _arrUserList = [];
        }

        function addActionPlanEmployee() {
            let _arrUNameList = [];
            var _htmlString = document.getElementById('<%= lblAPUserList.ClientID %>').innerHTML;
            var _idString = document.getElementById('txtAPSelectedEmpIds').value;
            if (_idString) {
                _arrUserList = _idString.split(",");
                _arrUNameList = _htmlString.split(",");
            }
            var _nU = {};
            var _user = document.getElementById("cboUsersInBranch");
            _nU.Name = _user.options[_user.selectedIndex].innerHTML;
            _nU.Id = _user.value;
            var _newList = _arrUserList.filter((u) => u !== _nU.Id);
            _arrUserList = _newList;
            _arrUserList.push(_nU.Id);
            _arrUNameList.push(_nU.Name);
            _htmlString = '';
            _idString = '';
            _arrUserList.forEach((u, i) => {
                if (_idString === '') {
                    _htmlString = _arrUNameList[i];
                    _idString = u;
                } else {
                    _htmlString += ', ' + _arrUNameList[i];
                    _idString += ',' + u;
                }
            });
            document.getElementById('<%= lblAPUserList.ClientID %>').innerHTML = _htmlString;
            document.getElementById('txtAPSelectedEmpIds').value = _idString;

            return false;
        }

        function validateActionPlan(e) {
            var _uIds = document.getElementById('txtAPSelectedEmpIds').value;
            var _ap = document.getElementById('<%= txtAPDescription.ClientID %>').value;
            var _apd = document.getElementById('<%= txtAPResolutionDate.ClientID %>').value;
            var _sor = document.getElementById('<%= txtAPSchedOfficerResponse.ClientID %>').value;
            if (_ap && _uIds && _apd && _sor) {
                return true;
            }
            var mp = $find('programmaticModalPopupBehavior');
            if (mp) { mp.hide(); }
            showErrorMessage("Required Input Validation Failed!  \n\n\ Please Add Employee and Action Plan To Continue!");
            setTimeout(() => {
                var btn = document.getElementsByClassName('swal-button swal-button--confirm')[0]
                if (btn) { btn.click(); }
                mp.show();
            }, 2000);

            return false;
        }

        function validateActionTaken(e) {

            var _ad = document.getElementById('<%= txtATDate.ClientID %>').value;
            var _atd = document.getElementById('<%= txtATDetails.ClientID %>').value;
            if (_ad && _atd) {
                return true;
            }
            var mp = $find('progActionTakenPopup');
            if (mp) { mp.hide(); }
            showErrorMessage("Required Input Validation Failed!  \n\n\ Please set date and add details to continue!");
            setTimeout(() => {
                var btn = document.getElementsByClassName('swal-button swal-button--confirm')[0]
                if (btn) { btn.click(); }
                mp.show();
            }, 2000);

            return false;
        }

    </script>


    <style type="text/css">
        #tblFindingDetail {
            margin: 0px !important;
            padding: 0px 4px !important;
        }

            #tblFindingDetail > td {
            }

        td > p {
            margin-top: 5px !important;
            margin-bottom: 3px !important;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Update Reviews - Branch</asp:Label>
                        </div>


                        <asp:MultiView runat="server" ID="mlvBranchRecords" ActiveViewIndex="0">

                            <asp:View runat="server">

                                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                                    <tbody>

                                        <%--<tr>
                                            <td colspan="2">
                                                <asp:Panel runat="server" GroupingText="Branch">

                                                    <table width="100%" cellspacing="5" cellpadding="0">
                                                        <tr>
                                                            <td>Branch :
                                                        <asp:DropDownList DataValueField="Code" AutoPostBack="true" OnSelectedIndexChanged="cboReviewBranch_SelectedIndexChanged" DataTextField="Name" Width="320px" runat="server" ID="cboReviewBranch"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </asp:Panel>
                                            </td>
                                        </tr>--%>

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
                                                            <asp:TextBox runat="server" ID="txtSearchReview" CssClass="TextBox" Width="150px"></asp:TextBox>
                                                            <asp:Button CssClass="Button1" runat="server" OnClick="btnSearchReview_Click" ID="btnSearchReview" Height="21px" Width="65px" Text="Search" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grdReviews" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId"
                                                                AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnReviewPageIndexChanging"
                                                                SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc"
                                                                FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                <RowStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                <SelectedRowStyle BackColor="#A1DCF2" />
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Code" ItemStyle-Width="90px" DataField="ReviewCode"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="Description" ItemStyle-Width="400px" DataField="ReviewName"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="From" ItemStyle-Width="80px" DataFormatString="{0:dd-MMM-yyyy}" DataField="ReviewFrom"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="To" ItemStyle-Width="80px" DataFormatString="{0:dd-MMM-yyyy}" DataField="ReviewTo"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="Review By" ItemStyle-Width="200px" DataField="EmployeeName"></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton
                                                                                CssClass="gridEditButton"
                                                                                ID="lblUpdateReviewObservation"
                                                                                OnClick="lblUpdateReviewObservation_Click"
                                                                                CausesValidation="false"
                                                                                runat="server" ToolTip="Update Observation">Update Record
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


                            </asp:View>

                            <asp:View runat="server">
                                <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" CssClass="pageTitle" ID="lblSelectedReview" Text="No Selected Review Found!"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">

                                                <table width="90%" cellspacing="5" cellpadding="0" style="align-self: center; align-content: flex-start; align-items: flex-start; margin: auto !important">

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
                                                                                            <table width="100%" cellpadding="8" cellspacing="5" style="border-bottom: 1px solid #05075f">
                                                                                                <tr>
                                                                                                    <td style="width: 85%">
                                                                                                        <table cellpadding="8" cellspacing="2" class="table">
                                                                                                            <tr>
                                                                                                                <td valign="middle" align="left" style="width: 100px; color: #c80d0d; font-style: italic; font-weight: bold;">Finding No
                                                                                                                </td>
                                                                                                                <td valign="middle" align="center">:</td>
                                                                                                                <td valign="middle" align="left">
                                                                                                                    <asp:Label runat="server" ForeColor="#003399" Font-Bold="false" Text='<%# Eval("FindingNo") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td style="width: 10px;"></td>

                                                                                                                <td valign="middle" align="left" style="width: 100px; color: #c80d0d; font-style: italic; font-weight: bold; border-left: 1px #808080 solid;">Review Area:
                                                                                                                </td>
                                                                                                                <td valign="middle" align="center">:</td>
                                                                                                                <td valign="middle" align="left">
                                                                                                                    <asp:Label runat="server" Font-Bold="false" ForeColor="#003399" Text='<%# Eval("Description") %>'></asp:Label>
                                                                                                                </td>

                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td valign="middle" style="border-left: 1px #05075f solid; font-weight: bolder;">
                                                                                                        <asp:LinkButton runat="server" ID="lbtnViewFinding" Text="Update Findings" ForeColor="#4e1fda" CssClass="gridEditButton" CommandName="ViewFinding" ToolTip="View Review With Action Plan and Status" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
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


                                                                    <table id="tblFindingDetail" width="100%" style="padding: 0 10px;" cellspacing="5" cellpadding="5">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                    <p>Finding No :</p>
                                                                                </td>
                                                                                <td align="left" style="text-align: left; width: 150px; font-weight: bolder; color: #c80d0d">
                                                                                    <p runat="server" id="pFindingNumber"></p>
                                                                                </td>
                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                    <p>Review Area :</p>
                                                                                </td>
                                                                                <td align="left" style="text-align: left; font-weight: bolder; color: #c80d0d">
                                                                                    <p runat="server" id="pReviewArea"></p>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="4">
                                                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>


                                                                    <table width="100%" cellspacing="1" cellpadding="1">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="8">

                                                                                    <asp:DataList DataKeyField="ObservationId" HorizontalAlign="Center" OnItemCommand="dtlFndObservationList_ItemCommand" runat="server" ID="dtlFndObservationList">
                                                                                        <ItemTemplate>
                                                                                            <table runat="server" cellpadding="5" cellspacing="3" style="border-bottom: 1px solid #05075f; text-align: justify; font-size: 17px;">
                                                                                                <tr>
                                                                                                    <td style="width: 90%;">
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Observation No :</p>
                                                                                                                </td>
                                                                                                                <td style="width: 85%">
                                                                                                                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("ObservationNo")  %>'></asp:Label>

                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Observation Date :</p>
                                                                                                                </td>
                                                                                                                <td style="width: 85%">
                                                                                                                    <asp:Label runat="server" ID="Label2" Text='<%# FormatDateTime(Eval("ObservationDate"), DateFormat.ShortDate)  %>'></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td style="width: 15%;">&nbsp;</td>
                                                                                                </tr>

                                                                                                <tr>
                                                                                                    <td style="width: 90%;">
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Description :</p>
                                                                                                                </td>
                                                                                                                <td valign="top" style="width: 85%">
                                                                                                                    <p runat="server" style="line-height: 18px; word-wrap: normal; font-weight: normal">
                                                                                                                        <%# Eval("Description")  %>
                                                                                                                    </p>

                                                                                                                </td>
                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Recommendations :</p>
                                                                                                                </td>
                                                                                                                <td valign="top" style="width: 85%">

                                                                                                                    <asp:DataList runat="server"
                                                                                                                        DataKeyField="RecommendationId"
                                                                                                                        DataSource='<%# Eval("Recommendations") %>'
                                                                                                                        ID="dtlRecommendationList"
                                                                                                                        RepeatColumns="1" CellSpacing="5"
                                                                                                                        CellPadding="3"
                                                                                                                        AlternatingItemStyle-VerticalAlign="Top"
                                                                                                                        AlternatingItemStyle-HorizontalAlign="Justify"
                                                                                                                        RepeatLayout="Table" Width="100%">
                                                                                                                        <ItemTemplate>
                                                                                                                            <table width="100%" style="margin: 0px; padding: 0px;" cellpadding="0" cellspacing="3">
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 100%" align="left" valign="top">
                                                                                                                                        <p runat="server" style="color: #048d1c; font-weight: normal; vertical-align: top; line-height: 17px; font-style: italic">
                                                                                                                                            <%# Eval("Description") %>
                                                                                                                                        </p>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:DataList>

                                                                                                                </td>
                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td colspan="3"></td>
                                                                                                            </tr>

                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td valign="top" style="border-left: 1px #05075f solid;">
                                                                                                        <table width="100%" style="height: 100%;" cellpadding="10">
                                                                                                            <tr>
                                                                                                                <td align="center">

                                                                                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="Add/Edit Action Plan" OnClientClick="showActionPlanModalPopup();" CommandName="AddCorrectiveActionPlan" CssClass="gridEditButton" ForeColor="#0000cc" Font-Bold="true" Font-Italic="true"></asp:LinkButton>

                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="center" valign="top">

                                                                                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="Add Corrective Action Taken" CommandName="AddCorrectiveActionTaken" CssClass="gridEditButton" ForeColor="#0000cc" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                        </table>

                                                                                                    </td>

                                                                                                </tr>

                                                                                                <tr>
                                                                                                    <td colspan="3">

                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Action Plans :</p>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DataList runat="server"
                                                                                                                        DataKeyField="ActionPlanId"
                                                                                                                        DataSource='<%# Eval("ActionPlans") %>'
                                                                                                                        ID="dtlActionPlanList"
                                                                                                                        RepeatColumns="1" CellSpacing="5"
                                                                                                                        CellPadding="3"
                                                                                                                        AlternatingItemStyle-VerticalAlign="Top"
                                                                                                                        AlternatingItemStyle-HorizontalAlign="Justify"
                                                                                                                        RepeatLayout="Table" Width="100%">

                                                                                                                        <HeaderTemplate>
                                                                                                                            <table width="100%" style="margin: 0px; padding: 0px; line-height: 10px" cellpadding="5" cellspacing="3">
                                                                                                                                <thead>
                                                                                                                                    <tr style="background-color: #005b82; color: #fff; font-weight: bold;">
                                                                                                                                        <td style="width: 60%" align="left" valign="top">Action Plan</td>
                                                                                                                                        <td style="width: 29%" align="left" valign="top">Assigned To</td>
                                                                                                                                        <td style="width: 11%" align="left" valign="top">Timing</td>
                                                                                                                                    </tr>
                                                                                                                                </thead>
                                                                                                                            </table>
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <table width="100%" style="margin: 0px; padding: 0px;" cellpadding="5" cellspacing="3">
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 60%; border-left: 1px #05075f solid; color: #0615bf; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                        <p runat="server" style="color: #0615bf; vertical-align: top; line-height: 17px; font-style: italic">
                                                                                                                                            <%# Eval("Description") %>
                                                                                                                                        </p>
                                                                                                                                    </td>
                                                                                                                                    <td style="width: 29%; color: #0615bf; border-left: 1px #05075f solid; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                        <%# Eval("AssignedEmployeeNames") %>
                                                                                                                                    </td>
                                                                                                                                    <td style="width: 11%; border-left: 1px #05075f solid; color: #0615bf; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                        <%# FormatDateTime(Eval("ResolutionTiming"), DateFormat.ShortDate) %>  
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </ItemTemplate>

                                                                                                                    </asp:DataList>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                            <tr>
                                                                                                                <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                                    <p>Actions Taken :</p>
                                                                                                                </td>
                                                                                                                <td>

                                                                                                                    <asp:DataList runat="server"
                                                                                                                        DataKeyField="CorrectiveActionId"
                                                                                                                        DataSource='<%# Eval("CorrectiveActions") %>'
                                                                                                                        ID="dtlCorrectionDone"
                                                                                                                        RepeatColumns="1" CellSpacing="5"
                                                                                                                        CellPadding="3" OnItemCommand="dtlFndObservationList_ItemCommand"
                                                                                                                        AlternatingItemStyle-VerticalAlign="Top"
                                                                                                                        AlternatingItemStyle-HorizontalAlign="Justify"
                                                                                                                        RepeatLayout="Table" Width="100%">
                                                                                                                        <HeaderTemplate>
                                                                                                                            <table width="100%" style="margin: 0px; padding: 0px; line-height: 10px" cellpadding="5" cellspacing="3">
                                                                                                                                <thead>
                                                                                                                                    <tr style="background-color: #005b82; color: #fff; font-weight: bold;">
                                                                                                                                        <td style="width: 10%" align="left" valign="top">Date</td>
                                                                                                                                        <td style="width: 80%" align="left" valign="top">Action Taken</td>
                                                                                                                                        <td style="width: 10%" align="left" valign="top">#</td>
                                                                                                                                    </tr>
                                                                                                                                </thead>
                                                                                                                            </table>
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <table width="100%" style="margin: 0px; padding: 0px;" cellpadding="5" cellspacing="3">
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 10%; border-left: 1px #05075f solid; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                        <%# FormatDateTime(Eval("CorrectionDate"), DateFormat.ShortDate) %> 
                                                                                                                                    </td>
                                                                                                                                    <td style="width: 80%; border-left: 1px #05075f solid; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                        <p runat="server" style="vertical-align: top; line-height: 17px; font-style: italic">
                                                                                                                                            <%# Eval("Remarks") %>
                                                                                                                                        </p>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:LinkButton runat="server" CommandArgument='<%# Eval("CorrectiveActionId") %>' ClientIDMode="AutoID" Text="Remove" CommandName="RemoveCorrectiveAction" CssClass="gridEditButton" ForeColor="Red" Font-Bold="true" Font-Italic="true"></asp:LinkButton>

                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </ItemTemplate>

                                                                                                                    </asp:DataList>



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

                                                                            <tr>
                                                                                <td colspan="7">
                                                                                    <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                                </td>
                                                                            </tr>

                                                                        </tbody>
                                                                    </table>







                                                                </asp:View>

                                                            </asp:MultiView>

                                                        </td>
                                                    </tr>

                                                </table>



                                            </td>

                                        </tr>
                                    </tbody>
                                </table>

                            </asp:View>


                        </asp:MultiView>


                    </td>
                </tr>

            </table>


            <asp:Label runat="server" Style="visibility: hidden;" Text="Show" ID="btnHndBtn"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpAddActionTaken" runat="server" ClientIDMode="Static"
                PopupControlID="pnlAddEditActionTaken" TargetControlID="btnHndBtn"
                CancelControlID="btnClose" BehaviorID="progActionTakenPopup"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="pnlAddEditActionTaken" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" Text="Add/Edit Action Taken"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:Label runat="server" Style="visibility: hidden" ID="Label3"></asp:Label>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <td align="left" valign="top" style="width: 90px; color: #0615bf; font-style: italic; font-weight: bolder;">Action Date</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td valign="top">
                                            <asp:TextBox runat="server" ID="txtATDate" Width="130px" Enabled="false" Text='<%# _actionTaken.CorrectionDate %>' CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="ibtnATDate" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                            <ajaxToolkit:CalendarExtender ID="valtxtATDate" runat="server" PopupButtonID="ibtnATDate" Format="yyyy-MM-dd" TargetControlID="txtATDate" />

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 90px; color: #0615bf; font-style: italic; font-weight: bolder;">Action Details</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="20" Width="390px" CssClass="MultiLineTextBox" runat="server" Text='<%# _actionTaken.Remarks %>' ID="txtATDetails"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 90px; color: #0615bf; font-style: italic; font-weight: bolder;">Status After Action</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td valign="top">
                                            <asp:DropDownList runat="server" ClientIDMode="Static" Width="150px" CssClass="DropDownList" DataValueField="ObservationStatusId" DataTextField="Description" ID="cboATtatus"></asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td align="left" valign="top" style="width: 90px; color: #0615bf; font-style: italic; font-weight: bolder;"></td>
                                        <td valign="top" style="width: 5px"></td>
                                        <td valign="top">
                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtATDetails" ValidationGroup="vgValATaken" ID="valATDetails" Text="Action details field is required!"></asp:RequiredFieldValidator>
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
                                <asp:Button ID="btnSaveActionTaken" Width="100px" OnClientClick="return validateActionTaken(this);" OnClick="btnSaveActionTaken_Click" CssClass="Button" runat="server" Text="Save Changes" />
                                <asp:Button ID="btnCancelActionTaken" CssClass="Button" Width="100px" runat="server" Text="Close" />
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>





            <asp:Label runat="server" Style="visibility: hidden;" Text="Show" ID="hndBnt"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpAddEditActionPlan" runat="server" ClientIDMode="Static"
                PopupControlID="pnlAddEditActionPlan" TargetControlID="hndBnt"
                CancelControlID="btnClose" BehaviorID="programmaticModalPopupBehavior"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="pnlAddEditActionPlan" Width="700px" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" Text="Add Action Plan"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:Label runat="server" Style="visibility: hidden" ID="hndAPObservationId"></asp:Label>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Observation Date</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAPObservationDate" Text="---"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Observation No</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAPObservationNo" Text="---"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Description</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAPObservationDescription" Text="No Record Selected"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Resolution Date</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAPResolutionDate" Width="130px" Enabled="false" Text='<%# _actionPlan.ResolutionTiming %>' CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="ibtnAPResolutionDate" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                            <ajaxToolkit:CalendarExtender ID="calAPResolutionDate" runat="server" PopupButtonID="ibtnAPResolutionDate" Format="yyyy-MM-dd" TargetControlID="txtAPResolutionDate" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Assign To</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ClientIDMode="Static" Width="250px" CssClass="DropDownList" DataValueField="ObjectID" DataTextField="ObjectName" ID="cboUsersInBranch"></asp:DropDownList>
                                            &nbsp;
                                            <asp:Button runat="server" Height="25px" CausesValidation="false" CssClass="Button1" Text="Add" Width="40px" ID="btnAddActionPlanEmployee" OnClientClick="return addActionPlanEmployee(this);" />
                                            <p>
                                                <label runat="server" style="color: blue; font-weight: bold; font-style: italic;" id="lblAPUserList"></label>
                                            </p>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Schedule Officer's Response</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="10" Width="100%" CssClass="MultiLineTextBox" runat="server" Text='<%# _actionPlan.ScheduleOfficerResponse %>' ID="txtAPSchedOfficerResponse"></asp:TextBox>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;">Action Plan</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="10" Width="100%" CssClass="MultiLineTextBox" runat="server" Text='<%# _actionPlan.Description %>' ID="txtAPDescription"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" style="width: 130px; color: #0615bf; font-style: italic; font-weight: bolder;"></td>
                                        <td valign="top" style="width: 5px"></td>
                                        <td>
                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtAPDescription" ValidationGroup="vgValActionPlan" ID="valtxtAPDescription" Text="Action plan field is required!"></asp:RequiredFieldValidator>
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
                                <asp:Button ID="btnSaveActionPlan" Width="100px" OnClientClick="return validateActionPlan(this);" OnClick="btnSaveActionPlan_Click" CssClass="Button" runat="server" Text="Save Changes" />
                                <asp:Button ID="btnClose" CssClass="Button" Width="100px" runat="server" Text="Close" />
                                <asp:TextBox runat="server" Style="visibility: hidden;" ID="txtAPSelectedEmpIds" ClientIDMode="Static"></asp:TextBox>
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
