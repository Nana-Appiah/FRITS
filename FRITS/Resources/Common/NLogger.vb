Public Class NLogger

    Private Logger As NLog.Logger

    Sub New()

    End Sub

    Sub Log(ByVal level As NLog.LogLevel, ByVal text As String, Optional ByVal type As LogType = LogType.Log, Optional ByVal customname As String = "", Optional ByVal write As Boolean = False)

        If write Then

            If type = LogType.Err Then
                Me.Logger = LogManager.GetLogger("Error")
            ElseIf type = LogType.Log Then
                Me.Logger = LogManager.GetLogger("Log")
            ElseIf type = LogType.Sys Then
                Me.Logger = LogManager.GetLogger("Sys")
            ElseIf type = LogType.Custom Then
                Me.Logger = LogManager.GetLogger(customname)
            End If

            Me.Logger.Log(level, text)

        End If

    End Sub

    Enum LogType As Integer
        Err
        Log
        Sys
        Custom
    End Enum

End Class
