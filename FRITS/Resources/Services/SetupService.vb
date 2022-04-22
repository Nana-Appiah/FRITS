Imports System.Data.Entity
Imports System.Threading.Tasks
Imports FRITS.DAL

Public Class SetupService
    Inherits ServiceBase

    Private branchList As List(Of Branch)

    Sub New(ByVal branchCode As String, ByVal branchName As String, ByVal UserId As Integer)
        _context = New AppDbContext()
        _currentUserBranchCode = branchCode
        _currentUserBranchName = branchName
        _currentUserId = UserId.ToString()
    End Sub



    Private ReadOnly Property _branchList As List(Of Branch)
        Get
            If IsNothing(branchList) Then
                branchList = New BranchContext().Branches.ToList()
            End If
            Return branchList
        End Get
    End Property




    Public Function Get_Branch_List_Async() As List(Of Branch)
        Return _branchList.ToList()
    End Function


    Public Async Function Get_Risk_Sub_Category_List() As Task(Of List(Of KeyValueObject(Of String)))
        Return Await _context.Findings.Select(Of KeyValueObject(Of String))(Function(x) New KeyValueObject(Of String) With {.Key = x.RiskSubCategory, .Value = x.RiskSubCategory}).Distinct().ToListAsync()
    End Function

    Public Async Function Get_Management_Awareness_List() As Task(Of List(Of KeyValueObject(Of String)))
        Return Await _context.Findings.Select(Of KeyValueObject(Of String))(Function(x) New KeyValueObject(Of String) With {.Key = x.ManagementAwareness, .Value = x.ManagementAwareness}).Distinct().ToListAsync()
    End Function


#Region "OBSERVATION_STATUS"

    Public Async Function Add_Update_ObservationStatus_Async(ByVal observationStatus As ObservationStatusVm) As Task(Of ServiceActionResult(Of String))

        If Not observationStatus.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid observation status record"}
        End If

        Dim _observationStatus = _context.ObservationStatus.SingleOrDefault(Function(x) x.StatusCode = observationStatus.StatusCode Or x.Description = observationStatus.Description)

        If Not _observationStatus Is Nothing Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Observation status already exit!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew(Function()

                                                   If observationStatus.isNew Then

                                                       _observationStatus = New ObservationStatus With {.Description = observationStatus.Description, .StatusCode = observationStatus.StatusCode, .IsEnabled = True, .Narration = observationStatus.Narration}

                                                       Call Add(_observationStatus)
                                                   Else

                                                       _observationStatus = _context.ObservationStatus.SingleOrDefault(Function(x) x.ObservationStatusId = observationStatus.ObservationStatusId)

                                                       _observationStatus.Description = observationStatus.Description
                                                       _observationStatus.StatusCode = observationStatus.StatusCode
                                                       _observationStatus.IsEnabled = observationStatus.IsEnabled
                                                       _observationStatus.Narration = observationStatus.Narration

                                                       Call Update(_observationStatus)
                                                   End If

                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation status successfully saved!"}

                                               End Function)



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_observationstatus")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Delete_ObservationStatus_Async(ByVal observationStatusId As Integer) As Task(Of ServiceActionResult(Of String))

        Dim _observationStatus = _context.ObservationStatus.SingleOrDefault(Function(x) x.ObservationStatusId = observationStatusId)

        If IsNothing(_observationStatus) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Observation status not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_observationStatus)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation status successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_observationstatus")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Get_ObservationStatus_List_Async() As Task(Of List(Of ObservationStatusVm))

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.ObservationStatus.Select(Of ObservationStatusVm)(Function(x) New ObservationStatusVm With {
                                                    .Description = x.Description,
                                                    .IsEnabled = x.IsEnabled,
                                                    .ObservationStatusId = x.ObservationStatusId,
                                                    .StatusCode = x.StatusCode,
                                                    .Narration = x.Narration
                                                }).ToList()
                                           End Function)

    End Function

    Public Async Function Get_ObservationStatus_ById_Async(ByVal observationStatusId As Integer) As Task(Of ObservationStatusVm)

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.ObservationStatus.Select(Of ObservationStatusVm)(Function(x) New ObservationStatusVm With {
                                                    .Description = x.Description,
                                                    .IsEnabled = x.IsEnabled,
                                                    .ObservationStatusId = x.ObservationStatusId,
                                                    .StatusCode = x.StatusCode,
                                                    .Narration = x.Narration
                                                }).SingleOrDefault(Function(s) s.ObservationStatusId = observationStatusId)
                                           End Function)

    End Function


#End Region




#Region "RISK LEVELS"

    Public Async Function Add_Update_RiskLevel_Async(ByVal riskLevel As RiskLevelVm) As Task(Of ServiceActionResult(Of String))

        If Not riskLevel.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid risk level record"}
        End If

        Dim _riskLevel = _context.RiskLevels.SingleOrDefault(Function(x) x.Description = riskLevel.Description)

        If Not _riskLevel Is Nothing Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Risk level already exit!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If riskLevel.isNew Then

                                                        _riskLevel = New RiskLevel With {.Description = riskLevel.Description, .RiskScore = riskLevel.RiskScore}

                                                        Call Add(_riskLevel)
                                                    Else
                                                        _riskLevel = _context.RiskLevels.SingleOrDefault(Function(x) x.RiskLevelId = riskLevel.RiskLevelId)
                                                        _riskLevel.Description = riskLevel.Description
                                                        _riskLevel.RiskScore = riskLevel.RiskScore
                                                        Call Update(_riskLevel)
                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Risk level successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_risklevel")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Delete_RiskLevel_Async(ByVal riskLevelId As Integer) As Task(Of ServiceActionResult(Of String))

        Dim _riskLevel = _context.RiskLevels.SingleOrDefault(Function(x) x.RiskLevelId = riskLevelId)

        If IsNothing(_riskLevel) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Risk level not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_riskLevel)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Risk level successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_risklevel")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Get_RiskLevel_List_Async() As Task(Of List(Of RiskLevelVm))

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.RiskLevels.Select(Of RiskLevelVm)(Function(x) New RiskLevelVm With {
                                                    .Description = x.Description,
                                                    .RiskLevelId = x.RiskLevelId,
                                                    .RiskScore = x.RiskScore
                                                }).ToList()
                                           End Function)

    End Function


    Public Async Function Get_RiskLevel_ById_Async(ByVal riskLevelId As Integer) As Task(Of RiskLevelVm)

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.RiskLevels.Select(Of RiskLevelVm)(Function(x) New RiskLevelVm With {
                                                    .Description = x.Description,
                                                    .RiskLevelId = x.RiskLevelId,
                                                     .RiskScore = x.RiskScore
                                                }).SingleOrDefault(Function(s) s.RiskLevelId = riskLevelId)
                                           End Function)

    End Function


#End Region




#Region "RISK CATEGORY"




    Public Async Function Add_Update_RiskCategory_Async(ByVal riskCategory As RiskCategoryVm) As Task(Of ServiceActionResult(Of String))

        If Not riskCategory.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid risk level record"}
        End If

        Dim _riskCategory = _context.RiskCategories.SingleOrDefault(Function(x) x.RiskCategoryDesc = riskCategory.RiskCategoryDesc)

        If Not _riskCategory Is Nothing Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Risk category already exit!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If riskCategory.isNew Then
                                                        _riskCategory = New RiskCategory With {.RiskCategoryDesc = riskCategory.RiskCategoryDesc}

                                                        Call Add(_riskCategory)
                                                    Else

                                                        _riskCategory = _context.RiskCategories.SingleOrDefault(Function(x) x.RiskCategoryId = riskCategory.RiskCategoryId)
                                                        _riskCategory.RiskCategoryDesc = riskCategory.RiskCategoryDesc
                                                        Call Update(_riskCategory)
                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Risk category successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_riskcategory")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Delete_RiskCategory_Async(ByVal riskCategoryId As Integer) As Task(Of ServiceActionResult(Of String))

        Dim _riskCategory = _context.RiskCategories.SingleOrDefault(Function(x) x.RiskCategoryId = riskCategoryId)

        If IsNothing(_riskCategory) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Risk category not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_riskCategory)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Risk category successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_riskcategory")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Get_RiskCategory_List_Async() As Task(Of List(Of RiskCategoryVm))

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.RiskCategories.Select(Of RiskCategoryVm)(Function(x) New RiskCategoryVm With {
                                                    .RiskCategoryId = x.RiskCategoryId,
                                                    .RiskCategoryDesc = x.RiskCategoryDesc
                                                }).ToList()
                                           End Function)

    End Function


    Public Async Function Get_RiskCategory_ById_Async(ByVal riskCategoryId As Integer) As Task(Of RiskCategoryVm)

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.RiskCategories.Select(Of RiskCategoryVm)(Function(x) New RiskCategoryVm With {
                                                    .RiskCategoryId = x.RiskCategoryId,
                                                    .RiskCategoryDesc = x.RiskCategoryDesc
                                                }).SingleOrDefault(Function(s) s.RiskCategoryId = riskCategoryId)
                                           End Function)

    End Function





#End Region











End Class
