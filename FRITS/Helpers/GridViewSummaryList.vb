
'------------------------------------------------------------------------------------------
' Copyright � 2006 Agrinei Sousa [www.agrinei.com]
'
' Esse c�digo fonte � fornecido sem garantia de qualquer tipo.
' Sinta-se livre para utiliz�-lo, modific�-lo e distribu�-lo,
' inclusive em aplica��es comerciais.
' � altamente desej�vel que essa mensagem n�o seja removida.
'------------------------------------------------------------------------------------------

Imports System.Collections.Generic

''' <summary>
''' Summary description for GridViewSummaryList
''' </summary>
Public Class GridViewSummaryList
	Inherits List(Of GridViewSummary)
    Default Public ReadOnly Property Item(name As String) As GridViewSummary
        Get
            Return Me.FindSummaryByColumn(name)
        End Get
    End Property

	Public Function FindSummaryByColumn(columnName As String) As GridViewSummary
		For Each s As GridViewSummary In Me
			If s.Column.ToLower() = columnName.ToLower() Then
				Return s
			End If
		Next

		Return Nothing
	End Function
End Class

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
