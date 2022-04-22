Imports System.Threading.Tasks

Public Class ViewReviews
    Inherits PageBase


    Public ReadOnly Property _filterParms As RecordFilterOption
        Get
            Dim _params = New RecordFilterOption()
            '_params.employeeId = Me.CurrentUser.ObjectID
            '_params.branchCode = Me.BranchCode
            _params.reviewCode = IIf(cboSearchOption.SelectedValue = "0", _searchText, String.Empty)
            _params.reviewName = IIf(cboSearchOption.SelectedValue = "1", _searchText, String.Empty)

            If Not String.IsNullOrWhiteSpace(txtFilterReviewFrom.Text) Then
                _params.fromDate = CDate(txtFilterReviewFrom.Text)
            End If
            If Not String.IsNullOrWhiteSpace(txtFilterReviewTo.Text) Then
                _params.toDate = CDate(txtFilterReviewTo.Text)
            End If

            _params.branchCode = cboReviewBranch.SelectedValue
            Return _params
        End Get
    End Property

    Private ReadOnly Property _searchText As String
        Get
            Return IIf(Not String.IsNullOrWhiteSpace(txtSearchReview.Text), txtSearchReview.Text, String.Empty)
        End Get
    End Property

    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await Show_SelectedReview_Details()
            Call Bind_Dropdown_Combos()
        End If
    End Sub

    Private Sub clearReviewControls()
        cpFilberBody.Visible = True
        cpFilter.Visible = True
        rwSearch.Visible = True
    End Sub

    Private Sub Bind_Dropdown_Combos()
        cboReviewBranch.DataSource = _setupService.Get_Branch_List_Async()
        cboReviewBranch.DataBind()
    End Sub


    Protected Async Sub OnReviewAssignmentDetailsPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviewAssignmentDetails.PageIndex = e.NewPageIndex
        Await Show_SelectedReview_Details()
    End Sub

    Protected Async Sub cboReviewBranch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Await Show_SelectedReview_Details()
    End Sub

    Private Async Function Show_SelectedReview_Details(Optional ByVal clearList As Boolean = False) As Task

        Dim _selectedReviewDetail As List(Of BranchReviewVm) = New List(Of BranchReviewVm)

        If Not clearList Then _selectedReviewDetail = Await _itService.Get_All_Review_Submitted_Assignments_List_Async(_filterParms)

        grdReviewAssignmentDetails.DataSource = _selectedReviewDetail.ToList()
        grdReviewAssignmentDetails.DataBind()

    End Function

    Protected Sub btnViewReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviewAssignmentDetails) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviewAssignmentDetails.DataKeys(gr.RowIndex).Value.ToString())

        Dim param As New Dictionary(Of String, String)
        param.Add("brvId", recordId.ToString())
        Call Redirect_To_Path_With_QueryString("/App/ViewUpdateReview", "/App/ViewReviews", param)

    End Sub

    Protected Sub btnSearchReview_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtSearchReview.Text) Then
            ShowErrorMessage("No Search Parameter Found! Please enter a search parameter to continue.")
            Return
        End If

        Call Show_SelectedReview_Details()
    End Sub
End Class