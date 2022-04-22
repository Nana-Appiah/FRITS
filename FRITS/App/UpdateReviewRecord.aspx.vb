Imports System.Threading.Tasks

Public Class UpdateReviewRecord
    Inherits PageBase



    Private actionPlan As ActionPlanVm
    Public Property _actionPlan As ActionPlanVm
        Get
            actionPlan = DirectCast(Session.Item("_actionPlan"), ActionPlanVm)
            Return actionPlan
        End Get
        Set(value As ActionPlanVm)
            actionPlan = value
            Session.Add("_actionPlan", actionPlan)
        End Set
    End Property


    Private selectedReview As BranchReviewVm
    Public Property _selectedReview As BranchReviewVm
        Get
            selectedReview = DirectCast(Session.Item("_selectedReview"), BranchReviewVm)
            Return selectedReview
        End Get
        Set(value As BranchReviewVm)
            selectedReview = value
            Session.Add("_selectedReview", selectedReview)
        End Set
    End Property

    Public ReadOnly Property _selectedBranch4Review As String
        Get
            Return Me.BranchCode ' cboReviewBranch.SelectedValue
        End Get

    End Property


    Private selectedReviewFindings As List(Of FindingVm)
    Public Property _selectedReviewFindings As List(Of FindingVm)
        Get
            selectedReviewFindings = DirectCast(Session.Item("_selectedReviewFindings"), List(Of FindingVm))
            Return selectedReviewFindings
        End Get
        Set(value As List(Of FindingVm))
            selectedReviewFindings = value
            Session.Add("_selectedReviewFindings", selectedReviewFindings)
        End Set
    End Property


    Private selectedFindingObservationList As List(Of ObservationVm)
    Public Property _selectedFindingObservationList As List(Of ObservationVm)
        Get
            selectedFindingObservationList = DirectCast(Session.Item("_selectedFindingObservationList"), List(Of ObservationVm))
            Return selectedFindingObservationList
        End Get
        Set(value As List(Of ObservationVm))
            selectedFindingObservationList = value
            Session.Add("_selectedFindingObservationList", selectedFindingObservationList)
        End Set
    End Property




    Private selectedObservationRecommendationList As List(Of RecommendationVm)
    Public Property _selectedObservationRecommendationList As List(Of RecommendationVm)
        Get
            selectedObservationRecommendationList = DirectCast(Session.Item("_selectedObservationRecommendationList"), List(Of RecommendationVm))
            Return selectedObservationRecommendationList
        End Get
        Set(value As List(Of RecommendationVm))
            selectedObservationRecommendationList = value
            Session.Add("_selectedObservationRecommendationList", selectedObservationRecommendationList)
        End Set
    End Property



    Private findingRecord As FindingVm
    Public Property _findingRecord As FindingVm
        Get
            findingRecord = DirectCast(Session.Item("_findingRecord"), FindingVm)
            Return findingRecord
        End Get
        Set(value As FindingVm)
            findingRecord = value
            Session.Add("_findingRecord", findingRecord)
        End Set
    End Property


    Private observationRecord As ObservationVm
    Public Property _observationRecord As ObservationVm
        Get
            observationRecord = DirectCast(Session.Item("_robFndObservation"), ObservationVm)
            Return observationRecord
        End Get
        Set(value As ObservationVm)
            observationRecord = value
            If Not IsNothing(observationRecord) Then
                _selectedObservationRecommendationList = IIf(IsNothing(observationRecord.Recommendations), New List(Of RecommendationVm), observationRecord.Recommendations)
            End If
            Session.Add("_robFndObservation", observationRecord)

        End Set
    End Property


    Private actionTaken As CorrectiveActionVm
    Public Property _actionTaken As CorrectiveActionVm
        Get
            actionTaken = DirectCast(Session.Item("_actionTaken"), CorrectiveActionVm)
            Return actionTaken
        End Get
        Set(value As CorrectiveActionVm)
            actionTaken = value

            Session.Add("_actionTaken", actionTaken)

        End Set
    End Property


    Private Async Function Bind_Dropdown_Combos() As Task

        'cboReviewBranch.DataSource = _setupService.Get_Branch_List_Async()
        'cboReviewBranch.DataBind()

        cboUsersInBranch.DataSource = _setupService.Get_User_In_Branch(Me.BranchCode)
        cboUsersInBranch.DataBind()

        cboATtatus.DataSource = Await _setupService.Get_ObservationStatus_List_Async()
        cboATtatus.DataBind()

    End Function

    Private Sub NavigateFindingViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mvFindings, viewIndex)
        Session.Add("_activeFindingView", viewIndex)
    End Sub

    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await Bind_Dropdown_Combos()
            Await ShowReviews()
        End If
    End Sub


    'Protected Async Sub cboReviewBranch_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Await ShowReviews()
    'End Sub

    Protected Async Sub OnReviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.ShowReviews()
    End Sub



    Public ReadOnly Property _filterParms As RecordFilterOption
        Get
            Dim _params = New RecordFilterOption()
            _params.branchCode = _selectedBranch4Review
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


    Private Async Function ShowReviews() As Task
        grdReviews.DataSource = Await _itService.Get_BranchReviews_Async(_filterParms)
        grdReviews.DataBind()
    End Function



#Region "OBSERVATION METHODS"
    Protected Async Sub lblUpdateReviewObservation_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        _selectedReview = Await _itService.Get_BranchReview_ById_Async(recordId)

        lblSelectedReview.Text = $"{_selectedReview.ReviewCode}  -:-  {_selectedReview.ReviewName}"
        Call NavigateBranchRecordsViews(MultiViewOption.AddEditEntityView)
        Await Load_Review_Findings()

    End Sub

    Private Sub NavigateBranchRecordsViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mlvBranchRecords, viewIndex)
        Session.Add("_activeBranchRecordsView", viewIndex)
    End Sub

    Private Async Function Load_Review_Findings() As Task

        _selectedReviewFindings = Await _itService.Get_Review_Findings_List_Async(_selectedReview.BranchReviewId)

        Call Show_Findings()

    End Function

    Private Sub Show_Findings()
        dlFindingSummary.DataSource = _selectedReviewFindings.ToList()
        dlFindingSummary.DataBind()

        If _selectedReviewFindings.Count > 0 Then
            dlFindingSummary.ShowFooter = False
        Else
            dlFindingSummary.ShowFooter = True
        End If

    End Sub


    Protected Async Sub dlFindingSummary_ItemCommand(source As Object, e As DataListCommandEventArgs)

        Dim _recordId = Guid.Parse(dlFindingSummary.DataKeys(e.Item.ItemIndex).ToString())

        If _recordId = New Guid Then
            ShowErrorMessage("Invalid Record! Please refresh page to fetch latest copy of review records!")
            Return
        End If

        If e.CommandName.Equals("ViewFinding") Then

            _findingRecord = _selectedReviewFindings.SingleOrDefault(Function(x) x.FindingId = _recordId)
            pFindingNumber.InnerText = _findingRecord.FindingNo
            pReviewArea.InnerText = _findingRecord.Description

            Await Load_Finding_ObservationList()

            Call NavigateFindingViews(MultiViewOption.AddEditEntityView)

        End If
        Call Show_Observations()

    End Sub

    Private Async Function Load_Finding_ObservationList() As Task
        _selectedFindingObservationList = Await _itService.Get_Finding_Observation_List_Async(_findingRecord.FindingId)
    End Function

    Private Sub Show_Observations()
        dtlFndObservationList.DataSource = _selectedFindingObservationList.ToList()
        dtlFndObservationList.DataBind()
    End Sub


    Protected Async Sub dtlFndObservationList_ItemCommand(source As Object, e As DataListCommandEventArgs)

        Dim _recordId = Guid.Parse(dtlFndObservationList.DataKeys(e.Item.ItemIndex).ToString())

        If e.CommandName.Equals("AddCorrectiveActionPlan") Then

            _observationRecord = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationId = _recordId)

            hndAPObservationId.Text = _observationRecord.ObservationId.ToString()
            lblAPObservationNo.Text = _observationRecord.ObservationNo
            lblAPObservationDate.Text = _observationRecord.ObservationDate.ToShortDateString()
            lblAPObservationDescription.Text = _observationRecord.Description

            _actionPlan = Await _itService.Get_Observation_ActionPlan_ById_Async(_observationRecord.ObservationId)

            If Not IsNothing(_actionPlan) Then
                txtAPDescription.Text = _actionPlan.Description
                txtAPSchedOfficerResponse.Text = _actionPlan.ScheduleOfficerResponse
                txtAPSelectedEmpIds.Text = _actionPlan.ReferenceOfficerIds
                lblAPUserList.InnerHtml = _actionPlan.AssignedEmployeeNames
            Else
                _actionPlan.ResolutionTiming = DateTime.UtcNow
            End If
            txtAPResolutionDate.Text = _actionPlan.ResolutionTiming.ToString("yyyy-MM-dd")

            mpAddEditActionPlan.Show()

        ElseIf e.CommandName.Equals("AddCorrectiveActionTaken") Then

            _actionTaken = New CorrectiveActionVm() With {.ObservationId = _recordId, .CorrectionDate = DateTime.UtcNow.Date}

            txtATDetails.Text = String.Empty
            mpAddActionTaken.Show()

        ElseIf e.CommandName.Equals("RemoveCorrectiveAction") Then

            Dim _theActionId = Guid.Parse(e.CommandArgument.ToString())

            If _theActionId = New Guid Then
                Return
            End If

            Dim _result = Await _itService.Delete_Corrective_Action_Async(_theActionId)
            If _result.Status Then
                Await Load_Finding_ObservationList()
                Call Show_Observations()
                Call ShowSuccessMessage(_result.Message)
            Else
                Call ShowErrorMessage(_result.Message)
            End If

        End If
    End Sub

    Private Sub reset_All_Paramenters()
        _findingRecord = Nothing
        _observationRecord = Nothing
        _selectedFindingObservationList = New List(Of ObservationVm)
        _selectedObservationRecommendationList = New List(Of RecommendationVm)
        _selectedReview = Nothing
        _selectedReviewFindings = New List(Of FindingVm)
    End Sub

    Protected Async Sub btnSaveActionPlan_Click(sender As Object, e As EventArgs)

        _actionPlan.Description = txtAPDescription.Text
        _actionPlan.ReferenceOfficerIds = txtAPSelectedEmpIds.Text
        _actionPlan.ResolutionTiming = CDate(txtAPResolutionDate.Text)
        _actionPlan.ScheduleOfficerResponse = txtAPSchedOfficerResponse.Text
        If Not _actionPlan.IsValid() Then
            mpAddEditActionPlan.Hide()
            ShowErrorMessage("Record validation failded!")
            Thread.Sleep(2000)
            mpAddEditActionPlan.Show()
            Return
        End If

        Dim _result = Await _itService.Add_Update_ActionPlan_Async(_actionPlan)

        If _result.Status Then
            _actionPlan = _result.Entity
            Await Load_Finding_ObservationList()
            Call Show_Observations()
            txtAPDescription.Text = String.Empty
            Call ShowSuccessMessage(_result.Message)
            mpAddEditActionPlan.Hide()
        Else
            Call ShowErrorMessage(_result.Message)
            Thread.Sleep(2000)
            mpAddEditActionPlan.Show()
        End If

    End Sub

    Protected Async Sub btnSaveActionTaken_Click(sender As Object, e As EventArgs)

        If IsNothing(_actionTaken) Then
            mpAddActionTaken.Hide()
            ShowErrorMessage("Record validation failded!")
            Thread.Sleep(2000)
            mpAddActionTaken.Show()
            Return
        End If

        _actionTaken.Remarks = txtATDetails.Text
        _actionTaken.CorrectionDate = CDate(txtATDate.Text)
        _actionTaken.ObservationStatusId = CInt(cboATtatus.SelectedValue)

        If Not _actionTaken.IsValid() Then
            mpAddActionTaken.Hide()
            ShowErrorMessage("Record validation failded!")
            Thread.Sleep(2000)
            mpAddActionTaken.Show()
            Return
        End If

        Dim _result = Await _itService.Add_Update_Corrective_Action_Async(_actionTaken)

        If _result.Status Then
            _actionTaken = _result.Entity
            Await Load_Finding_ObservationList()
            Call Show_Observations()
            Call ShowSuccessMessage(_result.Message)
        Else
            Call ShowErrorMessage(_result.Message)
            Thread.Sleep(2000)
            mpAddEditActionPlan.Show()
        End If
    End Sub

    Protected Async Sub btnSearchReview_Click(sender As Object, e As EventArgs)

        Await ShowReviews()

    End Sub




#End Region




End Class