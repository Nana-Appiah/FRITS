Imports System.Threading.Tasks

Public Class AuthoriseReview
    Inherits PageBase

    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await ShowReviews()
        End If
    End Sub

    Protected Async Sub OnReviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.ShowReviews()
    End Sub


    Private Async Function ShowReviews() As Task
        Dim _reviewList As List(Of ReviewVm)

        _reviewList = Await _itService.Get_UnAuthorisd_Review_List_Async()

        grdReviews.DataSource = _reviewList
        grdReviews.DataBind()
    End Function

    Private Sub clearReviewControls()
        rwSearch.Visible = True
    End Sub


    Protected Async Sub lbtnAuthoriseReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        Dim _result = Await _itService.Authorise_Review_Async(recordId)

        If _result.Status Then
            ShowSuccessMessage(_result.Message)
            Await ShowReviews()
            Call clearReviewControls()
        Else
            ShowErrorMessage(_result.Message)
        End If


    End Sub

End Class