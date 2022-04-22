Public Partial Class Login
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

#End Region

#Region " Events "

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Me.Request.QueryString("pg") = "reset" Then
                Me.PlaceHolder1.Controls.Add(LoadControl("Utilities\ProfileManager\CtlForgottenPassword.ascx"))
            Else
                Me.PlaceHolder1.Controls.Add(LoadControl("Utilities\ProfileManager\CtlLogin.ascx"))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class