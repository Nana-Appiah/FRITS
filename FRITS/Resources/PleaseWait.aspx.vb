Imports System.Messaging

Partial Public Class PleaseWait
    Inherits PageBase

#Region " Declarations"

    Protected WithEvents MessageQueue As MessageQueue

    Private Const QUEUE_NAME As String = ".\private$\msg_out"

#End Region

#Region " Overrides"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub Receive()
        Try
            ' Get a MessageQueue object for the queue
            Me.MessageQueue = GetMessageQueue(QUEUE_NAME)

            ' Start listening for messages
            Me.MessageQueue.BeginReceive()

        Catch ex As Exception
            ''Log error to file
            'AppLog.Write("Error Receiving Message", Log.LogType.Err, True)
        End Try
    End Sub

    Private Function GetMessageQueue(ByVal queueName As String) As MessageQueue

        Dim msgQ As MessageQueue

        'Create the queue if it does not already exist
        If Not MessageQueue.Exists(queueName) Then
            Try
                'Create the message queue and the MessageQueue object
                msgQ = MessageQueue.Create(queueName)
            Catch CreateException As Exception
                'Error could occur creating queue if the code does
                'not have sufficient permissions to create queues.
                Throw New Exception("Error Creating Queue", CreateException)
            End Try
        Else
            Try
                msgQ = New MessageQueue(queueName)
            Catch GetException As Exception
                Throw New Exception("Error Getting Queue", GetException)
            End Try
        End If

        Return msgQ

    End Function

#End Region

#Region " Events "

    Private Sub ReceiveCompleted(ByVal sender As Object, ByVal e As ReceiveCompletedEventArgs) Handles MessageQueue.ReceiveCompleted
        'Try
        '    Dim msg As System.Messaging.Message
        '    Dim msgText As String

        '    'Get message from event arguments (e)
        '    msg = e.Message

        '    'Set up the formatter with the only expected type (string)
        '    msg.Formatter = New XmlMessageFormatter(New System.Type() {GetType(String)})

        '    'Grab the body and cast to String
        '    msgText = CStr(msg.Body)

        '    If msgText <> "" Then

        '        Me.Session("UPLOAD_SUCCESSFUL") = "1"
        '        Me.Session("UPLOAD_SUCCESSFUL_URL") = "FileUpload.aspx?sid=" & Me.Session.SessionID & "&msg=" & msgText

        '    End If

        'Catch ex As Exception

        'End Try

        '' Start listening for messages
        'Me.MessageQueue.BeginReceive()

    End Sub

#End Region

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Try
    '        Dim msg As System.Messaging.Message
    '        Dim msgText As String

    '        Me.MessageQueue = GetMessageQueue(QUEUE_NAME)

    '        'Get message from event arguments (e)
    '        msg = Me.MessageQueue.Receive(TimeSpan.FromSeconds(1))

    '        'Set up the formatter with the only expected type (string)
    '        msg.Formatter = New XmlMessageFormatter(New System.Type() {GetType(String)})

    '        'Grab the body and cast to String
    '        msgText = CStr(msg.Body)

    '        If msgText <> "" Then
    '            Me.ShowMessage(Functions.Decode64(msgText))
    '            Me.ClosePage("FileUpload.aspx?sid=" & Me.Session.SessionID)
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub

End Class