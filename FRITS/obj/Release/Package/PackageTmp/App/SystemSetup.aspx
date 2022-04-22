<%@ Page Title="System Setup" Async="true" MaintainScrollPositionOnPostback="true" EnableViewState="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="SystemSetup.aspx.vb" Inherits="FRITS.SystemSetup" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="950px" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblSystemSetupTitle">System Setup</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">
                            <tbody>

                                <tr>
                                    <td style="width: 100%; vertical-align: top; width: 100%;" nowrap="nowrap">

                                        <asp:Button runat="server" ID="btnObservationStatusSetup" OnClick="btnObservationStatusSetup_Click" Width="160px" CssClass="Button1" Text="Define Observation Status" />
                                        &nbsp  
                                        <asp:Button runat="server" ID="btnRistkLevelSetup" Width="160px" OnClick="btnRistkLevelSetup_Click" CssClass="Button1" Text="Define Risk Levels" />
                                        &nbsp  
                                        <asp:Button runat="server" ID="btnRiskCategorySetup" Width="160px" OnClick="btnRiskCategorySetup_Click" CssClass="Button1" Text="Define Risk Categories" />

                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 350px;">
                                        <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap" style="width: 100%; height: 100%; vertical-align: top; width: 100%;">
                                        <asp:Panel ID="Panel1" runat="server" Visible="true">
                                            <br />
                                            <div style="float: right; line-height: 20px; padding-right: 8px;">
                                                &nbsp;<label id="lblActiveViewTitle" runat="server" class="pageTitle">
                                                    No Selected Option
                                                </label>
                                            </div>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                        </asp:Panel>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:MultiView ID="mlvSystemSetup" runat="server" ActiveViewIndex="0" EnableViewState="true" ViewStateMode="Enabled">
                                            <asp:View ID="vObservationStatus" runat="server">

                                                <asp:MultiView runat="server" ID="mvObservation" ActiveViewIndex="0">
                                                    <asp:View runat="server" ID="vObservationList">
                                                        <table align="center" cellpadding="2" width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnAddObservation" OnClick="btnAddObservation_Click" Width="180px" CssClass="Button" Text="Add Observation Status" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>

                                                                        <div style="overflow-x: scroll; overflow: hidden; clear: both; width: 100%; margin-left: auto; margin-right: auto;">

                                                                            <asp:GridView ID="grdObservationStatuses" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                CssClass="gridview resizable" AllowSorting="True" DataKeyNames="ObservationStatusId" OnSelectedIndexChanged="grdObservationStatuses_SelectedIndexChanged"
                                                                                AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnObservationStatusPageIndexChanging"
                                                                                SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdObservationStatuses_RowDataBound"
                                                                                FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Code" ItemStyle-Width="5%" DataField="StatusCode"></asp:BoundField>
                                                                                    <asp:BoundField HeaderText="Description" ItemStyle-Width="30%" DataField="Description"></asp:BoundField>
                                                                                    <asp:BoundField HeaderText="Narration" ItemStyle-Width="50%" DataField="Narration"></asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Enabled?" SortExpression="" ItemStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label runat="server" ID="grdIsEnabled" Text='<%# Get_Observation_Status(Eval("IsEnabled")) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                                        <ItemTemplate>

                                                                                            <asp:LinkButton
                                                                                                OnClick="lblEditObserveStatus_Click"
                                                                                                CssClass="gridEditButton"
                                                                                                ID="lblEditObserveStatus"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Edit Observation Status">
                                                                                                Edit
                                                                                            </asp:LinkButton>
                                                                                            |
                                                                                            <asp:LinkButton
                                                                                                OnClick="lblDelObserveStatus_Click"
                                                                                                CssClass="gridDeleteButton"
                                                                                                ID="lblDelObserveStatus"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Delete Observation Status">
                                                                                                Delete
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

                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </asp:View>

                                                    <asp:View runat="server" ID="vAddEditObservation">

                                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                                            <div class="header">
                                                                <asp:Label runat="server" ID="aeObservationTitle" Text="Add New Observation Status"></asp:Label>
                                                            </div>
                                                            <div class="body" style="line-height: 18px !important; margin-top: 15px;">

                                                                <table width="400px" cellspacing="5" cellpadding="0" style="align-self: center; margin: auto !important">
                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Description
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" TextMode="SingleLine" ValidationGroup="vgObservation" ID="txtObservationStatus" CssClass="TextBox" Width="250px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtObservationStatus" ValidationGroup="vgObservation" ID="valObserveStatus" Text="Description field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Code
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" MaxLength="10" TextMode="SingleLine" ValidationGroup="vgObservation" ID="txtObservationStatusCode" CssClass="TextBox" Width="150px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtObservationStatusCode" ValidationGroup="vgObservation" ID="valObserveStausCoode" Text="Code field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>


                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Narration
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" TextMode="MultiLine" Rows="3" ValidationGroup="vgObservation" ID="txtObservationNarration" CssClass="MultiLineTextBox" Width="250px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtObservationNarration" ValidationGroup="vgObservation" ID="valtxtObservationNarration" Text="Narration field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>



                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Is Enabled
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="middle">
                                                                            <asp:DropDownList runat="server" ValidationGroup="vgObservation" Width="155px" ID="cboObserveStausEnabled" CssClass="DropDownList">
                                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" style="width: 250px;">
                                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="footer">

                                                                <asp:Button runat="server" ValidationGroup="vgObservation" OnClick="btnSaveObservation_Click" CssClass="Button" Width="150px" ID="btnSaveObservation" Text="Save Observation" CausesValidation="true" />
                                                                <asp:Button runat="server" ValidationGroup="vgObservation" OnClick="btnCancelObservation_Click" CssClass="Button" Width="120px" ID="btnCancelObservation" Text="Cancel / Close" CausesValidation="false" />

                                                            </div>

                                                        </asp:Panel>

                                                    </asp:View>

                                                </asp:MultiView>
                                            </asp:View>
                                            <asp:View ID="vRiskLevels" runat="server">

                                                <asp:MultiView runat="server" ID="mvRiskLevels" ActiveViewIndex="0">

                                                    <asp:View runat="server" ID="vRiskLevelList">

                                                        <table align="center" cellpadding="2" width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnAddRiskLevel" OnClick="btnAddRiskLevel_Click" Width="180px" CssClass="Button" Text="Add Risk Level" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>

                                                                        <div style="overflow-x: scroll; overflow: hidden; clear: both; width: 100%; margin-left: auto; margin-right: auto;">

                                                                            <asp:GridView ID="grdRiskLevels" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                CssClass="gridview resizable" AllowSorting="True" DataKeyNames="RiskLevelId" OnSelectedIndexChanged="grdRiskLevels_SelectedIndexChanged"
                                                                                AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnRiskLevelPageIndexChanging"
                                                                                SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdRiskLevels_RowDataBound"
                                                                                FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Description" ItemStyle-Width="50%" DataField="Description"></asp:BoundField>
                                                                                    <asp:BoundField HeaderText="Risk Score" ItemStyle-Width="20%" DataField="RiskScore"></asp:BoundField>

                                                                                    <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton
                                                                                                OnClick="lblEditRiskLevel_Click"
                                                                                                CssClass="gridEditButton"
                                                                                                ID="lblEditRiskLevel"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Edit Risk Level">
                                                                                                Edit
                                                                                            </asp:LinkButton>
                                                                                            |
                                                                                            <asp:LinkButton
                                                                                                OnClick="lblDelRiskLevel_Click"
                                                                                                CssClass="gridDeleteButton"
                                                                                                ID="lblDelRiskLevel"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Delete Risk Level">
                                                                                                Delete
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

                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>




                                                    </asp:View>

                                                    <asp:View runat="server" ID="vAddEditRiskLevel">

                                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                                            <div class="header">
                                                                <asp:Label runat="server" ID="aeRiskLevelTitle" Text="Add New Risk Level"></asp:Label>
                                                            </div>
                                                            <div class="body" style="line-height: 18px !important; margin-top: 15px;">

                                                                <table width="400px" cellspacing="5" cellpadding="0" style="align-self: center; margin: auto !important">
                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Description
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" TextMode="SingleLine" ValidationGroup="vgRiskLevel" ID="txtRiskLevelDesc" CssClass="TextBox" Width="250px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtRiskLevelDesc" ValidationGroup="vgRiskLevel" ID="valTxtRiskLevelDesc" Text="Description field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Score
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" TextMode="Number" ValidationGroup="vgRiskLevel" ID="txtRiskLevelScore" CssClass="TextBox" Width="250px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtRiskLevelScore" ValidationGroup="vgRiskLevel" InitialValue="0" ID="valtxtRiskLevelScore" Text="Score field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td colspan="3" style="width: 250px;">
                                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="footer">
                                                                <asp:Button runat="server" ValidationGroup="vgRiskLevel" OnClick="btnSaveRiskLevel_Click" CssClass="Button" Width="150px" ID="btnSaveRiskLevel" Text="Save Risk Level" CausesValidation="true" />
                                                                <asp:Button runat="server" ValidationGroup="vgRiskLevel" OnClick="btnCancelRiskLevel_Click" CssClass="Button" Width="120px" ID="btnCancelRiskLevel" Text="Cancel / Close" CausesValidation="false" />

                                                            </div>

                                                        </asp:Panel>

                                                    </asp:View>

                                                </asp:MultiView>

                                            </asp:View>

                                            <asp:View runat="server" ID="vRiskCategories">

                                                <asp:MultiView runat="server" ID="mlvCategories" ActiveViewIndex="0">

                                                    <asp:View runat="server" ID="vRiskCategoryList">

                                                        <table align="center" cellpadding="2" width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnAddRiskCategory" OnClick="btnAddRiskCategory_Click" Width="180px" CssClass="Button" Text="Add Risk Category" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>

                                                                        <div style="overflow-x: scroll; overflow: hidden; clear: both; width: 100%; margin-left: auto; margin-right: auto;">

                                                                            <asp:GridView ID="grdRiskCategories" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                CssClass="gridview resizable" AllowSorting="True" DataKeyNames="RiskCategoryId" OnSelectedIndexChanged="grdRiskCategories_SelectedIndexChanged"
                                                                                AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnRiskCategoriesPageIndexChanging"
                                                                                SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdRiskCategories_RowDataBound"
                                                                                FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                                <Columns>
                                                                                    <asp:BoundField HeaderText="Description" ItemStyle-Width="70%" DataField="RiskCategoryDesc"></asp:BoundField>


                                                                                    <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton
                                                                                                OnClick="lblEditRiskCategory_Click"
                                                                                                CssClass="gridEditButton"
                                                                                                ID="lblEditRiskCategory"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Edit Risk Category">
                                                                                                Edit
                                                                                            </asp:LinkButton>
                                                                                            |
                                                                                            <asp:LinkButton
                                                                                                OnClick="lblDelRiskCategory_Click"
                                                                                                CssClass="gridDeleteButton"
                                                                                                ID="lblDelRiskCategory"
                                                                                                CausesValidation="false"
                                                                                                runat="server" ToolTip="Delete Risk Category">
                                                                                                Delete
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

                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>




                                                    </asp:View>

                                                    <asp:View runat="server" ID="vAddEditRiskCategory">

                                                        <asp:Panel runat="server" Width="100%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                                            <div class="header">
                                                                <asp:Label runat="server" ID="aeRiskCategoryTitle" Text="Add New Risk Category"></asp:Label>
                                                            </div>
                                                            <div class="body" style="line-height: 18px !important; margin-top: 15px;">

                                                                <table width="400px" cellspacing="5" cellpadding="0" style="align-self: center; margin: auto !important">
                                                                    <tr>
                                                                        <td style="width: 35px" align="left" valign="top">Category Description
                                                                        </td>
                                                                        <td align="center" valign="top" style="width: 10px;">:</td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox runat="server" TextMode="SingleLine" ValidationGroup="vgRiskCategory" ID="txtRiskCategory" CssClass="TextBox" Width="250px"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtRiskCategory" ValidationGroup="vgRiskCategory" ID="valTxtRiskCategory" Text="Description field is required!"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td colspan="3" style="width: 250px;">
                                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="footer">
                                                                <asp:Button runat="server" ValidationGroup="vgRiskCategory" OnClick="btnSaveRiskCategory_Click" CssClass="Button" Width="150px" ID="btnSaveRiskCategory" Text="Save Risk Category" CausesValidation="true" />
                                                                <asp:Button runat="server" ValidationGroup="vgRiskCategory" OnClick="btnCancelRiskCategory_Click" CssClass="Button" Width="120px" ID="btnCancelRiskCategory" Text="Cancel / Close" CausesValidation="false" />

                                                            </div>

                                                        </asp:Panel>

                                                    </asp:View>


                                                </asp:MultiView>

                                            </asp:View>                                                                                      

                                        </asp:MultiView>
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
