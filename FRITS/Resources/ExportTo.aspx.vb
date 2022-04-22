Partial Public Class ExportTo
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

#End Region

#Region " Events "

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            If Me.txtLocation.Text <> "" AndAlso Me.txtName.Text <> "" Then

                Dim filepath As String = (IIf(Not Me.txtLocation.Text.EndsWith("\"), Me.txtLocation.Text & "\", Me.txtLocation.Text) & Me.txtName.Text & ".xls").Replace("\\", "\")

                Me.ClosePage(Functions.Encode64(filepath))
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('All fields required.');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

End Class