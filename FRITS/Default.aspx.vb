Public Class _Default
    Inherits PageBase

#Region " Properties"

    'Public ReadOnly Property BranchCode() As String
    '    Get
    '        Return Me.CurrentUser.GetUserObject.GetPropertyValue("BranchCode")
    '    End Get
    'End Property

#End Region

#Region " Methods"

#End Region

#Region " Events "

    Private Sub _Default_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Check for first time logon
        If Me.CurrentUser.GetUserObject.GetPropertyValue("FirstTimeLogOn") = "Yes" Then
            Me.Response.Redirect("MyProfile.aspx?FirstTimeLogOn=yes", True)
        End If
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Call Show_StartUpInfo("welcome")

        If Not IsPostBack Then
            'Call ShowNotifications()
            ' Call ShowAssetSummary()
            ' Call ShowStatusSummary()
            ' Call ShowConditionSummary()
            'Call TestPieChart()
            'Call TestLineChart()
            'Call TestBarChart()
            'Call ShowAssetPurchaseRateChart()

        End If

    End Sub


    'Private Async Function HasDueDepreciationRecords(ByVal catId As Integer) As Tasks.Task(Of Integer)

    '    Dim faItmRepo As New FixedAssetItemRepository(_context)
    '    Dim dueDep As Integer = 0

    '    Await Tasks.Task.Run(Sub()

    '                             For Each faItm As FixedAssetItem In faItmRepo.Get_AssignedFixedAssetItems.Where(Function(o) o.FixedAsset.CategoryId = catId).ToList()

    '                                 If Check_DueDepreciation(faItm) Then
    '                                     dueDep += 1
    '                                 End If

    '                             Next

    '                         End Sub)


    '    Return dueDep
    'End Function





    Public Sub showLineChart(chartContainerName As String, lineData As String, colsNames As String, chartTitle As String, chartHeight As Integer, chartWidth As Integer)
        Dim str = "drawLineChart('" & chartContainerName.ToString & "', " & lineData.ToString & ", " & colsNames.ToString & ", '" & chartTitle & "', " & chartHeight & ", " & chartWidth & ");"
        Call CallJavaFunction(str, "shLine")
    End Sub

    Public Sub showPieChart(chartContainerName As String, pieData As String, chartTitle As String, dispNmeDesc As String, dispValDesc As String, Optional do3D As Boolean = False, Optional doSlice As Boolean = False, Optional showLegend As Boolean = False)
        Dim str = "drawPieChart('" & chartContainerName.ToString & "', " & pieData.ToString & ", '" & chartTitle & "', '" & dispNmeDesc & "', '" & dispValDesc & "', " & LCase(do3D.ToString) & ", " & LCase(doSlice.ToString) & ", " & LCase(showLegend.ToString) & ");"
        Call CallJavaFunction(str, "shPieC")
    End Sub

    Public Sub showBarChart(chartContainerName As String, barData As String, chartTitle As String, barDirection As String, Optional showLegend As Boolean = False, Optional subTitles As String = Nothing)
        Dim str = "drawBarChart('" & chartContainerName.ToString & "', " & barData.ToString & ", '" & chartTitle & "', '" & barDirection & "', " & LCase(showLegend.ToString) & ", '" & subTitles & "');"
        Call CallJavaFunction(str, "shBarC")
    End Sub


    Private Class PieChartData
        Public displayName As String
        Public displayValue As String
        Sub New()
            displayName = Nothing
            displayValue = Nothing
        End Sub
    End Class

    Private Sub TestPieChart()

        'Dim dt As New List(Of PieChartData)
        'Dim faItmRepo As New FixedAssetItemRepository(_context)
        'Dim purchYrs = faItmRepo.GetRecords().GroupBy(Function(x) x.PurchaseDate.Year).Select(Function(o) New With {o.Key}).Distinct()

        'Dim astCats = New CategoryRepository(_context).GetRecords().ToList()

        'For Each cat In astCats.ToList()
        '    Dim iCount As Integer = 0
        '    For Each yr In purchYrs.ToList()
        '        Dim tCount = faItmRepo.GetRecords().Where(Function(o) o.PurchaseDate.Year = yr.Key.ToString AndAlso o.FixedAsset.Category.CategoryCode = cat.CategoryCode).ToList().Count()
        '        dt.Add(New PieChartData With {.displayName = cat.CategoryCode & " " & yr.Key.ToString,
        '                                       .displayValue = tCount})
        '    Next
        'Next

        'Dim chartData As String = ConvertToJsonFormat(dt.ToList())

        'showPieChart("pieChart", chartData, "Test Pie Chart", "Category", "Total Purchased", True, True)

    End Sub



    Private Class BarChartData
        Public seriesName As String
        Public displayValues As ArrayList
        Sub New()
            seriesName = Nothing
            displayValues = New ArrayList()
        End Sub
    End Class
    ' Private Sub TestBarChart()

    'Dim barData As New List(Of BarChartData)

    'Dim faItmRepo As New FixedAssetItemRepository(_context)
    'Dim purchYrs = faItmRepo.GetRecords().GroupBy(Function(x) x.PurchaseDate.Year).Select(Function(o) New With {o.Key}).Distinct()

    'Dim astCats = New CategoryRepository(_context).GetRecords().ToList()

    'For Each cat In astCats.ToList()

    '    Dim srData As New BarChartData
    '    srData.seriesName = cat.CategoryCode

    '    Dim ptData As New List(Of Decimal)

    '    For Each yr In purchYrs.ToList()
    '        Dim tCount = faItmRepo.GetRecords().Where(Function(o) o.PurchaseDate.Year = yr.Key.ToString AndAlso o.FixedAsset.Category.CategoryCode = cat.CategoryCode).ToList().Count()
    '        ptData.Add(tCount)
    '    Next

    '    srData.displayValues.Add(ptData)
    '    barData.Add(srData)

    'Next

    'Dim strBarData = ConvertToJsonFormat(barData)
    'showBarChart("barChart", strBarData, "Test Bar Chart", "vertical", True)

    ' End Sub
    ' Private Sub TestLineChart()

    'Dim chartCols As String = Nothing
    'Dim chartData As String = Nothing

    'Dim faItmRepo As New FixedAssetItemRepository(_context)

    'Dim purchYrs = faItmRepo.GetRecords().GroupBy(Function(x) x.PurchaseDate.Year).Select(Function(o) New With {o.Key}).Distinct()

    'Dim strBuild As New StringBuilder()
    'Dim iCnt As Integer = 0
    'strBuild.Append("[")
    'For Each yr In purchYrs.ToList()
    '    strBuild.AppendLine("['number', " & yr.Key.ToString & "]")
    '    iCnt += 1
    '    If iCnt <> purchYrs.Count Then
    '        strBuild.Append(",")
    '    End If
    'Next
    'strBuild.AppendLine("]")
    'chartCols = strBuild.ToString()

    'Dim astCats = New CategoryRepository(_context).GetRecords().ToList()

    'strBuild = New StringBuilder
    'strBuild.Append("[")
    'Dim imCount As Integer = 0
    'For Each cat In astCats.ToList()
    '    strBuild.AppendLine("[")
    '    strBuild.Append("'" & cat.CategoryCode.ToString() & "',")
    '    Dim iCount As Integer = 0
    '    For Each yr In purchYrs.ToList()
    '        Dim tCount = faItmRepo.GetRecords().Where(Function(o) o.PurchaseDate.Year = yr.Key.ToString AndAlso o.FixedAsset.Category.CategoryCode = cat.CategoryCode).ToList().Count()
    '        iCount += 1
    '        If iCount = purchYrs.Count Then
    '            strBuild.Append(tCount)
    '        Else
    '            strBuild.Append(tCount & ",")
    '        End If
    '    Next
    '    strBuild.Append("]")
    '    imCount += 1
    '    If imCount <> astCats.Count Then
    '        strBuild.Append(",")
    '    End If

    'Next
    'strBuild.AppendLine("]")

    'chartData = strBuild.ToString()


    'showLineChart("testChart", chartData, chartCols, "Test Line Chart", 300, 400)

    ' End Sub



    'Private Sub ShowAssetPurchaseRateChart()

    '    Dim dtYrs As New DataTable
    '    Dim dtSmry As New DataTable
    '    Dim dsSe As New DataSet
    '    Dim dtClone As New DataTable
    '    Dim chtSeries As String = Nothing

    '    Dim astCats = New CategoryRepository(_context).GetRecords().ToList()
    '    dtClone.Columns.Add("Category")
    '    dtClone.Columns.Add("Data")


    '    For i = 0 To astCats.Count - 1
    '        dtClone.Rows.Add(astCats(i).CategoryCode, 0)
    '        If IsNothing(chtSeries) Then
    '            chtSeries = astCats(i).CategoryCode
    '        Else
    '            chtSeries &= ", " & astCats(i).CategoryCode
    '        End If
    '    Next

    '    dtYrs.Columns.Add("Year", GetType(System.String))
    '    dtSmry.Columns.Add("Year", GetType(System.String))
    '    dtSmry.Columns.Add("Count", GetType(Integer))

    '    Dim faItmRepo As New FixedAssetItemRepository(_context)

    '    Dim purchYrs = faItmRepo.GetRecords().GroupBy(Function(x) x.PurchaseDate.Year).Select(Function(o) New With {o.Key}).Distinct()

    '    For Each yr In purchYrs.ToList()

    '        With dtYrs.Rows()
    '            .Add(yr.Key.ToString())
    '        End With
    '        With dtSmry.Rows
    '            .Add(yr.Key.ToString, faItmRepo.GetRecords().Where(Function(o) o.PurchaseDate.Year = yr.Key.ToString()).Count())
    '        End With
    '        dsSe.Tables.Add(yr.Key.ToString)
    '        dsSe.Tables(yr.Key.ToString).Merge(dtClone)

    '        For Each rw As DataRow In dsSe.Tables(yr.Key.ToString).Rows
    '            rw.Item("Data") = faItmRepo.GetRecords().Where(Function(o) o.PurchaseDate.Year = yr.Key.ToString AndAlso o.FixedAsset.Category.CategoryCode = rw.Item("Category").ToString()).ToList().Count()
    '        Next

    '    Next



    '    'With lcAssetPurchRate

    '    '    .CategoriesAxis = chtSeries.ToString()
    '    '    .DisplayValues = True
    '    '    .ValueAxisLines = 5


    '    '    If cboPurchaseChartOption.SelectedValue = "0" Then

    '    '        For Each dt As DataTable In dsSe.Tables()

    '    '            Dim s As New AjaxControlToolkit.LineChartSeries
    '    '            s.Name = dt.TableName
    '    '            Dim d As String = Nothing
    '    '            Dim iCount As Integer = 0
    '    '            Dim y As Decimal() = New Decimal(dt.Rows.Count - 1) {}
    '    '            For Each dr As DataRow In dt.Rows
    '    '                y(iCount) = (dr.Item("Data"))
    '    '                iCount += 1
    '    '            Next
    '    '            s.Data = y
    '    '            .Series.Add(s)

    '    '        Next

    '    '    End If

    '    'End With


    'End Sub








    'Private Sub ShowNotifications()

    '    Dim dt As New DataTable
    '    dt.TableName = "notifications"
    '    dt.Columns.Add("Indx", GetType(System.Int32))
    '    dt.Columns.Add("Title", GetType(System.String))
    '    dt.Columns.Add("NotifCount", GetType(System.Int32))
    '    dt.Columns.Add("NotifDetail", GetType(System.String))

    '    Dim catRepo As New CategoryRepository(_context)

    '    Dim recCount As Integer = 0


    '    If HasPermision("management.depreciation", "depreciation", False) Then

    '        For Each cat As Category In catRepo.GetRecords().ToList()
    '            Dim rslt = HasDueDepreciationRecords(cat.CategoryId).Result
    '            If rslt > 0 Then
    '                dt.Rows.Add(recCount, cat.CategoryName.ToString(), rslt, "There is/are " & rslt & " asset(s) due for depreciation processing!")
    '                recCount += 1
    '            End If
    '        Next

    '    End If

    '    Dim m_logRepo As New AssetMovementLogRepository(_context)

    '    Dim mvRslt As Integer = m_logRepo.GetUnAuthorized(_userBranch, _userDept, Me.ViewAllBranches, Me.ViewAllDepartments).Count()

    '    'For Each movLog As AssetMovementLog In m_logRepo.GetUnAuthorized(_userBranch, _userDept, Me.ViewAllBranches, Me.ViewAllDepartments)
    '    '    mvRslt += 1
    '    'Next
    '    If mvRslt > 0 Then
    '        dt.Rows.Add(recCount, "Un-Authorized Movement Records", mvRslt, "There is/are " & mvRslt & " un-authorized movement records awaiting your approval!")
    '        recCount += 1
    '    End If


    '    mvRslt = 0
    '    'For Each movLog As AssetMovementLog In m_logRepo.GetUnReceivedAssets(_userBranch, _userDept, , Me.ViewAllBranches, Me.ViewAllDepartments).ToList()
    '    '    mvRslt += 1
    '    'Next
    '    mvRslt = m_logRepo.GetUnReceivedAssets(_userBranch, _userDept, , Me.ViewAllBranches, Me.ViewAllDepartments).Count()

    '    If mvRslt > 0 Then
    '        dt.Rows.Add(recCount, "Un-Authorized Movement Records", mvRslt, "There is/are " & mvRslt & " asset(s) pending to be received to your branch!")
    '        recCount += 1
    '    End If


    '    dlNofications.DataSource = dt
    '    dlNofications.DataMember = "notifications"
    '    dlNofications.DataBind()


    'End Sub


    'Private Sub ShowStatusSummary()

    '    Dim statusRepo As New AssetStatusRepository(_context)
    '    grdStatusSummary.DataSource = statusRepo.StatusSummary(_userBranch, _userDept, Me.ViewAllBranches, Me.ViewAllDepartments)
    '    grdStatusSummary.DataBind()
    '    Call ColorGrid(grdStatusSummary)

    'End Sub


    'Private Sub ShowAssetSummary()
    '    Dim faRepo As New FixedAssetRepository(_context)
    '    grdAssetSummary.DataSource = faRepo.AssetSummary(_userBranch, _userDept, Me.ViewAllBranches, Me.ViewAllDepartments)
    '    grdAssetSummary.DataBind()
    '    Call ColorGrid(grdAssetSummary)
    'End Sub


    'Private Sub ShowConditionSummary()
    '    Dim condRepo As New AssetConditionRepository(_context)
    '    grdConditionSummary.DataSource = condRepo.ConditionSummary(_userBranch, _userDept, Me.ViewAllBranches, Me.ViewAllDepartments)
    '    grdConditionSummary.DataBind()
    '    Call ColorGrid(grdConditionSummary)
    'End Sub

End Class