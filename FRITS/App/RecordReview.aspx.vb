Imports System.Threading.Tasks

Public Class RecordReview
    Inherits PageBase


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
            Return cboReviewBranch.SelectedValue
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
            findingRecord.FindingNo = txtFindingNo.Text
            findingRecord.Description = txtFndReviewArea.Text
            findingRecord.ManagementAwareness = cboManageAwareness.SelectedValue
            findingRecord.RiskLevelId = CInt(cboRiskLevel.SelectedValue)
            findingRecord.RiskCategoryId = CInt(cboRiskCategory.SelectedValue)
            findingRecord.RiskSubCategory = cboRiskSubCategory.Text
            findingRecord.RiskCategoryName = cboRiskCategory.Text
            findingRecord.RiskLevelName = cboRiskLevel.Text
            findingRecord.BranchReviewId = _selectedReview.BranchReviewId
            findingRecord.BranchCode = Me.BranchCode
            Return findingRecord
        End Get
        Set(value As FindingVm)
            findingRecord = value
            If Not IsNothing(findingRecord) Then
                txtFindingNo.Text = findingRecord.FindingNo
                txtFndReviewArea.Text = findingRecord.Description
                cboManageAwareness.SelectedValue = findingRecord.ManagementAwareness
                cboRiskLevel.SelectedValue = IIf(findingRecord.RiskLevelId > 0, findingRecord.RiskLevelId, Nothing)
                cboRiskCategory.SelectedValue = IIf(findingRecord.RiskCategoryId > 0, findingRecord.RiskCategoryId, Nothing)
                cboRiskSubCategory.Text = findingRecord.RiskSubCategory

            End If
            Session.Add("_findingRecord", findingRecord)
        End Set
    End Property


    Private observationRecord As ObservationVm
    Public Property _observationRecord As ObservationVm
        Get
            observationRecord = DirectCast(Session.Item("_robFndObservation"), ObservationVm)
            observationRecord.ObservationNo = txtFndObservationNo.Text
            observationRecord.Description = txtFndObservationDescription.Text
            observationRecord.Implication = txtFndObservationImplication.Text
            observationRecord.RootCauseAnalysis = txtFndObservationRootCauseAnalysis.Text
            observationRecord.ObservationStatusId = CInt(cboFndObservationStatus.SelectedValue)
            observationRecord.Recommendations = _selectedObservationRecommendationList
            observationRecord.ObservationDate = CDate(txtFndObservationDate.Text)
            observationRecord.RiskLevelId = CInt(cboRiskLevel.SelectedValue)
            observationRecord.MitigatingControl = txtFndObservationAssumptions.Text
            observationRecord.Assumptions = txtFndObservationMitigationControl.Text
            Return observationRecord
        End Get
        Set(value As ObservationVm)
            observationRecord = value
            If Not IsNothing(observationRecord) Then

                txtFndObservationNo.Text = observationRecord.ObservationNo
                txtFndObservationDescription.Text = observationRecord.Description
                txtFndObservationImplication.Text = observationRecord.Implication
                txtFndObservationRootCauseAnalysis.Text = observationRecord.RootCauseAnalysis
                cboFndObservationStatus.SelectedValue = IIf(observationRecord.ObservationStatusId > 0, observationRecord.ObservationStatusId, Nothing)
                _selectedObservationRecommendationList = IIf(IsNothing(observationRecord.Recommendations), New List(Of RecommendationVm), observationRecord.Recommendations)
                txtFndObservationDate.Text = CDate(observationRecord.ObservationDate).ToString("yyyy-MM-dd")
                txtFndObservationAssumptions.Text = observationRecord.MitigatingControl
                txtFndObservationMitigationControl.Text = observationRecord.Assumptions

            End If

            Session.Add("_robFndObservation", observationRecord)

        End Set
    End Property


    Private recommendationRecord As RecommendationVm
    Public Property _recommendationRecord As RecommendationVm
        Get
            recommendationRecord = DirectCast(Session.Item("_robFndRecomm"), RecommendationVm)
            recommendationRecord.Description = txtRecommendation.Text
            recommendationRecord.RecommendationType = RecommendationTypes.Observation_Recommendation
            Return recommendationRecord
        End Get
        Set(value As RecommendationVm)
            recommendationRecord = value
            If Not IsNothing(recommendationRecord) Then
                txtRecommendation.Text = recommendationRecord.Description
            End If
            Session.Add("_robFndRecomm", recommendationRecord)
        End Set
    End Property



    Private Async Function Bind_Dropdown_Combos() As Task

        cboReviewBranch.DataSource = _setupService.Get_Branch_List_Async()
        cboReviewBranch.DataBind()

        cboRiskCategory.DataSource = Await _setupService.Get_RiskCategory_List_Async()
        cboRiskCategory.DataBind()

        cboRiskLevel.DataSource = Await _setupService.Get_RiskLevel_List_Async()
        cboRiskLevel.DataBind()

        cboRiskSubCategory.DataSource = Await _setupService.Get_Risk_Sub_Category_List()
        cboRiskSubCategory.DataBind()

        cboManageAwareness.DataSource = Await _setupService.Get_Management_Awareness_List()
        cboManageAwareness.DataBind()

        cboFndObservationStatus.DataSource = Await _setupService.Get_ObservationStatus_List_Async()
        cboFndObservationStatus.DataBind()

    End Function

    Private Sub NavigateFindingViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mvFindings, viewIndex)
        Session.Add("_activeFindingView", viewIndex)
    End Sub

    Private Sub NavigateObservationViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mlvFndObservation, viewIndex)
        Session.Add("_activeObservationView", viewIndex)
        If viewIndex = MultiViewOption.AddEditEntityView Then
            lblObservationPanelTitle.Text = "Add / Edit Observation"
        Else
            lblObservationPanelTitle.Text = "List of Observations"
        End If
    End Sub

    Private Sub NavigateRecommendationViews(ByVal viewIndex As MultiViewOption)
        NavigateViews(mlvFndRecomms, viewIndex)
        Session.Add("_activeRecommView", viewIndex)
        If viewIndex = MultiViewOption.AddEditEntityView Then
            lblRecommendationPanelTitle.Text = "Add / Edit Observation Recommendation"
        Else
            lblRecommendationPanelTitle.Text = "List of Observation Recommendations"
        End If
    End Sub

    'Private Sub NavigateActionPlanViews(ByVal viewIndex As MultiViewOption)
    '    NavigateViews(mlvFndActionPlan, viewIndex)
    '    Session.Add("_activeActionPlanView", viewIndex)
    'End Sub



    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await Bind_Dropdown_Combos()
            Await Show_Reviews()
        End If
    End Sub


    Protected Async Sub cboReviewBranch_SelectedIndexChanged(sender As Object, e As EventArgs)
        Await Show_Reviews()
    End Sub

    Protected Async Sub OnSubmittedReviewsPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.Show_Reviews(True, True)
    End Sub

    Protected Async Sub OnReviewPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdReviews.PageIndex = e.NewPageIndex
        Await Me.Show_Reviews(True, False)
    End Sub


    Protected Sub grdReview_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub


    Public ReadOnly Property _filterParms As RecordFilterOption
        Get
            Dim _params = New RecordFilterOption()
            _params.employeeId = Me.CurrentUser.ObjectID
            _params.branchCode = _selectedBranch4Review
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

    Private Async Function Show_Reviews(Optional ByVal isPageChange As Boolean = False, Optional ByVal isSubmittedGrid As Boolean = False) As Task

        Dim _reviewList = Await _itService.Get_Employee_BranchReviews_Async(_filterParms)

        If isPageChange And isSubmittedGrid = False Then
            grdReviews.DataSource = _reviewList.Where(Function(f) f.IsSubmitted = False).ToList()
            grdReviews.DataBind()
        End If
        If isPageChange And isSubmittedGrid Then
            grdSubmittedReviews.DataSource = _reviewList.Where(Function(f) f.IsSubmitted = True).ToList()
            grdSubmittedReviews.DataBind()
        End If

        If Not isPageChange Then
            grdReviews.DataSource = _reviewList.Where(Function(f) f.IsSubmitted = False).ToList()
            grdReviews.DataBind()
            grdSubmittedReviews.DataSource = _reviewList.Where(Function(f) f.IsSubmitted = True).ToList()
            grdSubmittedReviews.DataBind()
        End If
    End Function




#Region "FINDING METHODS"

    Protected Async Sub btnAddNewFinding_Click(sender As Object, e As EventArgs)

        If IsNothing(_selectedReview) Then
            ShowErrorMessage("No Review Selected! Please select a review to continue!")
            Return
        End If

        _findingRecord = New FindingVm
        txtFindingNo.Text = Await _itService.Generate_Finding_Code_Async(_selectedReview.BranchReviewId, _selectedReviewFindings.Count)
        _selectedFindingObservationList = New List(Of ObservationVm)
        _selectedObservationRecommendationList = New List(Of RecommendationVm)
        Call Show_Observations()
        Call NavigateFindingViews(MultiViewOption.AddEditEntityView)
        Call NavigateObservationViews(MultiViewOption.EntityListView)
        Call NavigateRecommendationViews(MultiViewOption.EntityListView)
        Call clearFindingControls()
        Call Show_Hide_Commit_Buttons(False)
    End Sub


    Private Sub Show_Hide_Commit_Buttons(ByVal showHide As Boolean)
        btnCommintChanges.Visible = showHide
        btnCancelReview.Visible = showHide
    End Sub


    Private Sub clearFindingControls()
        txtFndReviewArea.Text = String.Empty
    End Sub

    Protected Sub btnSaveFinding_Click(sender As Object, e As EventArgs)

        If Not Validate_Finding() Then
            ShowErrorMessage("Please input all required details to continue!")
            Return
        End If

        'If Not _findingRecord.FindingId = New Guid Then

        Dim _exitRec = _selectedReviewFindings.SingleOrDefault(Function(x) x.FindingNo = _findingRecord.FindingNo)
        If Not IsNothing(_exitRec) Then _selectedReviewFindings.Remove(_exitRec)

        'End If

        _selectedReviewFindings.Add(_findingRecord)
        Call clearFindingControls()
        Call NavigateFindingViews(MultiViewOption.EntityListView)
        Call Show_Findings()
        Call Show_Hide_Commit_Buttons(True)

    End Sub

    Protected Sub btnCancelFinding_Click(sender As Object, e As EventArgs)
        Call clearFindingControls()
        Call NavigateFindingViews(MultiViewOption.EntityListView)
        Call Show_Hide_Commit_Buttons(True)
    End Sub

#End Region




#Region "OBSERVATION METHODS"

    Private Sub clearObservationControls()
        txtFndObservationDescription.Text = ""
        txtFndObservationRootCauseAnalysis.Text = ""
        txtFndObservationImplication.Text = ""
    End Sub
    Protected Sub btnFndCancelObservation_Click(sender As Object, e As EventArgs)
        Call clearObservationControls()
        Call NavigateObservationViews(MultiViewOption.EntityListView)
        Call Show_Hide_Findings_Buttons(True)
    End Sub

    Protected Async Sub lblRecordFindings_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        _selectedReview = Await _itService.Get_BranchReview_ById_Async(recordId)

        lblSelectedReview.Text = $"{_selectedReview.ReviewCode}  -:-  {_selectedReview.ReviewName}"

        btnAddNewFinding.Visible = True
        btnAddNewFinding.Enabled = True

        Await Load_Review_Findings()

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
            btnCommintChanges.Visible = True
            btnCancelReview.Visible = True
        Else
            dlFindingSummary.ShowFooter = True
            btnCommintChanges.Visible = False
            btnCancelReview.Visible = False
        End If

    End Sub


    Protected Async Sub dlFindingSummary_ItemCommand(source As Object, e As DataListCommandEventArgs)

        Dim _recordId = Guid.Parse(dlFindingSummary.DataKeys(e.Item.ItemIndex).ToString())

        'If _recordId = New Guid Then
        '    ShowErrorMessage("Invalid Record! Please refresh page to fetch latest copy of review records!")
        '    Return
        'End If

        If e.CommandName.Equals("ViewFinding") Then

            _findingRecord = _selectedReviewFindings.SingleOrDefault(Function(x) x.FindingNo = e.CommandArgument.ToString())

            If _findingRecord.FindingId <> New Guid Then
                _selectedFindingObservationList = Await _itService.Get_Finding_Observation_List_Async(_recordId)
            End If
            Call NavigateFindingViews(MultiViewOption.AddEditEntityView)
            Call NavigateObservationViews(MultiViewOption.EntityListView)
            Call NavigateRecommendationViews(MultiViewOption.EntityListView)
            Call Show_Hide_Commit_Buttons(False)

            cboRiskCategory.SelectedValue = _findingRecord.RiskCategoryId
            cboRiskLevel.SelectedValue = _findingRecord.RiskLevelId
            cboRiskSubCategory.Text = _findingRecord.RiskSubCategory
            cboManageAwareness.Text = _findingRecord.ManagementAwareness
            Call Show_Observations()

        ElseIf e.CommandName.Equals("RemoveFinding") Then

            Dim _rmvFinding = _selectedReviewFindings.SingleOrDefault(Function(x) x.FindingNo = e.CommandArgument.ToString())
            If Not IsNothing(_rmvFinding) Then _selectedReviewFindings.Remove(_rmvFinding)

            Call Show_Findings()

            'Dim _rmvItem = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationId = _recordId)
            'If Not IsNothing(_rmvItem) Then _selectedFindingObservationList.Remove(_rmvItem)

            'Call Show_Observations()

        ElseIf e.CommandName.Equals("UploadFindingFile") Then

            Dim _uploadReferenceTypeId = Guid.Parse(e.CommandArgument())
            Show_Upload_Popup(ReviewFileTypes.Finding_File, _uploadReferenceTypeId, "Finding File", txtFindingNo.Text)

        ElseIf e.CommandName.Equals("ViewFindingFiles") Then

            Dim _uploadReferenceTypeId = Guid.Parse(e.CommandArgument())
            Show_ViewFiles_Popup(ReviewFileTypes.Finding_File, _uploadReferenceTypeId, "Finding Files")
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

    Private Sub Show_Upload_Popup(ByVal fileType As ReviewFileTypes, referenceTypeId As Guid, ByVal title As String, ByVal fileName As String)

        lblFileUploadTitle.Text = $"File Upload - {title}"
        ucUploadFile.ReferenceFileTypeId = referenceTypeId
        ucUploadFile.ReviewFileType = fileType
        ucUploadFile.UploadFileName = fileName
        mpUploadFile.Show()

    End Sub



    Private Function Validate_Finding(Optional ByVal forSave As Boolean = True) As Boolean

        Dim _result = (Not String.IsNullOrWhiteSpace(txtFndReviewArea.Text) And CInt(cboRiskLevel.SelectedValue) > 0 And CInt(cboRiskCategory.SelectedValue) And Not String.IsNullOrWhiteSpace(cboRiskSubCategory.Text) And Not String.IsNullOrWhiteSpace(cboManageAwareness.Text))

        If forSave Then
            _result = _selectedFindingObservationList.Count > 0
        End If

        Return _result

    End Function

    Protected Async Sub btnAddFndObservation_Click(sender As Object, e As EventArgs)

        If Not Validate_Finding(False) Then
            ShowErrorMessage("Please provided necessary details for finding before adding observations")
            Return
        End If

        _observationRecord = New ObservationVm
        txtFndObservationNo.Text = Await _itService.Generate_Observation_Code_Async(_findingRecord.FindingId, _selectedFindingObservationList.Count(), _findingRecord.FindingNo)
        _selectedObservationRecommendationList = New List(Of RecommendationVm)
        Call Show_Recommendations()
        Call clearObservationControls()
        Call NavigateObservationViews(MultiViewOption.AddEditEntityView)
        Call Show_Hide_Findings_Buttons(False)
    End Sub



    Private Function Validate_Observation(Optional ByVal forSave As Boolean = True) As Boolean

        Dim _result = (Not String.IsNullOrWhiteSpace(txtFndObservationDescription.Text) And Not String.IsNullOrWhiteSpace(txtFndObservationImplication.Text) And Not String.IsNullOrWhiteSpace(txtFndObservationImplication.Text) And Not String.IsNullOrWhiteSpace(cboFndObservationStatus.SelectedValue))

        If forSave Then
            _result = _selectedObservationRecommendationList.Count > 0
        End If

        Return _result

    End Function

    Private Sub clearRecommendationControls()
        txtRecommendation.Text = String.Empty
    End Sub

    Protected Sub btnAddFndRecomm_Click(sender As Object, e As EventArgs)

        If Not Validate_Observation(False) Then
            ShowErrorMessage("Please provided necessary details for observation before adding recommendation")
            Return
        End If
        _recommendationRecord = New RecommendationVm(RecommendationTypes.Observation_Recommendation)
        Call clearRecommendationControls()
        Call NavigateRecommendationViews(MultiViewOption.AddEditEntityView)
        Call Show_Hide_Observation_Buttons(False)
    End Sub

    Protected Sub btnSaveFindingRecomms_Click(sender As Object, e As EventArgs)

        If String.IsNullOrWhiteSpace(txtRecommendation.Text) Then
            ShowErrorMessage("Please input recommendation details to continue!")
            Return
        End If

        If Not _recommendationRecord.RecommendationId = New Guid Then

            Dim _exitRec = _selectedObservationRecommendationList.SingleOrDefault(Function(x) x.RecommendationId = _recommendationRecord.RecommendationId)

            If Not IsNothing(_exitRec) Then _selectedObservationRecommendationList.Remove(_exitRec)

        End If

        _selectedObservationRecommendationList.Add(_recommendationRecord)
        Call clearRecommendationControls()
        Call NavigateRecommendationViews(MultiViewOption.EntityListView)
        Call Show_Recommendations()
        Call Show_Hide_Observation_Buttons(True)
    End Sub


    Private Sub Show_Recommendations()
        dtlFndRecommList.DataSource = _selectedObservationRecommendationList.ToList()
        dtlFndRecommList.DataBind()
    End Sub



    Protected Sub btnCancelFindingRecomms_Click(sender As Object, e As EventArgs)
        Call clearRecommendationControls()
        Call NavigateRecommendationViews(MultiViewOption.EntityListView)
        Call Show_Hide_Observation_Buttons(True)
    End Sub



    Protected Sub btnFndSaveObservation_Click(sender As Object, e As EventArgs)

        If Not Validate_Observation() Then
            ShowErrorMessage("Observation record validation failed! Please provide all required details to continue!")
            Return
        End If

        'If Not _observationRecord.ObservationId = New Guid Then

        Dim _exitRec = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationNo = _observationRecord.ObservationNo)
        If Not IsNothing(_exitRec) Then _selectedFindingObservationList.Remove(_exitRec)

        'End If

        _selectedFindingObservationList.Add(_observationRecord)
        _findingRecord.Observations = _selectedFindingObservationList
        _selectedObservationRecommendationList = New List(Of RecommendationVm)
        Call clearObservationControls()
        Call Show_Recommendations()
        Call NavigateObservationViews(MultiViewOption.EntityListView)
        Call Show_Observations()
        Call Show_Hide_Findings_Buttons(True)
    End Sub

    Private Sub Show_Observations()
        dtlFndObservationList.DataSource = _selectedFindingObservationList.ToList()
        dtlFndObservationList.DataBind()
    End Sub


    Protected Sub dtlFndRecommList_ItemCommand(source As Object, e As DataListCommandEventArgs)

        If e.CommandName.Equals("RemoveItem") Then

            Dim _recordId = Guid.Parse(dtlFndRecommList.DataKeys(e.Item.ItemIndex).ToString())

            Dim _rmvItem = _selectedObservationRecommendationList.SingleOrDefault(Function(x) x.RecommendationId = _recordId)

            If Not IsNothing(_rmvItem) Then _selectedObservationRecommendationList.Remove(_rmvItem)

            Call Show_Recommendations()
        End If

    End Sub

    Protected Sub dtlFndObservationList_ItemCommand(source As Object, e As DataListCommandEventArgs)

        Dim _recordId = Guid.Parse(dtlFndObservationList.DataKeys(e.Item.ItemIndex).ToString())

        If e.CommandName.Equals("RemoveObservation") Then

            Dim _rmvItem = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationNo = e.CommandArgument.ToString())

            If Not IsNothing(_rmvItem) Then _selectedFindingObservationList.Remove(_rmvItem)

            Call Show_Observations()

        ElseIf e.CommandName.Equals("ViewObservation") Then

            _observationRecord = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationNo = e.CommandArgument.ToString())

            If Not IsNothing(_observationRecord) Then

                cboFndObservationStatus.SelectedValue = _observationRecord.ObservationStatusId
                _selectedObservationRecommendationList = _observationRecord.Recommendations.ToList()
                Call NavigateObservationViews(MultiViewOption.AddEditEntityView)
                Call Show_Recommendations()
                Call Show_Hide_Findings_Buttons(False)
            Else
                ShowErrorMessage("Record not found!")
            End If

        ElseIf e.CommandName.Equals("UploadObservationFile") Then

            'Dim _uploadReferenceTypeId = Guid.Parse(e.CommandArgument().ToString().Split("-||-")(0))
            Dim _uploadRefNo As String = e.CommandArgument().ToString()

            Dim _obsv = _selectedFindingObservationList.SingleOrDefault(Function(x) x.ObservationNo = _uploadRefNo)

            Call Show_Upload_Popup(ReviewFileTypes.Observation_File, _obsv.ObservationId, "Observation File", _obsv.ObservationNo)

        End If

    End Sub


    Public Function Show_Upload_Button(ByVal refTypeId As Guid) As Boolean
        Return (refTypeId <> New Guid)
    End Function



    Private Sub Show_Hide_Findings_Buttons(ByVal showHide As Boolean)
        btnSaveFinding.Visible = showHide
        btnCancelFinding.Visible = showHide
    End Sub
    Private Sub Show_Hide_Observation_Buttons(ByVal showHide As Boolean)
        btnFndSaveObservation.Visible = showHide
        btnFndCancelObservation.Visible = showHide
    End Sub

    Protected Async Sub btnCommintChanges_Click(sender As Object, e As EventArgs)

        If _selectedReviewFindings.Count = 0 Then
            ShowErrorMessage("No Finding Record(s) Found!  Please Check And Try Again")
            Return
        End If

        Dim _result = Await _itService.Add_Update_Review_Details_Async(_selectedReviewFindings)

        If _result.Status Then
            Call ShowSuccessMessage(_result.Message)
            Call reset_All_Paramenters()
            Call clearFindingControls()
            Call NavigateFindingViews(MultiViewOption.EntityListView)
        Else
            Call ShowErrorMessage(_result.Message)
        End If

    End Sub

    Private Sub reset_All_Paramenters()
        _findingRecord = Nothing
        _observationRecord = Nothing
        _recommendationRecord = Nothing
        _selectedFindingObservationList = New List(Of ObservationVm)
        _selectedObservationRecommendationList = New List(Of RecommendationVm)
        _selectedReview = Nothing
        _selectedReviewFindings = New List(Of FindingVm)
        lblSelectedReview.Text = "No Selected Review Found!"
        btnAddNewFinding.Visible = False
        btnAddNewFinding.Enabled = False
    End Sub

    Protected Sub btnCancelReview_Click(sender As Object, e As EventArgs)
        Call reset_All_Paramenters()
        Call Show_Reviews()
    End Sub

    Protected Async Sub lblSubmitReview_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        Dim _result = Await _itService.Submit_Review_Async(recordId)

        If _result.Status Then
            Call ShowSuccessMessage(_result.Message)
            Await Show_Reviews()
        Else
            Call ShowErrorMessage(_result.Message)
        End If

    End Sub

    Protected Sub btnSearchReview_Click(sender As Object, e As EventArgs)

        If String.IsNullOrWhiteSpace(txtSearchReview.Text) Then
            ShowErrorMessage("No Search Parameter Found! Please enter a search parameter to continue.")
            Return
        End If

        Call Show_Reviews()

    End Sub

    Protected Async Sub lblUnSubmitReview_Click(sender As Object, e As EventArgs)
        If Not GridHasRows(grdReviews) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdReviews.DataKeys(gr.RowIndex).Value.ToString())

        Dim _result = Await _itService.UnSubmit_Review_Async(recordId)

        If _result.Status Then
            Call ShowSuccessMessage(_result.Message)
            Await Show_Reviews()
        Else
            Call ShowErrorMessage(_result.Message)
        End If
    End Sub

#End Region




End Class