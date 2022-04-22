<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="ViewUpdateReview.aspx.vb" Inherits="FRITS.ViewUpdateReview" %>

<%@ Register Src="~/Resources/uControls/ucFileListControl.ascx" TagPrefix="uc1" TagName="ucFileListControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">      

        function validateManagementResponse(e) {
            var _mr = document.getElementById('<%= txtManagementResponse.ClientID %>').value;
            if (_mr) {
                return true;
            }
            var mp = $find('mpUpdateManagementResponse');
            if (mp) { mp.hide(); }
            showErrorMessage("Please enter response text to continue!");
            setTimeout(() => {
                var btn = document.getElementsByClassName('swal-button swal-button--confirm')[0]
                if (btn) { btn.click(); }
                mp.show();
            }, 2000);

            return false;
        }

        function validateFollowUp(e) {
            var _fud = document.getElementById('<%= txtFUDate.ClientID %>').value;
            var _fut = document.getElementById('<%= txtFUTitle.ClientID %>').value;
            var _fur = document.getElementById('<%= txtFURemarks.ClientID %>').value;
            if (_fud && _fur && _fut) {
                return true;
            }
            var mp = $find('mpbCreateFollowUp');
            if (mp) { mp.hide(); }
            showErrorMessage("Please make entires in all required fields to continue!");
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
                            <asp:Label runat="server" ID="lblReviewTitle">View / Update Review</asp:Label>
                        </div>


                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td align="center" style="width: 80%">
                                        <asp:Label runat="server" CssClass="pageTitle" ID="lblSelectedReview" Text="No Selected Review Found!"></asp:Label>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:Button runat="server" ID="btnGoBack" OnClick="btnGoBack_Click" CssClass="Button" Width="100px" Text="<<  Go Back" Font-Bold="true" Font-Italic="true"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">

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
                                                                                            <td valign="middle" style="border-left: 1px #05075f solid;">

                                                                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                              <asp:LinkButton runat="server" ID="lbtnViewFinding" Text="View Observations" ForeColor="#4e1fda" CssClass="gridEditButton" CommandName="ViewObservations" ToolTip="View Observations With Action Plan and Status" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("FindingId") %>' ClientIDMode="AutoID" Text="View Files" CommandName="ViewFindingFiles" CssClass="gridEditButton" ForeColor="#660066"  ToolTip="View Finding Related Files" Font-Bold="true" Font-Italic="true"></asp:LinkButton>

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
                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                            <p>Risk Level :</p>
                                                                        </td>
                                                                        <td align="left" style="text-align: left; width: 150px; font-weight: bolder; color: #c80d0d">
                                                                            <p runat="server" id="pRiskLevel"></p>
                                                                        </td>
                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                            <p>Risk Category :</p>
                                                                        </td>
                                                                        <td align="left" style="text-align: left; font-weight: bolder; color: #c80d0d">
                                                                            <p runat="server" id="pRiskCategory"></p>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                            <p>Risk Sub-Category :</p>
                                                                        </td>
                                                                        <td align="left" style="text-align: left; width: 150px; font-weight: bolder; color: #c80d0d">
                                                                            <p runat="server" id="pRiskSubCategory"></p>
                                                                        </td>
                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                            <p>Management Awareness :</p>
                                                                        </td>
                                                                        <td align="left" style="text-align: left; font-weight: bolder; color: #c80d0d">

                                                                            <p runat="server" id="pManagementAwareness"></p>
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
                                                                                                <table width="100%" cellspacing="0" cellpadding="5">
                                                                                                    <tr>
                                                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                            <p>Description :</p>
                                                                                                        </td>
                                                                                                        <td valign="top" style="width: 85%; border-left: 1px #05075f solid;">
                                                                                                            <p runat="server" style="line-height: 18px; word-wrap: normal; font-weight: normal">
                                                                                                                <%# Eval("Description")  %>
                                                                                                            </p>

                                                                                                        </td>
                                                                                                    </tr>

                                                                                                    <tr>
                                                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                            <p>Recommendations :</p>
                                                                                                        </td>
                                                                                                        <td valign="top" style="width: 85%; border-left: 1px #05075f solid;">

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
                                                                                                                                <p runat="server" style="color: #05075f; vertical-align: top; line-height: 17px; font-style: italic">
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
                                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="Update Observation" CommandName="UpdateObservation" CssClass="gridEditButton" ForeColor="#0033cc" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                    <tr>
                                                                                                        <td align="center" valign="top">
                                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="Update Management Response" CommandName="AddManagementResponse" CssClass="gridEditButton" ForeColor="#cc0000" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center" valign="top">
                                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="Create Follow Up" CommandName="CreateFollowUp" CssClass="gridEditButton" ToolTip="View Observation Related Files" ForeColor="#900290" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
                                                                                                        </td>

                                                                                                    </tr>


                                                                                                    <tr>
                                                                                                        <td align="center" valign="top">
                                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ObservationId") %>' ClientIDMode="AutoID" Text="View Files" CommandName="ViewObservationFiles" CssClass="gridEditButton" ForeColor="#660066" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
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
                                                                                                                    <table width="100%" style="margin: 0px; padding: 0px; line-height: 13px" cellpadding="5" cellspacing="3">
                                                                                                                        <thead>
                                                                                                                            <tr style="background-color: #005b82; color: #fff; font-weight: bold;">
                                                                                                                                <td style="width: 60%" align="left" valign="top">Corrective Action</td>
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
                                                                                                        <td></td>
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
                                                                                                                    <table width="100%" style="margin: 0px; padding: 0px; border-bottom: 1px solid #05075f;" cellpadding="5" cellspacing="3">
                                                                                                                        <tr>
                                                                                                                            <td style="width: 10%; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                <%# FormatDateTime(Eval("CorrectionDate"), DateFormat.ShortDate) %> 
                                                                                                                            </td>
                                                                                                                            <td style="width: 80%; border-left: 1px #05075f solid; vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                <p runat="server" style="vertical-align: top; line-height: 17px; font-style: italic">
                                                                                                                                    <%# Eval("Remarks") %>
                                                                                                                                </p>
                                                                                                                            </td>
                                                                                                                            <td>&nbsp;
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </ItemTemplate>

                                                                                                            </asp:DataList>

                                                                                                        </td>
                                                                                                    </tr>


                                                                                                    <tr>
                                                                                                        <td valign="top" style="width: 140px; color: #05075f; font-style: italic; font-weight: bold;">
                                                                                                            <p>Follow-Ups :</p>
                                                                                                        </td>
                                                                                                        <td>

                                                                                                            <asp:DataList runat="server"
                                                                                                                DataKeyField="FollowUpId"
                                                                                                                DataSource='<%# Eval("FollowUpDetails") %>'
                                                                                                                ID="dtlObservationFollowups"
                                                                                                                RepeatColumns="1" CellSpacing="5"
                                                                                                                CellPadding="3" OnItemCommand="dtlFndObservationList_ItemCommand"
                                                                                                                AlternatingItemStyle-VerticalAlign="Top"
                                                                                                                AlternatingItemStyle-HorizontalAlign="Justify"
                                                                                                                RepeatLayout="Table" Width="100%">
                                                                                                                <ItemTemplate>
                                                                                                                    <table style="width: 100%; border-bottom: 1px solid #05075f;" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td style="width: 93%">
                                                                                                                                <table style="margin: 0px; width: 100%; padding: 0px;" cellpadding="5" cellspacing="5">
                                                                                                                                    <tr>
                                                                                                                                        <td align="left" valign="top" style="width: 120px; color: #900290; font-weight: bold">Follow-up Date :
                                                                                                                                        </td>
                                                                                                                                        <td style="vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                            <%# FormatDateTime(Eval("FollowUpDate"), DateFormat.ShortDate) %> 
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 20px;"></td>
                                                                                                                                        <td align="left" valign="top" style="width: 120px; color: #900290; font-weight: bold">Current Status : 
                                                                                                                                        </td>

                                                                                                                                        <td style="line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                            <%# Eval("ObservationStatusName") %> 
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td align="left" valign="top" style="width: 120px; color: #900290; font-weight: bold">Title : 
                                                                                                                                        </td>

                                                                                                                                        <td colspan="4" style="vertical-align: top; line-height: 17px; font-style: italic" align="left" valign="top">
                                                                                                                                            <%# Eval("FollowUpDescription") %> 
                                                                                                                                        </td>
                                                                                                                                    </tr>

                                                                                                                                    <tr>
                                                                                                                                        <td align="left" valign="top" style="width: 120px; color: #900290; font-weight: bold">
                                                                                                                                            <p>Detailed Remark :</p>
                                                                                                                                        </td>
                                                                                                                                        <td colspan="4" align="left" valign="top">
                                                                                                                                            <p runat="server" style="vertical-align: top; line-height: 17px;">
                                                                                                                                                <%# Eval("Remarks") %>
                                                                                                                                            </p>
                                                                                                                                        </td>
                                                                                                                                        <td>&nbsp;
                                                                                                                                        </td>
                                                                                                                                    </tr>



                                                                                                                                    <tr>
                                                                                                                                        <td align="left" valign="top" style="width: 120px; color: #900290; font-weight: bold">
                                                                                                                                            <p>Follow-Up By :</p>
                                                                                                                                        </td>
                                                                                                                                        <td colspan="4" align="left" valign="top">
                                                                                                                                            <p runat="server" style="vertical-align: top; font-weight: bold; line-height: 17px;">
                                                                                                                                                <%# Eval("FollowUpByEmployeeName") %>
                                                                                                                                            </p>
                                                                                                                                        </td>
                                                                                                                                        <td>&nbsp;
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                            <td style="width: 7%" align="center" valign="middle">
                                                                                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("FollowUpId") %>' ClientIDMode="AutoID" Text="Remove Follow-Up" ToolTip="Remove Follow-Up Record" CommandName="RemoveFollowUp" CssClass="gridEditButton" ForeColor="Red" Font-Bold="true" Font-Italic="true"></asp:LinkButton>
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


                    </td>
                </tr>

            </table>


            <asp:Label runat="server" Style="visibility: hidden;" ID="hndBnt" Text="Show Popup"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpUpdateManagementResponse" runat="server" ClientIDMode="Static"
                PopupControlID="pnlUpdateManagementResponse" TargetControlID="hndBnt1"
                CancelControlID="btnCancelManagementResponse" BehaviorID="mpUpdateManagementResponse"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlUpdateManagementResponse" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" Text="Update Management Response"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <td align="left" valign="top" style="width: 100px; color: #0615bf; font-style: italic; font-weight: bolder;">Management Response</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtManagementResponse" CssClass="MultiLineTextBox" Rows="10" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                <asp:Button ID="btnSaveManagementResponse" Width="100px" OnClientClick="return validateManagementResponse(this);" OnClick="btnSaveManagementResponse_Click" CssClass="Button" runat="server" Text="Save Changes" />
                                <asp:Button ID="btnCancelManagementResponse" CssClass="Button" Width="100px" runat="server" Text="Cancel / Close" />
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>

            <asp:Label runat="server" Style="visibility: hidden;" ID="hndBnt1" Text="Show Popup"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpUpdateObservationStatus" runat="server" ClientIDMode="Static"
                PopupControlID="pnlUpdateStatus" TargetControlID="hndBnt"
                CancelControlID="btnCloseUpdateStatus" BehaviorID="mpUpdateObsvStatus"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlUpdateStatus" Width="700px" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header">
                    <asp:Label runat="server" Text="Update Observation Status"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    
                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Description</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUODescription" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Implication</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUOImplication" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Root Cause</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUORootCause" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Assumption</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUOAssumption" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Mitigating Control</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUOMitigatingControl" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">1st Recommendation</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtUO1stRecommendation" Width="100%" CssClass="MultiLineTextBox" Rows="5" TextMode="MultiLine" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 60px; color: #0615bf; font-style: italic; font-weight: bolder;">Current Status</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ClientIDMode="Static"  Width="150px" CssClass="DropDownList" DataValueField="ObservationStatusId" DataTextField="Description" ID="cboObservationStatus"></asp:DropDownList>
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
                                <asp:Button ID="btnSaveStatusChange" Width="100px" OnClick="btnSaveStatusChange_Click" CssClass="Button" runat="server" Text="Save Changes" />
                                <asp:Button ID="btnCloseUpdateStatus" CssClass="Button" Width="100px" runat="server" Text="Close" />
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>



            <asp:Label runat="server" Style="visibility: hidden;" ID="cfHndBtn" Text="Show Popup"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mpCreateFollowUp" runat="server" ClientIDMode="Static"
                PopupControlID="pnlCreateFollowUp" TargetControlID="cfHndBtn"
                CancelControlID="btnCloseFollowUp" BehaviorID="mpbCreateFollowUp"
                DropShadow="true"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="pnlCreateFollowUp" runat="server" CssClass="modalPopup" align="center" HorizontalAlign="Center" BorderColor="Navy" BorderStyle="Dotted" Style="display: none">
                <div class="header red">
                    <asp:Label runat="server" Text="Create/Edit Follow-up Record"></asp:Label>
                </div>
                <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: left;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tbody>
                                    <tr>
                                        <td align="left" valign="top" style="width: 120px; color: #0615bf; font-style: italic; font-weight: bolder;">Follow-up Date</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td valign="top">
                                            <asp:TextBox runat="server" ID="txtFUDate" Width="130px" Enabled="false" CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="ibtnFUDate" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                            <ajaxToolkit:CalendarExtender ID="valtxtFUDate" runat="server" PopupButtonID="ibtnFUDate" Format="yyyy-MM-dd" TargetControlID="txtFUDate" />

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 120px; color: #0615bf; font-style: italic; font-weight: bolder;">Title</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox TextMode="SingleLine" Width="390px" CssClass="TextBox" runat="server" ID="txtFUTitle"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 120px; color: #0615bf; font-style: italic; font-weight: bolder;">Current Status</td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ClientIDMode="Static" Width="250px" CssClass="DropDownList" DataValueField="ObservationStatusId" DataTextField="Description" ID="cboFUCurrentStatus"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" valign="top" style="width: 120px; color: #0615bf; font-style: italic; font-weight: bolder;">Detailed Remark </td>
                                        <td valign="top" style="width: 5px">:</td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="30" Width="390px" CssClass="MultiLineTextBox" runat="server" ID="txtFURemarks"></asp:TextBox>
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
                                <asp:Button ID="btnSaveFollowUp" OnClientClick="return validateFollowUp(this);" Width="100px" OnClick="btnSaveFollowUp_Click" CssClass="Button" runat="server" Text="Save Changes" />
                                <asp:Button ID="btnCloseFollowUp" CssClass="Button" Width="100px" runat="server" Text="Close" />
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
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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

