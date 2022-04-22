
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Public Class Pager
    Inherits System.Web.UI.UserControl
    Public Event PageChanged As PageChangedEventHandler

    Private _currentPageNumber As Integer
    Public Property CurrentPageNumber() As Integer
        Get
            Return _currentPageNumber
        End Get
        Set(value As Integer)
            _currentPageNumber = value
        End Set
    End Property

    Private _totalPages As Integer
    Public Property TotalPages() As Integer
        Get
            Return _totalPages
        End Get
        Set(value As Integer)
            _totalPages = value
        End Set
    End Property

    Private _currentPageSize As Integer
    Public Property CurrentPageSize() As Integer
        Get
            Return _currentPageSize
        End Get
        Set(value As Integer)
            _currentPageSize = value
        End Set
    End Property

   

    Protected Sub ddlPageSize_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim args As New CustomPageChangeArgs
        args.CurrentPageSize = Convert.ToInt32(Me.ddlPageSize.SelectedItem.Value)
        args.CurrentPageNumber = 1
        args.TotalPages = Convert.ToInt32(Me.lblShowRecords.Text)
        Pager_PageChanged(Me, args)

        ddlPageNumber.Items.Clear()
        For count As Integer = 1 To Me.TotalPages
            ddlPageNumber.Items.Add(count.ToString())
        Next
        ddlPageNumber.Items(0).Selected = True
        lblShowRecords.Text = String.Format(" {0} ", Me.TotalPages.ToString())
    End Sub

    Private Sub Pager_PageChanged(sender As Object, e As CustomPageChangeArgs)
        RaiseEvent PageChanged(Me, e)
        'throw new Exception("The method or operation is not implemented.");
    End Sub

    Protected Sub ddlPageNumber_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim args As New CustomPageChangeArgs()
        args.CurrentPageSize = Convert.ToInt32(Me.ddlPageSize.SelectedItem.Value)
        args.CurrentPageNumber = Convert.ToInt32(Me.ddlPageNumber.SelectedItem.Text)
        args.TotalPages = Convert.ToInt32(Me.lblShowRecords.Text)
        Pager_PageChanged(Me, args)

        lblShowRecords.Text = String.Format(" {0} ", args.TotalPages.ToString())
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            For count As Integer = 1 To Me.TotalPages
                ddlPageNumber.Items.Add(count.ToString())
            Next
            ddlPageNumber.Items(0).Selected = True

            lblShowRecords.Text = String.Format(" {0} ", Me.TotalPages.ToString())
        End If
    End Sub
End Class

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
