<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Selector.aspx.vb" Inherits="FRITS.Selector" Theme="ProfileManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Selector</title>

    <script type="text/javascript" src="../Resources/Scripts/global.js"></script>

    <script type="text/javascript">
    
			function selectItem()
			{
				try
				{
					var collSel = document.all['lstSelection']
					for (i=0; i< collSel.options.length; i++)
					{
						if (collSel.options(i).selected)
						{
							addItem(collSel.options(i).value, collSel.options(i).text);
						}						
					}
				}
				catch (e)
				{
				}
			}
			function removeSelItems()
			{
				try
				{
					var coll = document.all['lstSelectedItems']
					for (i=0; i< coll.options.length; i++)
					{
						if (coll.options(i).selected)
						{
							coll.options.remove(i);
							removeSelItems();
						}						
					}
				}
				catch (e)
				{
				}
			}
			function addItem(val, txt)
			{		    
				var coll = document.all['lstSelectedItems'];
				if (coll != null)
				{
					found = 0;
					if (coll.options.length>0) {
						for (j=0; j< coll.options.length; j++)
						{		
						if (coll.options(j).text == txt)
							found = 1;  
						}
					}
					if (found == 0)
					{		  		  
						var oOption = document.createElement("OPTION");
						coll.options.add(oOption);
						oOption.innerText = ""+txt;
						oOption.value = ""+val;
					}
				}
			}
			function ReturnResult()
			{
				var result = '';
				var coll = document.all['lstSelectedItems'];						
					
				for (i=0; i< coll.options.length; i++)						
				{
					coll.options.item(i).value;
					result += coll.options.item(i).value +';'
				}
				if (result == '')
				{
					alert("You haven't selected any item. Please select at least one item and click OK.");
				}
				else
				{
					if (result.length > 4000)
					{
					alert("You're trying to add too many items at once. It's possible that some of the items you have selected won't be stored successfully. If this occurs please open this dialog again and try to add remaining items.");
					}					 					  
					window.returnValue = result;
					window.close();
				}
			}
    </script>

</head>
<body class="dialog" onload="parent.window.document.title='Selector';">
    <form id="frm" runat="server">
    <asp:Panel ID="pnlContentPane" runat="server">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" align="center"
            border="0">
            <tr>
                <td valign="top" height="100%">
                    <table id="Table1" cellspacing="0" cellpadding="5" align="center" border="0">
                        <tr>
                            <td nowrap height="5">
                            </td>
                            <td nowrap align="center" height="5">
                            </td>
                            <td valign="top" nowrap height="5">
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <asp:ListBox ID="lstSelection" runat="server" Width="250px" Height="350px" SelectionMode="Multiple"
                                    CssClass="TextBox"></asp:ListBox>
                            </td>
                            <td nowrap align="center">
                                <p>
                                    <input class="Button" style="width: 75px; height: 20px" onclick="selectItem();" type="button"
                                        size="20" value="Add >" /></p>
                                <p align="center">
                                    <input class="Button" style="width: 75px; height: 20px" onclick="removeSelItems();"
                                        type="button" value=" < Remove" /></p>
                            </td>
                            <td valign="top" nowrap>
                                <asp:ListBox ID="lstSelectedItems" runat="server" Width="250px" Height="350px" SelectionMode="Multiple"
                                    CssClass="TextBox"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" nowrap align="center">
                            </td>
                            <td style="height: 24px" nowrap align="center">
                            </td>
                            <td style="height: 24px" nowrap align="right">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" nowrap align="center">
                    <input class="Button" style="width: 75px; height: 24px" onclick="javascript:ReturnResult()"
                        type="button" value="OK">&nbsp;<input class="Button" style="width: 75px; height: 24px"
                            onclick="javascript:window.close();" type="Button" value="Cancel">
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
