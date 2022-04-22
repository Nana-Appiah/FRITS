<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="FRITS.ProfileManager_Default"
    Theme="ProfileManager" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profile Manager</title>

    <script type="text/javascript" src="../../Resources/Scripts/global.js"></script>

    <script type="text/javascript">
        //<![CDATA[

        function AddOperator() {

            var resourcenode = lstApplicationParts.get_selectedNode()

            if (resourcenode != null) {
                // we must encode url and decode it in iframe as source url
                var url = base64encode(applicationPath + '/Utilities/ProfileManager/Selector.aspx?multipleselection=true&objecttypenamespace=arobject.aroperator');

                var operator = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');

                if (operator != null) {
                    cmdCallBack.Callback('new:operator', resourcenode.get_value(), operator);
                    lstAuthorisedOperatorsCallBack.Callback(resourcenode.get_value());
                }
            }
            else {
                alert('Please select a form before adding an operator.');
            }
        }

        function DeleteOperator() {

            var resourcenode = lstApplicationParts.get_selectedNode()
            var operatornode = lstAuthorisedOperators.get_selectedNode()

            if (operatornode != null) {
                if (confirm('Do you really want to delete the selected operator?')) {
                    cmdCallBack.Callback('del:operator', resourcenode.get_value(), operatornode.get_value());
                    lstAuthorisedOperatorsCallBack.Callback(resourcenode.get_value());
                }
            }
            else {
                alert('Please select an operator to delete.');
            }
        }

        function ReloadOperator() {
            lstApplicationPartsCallBack.Callback('load:operator');
        }

        function AddOU() {

            var url = base64encode(applicationPath + '/Utilities/ProfileManager/OrgUnit.aspx'); // we must encode url and decode it in iframe as source url

            var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

            if (retval != undefined) {
                lstOrganizationalUnitsCallBack.Callback('add:ou', retval);
            }
        }

        function DeleteOU() {

            var snode = lstOrganizationalUnits.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                if (snode.get_text() == 'Group') {
                    alert('You do not have permission to delete OU: Group ');
                    return;
                }
                if (confirm('Do you really want to delete the selected user?')) {
                    lstOrganizationalUnitsCallBack.Callback('del:ou', snode.get_value());
                }
            }
            else {
                alert('Please select an OU to delete.');
            }
        }

        function OUProperties() {

            var snode = lstOrganizationalUnits.get_selectedNode();

            if (snode != null && snode.get_value() != 'root') {
                var url = base64encode(applicationPath + '/Utilities/ProfileManager/OrgUnit.aspx?id=' + snode.get_value()); // we must encode url and decode it in iframe as source url

                var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

                if (retval != undefined) {
                    lstOrganizationalUnitsCallBack.Callback('load:user');
                }
            }
            else {
                alert('Please select an OU to view properties.');
            }
        }

        function ReloadOU() {
            lstOrganizationalUnitsCallBack.Callback('load:ou');
        }

        function AddUser() {

            var url = base64encode(applicationPath + '/Utilities/ProfileManager/User.aspx'); // we must encode url and decode it in iframe as source url

            var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:465px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

            if (retval != undefined) {
                lstUsersCallBack.Callback('load:user', retval);
            }
        }

        function ImportActiveDirectory() {

            //var url = base64encode('Selector.aspx?multipleselection=true&objecttypenamespace=ActiveDirectory'); // we must encode url and decode it in iframe as source url

            //var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');	

            //if(retval != undefined)
            //{
            //	lstUsersCallBack.Callback('new:aduser', retval);
            //}

            alert('Not Available');
        }

        function ImportCustomTable() {

            //var url = base64encode(applicationPath + '/Utilities/ProfileManager/Selector.aspx?multipleselection=true&objecttypenamespace=CustomTable'); // we must encode url and decode it in iframe as source url

            //var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:450px;dialogWidth:635px;center:yes;resizable:no;scroll:no;status:no;help:no');	

            //if(retval != undefined)
            //{
            //	lstUsersCallBack.Callback('new:ctuser', retval);
            //}

            lstUsersCallBack.Callback('import:custom');
        }

        function ImportFile() {

            var url = base64encode(applicationPath + '/Utilities/ProfileManager/Ufile.aspx'); // we must encode url and decode it in iframe as source url

            var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:225px;dialogWidth:500px;center:yes;resizable:no;scroll:no;status:no;help:no');

            if (retval != undefined) {
                lstUsersCallBack.Callback('load:user', retval);
            }
        }

        function DeleteUser() {

            var snode = lstUsers.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                if (snode.get_text() == 'Administrator') {
                    alert('You do not have permission to delete user: Administrator ');
                    return;
                }
                for (var i = 0; i < user_has_record.length; i++) {
                    if (user_has_record[i] == snode.get_value()) {
                        alert('Delete aborted.');
                        return;
                    }
                }
                if (confirm('Do you really want to delete the selected user?')) {
                    lstUsersCallBack.Callback('del:user', snode.get_value());
                }
            }
            else {
                alert('Please select a user to delete.');
            }
        }

        function ReloadUser() {
            lstUsersCallBack.Callback('load:user');
        }

        function UserProperties() {

            var snode = lstUsers.get_selectedNode();

            if (snode != null && snode.get_value() != 'root') {
                var url = base64encode(applicationPath + '/Utilities/ProfileManager/User.aspx?id=' + snode.get_value()); // we must encode url and decode it in iframe as source url

                var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:465px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

                if (retval != undefined) {
                    lstUsersCallBack.Callback('load:user');
                }
            }
            else {
                alert('Please select a user to view properties.');
            }
        }

        function AddGroup() {

            var url = base64encode(applicationPath + '/Utilities/ProfileManager/Group.aspx'); // we must encode url and decode it in iframe as source url

            var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

            if (retval != undefined) {
                lstGroupsCallBack.Callback('load:group', retval);
            }
        }

        function DeleteGroup() {

            var snode = lstGroups.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                if (snode.get_text() == 'All Users') {
                    alert('You do not have permission to delete All Users group');
                    return;
                }
                else if (snode.get_text() == 'Administrators') {
                    alert('You do not have permission to delete Administrators group');
                    return;
                }
                else if (confirm('Do you really want to delete the selected group?')) {
                    lstGroupsCallBack.Callback('del:group', snode.get_value());
                }
            }
            else {
                alert('Please select a group to delete.');
            }
        }

        function ReloadGroup() {
            lstGroupsCallBack.Callback('load:group');
        }

        function GroupProperties() {

            var snode = lstGroups.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                var url = base64encode(applicationPath + '/Utilities/ProfileManager/Group.aspx?id=' + snode.get_value()); // we must encode url and decode it in iframe as source url

                var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

                if (retval != undefined) {
                    lstGroupsCallBack.Callback('load:group');
                }

            }
            else {
                alert('Please select a group to view properties.');
            }
        }

        function AddRole() {

            var url = base64encode(applicationPath + '/Utilities/ProfileManager/Role.aspx'); // we must encode url and decode it in iframe as source url

            var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

            if (retval != undefined) {
                lstRolesCallBack.Callback('load:role', retval);
            }
        }

        function DeleteRole() {

            var snode = lstRoles.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                if (snode.get_text() == 'All Users') {
                    alert('You do not have permission to delete All Users Role');
                    return;
                }
                else if (snode.get_text() == 'Administrators') {
                    alert('You do not have permission to delete Administrators Role');
                    return;
                }
                else if (confirm('Do you really want to delete the selected Role?')) {
                    lstRolesCallBack.Callback('del:role', snode.get_value());
                }
            }
            else {
                alert('Please select a Role to delete.');
            }
        }

        function ReloadRole() {
            lstRolesCallBack.Callback('load:role');
        }

        function RoleProperties() {

            var snode = lstRoles.get_selectedNode()

            if (snode != null && snode.get_value() != 'root') {
                var url = base64encode(applicationPath + '/Utilities/ProfileManager/Role.aspx?id=' + snode.get_value()); // we must encode url and decode it in iframe as source url

                var retval = window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:315px;dialogWidth:425px;center:yes;resizable:no;scroll:no;status:no;help:no');

                if (retval != undefined) {
                    lstRolesCallBack.Callback('load:role');
                }

            }
            else {
                alert('Please select a Role to view properties.');
            }
        }

        function PermissionCheck(param) {

            var resourcenode = lstApplicationParts.get_selectedNode()
            var operatornode = lstAuthorisedOperators.get_selectedNode()

            if (resourcenode != null && operatornode != null) {
                if (operatornode.get_text() != 'Administrators') {
                    PermissionCallBack.Callback(resourcenode.get_value(), operatornode.get_value(), param, document.all['chk' + param].checked);
                }
                else {
                    document.all['chk' + param].checked = true; alert('Administrators previleges cannot be edited.');
                }
            }
            else {
                document.all['chk' + param].checked = false; alert('An authorised operator is not selected.');
            }
        }

        function lstApplicationParts_onNodeSelect(sender, eventArgs) {

            var node = eventArgs.get_node()

            if (node != null) {
                if (node.get_value() != "root") {
                    lstAuthorisedOperatorsCallBack.Callback(node.get_value()); PermissionsCallBack.Callback(node.get_value(), 0, "");
                }
            }
        }

        function lstAuthorisedOperators_onNodeSelect(sender, eventArgs) {

            var node = eventArgs.get_node()
            var resourcenode = lstApplicationParts.get_selectedNode()

            if (resourcenode != null && node != null) {
                if (node.get_text() == 'Administrators') {
                    alert('Administrators previleges cannot be edited.');
                }
                else if (node.get_text() == 'Operators') {
                    return;
                }
                PermissionsCallBack.Callback(resourcenode.get_value(), node.get_value(), node.get_text());
            }
        }

        function lstApplicationParts_onContextMenu(sender, eventArgs) {
            applicationPartsMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }

        function lstAuthorisedOperators_onContextMenu(sender, eventArgs) {
            authorisedOperatorsMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }

        function lstUsers_onContextMenu(sender, eventArgs) {
            usersMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }

        function lstGroups_onContextMenu(sender, eventArgs) {
            groupsMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }

        function lstRoles_onContextMenu(sender, eventArgs) {
            rolesMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }
        function lstOrganizationalUnits_onContextMenu(sender, eventArgs) {
            organizationalUnitsMenu.showContextMenu(eventArgs.get_event(), eventArgs.get_node());
        }

        function Grid_onContextMenu(sender, eventArgs) {
            Grid.select(eventArgs.get_item());
            GridContextMenu.showContextMenu(eventArgs.get_event());
            GridContextMenu.set_contextData(eventArgs.get_item());
        }

        function ReloadAuditLogs() {
            gridCallBack.Callback('reload');
        }

        function PurgeAuditLogs() {
            if (confirm("Are you sure you want to purge the log file?")) {
                gridCallBack.Callback('purge');
                gridCallBack.Callback('reload');
            }
        }

        function NoDelete() {
            alert('You can not delete user from the system.');
        }

        //-->			
    </script>

</head>
<body class="dialog" onload="parent.window.document.title='Profile Manager';" oncontextmenu="return false;">
    <form id="frm" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="pnlContentPane" runat="server">
        <table style="height: 100%; width: 100%" cellspacing="0" cellpadding="0" border="0"
            align="center">
            <tr>
                <td valign="top">
                    <ComponentArt:TabStrip ID="TabStrip" runat="server" DefaultItemLookId="DefaultTabLook"
                        DefaultSelectedItemLookId="SelectedTabLook" DefaultDisabledItemLookId="DisabledTabLook"
                        DefaultGroupTabSpacing="1" MultiPageId="MultiPage" SiteMapXmlFile="~/Resources/XML/tabProfileManager.xml"
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
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewGeneral">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" height="405">
                                    <tr>
                                        <td valign="top" width="400" nowrap>
                                            <div style="padding: 15px">
                                                <ComponentArt:CallBack ID="lstApplicationPartsCallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstApplicationParts" runat="server" Height="350" Width="90%"
                                                            EnableViewState="true" KeyboardEnabled="false" ShowLines="true" CssClass="TreeView"
                                                            NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode"
                                                            LineImageWidth="19" LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16"
                                                            NodeLabelPadding="3" ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif"
                                                            ImagesBaseUrl="~/Images/" LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                            <ClientEvents>
                                                                <NodeSelect EventHandler="lstApplicationParts_onNodeSelect" />
                                                                <ContextMenu EventHandler="lstApplicationParts_onContextMenu" />
                                                            </ClientEvents>
                                                        </ComponentArt:TreeView>
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
                                                                                <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                                <ComponentArt:CallBack ID="cmdCallBack" runat="server">
                                                </ComponentArt:CallBack>
                                            </div>
                                        </td>
                                        <td nowrap width="85" valign="top">
                                        </td>
                                        <td valign="top" nowrap>
                                            <br />
                                            <table cellspacing="0" cellpadding="0" border="0">
                                                <tr height="150" width="200">
                                                    <td>
                                                        <fieldset>
                                                            <legend>Authorised Operators</legend>
                                                            <br />
                                                            <table height="170" width="200" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td align="center" nowrap>
                                                                        <asp:Panel ID="lstAuthorisedOperatorsPanel" runat="server">
                                                                            <ComponentArt:CallBack ID="lstAuthorisedOperatorsCallBack" runat="server" EnableViewState="true">
                                                                                <Content>
                                                                                    <ComponentArt:TreeView ID="lstAuthorisedOperators" Height="150" Width="200" KeyboardEnabled="false"
                                                                                        CssClass="TreeView" NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode"
                                                                                        HoverNodeCssClass="HoverTreeNode" LineImageWidth="19" LineImageHeight="20" DefaultImageWidth="16"
                                                                                        DefaultImageHeight="16" NodeLabelPadding="3" ImagesBaseUrl="~/Images/" ParentNodeImageUrl="folders.gif"
                                                                                        LeafNodeImageUrl="folder.gif" ShowLines="true" LineImagesFolderUrl="../../Images/lines/"
                                                                                        EnableViewState="true" runat="server">
                                                                                        <ClientEvents>
                                                                                            <NodeSelect EventHandler="lstAuthorisedOperators_onNodeSelect" />
                                                                                            <ContextMenu EventHandler="lstAuthorisedOperators_onContextMenu" />
                                                                                        </ClientEvents>
                                                                                    </ComponentArt:TreeView>
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
                                                                                                            <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LoadingPanelClientTemplate>
                                                                            </ComponentArt:CallBack>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr height="150" width="200">
                                                    <td>
                                                        <br />
                                                        <fieldset>
                                                            <legend>Permissions</legend>
                                                            <table height="120" width="200" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td align="left" valign="top" nowrap>
                                                                        <ComponentArt:CallBack ID="PermissionsCallBack" runat="server">
                                                                            <Content>
                                                                                <asp:PlaceHolder ID="PermissionsPlaceHolder" runat="server">
                                                                                    <br />
                                                                                    <div id="divPermissions" runat="server">
                                                                                        <div id="divChange" runat="server">
                                                                                            <asp:CheckBox ID="chkChange" runat="server" Text="Change" onclick="javascript:PermissionCheck('Change');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divCreate" runat="server">
                                                                                            <asp:CheckBox ID="chkCreate" runat="server" Text="Create" onclick="javascript:PermissionCheck('Create');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divDelete" runat="server">
                                                                                            <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" onclick="javascript:PermissionCheck('Delete');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divRead" runat="server">
                                                                                            <asp:CheckBox ID="chkRead" runat="server" Text="Read" onclick="javascript:PermissionCheck('Read');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divUpload" runat="server">
                                                                                            <asp:CheckBox ID="chkUpload" runat="server" Text="Upload" onclick="javascript:PermissionCheck('Upload');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divSubmit" runat="server">
                                                                                            <asp:CheckBox ID="chkSubmit" runat="server" Text="Submit" onclick="javascript:PermissionCheck('Submit');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divCancel" runat="server">
                                                                                            <asp:CheckBox ID="chkCancel" runat="server" Text="Cancel" onclick="javascript:PermissionCheck('Cancel');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divAuthorize" runat="server">
                                                                                            <asp:CheckBox ID="chkAuthorize" runat="server" Text="Authorize" onclick="javascript:PermissionCheck('Authorize');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divDecline" runat="server">
                                                                                            <asp:CheckBox ID="chkDecline" runat="server" Text="Decline" onclick="javascript:PermissionCheck('Decline');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divRelease" runat="server">
                                                                                            <asp:CheckBox ID="chkRelease" runat="server" Text="Release" onclick="javascript:PermissionCheck('Release');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divReverse" runat="server">
                                                                                            <asp:CheckBox ID="chkReverse" runat="server" Text="Reverse" onclick="javascript:PermissionCheck('Reverse');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divCommit" runat="server">
                                                                                            <asp:CheckBox ID="chkCommit" runat="server" Text="Commit" onclick="javascript:PermissionCheck('Commit');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divManage" runat="server">
                                                                                            <asp:CheckBox ID="chkManage" runat="server" Text="Manage" onclick="javascript:PermissionCheck('Manage');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divGenerate" runat="server">
                                                                                            <asp:CheckBox ID="chkGenerate" runat="server" Text="Generate" onclick="javascript:PermissionCheck('Generate');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divPrint" runat="server">
                                                                                            <asp:CheckBox ID="chkPrint" runat="server" Text="Print" onclick="javascript:PermissionCheck('Print');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divExport" runat="server">
                                                                                            <asp:CheckBox ID="chkExport" runat="server" Text="Export" onclick="javascript:PermissionCheck('Export');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divList" runat="server">
                                                                                            <asp:CheckBox ID="chkList" runat="server" Text="List" onclick="javascript:PermissionCheck('List');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divSend" runat="server">
                                                                                            <asp:CheckBox ID="chkSend" runat="server" Text="Send" onclick="javascript:PermissionCheck('Send');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divScan" runat="server">
                                                                                            <asp:CheckBox ID="chkScan" runat="server" Text="Scan" onclick="javascript:PermissionCheck('Scan');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divCheck" runat="server">
                                                                                            <asp:CheckBox ID="chkCheck" runat="server" Text="Check" onclick="javascript:PermissionCheck('Check');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divActivate" runat="server">
                                                                                            <asp:CheckBox ID="chkActivate" runat="server" Text="Activate" onclick="javascript:PermissionCheck('Activate');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divReset" runat="server">
                                                                                            <asp:CheckBox ID="chkReset" runat="server" Text="Reset" onclick="javascript:PermissionCheck('Reset');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divOrder" runat="server">
                                                                                            <asp:CheckBox ID="chkOrder" runat="server" Text="Order" onclick="javascript:PermissionCheck('Order');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divReceive" runat="server">
                                                                                            <asp:CheckBox ID="chkReceive" runat="server" Text="Receive" onclick="javascript:PermissionCheck('Receive');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                        <div id="divSearch" runat="server">
                                                                                            <asp:CheckBox ID="chkSearch" runat="server" Text="Search" onclick="javascript:PermissionCheck('Search');">
                                                                                            </asp:CheckBox><br />
                                                                                        </div>
                                                                                    </div>
                                                                                    <ComponentArt:CallBack ID="PermissionCallBack" runat="server">
                                                                                    </ComponentArt:CallBack>
                                                                                </asp:PlaceHolder>
                                                                            </Content>
                                                                            <LoadingPanelClientTemplate>
                                                                                <table height="120" width="200" cellspacing="0" cellpadding="0" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                                                <tr>
                                                                                                    <td style="font-size: 10px;">
                                                                                                        Loading...&nbsp;
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </LoadingPanelClientTemplate>
                                                                        </ComponentArt:CallBack>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewUsers">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" height="405">
                                    <tr>
                                        <td valign="top" width="100%" nowrap>
                                            <div style="padding: 15px">
                                                <ComponentArt:CallBack ID="lstUsersCallBack" runat="server">
                                                    <Content>
                                                        <table id="tbl" cellspacing="0" cellpadding="3" border="0" runat=server>
                                                            <tr>
                                                                <td nowrap>
                                                                    Serach
                                                                </td>
                                                                <td nowrap align="center">
                                                                    :
                                                                </td>
                                                                <td nowrap>
                                                                    <asp:TextBox ID="txtSearchUser" CssClass="TextBox" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <ComponentArt:TreeView ID="lstUsers" runat="server" Height="335" Width="98%" EnableViewState="true"
                                                            KeyboardEnabled="false" ShowLines="true" CssClass="TreeView" NodeCssClass="TreeNode"
                                                            SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode" LineImageWidth="19"
                                                            LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16" NodeLabelPadding="3"
                                                            ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif" ImagesBaseUrl="~/Images/"
                                                            LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                            <ClientEvents>
                                                                <ContextMenu EventHandler="lstUsers_onContextMenu" />
                                                            </ClientEvents>
                                                        </ComponentArt:TreeView>
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
                                                                                <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewGroups">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" height="405">
                                    <tr>
                                        <td valign="top" width="100%" nowrap>
                                            <div style="padding: 15px">
                                                <ComponentArt:CallBack ID="lstGroupsCallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstGroups" runat="server" Height="350" Width="98%" EnableViewState="true"
                                                            KeyboardEnabled="false" ShowLines="true" CssClass="TreeView" NodeCssClass="TreeNode"
                                                            SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode" LineImageWidth="19"
                                                            LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16" NodeLabelPadding="3"
                                                            ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif" ImagesBaseUrl="~/Images/"
                                                            LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                            <ClientEvents>
                                                                <ContextMenu EventHandler="lstGroups_onContextMenu" />
                                                            </ClientEvents>
                                                        </ComponentArt:TreeView>
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
                                                                                <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewRoles">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" height="405">
                                    <tr>
                                        <td valign="top" width="100%" nowrap>
                                            <div style="padding: 15px">
                                                <ComponentArt:CallBack ID="lstRolesCallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstRoles" runat="server" Height="350" Width="98%" EnableViewState="true"
                                                            KeyboardEnabled="false" ShowLines="true" CssClass="TreeView" NodeCssClass="TreeNode"
                                                            SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode" LineImageWidth="19"
                                                            LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16" NodeLabelPadding="3"
                                                            ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif" ImagesBaseUrl="~/Images/"
                                                            LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                            <ClientEvents>
                                                                <ContextMenu EventHandler="lstRoles_onContextMenu" />
                                                            </ClientEvents>
                                                        </ComponentArt:TreeView>
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
                                                                                <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewOrganizationalUnits">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" height="405">
                                    <tr>
                                        <td valign="top" width="100%" nowrap>
                                            <div style="padding: 15px">
                                                <ComponentArt:CallBack ID="lstOrganizationalUnitsCallBack" runat="server">
                                                    <Content>
                                                        <ComponentArt:TreeView ID="lstOrganizationalUnits" runat="server" Height="350" Width="98%"
                                                            EnableViewState="true" KeyboardEnabled="false" ShowLines="true" CssClass="TreeView"
                                                            NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode"
                                                            LineImageWidth="19" LineImageHeight="20" DefaultImageWidth="16" DefaultImageHeight="16"
                                                            NodeLabelPadding="3" ParentNodeImageUrl="folders.gif" LeafNodeImageUrl="folder.gif"
                                                            ImagesBaseUrl="~/Images/" LineImagesFolderUrl="../../Images/lines/" AutoPostBackOnSelect="false">
                                                            <ClientEvents>
                                                                <ContextMenu EventHandler="lstOrganizationalUnits_onContextMenu" />
                                                            </ClientEvents>
                                                        </ComponentArt:TreeView>
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
                                                                                <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LoadingPanelClientTemplate>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="PageviewAuditLogs">
                            <div style="padding: 0px">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" height="405">
                                    <tr height="100%">
                                        <td valign="top" width="100%" nowrap>
                                            <ComponentArt:CallBack ID="gridCallBack" runat="server">
                                                <Content>
                                                    <ComponentArt:Grid ID="Grid" CssClass="Grid" FooterCssClass="GridFooter" RunningMode="Client"
                                                        PagerStyle="Numbered" PagerTextCssClass="PagerText" PageSize="18" ImagesBaseUrl="~/Images/"
                                                        Width="100%" Height="400" DataSourceID="LogSqlDataSource" runat="server">
                                                        <ClientEvents>
                                                            <ContextMenu EventHandler="Grid_onContextMenu" />
                                                        </ClientEvents>
                                                        <Levels>
                                                            <ComponentArt:GridLevel HeadingCellCssClass="HeadingCell" HeadingRowCssClass="HeadingRow"
                                                                HeadingTextCssClass="HeadingCellText" DataCellCssClass="DataCell" RowCssClass="Row"
                                                                SelectedRowCssClass="SelectedRow" SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif"
                                                                SortImageWidth="10" SortImageHeight="10">
                                                                <Columns>
                                                                    <ComponentArt:GridColumn DataField="LogTime" Width="150" HeadingText="Log Time" />
                                                                    <ComponentArt:GridColumn DataField="ByObjectName" Width="100" HeadingText="Operator" />
                                                                    <ComponentArt:GridColumn DataField="ToObjectName" Width="100" HeadingText="Resource" />
                                                                    <ComponentArt:GridColumn DataField="LogText" Width="200" HeadingText="Text" />
                                                                    <ComponentArt:GridColumn DataField="EventCode" Width="0" HeadingText="Event Code"
                                                                        Visible="False" />
                                                                </Columns>
                                                            </ComponentArt:GridLevel>
                                                        </Levels>
                                                    </ComponentArt:Grid>
                                                </Content>
                                                <LoadingPanelClientTemplate>
                                                    <table cellspacing="0" cellpadding="0" border="0" style="height: 300px; width: 100%;">
                                                        <tr>
                                                            <td align="center">
                                                                <table cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td style="font-size: 10px;">
                                                                            Loading...&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img alt="" src="../../Images/spinner.gif" width="16" height="16" border="0" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LoadingPanelClientTemplate>
                                            </ComponentArt:CallBack>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ComponentArt:PageView>
                    </ComponentArt:MultiPage>
                    <ComponentArt:Menu ID="applicationPartsMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuAppPart.xml" DefaultItemLookId="DefaultItemLook"
                        DefaultGroupItemSpacing="1" EnableViewState="false" ContextMenu="Custom" runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="authorisedOperatorsMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuAuthorisedOperators.xml"
                        DefaultItemLookId="DefaultItemLook2" DefaultGroupItemSpacing="1" EnableViewState="false"
                        ContextMenu="Custom" runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook2" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="usersMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuUsers.xml" DefaultItemLookId="DefaultItemLook3"
                        DefaultGroupItemSpacing="1" EnableViewState="false" ContextMenu="Custom" ShadowEnabled="true"
                        runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="groupsMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuGroups.xml" DefaultItemLookId="DefaultItemLook3"
                        DefaultGroupItemSpacing="1" EnableViewState="false" ContextMenu="Custom" ShadowEnabled="true"
                        runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="rolesMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuRoles.xml" DefaultItemLookId="DefaultItemLook3"
                        DefaultGroupItemSpacing="1" EnableViewState="false" ContextMenu="Custom" ShadowEnabled="true"
                        runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="organizationalUnitsMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuOrganizationalUnits.xml"
                        DefaultItemLookId="DefaultItemLook3" DefaultGroupItemSpacing="1" EnableViewState="false"
                        ContextMenu="Custom" ShadowEnabled="true" runat="server">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <ComponentArt:Menu ID="GridContextMenu" Orientation="Vertical" DefaultGroupCssClass="MenuGroup"
                        ImagesBaseUrl="~/Images/" SiteMapXmlFile="~/Resources/XML/menuAuditLogs.xml"
                        DefaultItemLookId="DefaultItemLook3" DefaultGroupItemSpacing="1" EnableViewState="false"
                        ContextMenu="ControlSpecific" ShadowEnabled="true" runat="server" ContextControlId="PageviewAuditLogs">
                        <ClientEvents>
                        </ClientEvents>
                        <ItemLooks>
                            <ComponentArt:ItemLook LookId="DefaultItemLook3" CssClass="MenuItem" HoverCssClass="MenuItemHover"
                                ExpandedCssClass="MenuItemHover" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10"
                                LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                        </ItemLooks>
                    </ComponentArt:Menu>
                    <asp:SqlDataSource ID="LogSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SecureAccessConnectionString %>"
                        SelectCommand="SELECT [LogTime], [EventCode], [ByObjectName], [LogText], [ToObjectName] FROM [View_AR_Log_Joined] WHERE ([EventCode] = 'FRITS')">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Literal ID="ltlUserScript" runat="server"></asp:Literal>
    </form>
</body>
</html>
