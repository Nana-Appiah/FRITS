Imports System.Threading.Tasks

Public Class SystemSetup
    Inherits PageBase

    Private Sub InitializePage()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.application"

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                'Check permission to open page.
                If Me.CurrentUser.IsMemberAll("Administrators") OrElse Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "read") Then

                    'Display Details
                    Me.DisplayPageHeaderDetails()

                    'Display Menu Items
                    Me.DisplayMenuItems()

                    If Not Page.IsPostBack Then


                    End If

                Else
                    'Redirect to access denied page.
                    Me.Response.Redirect(Me.Request.ApplicationPath & "/AccessDenied.aspx", True)
                End If
            Else
                'Redirect to sign in page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/Login.aspx", True)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub


    Public Overrides Sub NavigateViews(ByRef mlvControl As MultiView, viewIndex As MultiViewOption, Optional sessionVariable As String = "")
        If viewIndex = SystemSetupMultiViewPages.SystemSetupObservationStatus Then
            lblActiveViewTitle.InnerText = "Observation Status Setting"
        ElseIf viewIndex = SystemSetupMultiViewPages.SystemSetupRiskLevels Then
            lblActiveViewTitle.InnerText = "Risk Level Setting"
        Else
            lblActiveViewTitle.InnerText = "Risk Category Setting"
        End If
        Session.Add("_activeView", viewIndex)
        MyBase.NavigateViews(mlvSystemSetup, viewIndex, "_activeView")
    End Sub

    Private Sub ChangeView(ByVal viewIndex As MultiViewOption)
        MyBase.NavigateViews(mlvSystemSetup, viewIndex)
    End Sub

    Private Sub NavigateObservationViews(ByVal viewIndex As MultiViewOption, Optional ByVal IsNew As Boolean = True)
        mvObservation.ActiveViewIndex = viewIndex
        If viewIndex = MultiViewOption.AddEditEntityView Then
            aeObservationTitle.Text = IIf(IsNew, "Add New Observation Status", "Edit Observation Status")
        End If
        Session.Add("_obsView", viewIndex)
    End Sub
    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _setupService = New SetupService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Await ShowObservationStatues()
            Me.NavigateViews(mlvSystemSetup, MultiViewOption.EntityListView, "_activeView")
            ChangeView(SystemSetupMultiViewPages.SystemSetupObservationStatus)
        Else
            'If Not IsNothing(Session.Item("_activeView")) Then
            '    ChangeView(DirectCast(Session.Item("_activeView"), SystemSetupMultiViewPages))
            'Else
            '    ChangeView(SystemSetupMultiViewPages.SystemSetupObservationStatus)
            'End If
        End If

        'If IsReadHold Then
        '    _isReadHold = True
        '    Call Set_Values_For_Display()
        '    Call ShowGuarantors()
        '    _isReadHold = False
        '    If Not String.IsNullOrWhiteSpace(_loanApplication.CEFNumber) Then
        '        Call NavigateViews(MultiViewPages.DetailsView)
        '    End If
        'End If


    End Sub



#Region "OBSERVATION STATUS"

    Protected Sub grdObservationStatuses_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedVal = grdObservationStatuses.SelectedValue.ToString
        Try
            'Call ColorGrid(grdGuarantors)
            grdObservationStatuses.SelectedRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        Catch ex As Exception

        End Try
    End Sub

    Protected Async Sub OnObservationStatusPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdObservationStatuses.PageIndex = e.NewPageIndex
        Await Me.ShowObservationStatues()
    End Sub
    Protected Sub grdObservationStatuses_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub

    Function IsEnabled(ByVal _isEnabled As String) As String
        Return IIf(CBool(_isEnabled) = True, "YES", "NO")
    End Function

    Private Async Function ShowObservationStatues() As Task
        grdObservationStatuses.DataSource = Await _setupService.Get_ObservationStatus_List_Async()
        grdObservationStatuses.DataBind()
    End Function

    Protected Async Sub btnObservationStatusSetup_Click(sender As Object, e As EventArgs)
        lblActiveViewTitle.InnerText = "Observation Status Setting"
        ChangeView(SystemSetupMultiViewPages.SystemSetupObservationStatus)
        Await ShowObservationStatues()
    End Sub

    Private observationStatus As ObservationStatusVm
    Private Property _observationStatus As ObservationStatusVm
        Get
            observationStatus = DirectCast(Session.Item("_observeStatus"), ObservationStatusVm)
            With observationStatus
                .Description = txtObservationStatus.Text
                .IsEnabled = CBool(cboObserveStausEnabled.SelectedValue)
                .Narration = txtObservationNarration.Text
                .StatusCode = txtObservationStatusCode.Text
            End With
            Return observationStatus
        End Get
        Set(value As ObservationStatusVm)
            observationStatus = value
            Session.Add("_observeStatus", observationStatus)
        End Set
    End Property
    Protected Async Sub btnSaveObservation_Click(sender As Object, e As EventArgs)

        If String.IsNullOrWhiteSpace(txtObservationStatus.Text) Or String.IsNullOrWhiteSpace(txtObservationStatusCode.Text) Or String.IsNullOrWhiteSpace(txtObservationNarration.Text) Then
            ShowErrorMessage("Please provided details for all required fields")
            Return
        End If

        Dim _result = Await _setupService.Add_Update_ObservationStatus_Async(_observationStatus)

        If _result.Status Then
            ShowSuccessMessage(_result.Message)
            Await ShowObservationStatues()
            Call clearObservationStatus()
            NavigateObservationViews(MultiViewOption.EntityListView)
        Else
            ShowErrorMessage(_result.Message)
        End If
    End Sub

    Public Function Get_Observation_Status(ByVal status As Boolean) As String
        Return IIf(status, "Yes", "No")
    End Function
    Private Sub clearObservationStatus()
        txtObservationStatus.Text = String.Empty
        txtObservationStatusCode.Text = String.Empty
        txtObservationNarration.Text = String.Empty
    End Sub

    Protected Sub btnCancelObservation_Click(sender As Object, e As EventArgs)
        Call clearObservationStatus()
        NavigateObservationViews(MultiViewOption.EntityListView)
    End Sub

    Protected Sub btnAddObservation_Click(sender As Object, e As EventArgs)
        _observationStatus = New ObservationStatusVm()
        NavigateObservationViews(MultiViewOption.AddEditEntityView)
    End Sub

    Protected Async Sub lblDelObserveStatus_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdObservationStatuses) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdObservationStatuses.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _result = Await _setupService.Delete_ObservationStatus_Async(recordId)
            If _result.Status Then
                Await ShowObservationStatues()
                ShowSuccessMessage(_result.Message)
            Else
                ShowErrorMessage(_result.Message)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If
    End Sub

    Protected Async Sub lblEditObserveStatus_Click(sender As Object, e As EventArgs)
        If Not GridHasRows(grdObservationStatuses) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdObservationStatuses.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _record = Await _setupService.Get_ObservationStatus_ById_Async(recordId)
            If IsNothing(_record) Then
                ShowErrorMessage("Record Not Found!  Please refresh page to fetch current copy of records")
            Else
                _observationStatus = _record
                txtObservationStatus.Text = _record.Description
                txtObservationStatusCode.Text = _record.StatusCode
                cboObserveStausEnabled.SelectedItem.Value = IIf(_record.IsEnabled, 1, 0)
                NavigateObservationViews(MultiViewOption.AddEditEntityView, False)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If
    End Sub

#End Region




#Region "RISK LEVEL"



    Private riskLevel As RiskLevelVm
    Private Property _riskLevel As RiskLevelVm
        Get
            riskLevel = DirectCast(Session.Item("_riskLevel"), RiskLevelVm)
            riskLevel.Description = txtRiskLevelDesc.Text
            riskLevel.RiskScore = txtRiskLevelScore.Text
            Return riskLevel
        End Get
        Set(value As RiskLevelVm)
            riskLevel = value
            Session.Add("_riskLevel", riskLevel)
        End Set
    End Property


    Protected Async Sub btnRistkLevelSetup_Click(sender As Object, e As EventArgs)
        lblActiveViewTitle.InnerText = "Risk Level Setting"
        ChangeView(SystemSetupMultiViewPages.SystemSetupRiskLevels)
        Await ShowRiskLevels()
    End Sub



    Private Sub clearRiskLevel()
        txtRiskLevelDesc.Text = String.Empty
        txtRiskLevelScore.Text = 0.0
    End Sub


    Protected Sub grdRiskLevels_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedVal = grdRiskLevels.SelectedValue.ToString
        Try
            'Call ColorGrid(grdGuarantors)
            grdRiskLevels.SelectedRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        Catch ex As Exception

        End Try
    End Sub

    Protected Async Sub OnRiskLevelPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdRiskLevels.PageIndex = e.NewPageIndex
        Await Me.ShowRiskLevels()
    End Sub
    Protected Sub grdRiskLevels_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub

    Private Async Function ShowRiskLevels() As Task
        grdRiskLevels.DataSource = Await _setupService.Get_RiskLevel_List_Async()
        grdRiskLevels.DataBind()
    End Function

    Private Sub NavigateRiskLevelViews(ByVal viewIndex As MultiViewOption, Optional ByVal IsNew As Boolean = True)
        mvRiskLevels.ActiveViewIndex = viewIndex
        If viewIndex = MultiViewOption.AddEditEntityView Then
            aeRiskLevelTitle.Text = IIf(IsNew, "Add New Risk Level", "Edit Risk Level")
        End If
        Session.Add("_rlView", viewIndex)
    End Sub

    Protected Async Sub btnSaveRiskLevel_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtRiskLevelDesc.Text) Then
            ShowErrorMessage("Please provided details for all required fields")
            Return
        End If

        Dim _result = Await _setupService.Add_Update_RiskLevel_Async(_riskLevel)

        If _result.Status Then
            ShowSuccessMessage(_result.Message)
            Await ShowRiskLevels()
            Call clearRiskLevel()
            NavigateRiskLevelViews(MultiViewOption.EntityListView)
        Else
            ShowErrorMessage(_result.Message)
        End If
    End Sub

    Protected Sub btnCancelRiskLevel_Click(sender As Object, e As EventArgs)
        clearRiskLevel()
        NavigateRiskLevelViews(MultiViewOption.EntityListView)
    End Sub

    Protected Sub btnAddRiskLevel_Click(sender As Object, e As EventArgs)
        _riskLevel = New RiskLevelVm()
        NavigateRiskLevelViews(MultiViewOption.AddEditEntityView)
    End Sub

    Protected Async Sub lblDelRiskLevel_Click(sender As Object, e As EventArgs)
        If Not GridHasRows(grdRiskLevels) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdRiskLevels.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _result = Await _setupService.Delete_RiskLevel_Async(recordId)
            If _result.Status Then
                Await ShowRiskLevels()
                ShowSuccessMessage(_result.Message)
            Else
                ShowErrorMessage(_result.Message)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If
    End Sub

    Protected Async Sub lblEditRiskLevel_Click(sender As Object, e As EventArgs)
        If Not GridHasRows(grdRiskLevels) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdRiskLevels.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _record = Await _setupService.Get_RiskLevel_ById_Async(recordId)
            If IsNothing(_record) Then
                ShowErrorMessage("Record Not Found!  Please refresh page to fetch current copy of records")
            Else
                _riskLevel = _record
                txtRiskLevelDesc.Text = _record.Description
                txtRiskLevelScore.Text = _record.RiskScore

                NavigateRiskLevelViews(MultiViewOption.AddEditEntityView, False)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If
    End Sub



#End Region


#Region "RISK CATEGORIES"


    Protected Async Sub btnRiskCategorySetup_Click(sender As Object, e As EventArgs)
        lblActiveViewTitle.InnerText = "Risk Category Setting"
        ChangeView(SystemSetupMultiViewPages.SystemSetupRiskCategories)
        Await ShowRiskCategories()
    End Sub

    Private Sub NavigateRiskCategoryViews(ByVal viewIndex As MultiViewOption, Optional ByVal IsNew As Boolean = True)
        mlvCategories.ActiveViewIndex = viewIndex
        If viewIndex = MultiViewOption.AddEditEntityView Then
            aeRiskCategoryTitle.Text = IIf(IsNew, "Add New Risk Category", "Edit Risk Category")
        End If
        Session.Add("_rcView", viewIndex)

    End Sub

    Protected Sub grdRiskCategories_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedVal = grdRiskCategories.SelectedValue.ToString
        Try
            'Call ColorGrid(grdGuarantors)
            grdRiskCategories.SelectedRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        Catch ex As Exception

        End Try
    End Sub

    Protected Async Sub OnRiskCategoriesPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdRiskCategories.PageIndex = e.NewPageIndex
        Await Me.ShowRiskCategories()
    End Sub
    Protected Sub grdRiskCategories_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub

    Private Async Function ShowRiskCategories() As Task
        grdRiskCategories.DataSource = Await _setupService.Get_RiskCategory_List_Async()
        grdRiskCategories.DataBind()
    End Function


    Private riskCategory As RiskCategoryVm
    Private Property _riskCategory As RiskCategoryVm
        Get
            riskCategory = DirectCast(Session.Item("_riskCategory"), RiskCategoryVm)
            With riskCategory
                .RiskCategoryDesc = txtRiskCategory.Text
            End With
            Return riskCategory
        End Get
        Set(value As RiskCategoryVm)
            riskCategory = value
            Session.Add("_riskCategory", riskCategory)
        End Set
    End Property

    Private Sub clearRiskCategory()
        txtRiskCategory.Text = String.Empty
    End Sub

    Protected Async Sub lblDelRiskCategory_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdRiskCategories) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdRiskCategories.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _result = Await _setupService.Delete_RiskCategory_Async(recordId)
            If _result.Status Then
                Await ShowRiskCategories()
                ShowSuccessMessage(_result.Message)
            Else
                ShowErrorMessage(_result.Message)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If
    End Sub

    Protected Async Sub lblEditRiskCategory_Click(sender As Object, e As EventArgs)


        If Not GridHasRows(grdRiskCategories) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As String = Convert.ToInt32(grdRiskCategories.DataKeys(gr.RowIndex).Value.ToString())

        If recordId > 0 Then
            Dim _record = Await _setupService.Get_RiskCategory_ById_Async(recordId)
            If IsNothing(_record) Then
                ShowErrorMessage("Record Not Found!  Please refresh page to fetch current copy of records")
            Else
                _riskCategory = _record
                txtRiskCategory.Text = _record.RiskCategoryDesc
                NavigateRiskCategoryViews(MultiViewOption.AddEditEntityView, False)
            End If
        Else
            ShowErrorMessage("Invalid Record!  Please refresh page to fetch current copy of records")
        End If

    End Sub

    Protected Sub btnAddRiskCategory_Click(sender As Object, e As EventArgs)
        _riskCategory = New RiskCategoryVm()
        NavigateRiskCategoryViews(MultiViewOption.AddEditEntityView)
    End Sub

    Protected Async Sub btnSaveRiskCategory_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtRiskCategory.Text) Then
            ShowErrorMessage("Please provided details for all required fields")
            Return
        End If

        Dim _result = Await _setupService.Add_Update_RiskCategory_Async(_riskCategory)

        If _result.Status Then
            ShowSuccessMessage(_result.Message)
            Await ShowRiskCategories()
            Call clearRiskCategory()
            NavigateRiskCategoryViews(MultiViewOption.EntityListView)
        Else
            ShowErrorMessage(_result.Message)
        End If
    End Sub

    Protected Sub btnCancelRiskCategory_Click(sender As Object, e As EventArgs)
        Call clearRiskCategory()
        NavigateRiskCategoryViews(MultiViewOption.EntityListView)
    End Sub







#End Region
End Class