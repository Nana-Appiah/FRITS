Imports System.Threading.Tasks

Public Class ViewUpdateReview
    Inherits PageBase

    Private reviewId As Guid
    Private Property _reviewId As Guid
        Get
            Dim _rvId = GetRequestParameter("brvId")
            If Not String.IsNullOrWhiteSpace(_rvId) Then
                reviewId = Guid.Parse(_rvId)
            End If
            Return reviewId
        End Get
        Set(value As Guid)
            reviewId = value
        End Set
    End Property

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


    Dim the1stRecomm As KeyValueObject(Of Guid)
    Public Property _the1stRecomm As KeyValueObject(Of Guid)
        Get
            the1stRecomm = DirectCast(Session.Item("_the1stRecomm"), KeyValueObject(Of Guid))
            Return the1stRecomm
        End Get
        Set(value As KeyValueObject(Of Guid))
            the1stRecomm = value
            Session.Add("_the1stRecomm", the1stRecomm)
        End Set
    End Property


    Private Async Function Bind_Dropdown_Combos() As Task

        cboObservationStatus.DataSource = Await _setupService.Get_ObservationStatus_List_Async()
        cboObservationStatus.DataBind()

        cboFUCurrentStatus.DataSource = Await _setupService.Get_ObservationStatus_List_Async()
        cboFUCurrentStatus.DataBind()

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
            Await Load_Review()
        End If
    End Sub


    Protected Async Function Load_Review() As Task

        _selectedReview = Await _itService.Get_BranchReview_ById_Async(_reviewId)
        If Not IsNothing(_selectedReview) Then
            lblSelectedReview.Text = $"{_selectedReview.ReviewCode}  -:-  {_selectedReview.ReviewName}"
            Await Load_Review_Findings()
        End If

    End Function

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

        If e.CommandName.Equals("ViewObservations") Then

            _findingRecord = _selectedReviewFindings.SingleOrDefault(Function(x) x.FindingId = _recordId)
            pFindingNumber.InnerText = _findingRecord.FindingNo
            pReviewArea.InnerText = _findingRecord.Description
            pRiskLevel.InnerText = _findingRecord.RiskLevelName
            pRiskCategory.InnerText = _findingRecord.RiskCategoryName
            pRiskSubCategory.InnerText = _findingRecord.RiskSubCategory
            pManagementAwareness.InnerText = _findingRecord.ManagementAwareness

            Await Load_Finding_ObservationList()

            Call NavigateFindingViews(MultiViewOption.AddEditEntityView)
            Call Show_Observations()

        ElseIf e.CommandName.Equals("ViewFindingFiles") Then

            Dim _uploadReferenceTypeId = Guid.Parse(e.CommandArgument)
            Show_ViewFiles_Popup(ReviewFileTypes.Observation_File, _uploadReferenceTypeId, "Findings Files")

        End If


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
        _observationRecord = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationId = _recordId)

        If e.CommandName.Equals("UpdateObservation") Then

            cboObservationStatus.SelectedValue = _observationRecord.ObservationStatusId
            txtUOAssumption.Text = _observationRecord.Assumptions
            txtUODescription.Text = _observationRecord.Description
            txtUOImplication.Text = _observationRecord.Implication
            txtUOMitigatingControl.Text = _observationRecord.MitigatingControl
            txtUORootCause.Text = _observationRecord.RootCauseAnalysis

            If _observationRecord.Recommendations.Count > 0 Then
                _the1stRecomm = New KeyValueObject(Of Guid)
                Dim _recomm = _observationRecord.Recommendations.First
                txtUO1stRecommendation.Visible = True
                _the1stRecomm.Key = _recomm.RecommendationId
                _the1stRecomm.Value = _recomm.Description
                txtUO1stRecommendation.Text = _recomm.Description
            End If

            mpUpdateObservationStatus.Show()

        ElseIf e.CommandName.Equals("AddManagementResponse") Then

            txtManagementResponse.Text = _observationRecord.ManagementResponse
            mpUpdateManagementResponse.Show()

        ElseIf e.CommandName.Equals("CreateFollowUp") Then

            Call ClearFollowUpControls()
            mpCreateFollowUp.Show()

        ElseIf e.CommandName.Equals("ViewObservationFiles") Then

            Dim _uploadReferenceTypeId = Guid.Parse(e.CommandArgument())
            Show_ViewFiles_Popup(ReviewFileTypes.Observation_File, _uploadReferenceTypeId, "Observation Files")

        ElseIf e.CommandName.Equals("RemoveFollowUp") Then

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

    Private Sub Show_ViewFiles_Popup(ByVal fileType As ReviewFileTypes, referenceTypeId As Guid, ByVal title As String)

        lblViewFilesTitle.Text = $"{title}"
        ucViewFiles.ReferenceFileTypeId = referenceTypeId
        ucViewFiles.ReviewFileType = fileType
        Dim _fileList = _itService.Get_Uploaded_Files_By_ReferenceTypeId(referenceTypeId)
        ucViewFiles.FileList = _fileList
        ucViewFiles.Show_Files()
        mpShowUploadedFiles.Show()

    End Sub


    Private Sub ClearFollowUpControls()
        txtFURemarks.Text = String.Empty
        txtFUTitle.Text = String.Empty
        txtFUDate.Text = String.Empty
    End Sub

    Protected Async Sub btnSaveStatusChange_Click(sender As Object, e As EventArgs)

        If IsNothing(_observationRecord) Then
            mpUpdateObservationStatus.Hide()
            ShowErrorMessage("Record validation failded!")
            Thread.Sleep(2000)
            mpUpdateObservationStatus.Show()
            Return
        End If

        Dim _updateObservation = _observationRecord

        _updateObservation.ObservationStatusId = cboObservationStatus.SelectedValue
        _updateObservation.Assumptions = txtUOAssumption.Text
        _updateObservation.Description = txtUODescription.Text
        _updateObservation.Implication = txtUOImplication.Text
        _updateObservation.MitigatingControl = txtUOMitigatingControl.Text
        _updateObservation.RootCauseAnalysis = txtUORootCause.Text

        Dim _recomm = _the1stRecomm
        If Not IsNothing(_recomm) Then
            _recomm.Value = txtUO1stRecommendation.Text
        End If

        Dim _result = Await _itService.Update_Observation_Async(_updateObservation, _recomm)

        If _result.Status Then
            _the1stRecomm = Nothing
            mpUpdateObservationStatus.Hide()
            Await Load_Finding_ObservationList()
            Call Show_Observations()
            Call ShowSuccessMessage(_result.Message)
        Else
            mpUpdateObservationStatus.Hide()
            Call ShowErrorMessage(_result.Message)
            Thread.Sleep(2000)
            mpUpdateObservationStatus.Show()
        End If

    End Sub


    Protected Sub btnGoBack_Click(sender As Object, e As EventArgs)
        If mvFindings.ActiveViewIndex = 1 Then
            mvFindings.ActiveViewIndex = 0
        Else
            GoTo_GoBackTo_Page()
        End If
    End Sub

    Protected Async Sub btnSaveManagementResponse_Click(sender As Object, e As EventArgs)


        If IsNothing(_observationRecord) Then
            mpUpdateManagementResponse.Hide()
            ShowErrorMessage("Invalid Observation Record! Please REFRESH Page to fetch a fresh copy of data from database!")
            Thread.Sleep(2000)
            mpUpdateManagementResponse.Show()
            Return
        End If

        Dim _result = Await _itService.Update_Observation_Management_Response_Async(_observationRecord.ObservationId, txtManagementResponse.Text)

        If _result.Status Then
            Await Load_Finding_ObservationList()
            Call Show_Observations()
            txtManagementResponse.Text = String.Empty
            mpUpdateManagementResponse.Hide()
            Call ShowSuccessMessage(_result.Message)
        Else
            mpUpdateManagementResponse.Hide()
            Call ShowErrorMessage(_result.Message)
            Thread.Sleep(2000)
            mpUpdateManagementResponse.Show()
        End If


    End Sub

    Protected Async Sub btnSaveFollowUp_Click(sender As Object, e As EventArgs)


        If IsNothing(_observationRecord) Then
            mpCreateFollowUp.Hide()
            ShowErrorMessage("Invalid Observation Record! Please REFRESH Page to fetch a fresh copy of data from database!")
            Thread.Sleep(2000)
            mpCreateFollowUp.Show()
            Return
        End If


        Dim _followUpRecord As New FollowUpVm With {
            .Description = txtFUTitle.Text,
            .FollowUpDate = CDate(txtFUDate.Text),
            .FollowUpDetails = New List(Of FollowUpDetailVm)
        }

        Dim _followUpDetail = New FollowUpDetailVm() With {
                .ObservationId = _observationRecord.ObservationId,
                .ObservationStatusId = CInt(cboFUCurrentStatus.SelectedValue),
                .Remarks = txtFURemarks.Text
                }

        _followUpRecord.FollowUpDetails.Add(_followUpDetail)

        If Not _followUpRecord.IsValid() Or _followUpDetail.IsValid() Then
            mpCreateFollowUp.Hide()
            ShowErrorMessage("Record validation failed, please check entries for all required fields and try again!")
            Thread.Sleep(2000)
            mpCreateFollowUp.Show()
            Return
        End If



        Dim _result = Await _itService.Add_Update_FollowUp_Async(_followUpRecord)

        If _result.Status Then
            Call ClearFollowUpControls()
            Await Load_Finding_ObservationList()
            Call Show_Observations()
            mpCreateFollowUp.Hide()
            Call ShowSuccessMessage(_result.Message)
        Else
            mpCreateFollowUp.Hide()
            Call ShowErrorMessage(_result.Message)
            Thread.Sleep(2000)
            mpCreateFollowUp.Show()
        End If



    End Sub


End Class