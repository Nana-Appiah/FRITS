
'------------------------------------------------------------------------------------------
' Copyright � 2006 Agrinei Sousa [www.agrinei.com]
'
' Esse c�digo fonte � fornecido sem garantia de qualquer tipo.
' Sinta-se livre para utiliz�-lo, modific�-lo e distribu�-lo,
' inclusive em aplica��es comerciais.
' � altamente desej�vel que essa mensagem n�o seja removida.
'------------------------------------------------------------------------------------------

Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls


Public Delegate Sub GroupEvent(groupName As String, values As Object(), row As GridViewRow)

''' <summary>
''' A class that represents a group consisting of a set of columns
''' </summary>
Public Class GridViewGroup
	#Region "Fields"

	Private _columns As String()
	Private _actualValues As Object()
	Private _quantity As Integer
	Private _automatic As Boolean
	Private _hideGroupColumns As Boolean
	Private _isSuppressGroup As Boolean
	Private _generateAllCellsOnSummaryRow As Boolean
	Private mSummaries As GridViewSummaryList

	#End Region

	#Region "Properties"

	Public ReadOnly Property Columns() As String()
		Get
			Return _columns
		End Get
	End Property

	Public ReadOnly Property ActualValues() As Object()
		Get
			Return _actualValues
		End Get
	End Property

	Public ReadOnly Property Quantity() As Integer
		Get
			Return _quantity
		End Get
	End Property

	Public Property Automatic() As Boolean
		Get
			Return _automatic
		End Get
		Set
			_automatic = value
		End Set
	End Property

	Public Property HideGroupColumns() As Boolean
		Get
			Return _hideGroupColumns
		End Get
		Set
			_hideGroupColumns = value
		End Set
	End Property

	Public ReadOnly Property IsSuppressGroup() As Boolean
		Get
			Return _isSuppressGroup
		End Get
	End Property

	Public Property GenerateAllCellsOnSummaryRow() As Boolean
		Get
			Return _generateAllCellsOnSummaryRow
		End Get
		Set
			_generateAllCellsOnSummaryRow = value
		End Set
	End Property

	Public ReadOnly Property Name() As String
		Get
			Return [String].Join("+", Me._columns)
		End Get
	End Property

	Public ReadOnly Property Summaries() As GridViewSummaryList
		Get
			Return mSummaries
		End Get
	End Property

	#End Region

	#Region "Constructors"

	Public Sub New(cfams As String(), isSuppressGroup As Boolean, auto As Boolean, hideGroupColumns As Boolean, generateAllCellsOnSummaryRow As Boolean)
		Me.mSummaries = New GridViewSummaryList()
		Me._actualValues = Nothing
		Me._quantity = 0
		Me._columns = cfams
		Me._isSuppressGroup = isSuppressGroup
		Me._automatic = auto
		Me._hideGroupColumns = hideGroupColumns
		Me._generateAllCellsOnSummaryRow = generateAllCellsOnSummaryRow
	End Sub

	Public Sub New(cfams As String(), auto As Boolean, hideGroupColumns As Boolean, generateAllCellsOnSummaryRow As Boolean)
		Me.New(cfams, False, auto, hideGroupColumns, generateAllCellsOnSummaryRow)
	End Sub

	Public Sub New(cfams As String(), auto As Boolean, hideGroupColumns As Boolean)
		Me.New(cfams, auto, hideGroupColumns, False)
	End Sub

	#End Region

	Friend Sub SetActualValues(values As Object())
		Me._actualValues = values
	End Sub

	Public Function ContainsSummary(s As GridViewSummary) As Boolean
		Return mSummaries.Contains(s)
	End Function

	Public Sub AddSummary(s As GridViewSummary)
		If Me.ContainsSummary(s) Then
			Throw New Exception("Summary already exists in this group.")
		End If

		If Not s.Validate() Then
			Throw New Exception("Invalid summary.")
		End If
		s.SetGroup(Me)
		Me.mSummaries.Add(s)
	End Sub

	Public Sub Reset()
		Me._quantity = 0

		For Each s As GridViewSummary In mSummaries
			s.Reset()
		Next
	End Sub

	Public Sub AddValueToSummaries(dataitem As Object)
		Me._quantity += 1

		For Each s As GridViewSummary In mSummaries
			s.AddValue(DataBinder.Eval(dataitem, s.Column))
		Next
	End Sub

	Public Sub CalculateSummaries()
		For Each s As GridViewSummary In mSummaries
			s.Calculate()
		Next
	End Sub
End Class

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
