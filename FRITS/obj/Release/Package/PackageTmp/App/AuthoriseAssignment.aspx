<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Resources/MasterPages/Layout1.Master" CodeBehind="AuthoriseAssignment.aspx.vb" Inherits="FRITS.AuthoriseAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" border="0" width="100%" align="center">

                <tr>
                    <td style="width: 70%" valign="top">
                        <div class="NavHeader" id="configDetailHeader" style="width: 180px">
                            <asp:Label runat="server" ID="lblReviewTitle">Authorise Review Assignment</asp:Label>
                        </div>
                        <table class="navcontainer" cellspacing="1" cellpadding="5" width="100%" border="0">

                            <tbody>

                                <tr>
                                    <td align="center" valign="top">


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
                                                            CssClass="gridEditButton"
                                                            ID="lblAuthoriseAssignment"
                                                            OnClick="lblAuthoriseAssignment_Click"
                                                            CausesValidation="false"
                                                            runat="server" ToolTip="Authorize Assignment">Authorize
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
