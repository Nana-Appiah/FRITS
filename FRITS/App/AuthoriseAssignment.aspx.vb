Imports System.Threading.Tasks

Public Class AuthoriseAssignment
    Inherits PageBase


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


    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Call Show_StartUpInfo("welcome")
        If Not IsPostBack Then
            _itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            _assignList = New List(Of BranchReviewVm)
            Await Show_Assignments()
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

    Protected Async Sub OnAssignListPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdAssignList.PageIndex = e.NewPageIndex
        Await Me.Show_Assignments()
    End Sub
    Protected Sub grdAssignList_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" & e.Row.RowIndex.ToString))
        End If
    End Sub


    Private Async Function Show_Assignments() As Task
        _assignList = Await _itService.Get_UnAuthorisd_Review_Assignments_List_Async()
        grdAssignList.DataSource = _assignList.ToList()
        grdAssignList.DataBind()
    End Function

    Protected Async Sub lblAuthoriseAssignment_Click(sender As Object, e As EventArgs)

        If Not GridHasRows(grdAssignList) Then Return

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)

        Dim recordId As Guid = Guid.Parse(grdAssignList.DataKeys(gr.RowIndex).Value.ToString())

        Dim _result = Await _itService.Authorise_Review_Assignment_Async(recordId)

        If _result.Status Then
            _assignList = New List(Of BranchReviewVm)
            Call ShowSuccessMessage(_result.Message)
            Await Show_Assignments()
        Else
            ShowErrorMessage(_result.Message)
        End If

    End Sub



    Protected Sub btnCancelAssignment_Click(sender As Object, e As EventArgs)
        _assignList = New List(Of BranchReviewVm)
        Call GoTo_GoBackTo_Page()
    End Sub



End Class