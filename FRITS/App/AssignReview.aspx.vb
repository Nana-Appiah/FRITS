Imports System.Threading.Tasks

Public Class AssignReview
    Inherits PageBase

    Private review As ReviewVm
    Public Property _review As ReviewVm
        Get
            review = DirectCast(Session.Item("asReview"), ReviewVm)
            Return review
        End Get
        Set(value As ReviewVm)
            review = value
            Session.Add("asReview", review)
        End Set
    End Property

    Private reviewId As Guid
    Private Property _reviewId As Guid
        Get
            Dim _rvId = GetRequestParameter("reviewId")
            If Not String.IsNullOrWhiteSpace(_rvId) Then
                reviewId = Guid.Parse(_rvId)
            End If
            Return reviewId
        End Get
        Set(value As Guid)
            reviewId = value
        End Set
    End Property


    Private assignList As List(Of BranchReviewVm)
    Private Property _assignList As List(Of BranchReviewVm)
        Get
            Return DirectCast(Session.Item("_assignList"), List(Of BranchReviewVm))
        End Get
        Set(value As List(Of BranchReviewVm))
            assignList = value
            Session.Add("_assignLIst", assignList)
        End Set
    End Property




    Private ReadOnly Property _assignRecord As BranchReviewVm
        Get
            Return New BranchReviewVm With {
                .BranchCode = cboAssignBranch.SelectedValue,
                .BranchName = cboAssignBranch.SelectedItem.Text,
                .EmployeeId = CInt(cboToEmployee.SelectedValue),
                .EmployeeName = cboToEmployee.SelectedItem.Text,
                .ReviewFrom = CDate(txtReviewFrom.Text),
                .ReviewTo = CDate(txtReviewTo.Text),
                .ReviewId = _review.ReviewId,
                .ReviewInstruction = txtInstruction.Text
            }
        End Get
    End Property




    Private ReadOnly Property _selectedDepartmentAlias As String
        Get
            Return cboToDepartment.SelectedValue
        End Get
    End Property



    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            Call Bind_Dropdown_Combos()
            _assignList = New List(Of BranchReviewVm)
            Await Load_Review()
            Call Show_Assignments()
        End If

    End Sub


    Protected Async Function Load_Review() As Task

        _review = Await _itService.Get_Review_ById_Async(_reviewId)
        If Not IsNothing(_review) Then
            reviewTitle.Text = $"ASSIGN REVIEW - {_review.Description}"

            _assignList = Await _itService.Get_Review_Assignments_List_Async(_review.ReviewId)

        End If

    End Function




    Protected Sub Bind_Dropdown_Combos()

        cboAssignBranch.DataSource = _setupService.Get_Branch_List_Async()
        cboAssignBranch.DataBind()

        cboToDepartment.DataSource = _setupService.Get_UserGroups()
        cboToDepartment.DataBind()

    End Sub

    Private Sub Bind_Employees_Combo()
        Dim _employeesInGroup = _setupService.Get_Users_In_Group(_selectedDepartmentAlias)
        cboToEmployee.DataSource = _employeesInGroup
        cboToEmployee.DataBind()
    End Sub


    Protected Sub cboToDepartment_SelectedIndexChanged(sender As Object, e As EventArgs)

        If Not String.IsNullOrWhiteSpace(_selectedDepartmentAlias) Then
            Call Bind_Employees_Combo()
        End If
    End Sub




    Protected Sub grdAssignList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedVal = grdAssignList.SelectedValue.ToString
        Try
            'Call ColorGrid(grdGuarantors)
            grdAssignList.SelectedRow.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub OnAssignListPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdAssignList.PageIndex = e.NewPageIndex
        Me.Show_Assignments()
    End Sub
    Protected Sub grdAssignList_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub


    Private Sub Show_Assignments()
        grdAssignList.DataSource = _assignList.ToList()
        grdAssignList.DataBind()
    End Sub

    Protected Sub btnAssign_Click(sender As Object, e As EventArgs)

        If Not _assignRecord.IsValid Then
            ShowErrorMessage("Record validation failed! Please check and correct all entries to continue!")
            Return
        End If

        Dim _existRecord = _assignList.SingleOrDefault(Function(x) x.EmployeeId.Equals(_assignRecord.EmployeeId) AndAlso x.BranchCode.Equals(_assignRecord.BranchCode))

        If Not IsNothing(_existRecord) Then _assignList.Remove(_existRecord)

        _assignList.Add(_assignRecord)

        Call Show_Assignments()

        Call ShowSuccessMessage($"Review successfully assigned to {_assignRecord.EmployeeName} - {_assignRecord.BranchName}")

    End Sub


    Protected Async Sub btnSaveAssignment_Click(sender As Object, e As EventArgs)

        Dim _result = Await _itService.Add_Update_BranchReview_Async(_assignList)

        If _result.Status Then
            _assignList = New List(Of BranchReviewVm)
            Call Show_Assignments()
            Call ShowSuccessMessage(_result.Message)
        Else
            ShowErrorMessage(_result.Message)
        End If

    End Sub

    Protected Sub btnCancelAssignment_Click(sender As Object, e As EventArgs)
        _assignList = New List(Of BranchReviewVm)
        Call GoTo_GoBackTo_Page()
    End Sub

    Protected Sub lblRemoveAssignment_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdAssignList) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdAssignList.DataKeys(gr.RowIndex).Value.ToString())

        Dim _newList = _assignList.Where(Function(x) x.BranchReviewId <> recordId).ToList()

        _assignList = _newList

        Call Show_Assignments()

        Call ShowSuccessMessage($"Record successfully removed!")

    End Sub
End Class