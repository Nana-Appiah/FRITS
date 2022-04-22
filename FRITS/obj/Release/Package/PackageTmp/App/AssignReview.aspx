<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="AssignReview.aspx.vb" Inherits="FRITS.AssignReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">



    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Assign Review</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">

                            <tbody>

                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Panel runat="server" Width="65%" HorizontalAlign="Center" CssClass="modalPopup" BorderColor="Navy" BorderStyle="Dotted">
                                            <div class="header">
                                                <asp:Label runat="server" ID="reviewTitle" Text="ASSIGN REVIEW"></asp:Label>
                                            </div>
                                            <div class="body" style="line-height: 18px !important; margin-top: 15px; text-align: center !important; margin: auto !important">
                                                <table width="70%" cellspacing="3" cellpadding="3" style="align-self: center; margin: auto !important">

                                                    <tr>
                                                        <td valign="top" align="left" style="width: 110px">To Branch</td>
                                                        <td valign="top">:</td>
                                                        <td align="left">

                                                            <ajaxToolkit:ComboBox ID="cboAssignBranch" ValidationGroup="vgAssignReview" Width="300px" runat="server" AutoCompleteMode="Suggest" DataTextField="Name" DataValueField="Code" AppendDataBoundItems="true"></ajaxToolkit:ComboBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">
                                                           <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="cboAssignBranch" ValidationGroup="vgAssignReview" ID="valcboAssignBranch" Text="Branch field is required!"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td valign="top" align="left" style="width: 110px">Department</td>
                                                        <td valign="top">:</td>
                                                        <td align="left">

                                                            <ajaxToolkit:ComboBox AutoPostBack="true" ID="cboToDepartment" OnSelectedIndexChanged="cboToDepartment_SelectedIndexChanged" ValidationGroup="vgAssignReview" Width="300px" runat="server" AutoCompleteMode="Suggest" DataTextField="ObjectName" DataValueField="ObjectAlias" AppendDataBoundItems="true"></ajaxToolkit:ComboBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td  align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">
                                                           <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="cboToDepartment" ValidationGroup="vgAssignReview" ID="valcboToDepartment" Text="Department field is required!"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td valign="top" align="left" style="width: 110px">To Employee</td>
                                                        <td valign="top">:</td>
                                                        <td align="left">
                                                            <ajaxToolkit:ComboBox ID="cboToEmployee" ValidationGroup="vgAssignReview" Width="300px" runat="server" AutoCompleteMode="Suggest" DataTextField="ChildObjectName" DataValueField="ChildObjectID" AppendDataBoundItems="true"></ajaxToolkit:ComboBox>
                                                        </td>
                                                    </tr>


                                                     <tr>
                                                        <td align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">
                                                           <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="cboToEmployee" ValidationGroup="vgAssignReview" ID="valcboToEmployee" Text="Employee field is required!"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" style="width: 110px">Period</td>
                                                        <td valign="top">:</td>
                                                        <td align="left">

                                                            <table width="100%" cellspacing="5" cellpadding="0">
                                                                <tr>
                                                                    <td colspan="3" align="left">From :&nbsp
                                                                            <asp:TextBox runat="server" ID="txtReviewFrom" Width="85px" Enabled="false" ValidationGroup="vgAssignReview" CssClass="TextBox"></asp:TextBox>
                                                                        <asp:ImageButton runat="server" ID="ibtnFrom" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                        <ajaxToolkit:CalendarExtender ID="ajtCalFilterFrom" runat="server" PopupButtonID="ibtnFrom" Format="yyyy-MM-dd" TargetControlID="txtReviewFrom" />

                                                                        To : &nbsp
                                                                             <asp:TextBox runat="server" ID="txtReviewTo" Width="85px" Enabled="false" ValidationGroup="vgAssignReview" CssClass="TextBox"></asp:TextBox>
                                                                        <asp:ImageButton runat="server" ID="ibtnTo" CausesValidation="false" ImageUrl="~/Images/btn_calendar.gif" AlternateText="Select"></asp:ImageButton>
                                                                        <ajaxToolkit:CalendarExtender ID="ajtCalFilterTo" runat="server" PopupButtonID="ibtnTo" Format="yyyy-MM-dd" TargetControlID="txtReviewTo" />

                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">

                                                           <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtReviewFrom" ValidationGroup="vgAssignReview" ID="valtxtReviewFrom" Text="From field is required!"></asp:RequiredFieldValidator>

                                                             <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtReviewTo" ValidationGroup="vgAssignReview" ID="valtxtReviewTo" Text="To field is required!"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td align="left" valign="top" style="width: 110px">Instruction</td>
                                                        <td valign="top">:</td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtInstruction" Width="325px" TextMode="MultiLine" Rows="3" ValidationGroup="vgAssignReview" CssClass="MultiLineTextBox"></asp:TextBox>                                                            
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">
                                                           <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="validateErrorMsg" Width="200px" ControlToValidate="txtInstruction" ValidationGroup="vgAssignReview" ID="valtxtInstruction" Text="Review Instruction field is required!"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td align="left" style="width: 110px"></td>
                                                        <td valign="top"></td>
                                                        <td align="left">
                                                            <asp:Button runat="server" Width="330px" ID="btnAssign" OnClick="btnAssign_Click" CssClass="Button" CausesValidation="true" ValidationGroup="vgAssignReview" Text="Assign To Employee" />
                                                        </td>
                                                    </tr>







                                                    <tr>
                                                        <td colspan="3">
                                                            <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                        </td>
                                                    </tr>

                                                </table>

                                                <table width="100%" cellspacing="5" cellpadding="3" style="align-self: center; margin: auto !important">
                                                    <tr>
                                                        <td colspan="3">

                                                            <asp:GridView ID="grdAssignList" AutoGenerateSelectButton="true" HeaderStyle-BackColor="WhiteSmoke" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CssClass="gridview resizable" AllowSorting="True" DataKeyNames="BranchReviewId" OnSelectedIndexChanged="grdAssignList_SelectedIndexChanged"
                                                                AllowPaging="true" AlternatingRowStyle-Wrap="false" PagerSettings-Visible="false" OnPageIndexChanging="OnAssignListPageIndexChanging"
                                                                SortedAscendingHeaderStyle-CssClass="sortedasc" CellSpacing="0" RowStyle-Wrap="false" PageSize="50" SortedDescendingHeaderStyle-CssClass="sorteddesc" OnRowDataBound="grdAssignList_RowDataBound"
                                                                FooterStyle-CssClass="footer" RowStyle-Height="15" ShowHeader="true" ShowHeaderWhenEmpty="true" PagerSettings-Mode="NumericFirstLast" CellPadding="1">
                                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                                                <Columns>
                                                                   
                                                                    <asp:BoundField HeaderText="Employee Name" ItemStyle-Width="35%" DataField="EmployeeName"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="Branch" ItemStyle-Width="25%" DataField="BranchName"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="From" ItemStyle-Width="10%" DataFormatString="{0:ddd, dd-MMM-yyyy}" DataField="ReviewFrom"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="To" ItemStyle-Width="10%" DataFormatString="{0:ddd, dd-MMM-yyyy}" DataField="ReviewTo"></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Width="25px">
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton
                                                                                CssClass="gridDeleteButton"
                                                                                ID="lblRemoveAssignment"
                                                                                 OnClick="lblRemoveAssignment_Click"
                                                                                CausesValidation="false"
                                                                                runat="server" ToolTip="Delete Review">Remove
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
                                            </div>
                                            <div class="footer center">
                                                <hr class="panLine1" style="margin: 0px; padding: 0px" />
                                                <br />
                                                <asp:Button runat="server" CssClass="Button" OnClick="btnSaveAssignment_Click" Width="120px" Text="Save Assignment" ID="btnSaveAssignment" />
                                                <asp:Button runat="server" CssClass="Button" OnClick="btnCancelAssignment_Click" Width="120px" Text="Cancel" ID="btnCancelAssignment" />
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
