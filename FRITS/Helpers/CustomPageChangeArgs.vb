Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls


Public Class CustomPageChangeArgs
    Inherits EventArgs

    Private _currentPageNumber As Integer
    Public Property CurrentPageNumber() As Integer
        Get
            Return _currentPageNumber
        End Get
        Set(value As Integer)
            _currentPageNumber = value
        End Set
    End Property

    Private _totalPages As Integer
    Public Property TotalPages() As Integer
        Get
            Return _totalPages
        End Get
        Set(value As Integer)
            _totalPages = value
        End Set
    End Property

    Private _currentPageSize As Integer
    Public Property CurrentPageSize() As Integer
        Get
            Return _currentPageSize
        End Get
        Set(value As Integer)
            _currentPageSize = value
        End Set
    End Property

End Class