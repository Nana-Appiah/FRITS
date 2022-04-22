<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Group.aspx.vb" Inherits="FRITS.Group"
    Theme="ProfileManager" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Group</title>

    <script type="text/javascript" src="../../Resources/Scripts/global.js"></script>

    <script type="text/javascript">
		<!--	
	    function addMember()
		{
		    // we must encode url and decode it in iframe as source url
		    var url = base64encode(applicationPath + '/Utilities/ProfileManager/Selector.aspx?multipleselection=true&objecttypenamespace=Users'); 
		
			var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url='+ url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');
										
			if(retval != undefined)
			{
				lstMemberCallBack.Callback('new:member', retval);
			}
		}
		function delMember()
		{
			var snode = lstMember.get_selectedNode();
			
			if (confirm('Do you really want to remove the selected member?'))
			{
				lstMemberCallBack.Callback('delete:member', snode.get_value());
			}
		}
		function addMemberOf()
		{
		    // we must encode url and decode it in iframe as source url
		    var url = base64encode(applicationPath + '/Utilities/ProfileManager/Selector.aspx?multipleselection=true&objecttypenamespace=GroupsRolesOrgUnits'); 
		
			var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url='+ url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');
									
			if(retval != undefined)
			{
				lstMemberOfCallBack.Callback('new:memberof', retval);
			}
		}
		function delMemberOf()
		{
			var snode = lstMemberOf.get_selectedNode();
		
			if (confirm('Do you really want to remove the selected member of?'))
			{
				lstMemberOfCallBack.Callback('delete:memberof', snode.get_value());
			}
		}
		function addDelegatedUser()
		{
		    // we must encode url and decode it in iframe as source url
		    var url = base64encode(applicationPath + '/Utilities/ProfileManager/Selector.aspx?multipleselection=true&objecttypenamespace=Users'); 
		
			var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url='+ url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');
							
			if(retval != undefined)
			{
				lstDelegatedUserCallBack.Callback('new:delegateduser', retval);
			}
		}
		function delDelegatedUser()
		{
			var snode = lstDelegatedUser.get_selectedNode();
		
			if (confirm('Do you really want to remove the selected delegated user?'))
			{
				lstDelegatedUserCallBack.Callback('delete:delegateduser', snode.get_value());
			}
		}
        function PickerFrom_OnDateChange(sender, eventArgs)
          {
            var fromDate = PickerFrom.getSelectedDate();
            var toDate = PickerTo.getSelectedDate();
            CalendarFrom.setSelectedDate(fromDate);
            if (fromDate > toDate)
            {
              PickerTo.setSelectedDate(fromDate);
              CalendarTo.setSelectedDate(fromDate);
            }
          }
          function PickerTo_OnDateChange(sender, eventArgs)
          {
            var fromDate = PickerFrom.getSelectedDate();
            var toDate = PickerTo.getSelectedDate();
            CalendarTo.setSelectedDate(toDate);
            if (fromDate > toDate)
            {
              PickerFrom.setSelectedDate(toDate);
              CalendarFrom.setSelectedDate(toDate);
            }
          }
          function CalendarFrom_OnChange(sender, eventArgs)
          {
            var fromDate = CalendarFrom.getSelectedDate();
            var toDate = PickerTo.getSelectedDate();
            PickerFrom.setSelectedDate(fromDate);
            if (fromDate > toDate)
            {
              PickerTo.setSelectedDate(fromDate);
              CalendarTo.setSelectedDate(fromDate);
            }
          }
          function CalendarTo_OnChange(sender, eventArgs)
          {
            var fromDate = PickerFrom.getSelectedDate();
            var toDate = CalendarTo.getSelectedDate();
            PickerTo.setSelectedDate(toDate);
            if (fromDate > toDate)
            {
              PickerFrom.setSelectedDate(toDate);
              CalendarFrom.setSelectedDate(toDate);
            }
          }
          function ButtonFrom_OnClick(event)
          {
            if (CalendarFrom.get_popUpShowing())
            {
              CalendarFrom.hide();
            }
            else
            {
              CalendarFrom.setSelectedDate(PickerFrom.getSelectedDate());
              CalendarFrom.show();
            }
          }
          function ButtonTo_OnClick(event)
          {
            if (CalendarTo.get_popUpShowing())
            {
              CalendarTo.hide();
            }
            else
            {
              CalendarTo.setSelectedDate(PickerTo.getSelectedDate());
              CalendarTo.show();
            }
          }
          function ButtonFrom_OnMouseUp(event)
          {
            if (CalendarFrom.get_popUpShowing())
            {
              event.cancelBubble=true;
              event.returnValue=false;
              return false;
            }
            else
            {
              return true;
            }
          }
          function ButtonTo_OnMouseUp(event)
          {
            if (CalendarTo.get_popUpShowing())
            {
              event.cancelBubble=true;
              event.returnValue=false;
              return false;
            }
            else
            {
              return true;
            }
          }
		//-->
    </script>

</head>
<body class="dialog">
    <form id="frm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContentPane" runat="server">
        <div id="Toolbar" style="border-right: silver 0px solid; padding-right: 5px; border-top: silver 0px solid;
            padding-left: 5px; padding-bottom: 0px; border-left: silver 0px solid; padding-top: 0px;
            border-bottom: silver 0px solid; background-color: #FFFFFF;" align="right" runat="server">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" width="100%" height="25">
                        <ComponentArt:ToolBar ID="tbrMain" CssClass="h_toolbar" ImagesBaseUrl="~/Images/"
                            DefaultItemCssClass="item" DefaultItemHoverCssClass="itemHover" DefaultItemActiveCssClass="itemActive"
                            DefaultItemCheckedCssClass="itemChecked" DefaultItemCheckedHoverCssClass="itemActive"
                            DefaultItemTextImageRelation="ImageOnly" DefaultItemImageHeight="16" DefaultItemImageWidth="16"
                            ItemSpacing="1" Orientation="Horizontal" runat="server" AutoPostBackOnSelect="true"
                            AutoPostBackOnCheckChanged="true">
                            <Items>
                                <ComponentArt:ToolBarItem ItemType="Command" Value="Save" Text="Save" ToolTip="Save"
                                    TextImageRelation="ImageBeforeText" ImageUrl="saveitem.gif" />
                                <ComponentArt:ToolBarItem ItemType="Separator" Text="Help" ToolTip="Help" TextImageRelation="ImageOnly"
                                    ImageUrl="h_break.gif" ImageHeight="16" ImageWidth="2" />
                                <ComponentArt:ToolBarItem ToolTip="Help" ImageUrl="help.gif" />
                            </Items>
                        </ComponentArt:ToolBar>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-right: 15px; padding-left: 15px; padding-bottom: 15px; padding-top: 0px;">
            <table height="100%" cellspacing="0" cellpadding="0" width="100%" align="center"
                border="0">
                <tr>
                    <td valign="top">
                        <ComponentArt:TabStrip ID="TabStrip" runat="server" DefaultItemLookId="DefaultTabLook"
                            DefaultSelectedItemLookId="SelectedTabLook" DefaultDisabledItemLookId="DisabledTabLook"
                            DefaultGroupTabSpacing="1" MultiPageId="MultiPage" SiteMapXmlFile="~/Resources/XML/tabGroup.xml"
                            ImagesBaseUrl="~/Images" CssClass="TopGroup1">
                            <ItemLooks>
                                <ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover"
                                    LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="5" LabelPaddingBottom="4"
                                    LeftIconUrl="tab_left_icon.gif" RightIconUrl="tab_right_icon.gif" HoverLeftIconUrl="hover_tab_left_icon.gif"
                                    HoverRightIconUrl="hover_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21"
                                    RightIconWidth="3" RightIconHeight="21" />
                                <ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="10"
                                    LabelPaddingRight="10" LabelPaddingTop="4" LabelPaddingBottom="4" LeftIconUrl="selected_tab_left_icon.gif"
                                    RightIconUrl="selected_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21"
                                    RightIconWidth="3" RightIconHeight="21" />
                            </ItemLooks>
                        </ComponentArt:TabStrip>
                    </td>
                </tr>
                <tr>
                    <td valign="top" height="100%">
                        <ComponentArt:MultiPage ID="MultiPage" runat="server" CssClass="MultiPage1">
                            <ComponentArt:PageView CssClass="PageContent" runat="server" ID="Pageview0">
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <div style="padding: 0px">
                                            <table cellspacing="0" cellpadding="0" border="0" style="width: 100%; height: 210px;"
                                                align="center">
                                                <tr>
                                                    <td nowrap valign="top">
                                                        <table cellspacing="0" cellpadding="3" align="center" border="0" width="100%">
                                                            <tr>
                                                                <td nowrap height="25">
                                                                    <p>
                                                                        Group Name</p>
                                                                </td>
                                                                <td nowrap height="25">
                                                                    :
                                                                </td>
                                                                <td nowrap height="25">
                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="TextBox" Width="200"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap height="25">
                                                                    Description
                                                                </td>
                                                                <td nowrap height="25">
                                                                    :
                                                                </td>
                                                                <td nowrap height="25">
                                                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="TextBox" Width="200"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap height="25">
                                                                    <p>
                                                                        Valid From</p>
                                                                </td>
                                                                <td nowrap align="center" height="25">
                                                                    :
                                                                </td>
                                                                <td nowrap height="25">
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td onmouseup="ButtonFrom_OnMouseUp(event)">
                                                                                <ComponentArt:Calendar ID="PickerFrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd-MMM-yyyy"
                                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                                    <ClientEvents>
                                                                                        <SelectionChanged EventHandler="PickerFrom_OnDateChange" />
                                                                                    </ClientEvents>
                                                                                </ComponentArt:Calendar>
                                                                            </td>
                                                                            <td style="font-size: 10px;">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img id="calendar_from_button" alt="" onclick="ButtonFrom_OnClick(event)" onmouseup="ButtonFrom_OnMouseUp(event)"
                                                                                    class="calendar_button" src="../../images/btn_calendar.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap height="25">
                                                                    Valid To
                                                                </td>
                                                                <td nowrap align="center" height="25">
                                                                    :
                                                                </td>
                                                                <td nowrap height="25">
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td onmouseup="ButtonTo_OnMouseUp(event)">
                                                                                <ComponentArt:Calendar ID="PickerTo" runat="server" PickerFormat="Custom" PickerCustomFormat="dd-MMM-yyyy"
                                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                                    <ClientEvents>
                                                                                        <SelectionChanged EventHandler="PickerTo_OnDateChange" />
                                                                                    </ClientEvents>
                                                                                </ComponentArt:Calendar>
                                                                            </td>
                                                                            <td style="font-size: 10px;">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img id="calendar_to_button" alt="" onclick="ButtonTo_OnClick(event)" onmouseup="ButtonTo_OnMouseUp(event)"
                                                                                    class="calendar_button" src="../../images/btn_calendar.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <ComponentArt:Calendar runat="server" ID="CalendarFrom" AllowMultipleSelection="false"
                                                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                            PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                            DayHeaderCssClass="dayheader" DayCssClass="day" DayHoverCssClass="dayhover" OtherMonthDayCssClass="othermonthday"
                                                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                            ImagesBaseUrl="~/Images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="CalendarFrom_OnChange" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                        <ComponentArt:Calendar runat="server" ID="CalendarTo" AllowMultipleSelection="false"
                                                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                            PopUp="Custom" PopUpExpandControlId="calendar_to_button" CalendarTitleCssClass="title"
                                                            DayHeaderCssClass="dayheader" DayCssClass="day" DayHoverCssClass="dayhover" OtherMonthDayCssClass="othermonthday"
                                                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                            ImagesBaseUrl="~/Images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="CalendarTo_OnChange" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger EventName="ItemCommand" ControlID="tbrMain" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                                    <ProgressTemplate>
                                        <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0">
                                            <tr>
                                                <td valign="middle" nowrap="nowrap" align="center" height="100%" width="100%">
                                                    <img alt="" src="../../Images/spinner.gif" border="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </ComponentArt:PageView>
                            <ComponentArt:PageView CssClass="PageContent" runat="server" ID="Pageview1">
                                <div style="padding: 0px">
                                    <table cellspacing="0" cellpadding="0" border="0" style="width: 100%; height: 210px;"
                                        align="center">
                                        <tr>
                                            <td nowrap valign="top">
                                                <ComponentArt:CallBack ID="lstMemberOfCallBack" CssClass="CallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstMemberOf" runat="server" Height="200" Width="100%"
                                                            EnableViewState="true" KeyboardEnabled="false" ShowLines="true" CssClass="TreeView"
                                                            NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode"
                                                            LineImageWidth="19" LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16"
                                                            NodeLabelPadding="3" ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif"
                                                            ImagesBaseUrl="~/Images/" LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                        </ComponentArt:TreeView>
                                                        <asp:Literal ID="ltlMemberOfScript" runat="server"></asp:Literal>
                                                    </Content>
                                                    <LoadingPanelClientTemplate>
                                                        <table height="150" width="200" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td style="font-size: 10px;">
                                                                                Loading...&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img src="../../Images/spinner.gif" width="16" height="16" border="0">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                                <ComponentArt:Menu ID="MenuMemberOf" runat="server" EnableViewState="false" ImagesBaseUrl="~/Images/"
                                                    ShadowEnabled="true" ContextMenu="ControlSpecific" ContextControlId="Pageview1"
                                                    DefaultGroupItemSpacing="1" DefaultItemLookId="DefaultItemLook3" DefaultGroupCssClass="MenuGroup"
                                                    Orientation="Vertical">
                                                    <ItemLooks>
                                                        <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                                            ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                                            LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                                                        <ComponentArt:ItemLook LookId="BreakItem3" CssClass="MenuBreak" />
                                                    </ItemLooks>
                                                    <Items>
                                                        <ComponentArt:MenuItem Text="Add New" Look-LeftIconUrl="icon_new.gif" Look-HoverLeftIconUrl="icon_new_over.gif"
                                                            NavigateUrl="javascript:addMemberOf();">
                                                        </ComponentArt:MenuItem>
                                                        <ComponentArt:MenuItem Text="Delete" Look-LeftIconUrl="icon_delete.gif" Look-HoverLeftIconUrl="icon_delete_over.gif"
                                                            NavigateUrl="javascript:delMemberOf();">
                                                        </ComponentArt:MenuItem>
                                                    </Items>
                                                </ComponentArt:Menu>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ComponentArt:PageView>
                            <ComponentArt:PageView CssClass="PageContent" runat="server" ID="Pageview2" NAME="Pageview2">
                                <div style="padding: 0px">
                                    <table cellspacing="0" cellpadding="0" border="0" style="width: 100%; height: 210px;"
                                        align="center">
                                        <tr>
                                            <td nowrap valign="top">
                                                <ComponentArt:CallBack ID="lstMemberCallBack" CssClass="CallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstMember" runat="server" Height="200" Width="100%" EnableViewState="true"
                                                            KeyboardEnabled="false" ShowLines="true" CssClass="TreeView" NodeCssClass="TreeNode"
                                                            SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode" LineImageWidth="19"
                                                            LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16" NodeLabelPadding="3"
                                                            ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif" ImagesBaseUrl="~/Images/"
                                                            LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                        </ComponentArt:TreeView>
                                                        <asp:Literal ID="ltlMemberScript" runat="server"></asp:Literal>
                                                    </Content>
                                                    <LoadingPanelClientTemplate>
                                                        <table height="150" width="200" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td style="font-size: 10px;">
                                                                                Loading...&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img src="../../Images/spinner.gif" width="16" height="16" border="0">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                                <ComponentArt:Menu ID="MenuMember" runat="server" EnableViewState="false" ImagesBaseUrl="~/Images/"
                                                    ShadowEnabled="true" ContextMenu="ControlSpecific" ContextControlId="Pageview2"
                                                    DefaultGroupItemSpacing="1" DefaultItemLookId="DefaultItemLook3" DefaultGroupCssClass="MenuGroup"
                                                    Orientation="Vertical">
                                                    <ItemLooks>
                                                        <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                                            ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                                            LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                                                        <ComponentArt:ItemLook LookId="BreakItem3" CssClass="MenuBreak" />
                                                    </ItemLooks>
                                                    <Items>
                                                        <ComponentArt:MenuItem Text="Add New" Look-LeftIconUrl="icon_new.gif" Look-HoverLeftIconUrl="icon_new_over.gif"
                                                            NavigateUrl="javascript:addMember();">
                                                        </ComponentArt:MenuItem>
                                                        <ComponentArt:MenuItem Text="Delete" Look-LeftIconUrl="icon_delete.gif" Look-HoverLeftIconUrl="icon_delete_over.gif"
                                                            NavigateUrl="javascript:delMember();">
                                                        </ComponentArt:MenuItem>
                                                    </Items>
                                                </ComponentArt:Menu>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ComponentArt:PageView>
                        </ComponentArt:MultiPage>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    </form>
</body>
</html>
