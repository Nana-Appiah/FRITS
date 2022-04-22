Imports System.Threading.Tasks

Public Class Reviews
    Inherits PageBase

    Private review As ReviewVm
    Public Property _review As ReviewVm
        Get
            review = DirectCast(Session.Item("_Review"), ReviewVm)
            review.Description = txtReviewDescription.Text
            review.ReviewCode = txtReviewCode.Text
            Return review
        End Get
        Set(value As ReviewVm)
            review = value
            Session.Add("_Review", review)
        End Set
    End Property


    Private selectedReview As ReviewVm
    Public Property _selectedReview As ReviewVm
        Get
            selectedReview = DirectCast(Session.Item("_selectedReview"), ReviewVm)
            Return selectedReview
        End Get
        Set(value As ReviewVm)
            selectedReview = value
            Session.Add("_selectedReview", selectedReview)
        End Set
    End Property


    Public ReadOnly Property _filterParms As RecordFilterOption
        Get
            Dim _params = New RecordFilterOption()
            _params.employeeId = Me.CurrentUser.ObjectID
            _params.reviewCode = IIf(cboSearchOption.SelectedValue = "0", _searchText, String.Empty)
            _params.reviewName = IIf(cboSearchOption.SelectedValue = "1", _searchText, String.Empty)

            If Not String.IsNullOrWhiteSpace(txtFilterReviewFrom.Text) Then
                _params.fromDate = CDate(txtFilterReviewFrom.Text)
            End If
            If Not String.IsNullOrWhiteSpace(txtFilterReviewTo.Text) Then
                _params.toDate = CDate(txtFilterReviewTo.Text)
            End If

            Return _params
        End Get
    End Property

    Private ReadOnly Property _searchText As String
        Get
            Return IIf(Not String.IsNullOrWhiteSpace(txtSearchReview.Text), txtSearchReview.Text, String.Empty)
        End Get
    End Property


    Private Sub NavigateReviewViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mlvReviews, viewIndex, "_activeReviewView")
    End Sub


    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await ShowReviews()
            Await Show_Closed_Reviews()
        End If
    End Sub


    Protected Async Sub grdReview_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedVal = grdReviews.SelectedValue.ToString
        Try
            Dim _rvId As Guid = Guid.Parse(selectedVal)
            Await Set_Selected_Review(_rvId)
        Catch ex As Exception

        End Try
    End Sub

    Private Async Function Set_Selected_Review(ByVal selectedReviewId As Guid) As Task

        _selectedReview = Await _itService.Get_Review_ById_Async(selectedReviewId)
        lblSelectedReview.Text = IIf(IsNothing(_selectedReview), "No Selected Review Found!", _selectedReview.Description)
        Await Show_SelectedReview_Details()
    End Function

    Protected Async Sub OnReviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.ShowReviews()
    End Sub


    Protected Async Sub OnClosedReviewsPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.ShowReviews()
    End Sub

    Protected Sub grdReview_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub


    Private Async Function Show_Closed_Reviews() As Task

        grdClosedReviews.DataSource = Await _itService.Get_Closed_Review_List_Async(_filterParms)
        grdClosedReviews.DataBind()

    End Function


    Private Async Function ShowReviews() As Task
        Dim _reviewList As List(Of ReviewVm)

        _reviewList = Await _itService.Get_Review_List_Async(_filterParms)

        grdReviews.DataSource = _reviewList
        grdReviews.DataBind()
        Await Show_SelectedReview_Details(True)
    End Function

    Private Sub clearReviewControls()
        cpFilberBody.Visible = True
        cpFilter.Visible = True
        rwSearch.Visible = True
    End Sub

    Protected Async Sub btnCreateReview_Click(sender As Object, e As EventArgs)
        NavigateReviewViews(MultiViewOption.AddEditEntityView)
        _review = New ReviewVm
        cpFilberBody.Visible = False
        cpFilter.Visible = False
        rwSearch.Visible = False
        txtReviewCode.Text = Await _itService.Generate_ReviewCode()
    End Sub

    Protected Sub btnCancelReview_Click(sender As Object, e As EventArgs)
        clearReviewControls()
        NavigateReviewViews(MultiViewOption.EntityListView)
    End Sub



    Protected Async Sub btnSaveReview_Click(sender As Object, e As EventArgs)

        Dim _result = Await _itService.Add_Update_Review_Async(_review)

        If _result.Status Then
            ShowSuccessMessage(_result.Message)
            Await ShowReviews()
            Call clearReviewControls()
            NavigateReviewViews(MultiViewOption.EntityListView)
        Else
            ShowErrorMessage(_result.Message)
        End If

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
            NavigateReviewViews(MultiViewOption.EntityListView)
        Else
            ShowErrorMessage(_result.Message)
        End If


    End Sub

    Protected Sub lblAssignReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        Dim param As New Dictionary(Of String, String)
        param.Add("reviewId", recordId.ToString())
        Redirect_To_Path_With_QueryString("/App/AssignReview", "/App/Reviews", param)

    End Sub


    Protected Async Sub OnReviewAssignmentDetailsPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviewAssignmentDetails.PageIndex = e.NewPageIndex
        Await Me.Show_SelectedReview_Details()
    End Sub
    Protected Sub grdReviewAssignmentDetails_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub


    Private Async Function Show_SelectedReview_Details(Optional ByVal clearList As Boolean = False) As Task

        Dim _selectedReviewDetail As List(Of BranchReviewVm) = New List(Of BranchReviewVm)

        If Not clearList Then _selectedReviewDetail = Await _itService.Get_Review_Assignments_List_Async(_selectedReview.ReviewId)

        grdReviewAssignmentDetails.DataSource = _selectedReviewDetail.ToList()
        grdReviewAssignmentDetails.DataBind()

    End Function

    Protected Sub btnViewReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviewAssignmentDetails) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviewAssignmentDetails.DataKeys(gr.RowIndex).Value.ToString())

        Dim param As New Dictionary(Of String, String)
        param.Add("brvId", recordId.ToString())
        Call Redirect_To_Path_With_QueryString("/App/ViewUpdateReview", "/App/Reviews", param)

    End Sub

    Protected Async Sub lblCloseReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        Dim _result = Await _itService.Close_Review_Async(recordId)

        If _result.Status Then
            Call ShowSuccessMessage(_result.Message)
            Await ShowReviews()
            Await Show_Closed_Reviews()
        Else
            Call ShowErrorMessage(_result.Message)
        End If

    End Sub

    Protected Async Sub btnSearchReview_Click(sender As Object, e As EventArgs)
        Await ShowReviews()
        Await Show_Closed_Reviews()
    End Sub
End Class