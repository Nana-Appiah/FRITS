Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Partial Public Class LinkPager
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

    Private _totalRecords As Integer
    Public Property TotalRecords() As Integer
        Get
            Return _totalRecords
        End Get
        Set(value As Integer)
            _totalRecords = value
        End Set
    End Property


    Public Sub RefreshPager()
        Call RefreshDisplaySummary()
    End Sub


    Private lstPageNumbers As System.Collections.Generic.List(Of Integer)
    Private args As CustomPageChangeArgs

   

    Protected Sub rptPages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'If IsNumeric(e.Item.DataItem) Then
            If Convert.ToInt32(e.Item.DataItem) = Me.CurrentPageNumber Then
                DirectCast(e.Item.FindControl("lnkPageNumbers"), LinkButton).Style(HtmlTextWriterStyle.Display) = "none"
                DirectCast(e.Item.FindControl("lblPageNumbers"), Label).Text = e.Item.DataItem.ToString()
            Else
                DirectCast(e.Item.FindControl("lnkPageNumbers"), LinkButton).Text = e.Item.DataItem.ToString()
                DirectCast(e.Item.FindControl("lblPageNumbers"), Label).Style(HtmlTextWriterStyle.Display) = "none"
            End If
            'Else
            '    DirectCast(e.Item.FindControl("lnkPageNumbers"), LinkButton).Style(HtmlTextWriterStyle.Display) = "none"
            '    DirectCast(e.Item.FindControl("lblPageNumbers"), Label).Text = e.Item.DataItem.ToString()
            'End If
        End If
    End Sub

    Protected Sub lnkPageNumbers_Click(sender As Object, e As EventArgs)
        args = New CustomPageChangeArgs()

        Dim strPgNo = DirectCast(sender, LinkButton).Text.ToString()

        'If IsNumeric(strPgNo) Then
        args.CurrentPageNumber = Convert.ToInt32(DirectCast(sender, LinkButton).Text)
        args.CurrentPageSize = Me.ddlPageSize.SelectedItem.Value.ToString
        Me.CurrentPageNumber = args.CurrentPageNumber
        Me.CurrentPageSize = args.CurrentPageSize
        Pager_PageChanged(Me, args)
        'End If

        Call RefreshDisplaySummary()
    End Sub

    Protected Sub lnkGOFPage_Click(sender As Object, e As EventArgs)
        args = New CustomPageChangeArgs()
        Select Case DirectCast(sender, LinkButton).ID
            Case "lnkFirstPage"
                args.CurrentPageNumber = 1
                HttpContext.Current.Items.Add("currentPage", 1)
                Exit Select
            Case "lnkPreviousPage"
                args.CurrentPageNumber = Convert.ToInt32(lblCurrentPage.Text) - 1
                Exit Select
            Case "lnkNextPage"
                args.CurrentPageNumber = Convert.ToInt32(lblCurrentPage.Text) + 1
                Exit Select
            Case "lnkLastPage"
                args.CurrentPageNumber = Convert.ToInt32(lblTotalRecords.Text)
                Exit Select
        End Select

        Me.CurrentPageNumber = args.CurrentPageNumber
        args.CurrentPageSize = ddlPageSize.SelectedItem.Value().ToString
        Me.CurrentPageSize = args.CurrentPageSize
        Pager_PageChanged(Me, args)

        Call RefreshDisplaySummary()
        
    End Sub

    Private Sub RefreshDisplaySummary()
        BindRepeater()
        lblCurrentPage.Text = IIf(Me.CurrentPageNumber.ToString() = "0", 1, Me.CurrentPageNumber.ToString())
        lblTotalRecords.Text = IIf(Me.TotalPages.ToString() = "0", 1, Me.TotalPages.ToString())
        lblTotalItems.Text = Me.TotalRecords.ToString()
        SetUnsetLinkButtons()
    End Sub


    Private Sub Pager_PageChanged(sender As Object, e As CustomPageChangeArgs)
        RaiseEvent PageChanged(Me, e)
        'throw new Exception("The method or operation is not implemented.");
    End Sub

    Private Sub BindRepeater()
        lstPageNumbers = New System.Collections.Generic.List(Of Integer)()
        Dim cCheck = Me.TotalPages - 10
        For count As Integer = 1 To Me.TotalPages
            'If count <= 10 Then
            lstPageNumbers.Add(count)
            'ElseIf count = 11 Then
            '    lstPageNumbers.Add("....")
            'ElseIf count >= cCheck Then
            '    lstPageNumbers.Add(count)
            'End If
        Next
        rptPages.DataSource = lstPageNumbers
        rptPages.DataBind()

        If lstPageNumbers.Count > 25 Then
            right.Visible = False
        Else
            right.Visible = True
        End If

    End Sub

    Private Sub SetUnsetLinkButtons()

        If Me.CurrentPageNumber <= 1 AndAlso Me.TotalPages = 1 Then
            lnkFirstPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkPreviousPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblFirstPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblPreviousPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblLastPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblNextPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkLastPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkNextPage.Style(HtmlTextWriterStyle.Display) = "none"
        ElseIf Me.CurrentPageNumber <= 1 AndAlso Me.TotalPages > 1 Then
            lnkFirstPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkPreviousPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblFirstPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblPreviousPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblLastPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblNextPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkLastPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkNextPage.Style(HtmlTextWriterStyle.Display) = "inline"
        ElseIf Me.CurrentPageNumber >= Me.TotalPages Then
            lnkFirstPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkPreviousPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblFirstPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblPreviousPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblLastPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblNextPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkLastPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkNextPage.Style(HtmlTextWriterStyle.Display) = "none"
        Else
            lnkFirstPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkPreviousPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lblFirstPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblPreviousPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblLastPage.Style(HtmlTextWriterStyle.Display) = "none"
            lblNextPage.Style(HtmlTextWriterStyle.Display) = "none"
            lnkLastPage.Style(HtmlTextWriterStyle.Display) = "inline"
            lnkNextPage.Style(HtmlTextWriterStyle.Display) = "inline"
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            ' ddlPageSize.SelectedValue = 50
            Me.CurrentPageNumber = 1
            Call RefreshDisplaySummary()
        End If

    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim args As New CustomPageChangeArgs
        args.CurrentPageSize = Convert.ToInt32(Me.ddlPageSize.SelectedItem.Value)
        args.CurrentPageNumber = 1
        args.TotalPages = Convert.ToInt32(Me.lblTotalRecords.Text) 'lblShowRecords
        Pager_PageChanged(Me, args)

        Call RefreshDisplaySummary()
       
    End Sub
End Class