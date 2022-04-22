Imports System.Data.Entity
Imports System.Threading.Tasks
Imports FRITS.DAL

Public Class ITService
    Inherits ServiceBase

    Sub New(ByVal branchCode As String, ByVal branchName As String, ByVal UserId As Integer)
        _context = New AppDbContext()
        _currentUserBranchCode = branchCode
        _currentUserBranchName = branchName
        _currentUserId = UserId
        _context.loggedOnUser = UserId
    End Sub


#Region "Review_Entity_Methods"


    Public Async Function Generate_ReviewCode() As Task(Of String)

        Dim _recCount = Await _context.Reviews.Where(Function(x) x.CreatedDate.Year = DateTime.UtcNow.Year.ToString()).CountAsync()
        _recCount += 1
        Dim strCount = _recCount.ToString.PadLeft(4, "0")
        Return $"IAD{DateTime.UtcNow.Year.ToString()}{strCount}"

    End Function

    Public Async Function Add_Update_Review_Async(ByVal review As ReviewVm) As Task(Of ServiceActionResult(Of String))

        If Not review.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid review record"}
        End If

        Dim _review = _context.Reviews.SingleOrDefault(Function(x) x.Description = review.Description)

        If Not _review Is Nothing Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review already exit!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If review.isNew Then

                                                        _review = New Review With {.Description = review.Description, .ReviewCode = review.ReviewCode}

                                                        Call Add(_review)
                                                    Else
                                                        _review = _context.Reviews.SingleOrDefault(Function(x) x.ReviewId = review.ReviewId)
                                                        _review.Description = review.Description
                                                        _review.ReviewCode = review.ReviewCode

                                                        Call Update(_review)
                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_review")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Get_Review_List_Async(Optional filterOptions As RecordFilterOption = Nothing) As Task(Of List(Of ReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               Dim _query = _context.Reviews.Where(Function(a) a.IsAuthorised = True And a.IsClosed = False)


                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewCode) Then
                                                   _query = _query.Where(Function(a) a.ReviewCode = filterOptions.reviewCode).AsEnumerable()
                                               End If

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewName) Then
                                                   _query = _query.Where(Function(a) a.Description.Contains(filterOptions.reviewName)).AsEnumerable()
                                               End If

                                               If Not filterOptions.fromDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date >= filterOptions.fromDate.Date)
                                               End If

                                               If Not filterOptions.toDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date <= filterOptions.fromDate.Date)
                                               End If

                                               Return _query.Select(Of ReviewVm)(Function(x) New ReviewVm With {
                                                    .Description = x.Description,
                                                    .ReviewId = x.ReviewId,
                                                    .ReviewCode = x.ReviewCode,
                                                    .IsClosed = x.ClosedById,
                                                    .ClosedDate = x.ClosedDate,
                                                    .ClosedById = x.ClosedById
                                                }).ToList()
                                           End Function)

    End Function


    Public Async Function Get_Closed_Review_List_Async(Optional filterOptions As RecordFilterOption = Nothing) As Task(Of List(Of ReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               'Dim _empList = Get_EmployeeList().AsEnumerable()

                                               Dim _query = _context.Reviews.Where(Function(a) a.IsAuthorised = True).Where(Function(b) b.IsClosed = True).AsQueryable()

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewCode) Then
                                                   _query = _query.Where(Function(a) a.ReviewCode = filterOptions.reviewCode).AsEnumerable()
                                               End If

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewName) Then
                                                   _query = _query.Where(Function(a) a.Description.Contains(filterOptions.reviewName)).AsEnumerable()
                                               End If

                                               If Not filterOptions.fromDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date >= filterOptions.fromDate.Date)
                                               End If

                                               If Not filterOptions.toDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date <= filterOptions.fromDate.Date)
                                               End If

                                               Return _query.Select(Of ReviewVm)(Function(x) New ReviewVm With {
                                                    .Description = x.Description,
                                                    .ReviewId = x.ReviewId,
                                                    .ReviewCode = x.ReviewCode,
                                                    .IsClosed = x.ClosedById,
                                                    .ClosedDate = x.ClosedDate,
                                                    .ClosedById = x.ClosedById
                                                }).ToList()  '.ClosedByName = _empList.FirstOrDefault(Function(f) f.EmployeeId = x.ClosedById).EmployeeName

                                           End Function)

    End Function


    Public Async Function Get_UnAuthorisd_Review_List_Async() As Task(Of List(Of ReviewVm))

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.Reviews.Where(Function(a) a.IsAuthorised = False And a.CreatedByID <> _currentUserId).Select(Of ReviewVm)(Function(x) New ReviewVm With {
                                                    .Description = x.Description,
                                                    .ReviewId = x.ReviewId,
                                                    .ReviewCode = x.ReviewCode,
                                                    .IsClosed = x.ClosedById,
                                                    .ClosedDate = x.ClosedDate,
                                                    .ClosedById = x.ClosedById
                                                }).ToList()
                                           End Function)

    End Function

    Public Async Function Authorise_Review_Async(ByVal reviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _review As Review = Await _context.Reviews.SingleOrDefaultAsync(Function(x) x.ReviewId = reviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record not found!"}

        _review.IsSelfAuthorise = True

        Call Update(_review)

        Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review successfully authorised!"}

    End Function

    Public Async Function Get_Review_ById_Async(ByVal reviewId As Guid) As Task(Of ReviewVm)

        Return Await Task.Factory.StartNew(Function()
                                               Return _context.Reviews.Where(Function(a) a.IsAuthorised = True).Select(Of ReviewVm)(Function(x) New ReviewVm With {
                                                    .Description = x.Description,
                                                    .ReviewId = x.ReviewId,
                                                    .ReviewCode = x.ReviewCode,
                                                    .IsClosed = x.ClosedById,
                                                    .ClosedDate = x.ClosedDate,
                                                    .ClosedById = x.ClosedById
                                                }).SingleOrDefault(Function(s) s.ReviewId = reviewId)
                                           End Function)

    End Function


    Public Async Function Delete_Review_Async(ByVal reviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _review = _context.Reviews.SingleOrDefault(Function(x) x.ReviewId = reviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_review)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_review")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

#End Region




#Region "Finding_Entity_Methods"


    Public Async Function Generate_Finding_Code_Async(ByVal branchReviewId As Guid, ByVal addedRecordCount As Integer) As Task(Of String)

        Dim _review = _context.BranchReviews.Include("Review").SingleOrDefault(Function(w) w.BranchReviewId = branchReviewId)

        Dim _rCount = Await _context.Findings.Where(Function(w) w.BranchReviewId = branchReviewId).CountAsync()

        Dim _srCount = IIf(_rCount > addedRecordCount, _rCount + 1, addedRecordCount + 1)

        Return $"{_review.Review.ReviewCode}-{_review.BranchCode}-F{_srCount}"

    End Function


    Private Function Validate_FindingList(ByVal findingLIst As List(Of FindingVm)) As Boolean
        Dim _result As Boolean = True
        For Each finding As FindingVm In findingLIst
            If Not finding.IsValid() Then
                _result = False
                Exit For
            End If
        Next
        Return _result
    End Function


    Public Async Function Add_Update_Review_Details_Async(ByVal findingList As List(Of FindingVm)) As Task(Of ServiceActionResult(Of String))

        If Not Validate_FindingList(findingList) Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"One or more finding record(s) failed the validation checks  Plase check and try again!"}
        End If

        Dim _branchReviewIdValidation = findingList.Select(Of Guid)(Function(s) s.BranchReviewId).Distinct().Count() = 1

        If Not _branchReviewIdValidation Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review Validation Failed!"}

        Dim _reviewId = findingList.Select(Of Guid)(Function(s) s.BranchReviewId).Distinct().SingleOrDefault()

        Dim _review = _context.BranchReviews.SingleOrDefault(Function(x) x.BranchReviewId = _reviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record Not found!"}


        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Dim _deletedObservationList As List(Of Observation) = New List(Of Observation)
                                                    Dim _deletedRecommList As List(Of Recommendation) = New List(Of Recommendation)

                                                    Dim _deletedFindingList As List(Of Finding) = _context.Findings.Where(Function(w) w.BranchReviewId = _review.BranchReviewId And w.CreatedByID = _currentUserId).ToList().Where(Function(d) Not findingList.Any(Function(a) d.FindingId = a.FindingId)).ToList()

                                                    For Each finding As FindingVm In findingList

                                                        Dim _finding = _context.Findings.SingleOrDefault(Function(x) x.FindingId = finding.FindingId)

                                                        Dim _findingIsNew As Boolean = IsNothing(_finding)

                                                        If _findingIsNew Then

                                                            _finding = New Finding With {
                                                            .Description = finding.Description,
                                                            .BranchReviewId = finding.BranchReviewId,
                                                            .RiskLevelId = finding.RiskLevelId,
                                                            .ManagementAwareness = finding.ManagementAwareness,
                                                            .RiskCategoryId = finding.RiskCategoryId,
                                                            .RiskSubCategory = finding.RiskSubCategory,
                                                            .FindingNo = finding.FindingNo,
                                                            .BranchCode = finding.BranchCode
                                                        }

                                                            Call Add(_finding)

                                                        Else

                                                            _finding.Description = finding.Description
                                                            _finding.RiskLevelId = finding.RiskLevelId
                                                            _finding.RiskLevelId = finding.RiskLevelId
                                                            _finding.ManagementAwareness = finding.ManagementAwareness
                                                            _finding.RiskCategoryId = finding.RiskCategoryId
                                                            _finding.RiskSubCategory = finding.RiskSubCategory

                                                            Call Update(_finding)

                                                        End If



                                                        _deletedObservationList = _context.Observations.Where(Function(f) f.FindingId = _finding.FindingId).AsEnumerable().Where(Function(d) Not finding.Observations.Any(Function(a) d.ObservationId = a.ObservationId)).ToList()


                                                        For Each observation In finding.Observations

                                                            Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = observation.ObservationId)

                                                            Dim _observationIsNew As Boolean = IsNothing(_observation)

                                                            If _observationIsNew Then
                                                                _observation = New Observation With {
                                                                .Description = observation.Description,
                                                                .FindingId = _finding.FindingId,
                                                                .ObservationDate = observation.ObservationDate,
                                                                .ObservationStatusId = observation.ObservationStatusId,
                                                                .RootCauseAnalysis = observation.RootCauseAnalysis,
                                                                .Implication = observation.Implication,
                                                                .ObservationNo = observation.ObservationNo,
                                                                .RiskLevelId = observation.RiskLevelId,
                                                                .MitigatingControl = observation.MitigatingControl,
                                                                .Assumptions = observation.Assumptions
                                                            }

                                                                Call Add(_observation)

                                                            Else
                                                                _observation.Description = observation.Description
                                                                _observation.ObservationDate = observation.ObservationDate
                                                                _observation.ObservationStatusId = observation.ObservationStatusId
                                                                _observation.RootCauseAnalysis = observation.RootCauseAnalysis
                                                                _observation.Implication = observation.Implication
                                                                _observation.ObservationStatusId = observation.ObservationStatusId
                                                                _observation.RiskLevelId = observation.RiskLevelId
                                                                _observation.MitigatingControl = observation.MitigatingControl
                                                                _observation.Assumptions = observation.Assumptions

                                                                Call Update(_observation)

                                                            End If

                                                            _deletedRecommList = _context.Recommendations.Where(Function(f) f.TypeReferenceId = _observation.ObservationId).AsEnumerable().Where(Function(d) Not observation.Recommendations.Any(Function(a) d.RecommendationId = a.RecommendationId)).ToList()


                                                            For Each recommend As RecommendationVm In observation.Recommendations

                                                                Dim _recommend = _context.Recommendations.SingleOrDefault(Function(x) x.RecommendationId = recommend.RecommendationId)

                                                                Dim _recommendIsNew As Boolean = IsNothing(_recommend)

                                                                If _recommendIsNew Then
                                                                    _recommend = New Recommendation With {
                                                                     .Description = recommend.Description,
                                                                     .TypeReferenceId = _observation.ObservationId,
                                                                     .RecommendationType = RecommendationTypes.Observation_Recommendation
                                                                    }

                                                                    Call Add(_recommend)

                                                                Else
                                                                    _recommend.Description = recommend.Description

                                                                    Call Update(_recommend)

                                                                End If

                                                            Next

                                                        Next



                                                    Next


                                                    If (_deletedRecommList.Count > 0) Then

                                                        Call Delete_Recommendations(_context, _deletedRecommList)

                                                    End If

                                                    If (_deletedObservationList.Count > 0) Then

                                                        Call Delete_Observations(_context, _deletedObservationList)

                                                    End If

                                                    If (_deletedFindingList.Count > 0) Then

                                                        Call Delete_Findings(_context, _deletedFindingList)

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Finding successfully saved!"}

                                                End Function))


        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_finding")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Private Function Delete_Findings(ByRef _context As AppDbContext, ByRef deleteList As List(Of Finding)) As Boolean

        Dim delList = deleteList

        Dim _observeList = _context.Observations.Where(Function(s) delList.AsEnumerable().Any(Function(a) s.FindingId = a.FindingId)).ToList()

        If Delete_Observations(_context, _observeList) Then
            If deleteList.Count > 0 Then
                _context.Observations.RemoveRange(deleteList)
                _context.SaveChanges()
            End If
            Return True
        End If
        Return False
    End Function

    Private Function Delete_Observations(ByRef _context As AppDbContext, ByRef observationList As List(Of Observation)) As Boolean

        Dim delList = observationList

        Dim _deleRecommList = _context.Recommendations.Where(Function(o) delList.Any(Function(a) o.TypeReferenceId = a.ObservationId)).ToList()

        If Delete_Recommendations(_context, _deleRecommList) Then
            If observationList.Count > 0 Then
                _context.Observations.RemoveRange(observationList)
                _context.SaveChanges()
            End If
            Return True
        End If
        Return False
    End Function

    Private Function Delete_Recommendations(ByRef _context As AppDbContext, ByRef recommList As List(Of Recommendation)) As Boolean
        If recommList.Count > 0 Then
            _context.Findings.RemoveRange(recommList)
            _context.SaveChanges()
        End If
        Return True
    End Function




    Public Async Function Add_Update_Finding_Async(ByVal finding As FindingVm) As Task(Of ServiceActionResult(Of String))

        If Not finding.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid finding record"}
        End If

        Dim _review = _context.BranchReviews.SingleOrDefault(Function(x) x.BranchReviewId = finding.BranchReviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record Not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Dim _finding = _context.Findings.SingleOrDefault(Function(x) x.FindingId = finding.FindingId)

            Return Await Task.Factory.StartNew((Function()

                                                    If finding.isNew Then

                                                        _finding = New Finding With {
                                                            .Description = finding.Description,
                                                            .BranchReviewId = finding.BranchReviewId,
                                                            .RiskLevelId = finding.RiskLevelId,
                                                            .ManagementAwareness = finding.ManagementAwareness,
                                                            .RiskCategoryId = finding.RiskCategoryId,
                                                            .RiskSubCategory = finding.RiskSubCategory,
                                                            .FindingNo = finding.FindingNo,
                                                            .BranchCode = finding.BranchCode
                                                        }

                                                        Call Add(_review)

                                                    Else

                                                        _finding.Description = finding.Description
                                                        _finding.RiskLevelId = finding.RiskLevelId
                                                        _finding.RiskLevelId = finding.RiskLevelId
                                                        _finding.ManagementAwareness = finding.ManagementAwareness
                                                        _finding.RiskCategoryId = finding.RiskCategoryId
                                                        _finding.RiskSubCategory = finding.RiskSubCategory

                                                        Call Update(_review)

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Finding successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_finding")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Get_Review_Findings_List_Async(ByVal branchReviewId As Guid, Optional ByVal fullDetails As Boolean = False) As Task(Of List(Of FindingVm))

        Dim _branchReviewObservationIds = _context.Observations.Where(Function(f) f.Finding.BranchReviewId = branchReviewId).Select(Of Guid)(Function(g) g.ObservationId).ToList()

        Dim _recommQueryable = _context.Recommendations.Where(Function(a) _branchReviewObservationIds.Any(Function(o) a.TypeReferenceId = o)).AsQueryable()

        If Not fullDetails Then

            Return Await _context.Findings.Include("Observations").Include("RiskLevel").Include("RiskCategory").Where(Function(w) w.BranchReviewId = branchReviewId).Select(Of FindingVm)(Function(x) New FindingVm With {
                                                    .Description = x.Description,
                                                    .FindingNo = x.FindingNo,
                                                    .BranchCode = x.BranchCode,
                                                    .FindingId = x.FindingId,
                                                    .ManagementAwareness = x.ManagementAwareness,
                                                    .RiskCategoryId = x.RiskCategoryId,
                                                    .RiskLevelId = x.RiskLevelId,
                                                    .RiskSubCategory = x.RiskSubCategory,
                                                    .RiskLevelName = x.RiskLevel.Description,
                                                    .RiskCategoryName = x.RiskCategory.RiskCategoryDesc,
                                                    .BranchReviewId = x.BranchReviewId,
                                                    .Observations = x.Observations.AsEnumerable().Select(Of ObservationVm)(Function(o) New ObservationVm With {
                                                                                           .Description = o.Description,
                                                                                           .FindingId = o.FindingId,
                                                                                           .ObservationDate = o.ObservationDate,
                                                                                           .ObservationId = o.ObservationId,
                                                                                           .ObservationStatusId = o.ObservationStatusId,
                                                                                           .ResolutionTiming = o.ResolutionTiming,
                                                                                           .Implication = o.Implication,
                                                                                           .RootCauseAnalysis = o.RootCauseAnalysis,
                                                                                           .Assumptions = o.Assumptions,
                                                                                           .MitigatingControl = o.MitigatingControl,
                                                                                           .ManagementResponse = o.ManagementResponse,
                                                                                           .ObservationNo = o.ObservationNo,
                                                                                           .RiskLevelId = o.RiskLevelId,
                                                                                           .Recommendations = _recommQueryable.Where(Function(s) s.TypeReferenceId = o.ObservationId).AsEnumerable().Select(Of RecommendationVm)(Function(r) New RecommendationVm() With {
                                                                    .Description = r.Description,
                                                                    .RecommendationId = r.RecommendationId,
                                                                    .RecommendationType = r.RecommendationType,
                                                                    .TypeReferenceId = r.TypeReferenceId}).ToList()
                                                                                            }).ToList()
                                                }).ToListAsync()

        Else

            Return Await _context.Findings.Include("Observations").Include("ActionPlans").Include("FollowUpDetails").Include("FollowUpDetails.FollowUp").Where(Function(w) w.BranchReviewId = branchReviewId).Select(Of FindingVm)(Function(x) New FindingVm With {
                                                    .Description = x.Description,
                                                    .FindingNo = x.FindingNo,
                                                    .BranchCode = x.BranchCode,
                                                    .FindingId = x.FindingId,
                                                    .ManagementAwareness = x.ManagementAwareness,
                                                    .RiskCategoryId = x.RiskCategoryId,
                                                    .RiskLevelId = x.RiskLevelId,
                                                    .RiskSubCategory = x.RiskSubCategory,
                                                    .RiskLevelName = x.RiskLevel.Description,
                                                    .RiskCategoryName = x.RiskCategory.RiskCategoryDesc,
                                                    .BranchReviewId = x.BranchReviewId,
                                                    .Observations = x.Observations.AsEnumerable().Select(Of ObservationVm)(Function(o) New ObservationVm With {
                                                                                           .Description = o.Description,
                                                                                           .FindingId = o.FindingId,
                                                                                           .ObservationDate = o.ObservationDate,
                                                                                           .ObservationId = o.ObservationId,
                                                                                           .ObservationStatusId = o.ObservationStatusId,
                                                                                           .ResolutionTiming = o.ResolutionTiming,
                                                                                           .Implication = o.Implication,
                                                                                           .RootCauseAnalysis = o.RootCauseAnalysis,
                                                                                           .Assumptions = o.Assumptions,
                                                                                           .MitigatingControl = o.MitigatingControl,
                                                                                           .ManagementResponse = o.ManagementResponse,
                                                                                           .ObservationNo = o.ObservationNo,
                                                                                           .RiskLevelId = o.RiskLevelId,
                                                                                           .Recommendations = _recommQueryable.Where(Function(s) s.TypeReferenceId = o.ObservationId).AsEnumerable().Select(Of RecommendationVm)(Function(r) New RecommendationVm(r.RecommendationType) With {
                                                                    .Description = r.Description,
                                                                    .RecommendationId = r.RecommendationId,
                                                                    .RecommendationType = r.RecommendationType,
                                                                    .TypeReferenceId = r.TypeReferenceId}).ToList(),
                                                                                           .ActionPlans = (o.ActionPlans.AsEnumerable().Select(Of ActionPlanVm)(Function(a) New ActionPlanVm With {
                                                                                                                                            .ActionPlanId = a.ActionPlanId,
                                                                                                                                            .Description = a.Description,
                                                                                                                                            .ObservationId = a.ObservationId,
                                                                                                                                            .ScheduleOfficerResponse = a.ScheduleOfficerResponse,
                                                                                                                                            .ResolutionTiming = o.ResolutionTiming,
                                                                                                                                            .ObservationStatusId = o.ObservationStatusId,
                                                                                                                                            .ReferenceOfficerIds = o.ReferenceOfficerIds
                                                                                                                                       }).ToList()),
                                                                                           .FollowUpDetails = (o.FollowUpDetails.AsEnumerable().Select(Of FollowUpDetailVm)(Function(f) New FollowUpDetailVm With {
                                                                                                                         .FollowUpDetailId = f.FollowUpDetailId,
                                                                                                                        .FollowUpId = f.FollowUpId,
                                                                                                                        .ObservationId = f.ObservationId,
                                                                                                                        .FollowUpDescription = f.FollowUp.Description,
                                                                                                                        .FollowUpDate = f.FollowUp.FollowUpDate,
                                                                                                                        .Remarks = f.Remarks}).ToList())
                                                                                            }).ToList()
                                                }).ToListAsync()
        End If


    End Function





    Public Async Function Delete_Finding_Async(ByVal findingId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _finding = _context.Findings.SingleOrDefault(Function(x) x.FindingId = findingId)

        If IsNothing(_finding) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Finding not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Call Remove(_finding)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Finding successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_finding")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

#End Region


#Region "Observation_Entity_Methods"

    Public Async Function Generate_Observation_Code_Async(ByVal findingId As Guid, ByVal addedRecordCount As Integer, findingCode As String) As Task(Of String)

        Dim _rCount = Await _context.Observations.Where(Function(w) w.FindingId = findingId).CountAsync()

        Dim _srCount = IIf(_rCount > addedRecordCount, _rCount + 1, addedRecordCount + 1)

        Return $"{findingCode}-O{_srCount}"

    End Function



    Public Async Function Add_Update_Observation_Async(ByVal observation As ObservationVm) As Task(Of ServiceActionResult(Of String))

        If Not observation.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid observation record"}
        End If

        Dim _finding = _context.Findings.SingleOrDefault(Function(x) x.FindingId = observation.FindingId)

        If IsNothing(_finding) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Finding record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = observation.ObservationId)

            Return Await Task.Factory.StartNew((Function()

                                                    If observation.isNew Then

                                                        _observation = New Observation With {
                                                            .Description = observation.Description,
                                                            .FindingId = observation.FindingId,
                                                            .ObservationDate = observation.ObservationDate,
                                                            .ObservationStatusId = observation.ObservationStatusId,
                                                            .ResolutionTiming = observation.ResolutionTiming,
                                                            .ActionPlans = observation.ActionPlans.Select(Of ActionPlan)(Function(s) New ActionPlan With {.Description = s.Description}).ToList()
                                                        }

                                                        Call Add(_observation)

                                                        Dim _recommendation = observation.Recommendations.Select(Of Recommendation)(Function(s) New Recommendation With {
                                                                .Description = s.Description,
                                                                .RecommendationType = RecommendationTypes.Observation_Recommendation,
                                                                .TypeReferenceId = _observation.ObservationId
                                                            })

                                                        Call AddRang(_recommendation)

                                                    Else

                                                        _observation.Description = observation.Description
                                                        _observation.ObservationDate = observation.ObservationDate
                                                        _observation.ObservationStatusId = observation.ObservationStatusId
                                                        _observation.ResolutionTiming = observation.ResolutionTiming

                                                        Call Update(_observation)

                                                        For Each actPlan As ActionPlanVm In observation.ActionPlans

                                                            Dim _actionPlan = _context.ActionPlans.SingleOrDefault(Function(a) a.ActionPlanId = actPlan.ActionPlanId)

                                                            _actionPlan.Description = actPlan.Description

                                                            Call Update(_actionPlan)

                                                        Next


                                                        For Each recomm As RecommendationVm In observation.Recommendations

                                                            Dim _recommendation = _context.Recommendations.SingleOrDefault(Function(a) a.RecommendationId = recomm.RecommendationId And a.RecommendationType = RecommendationTypes.Observation_Recommendation)

                                                            _recommendation.Description = recomm.Description

                                                            Call Update(_recommendation)

                                                        Next

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_observation")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Get_Finding_Observation_List_Async(ByVal findingId As Guid) As Task(Of List(Of ObservationVm))

        Return Await Task.Factory.StartNew((Function()

                                                ' Dim _obervationStatusList = _context.ObservationStatus.ToList()
                                                Dim _findingObservations = _context.Observations.Where(Function(s) s.FindingId = findingId).Select(Of Guid)(Function(e) e.ObservationId).ToList()
                                                Dim _empList = Get_EmployeeList().ToList()
                                                Dim _recommQueryable = _context.Recommendations.Where(Function(a) _findingObservations.Any(Function(o) a.TypeReferenceId = o)).ToList() '.Where(Function(x) x.RecommendationType = RecommendationTypes.Observation_Recommendation).ToList()

                                                Dim _queryList = _context.Observations.Include("ObservationStatus").Include("ActionPlans").Include("CorrectiveActions").Include("FollowUpDetails").Include("FollowUpDetails.FollowUp").Include("FollowUpDetails.ObservationStatus").Where(Function(s) s.FindingId = findingId).AsQueryable()

                                                Return _queryList.AsEnumerable().Select(Of ObservationVm)(Function(o) New ObservationVm With {
                                                                    .Description = o.Description,
                                                                    .FindingId = o.FindingId,
                                                                    .ObservationDate = o.ObservationDate,
                                                                    .ObservationId = o.ObservationId,
                                                                    .ObservationStatusId = o.ObservationStatusId,
                                                                    .ResolutionTiming = o.ResolutionTiming,
                                                                    .RootCauseAnalysis = o.RootCauseAnalysis,
                                                                    .Implication = o.Implication,
                                                                    .ObservationNo = o.ObservationNo,
                                                                    .Assumptions = o.Assumptions,
                                                                    .MitigatingControl = o.MitigatingControl,
                                                                    .RiskLevelId = o.RiskLevelId,
                                                                    .ManagementResponse = o.ManagementResponse,
                                                                    .CorrectiveActions = o.CorrectiveActions.Select(Of CorrectiveActionVm)(Function(c) New CorrectiveActionVm With {
                                                                    .CorrectionDate = c.CorrectionDate,
                                                                    .CorrectiveActionId = c.CorrectiveActionId,
                                                                    .ObservationId = c.ObservationId,
                                                                    .ObservationStatusId = c.ObservationStatusId,
                                                                    .Remarks = c.Remarks,
                                                                    .ObservationStatusName = c.ObservationStatus.Description}).ToList(),
                                                                    .ActionPlans = o.ActionPlans.Select(Of ActionPlanVm)(Function(a) New ActionPlanVm With {
                                                                    .ActionPlanId = a.ActionPlanId,
                                                                    .Description = a.Description,
                                                                    .ObservationId = a.ObservationId,
                                                                    .ReferenceOfficerIds = o.ReferenceOfficerIds,
                                                                    .ResolutionTiming = o.ResolutionTiming,
                                                                    .ObservationStatusId = o.ObservationStatusId,
                                                                    .AssignedEmployeeNames = (String.Join(", ", _empList.Where(Function(f) o.ReferenceOfficerIds.Split(",").ToList().Select(Of Int32)(Function(k) CInt(k)).ToList().Any(Function(k) f.EmployeeId = k)).Select(Of String)(Function(s) s.EmployeeName).ToList()))}).ToList(),
                                                                    .Recommendations = _recommQueryable.Where(Function(s) s.TypeReferenceId = o.ObservationId).AsEnumerable().Select(Of RecommendationVm)(Function(r) New RecommendationVm() With {
                                                                    .Description = r.Description,
                                                                    .RecommendationId = r.RecommendationId,
                                                                    .RecommendationType = r.RecommendationType,
                                                                    .TypeReferenceId = r.TypeReferenceId}).ToList(),
                                                                    .FollowUpDetails = o.FollowUpDetails.Select(Of FollowUpDetailVm)(Function(f) New FollowUpDetailVm With {
                                                                                                                            .FollowUpDate = f.FollowUp.FollowUpDate,
                                                                                                                            .FollowUpDescription = f.FollowUp.Description,
                                                                                                                            .FollowUpDetailId = f.FollowUpDetailId,
                                                                                                                            .FollowUpId = f.FollowUpId,
                                                                                                                            .ObservationId = f.ObservationId,
                                                                                                                            .ObservationStatusId = f.ObservationStatusId,
                                                                                                                            .ObservationStatusName = f.ObservationStatus.Description,
                                                                                                                            .Remarks = f.Remarks,
                                                                                                                            .FollowUpByEmployeeName = _empList.ToList().SingleOrDefault(Function(z) z.EmployeeId = f.CreatedByID).EmployeeName
                                                                                                                            }).ToList()
                                                    }).ToList()






                                            End Function))




    End Function


    Public Async Function Update_Observation_Async(ByVal observationRecord As ObservationVm, ByVal firstRecomm As KeyValueObject(Of Guid)) As Task(Of ServiceActionResult(Of String))

        Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = observationRecord.ObservationId)

        If IsNothing(_observation) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Observation record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   _observation.ObservationStatusId = observationRecord.ObservationStatusId
                                                   _observation.Description = observationRecord.Description
                                                   _observation.RootCauseAnalysis = observationRecord.RootCauseAnalysis
                                                   _observation.Implication = observationRecord.Implication
                                                   _observation.MitigatingControl = observationRecord.MitigatingControl
                                                   _observation.Assumptions = observationRecord.Assumptions

                                                   If Not IsNothing(firstRecomm) Then
                                                       Dim _recomm = _context.Recommendations.SingleOrDefault(Function(x) x.RecommendationId = firstRecomm.Key)
                                                       If Not IsNothing(_recomm) Then
                                                           _recomm.Description = firstRecomm.Value
                                                           Update(_recomm)
                                                       End If
                                                   End If

                                                   Update(_observation)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation successfully updated!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_observation_status")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function



    Public Async Function Update_Observation_Management_Response_Async(ByVal observationId As Guid, managementResponse As String) As Task(Of ServiceActionResult(Of String))

        Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = observationId)

        If IsNothing(_observation) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Observation record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   _observation.ManagementResponse = managementResponse

                                                   Update(_observation)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation successfully updated!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_observation_managementstatus")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function



    Public Async Function Delete_Observation_Async(ByVal observationId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = observationId)

        If IsNothing(_observation) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Observation record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   Remove(_observation)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Observation successfully removed!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_observation")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

#End Region


#Region "Recommendation_Entity_Methods"

    Public Async Function Add_Update_Recommendation_Async(ByVal recommendation As RecommendationVm) As Task(Of ServiceActionResult(Of String))

        If Not recommendation.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid recommendation record"}
        End If

        Dim _recommendation As Recommendation = _context.Recommendations.SingleOrDefault(Function(x) x.RecommendationType = recommendation.RecommendationType And x.RecommendationId = recommendation.RecommendationId)

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If recommendation.isNew Then

                                                        _recommendation = New Recommendation With {
                                                           .RecommendationType = recommendation.RecommendationType,
                                                           .TypeReferenceId = recommendation.TypeReferenceId,
                                                           .Description = recommendation.Description
                                                        }

                                                        Call Add(_recommendation)

                                                    Else

                                                        _recommendation.Description = recommendation.Description

                                                        Call Update(_recommendation)

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Recommendation successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_recommendation")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Get_Recommendation_List_Async(ByVal recommType As RecommendationTypes, ByVal typeReferenceId As Guid) As Task(Of List(Of RecommendationVm))

        Return Await _context.Recommendations.Where(Function(x) x.RecommendationType = recommType And x.TypeReferenceId = typeReferenceId).Select(Of RecommendationVm)(Function(r) New RecommendationVm(recommType) With {
                                                                                                                                   .Description = r.Description,
                                                                                                                                   .RecommendationId = r.RecommendationId,
                                                                                                                                   .RecommendationType = r.RecommendationType,
                                                                                                                                   .TypeReferenceId = r.TypeReferenceId
                                                                                                                                   }).ToListAsync()

    End Function

    Public Async Function Delete_Recommendation_Async(ByVal recommendationId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _recommendation = _context.Recommendations.SingleOrDefault(Function(x) x.RecommendationId = recommendationId)

        If IsNothing(_recommendation) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Recommendation record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   Remove(_recommendation)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Recommendation successfully removed!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_recommendation")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

#End Region




#Region "FollowUp_Entity_Methods"

    Public Async Function Add_Update_FollowUp_Async(ByVal followUp As FollowUpVm) As Task(Of ServiceActionResult(Of String))

        If Not followUp.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid follow up record"}
        End If

        Dim _followUp = _context.FollowUps.SingleOrDefault(Function(x) x.FollowUpId = followUp.FollowUpId)

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If followUp.isNew Then

                                                        _followUp = New FollowUp With {
                                                          .Description = followUp.Description,
                                                          .FollowUpDate = followUp.FollowUpDate,
                                                          .IsSelfAuthorise = True,
                                                          .FollowUpDetails = followUp.FollowUpDetails.Select(Of FollowUpDetail)(Function(s) New FollowUpDetail With {
                                                            .ObservationStatusId = s.ObservationStatusId,
                                                            .ObservationId = s.ObservationId,
                                                            .Remarks = s.Remarks,
                                                            .IsSelfAuthorise = True
                                                            }).ToList()
                                                        }

                                                        Call Add(_followUp)

                                                    Else

                                                        _followUp.Description = followUp.Description
                                                        _followUp.FollowUpDate = followUp.FollowUpDate
                                                        _followUp.IsSelfAuthorise = True

                                                        Call Update(_followUp)

                                                        For Each fuDetail As FollowUpDetailVm In followUp.FollowUpDetails

                                                            Dim _fuDetail = _context.FollowUpDetails.SingleOrDefault(Function(x) x.FollowUpDetailId = fuDetail.FollowUpDetailId)

                                                            If IsNothing(_fuDetail) Then Continue For

                                                            _fuDetail.Remarks = fuDetail.Remarks
                                                            _fuDetail.ObservationStatusId = fuDetail.ObservationStatusId
                                                            _fuDetail.IsSelfAuthorise = True

                                                            Call Update(_fuDetail)

                                                        Next

                                                    End If


                                                    Dim _lastFolloUp = _followUp.FollowUpDetails.LastOrDefault()
                                                    Dim _observation = _context.Observations.SingleOrDefault(Function(o) o.ObservationId = _lastFolloUp.ObservationId)

                                                    _observation.ObservationStatusId = _lastFolloUp.ObservationStatusId
                                                    _observation.IsSelfAuthorise = True
                                                    Call Update(_observation)

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Recommendation successfully saved!"}

                                                End Function))


        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_recommendation")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Get_Observation_FollowUp_List_Async(ByVal observationId As Guid, Optional ByVal fullDatail As Boolean = False) As Task(Of List(Of FollowUpVm))

        If Not fullDatail Then

            Return Await _context.FollowUps.Include("FollowUpDetails").Where(Function(x) x.FollowUpDetails.Any(Function(a) a.ObservationId = observationId)).Select(Of FollowUpVm)(Function(f) New FollowUpVm With {
                                                                                                                                    .Description = f.Description,
                                                                                                                                    .FollowUpDate = f.FollowUpDate,
                                                                                                                                    .FollowUpId = f.FollowUpId
                                                                                                                                   }).ToListAsync()

        Else

            Return Await _context.FollowUps.Include("FollowUpDetails").Where(Function(x) x.FollowUpDetails.Any(Function(a) a.ObservationId = observationId)).Select(Of FollowUpVm)(Function(f) New FollowUpVm With {
                                                                                                                                    .Description = f.Description,
                                                                                                                                    .FollowUpDate = f.FollowUpDate,
                                                                                                                                    .FollowUpId = f.FollowUpId,
                                                                                                                                    .FollowUpDetails = f.FollowUpDetails.Select(Of FollowUpDetailVm)(Function(d) New FollowUpDetailVm With {.ObservationId = d.ObservationId, .FollowUpDetailId = d.FollowUpDetailId, .FollowUpId = d.FollowUpId, .Remarks = d.Remarks})
                                                                                                                                   }).ToListAsync()

        End If

    End Function

    Public Async Function Delete_FollowUp_Async(ByVal followUpId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _followUp = _context.FollowUps.SingleOrDefault(Function(x) x.FollowUpId = followUpId)

        If IsNothing(_followUp) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Follow up record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   Remove(_followUp)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Follow up successfully removed!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_followup")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Add_Update_FollowUpDetail_Async(ByVal followUpDetail As FollowUpDetailVm) As Task(Of ServiceActionResult(Of String))

        If Not followUpDetail.IsValid() Then
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Invalid follow up record"}
        End If

        Dim _followUpDetail = _context.FollowUpDetails.SingleOrDefault(Function(x) x.FollowUpId = followUpDetail.FollowUpId)

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    If followUpDetail.isNew Then

                                                        _followUpDetail = New FollowUpDetail With {
                                                          .Remarks = followUpDetail.Remarks,
                                                          .FollowUpId = followUpDetail.FollowUpId,
                                                          .ObservationId = followUpDetail.ObservationId
                                                        }

                                                        Call Add(_followUpDetail)

                                                    Else

                                                        _followUpDetail.Remarks = followUpDetail.Remarks

                                                        Call Update(_followUpDetail)

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Follow up detail successfully saved!"}

                                                End Function))



        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_followupdetail")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

    Public Async Function Delete_FollowUpDetail_Async(ByVal followUpDetailId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _followUpDetail = _context.FollowUpDetails.SingleOrDefault(Function(x) x.FollowUpId = followUpDetailId)

        If IsNothing(_followUpDetail) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Follow up detail record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew(Function()

                                                   Remove(_followUpDetail)
                                                   trans.Commit()

                                                   Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Follow up detail successfully removed!"}

                                               End Function)

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_followup")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function

#End Region


#Region "BRANCH REVIEW METHODS"



    Public Async Function Add_Update_BranchReview_Async(ByVal reviewAssignments As List(Of BranchReviewVm)) As Task(Of ServiceActionResult(Of String))

        For Each record As BranchReviewVm In reviewAssignments
            If Not record.IsValid() Then
                Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Validation Failed! One or more Invalid assignment record"}
            End If
        Next

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()


                                                    Dim _theReviewId = reviewAssignments.FirstOrDefault().ReviewId
                                                    Dim _allAssignedList = _context.BranchReviews.Where(Function(x) x.ReviewId.Equals(_theReviewId)).ToList()

                                                    Dim _deleteList = _allAssignedList.Where(Function(a) Not reviewAssignments.Any(Function(s) s.BranchReviewId = a.BranchReviewId)).ToList()

                                                    If _deleteList.Count > 0 Then
                                                        Call RemoveList(_deleteList)
                                                    End If


                                                    For Each _assignment In reviewAssignments

                                                        Dim _rvAssignment As BranchReview = _context.BranchReviews.SingleOrDefault(Function(x) x.BranchReviewId = _assignment.BranchReviewId)

                                                        Dim _isNew As Boolean = _rvAssignment Is Nothing

                                                        If _isNew Then

                                                            _rvAssignment = New BranchReview With
                                                            {
                                                            .BranchCode = _assignment.BranchCode,
                                                            .DateAssigned = DateTime.UtcNow,
                                                            .EmployeeId = _assignment.EmployeeId,
                                                            .EndDate = _assignment.ReviewTo,
                                                            .ReviewId = _assignment.ReviewId,
                                                            .StartDate = _assignment.ReviewFrom,
                                                            .ReviewInstruction = _assignment.ReviewInstruction
                                                            }

                                                            Call Add(_rvAssignment)

                                                        Else
                                                            _rvAssignment.EndDate = _assignment.ReviewTo
                                                            _rvAssignment.StartDate = _assignment.ReviewFrom
                                                            _rvAssignment.ReviewInstruction = _assignment.ReviewInstruction

                                                            Call Update(_rvAssignment)
                                                        End If

                                                    Next


                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review assignment successfully saved!"}

                                                End Function))


        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_review")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Get_Review_Assignments_List_Async(ByVal selectedReviewId As Guid) As Task(Of List(Of BranchReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.Select(Of BranchVm)(Function(f) New BranchVm With {.BranchCode = f.Code, .BranchId = f.BranchId, .BranchName = f.Name}).ToList()
                                               Dim _empList As List(Of EmployeeVm) = Get_EmployeeList().ToList()

                                               Dim _review = _context.BranchReviews.Include("Review").Where(Function(a) a.ReviewId = selectedReviewId).AsEnumerable() 'a.IsAuthorised = True And

                                               Dim _joined1 = (From r In _review
                                                               Join b In _branchList.ToList() On r.BranchCode Equals b.BranchCode
                                                               Select
                           b.BranchCode, b.BranchName, r.BranchReviewId, r.IsAuthorised, r.EmployeeId, r.IsSubmitted, r.StartDate, r.EndDate, r.Review.ReviewCode, r.Review.Description, r.ReviewInstruction, r.ReviewId).ToList()

                                               Return (From r In _joined1
                                                       Join e In _empList On r.EmployeeId Equals e.EmployeeId
                                                       Select New BranchReviewVm() With {
                            .BranchCode = r.BranchCode,
                            .BranchName = r.BranchName,
                            .BranchReviewId = r.BranchReviewId,
                            .EmployeeId = r.EmployeeId,
                            .EmployeeName = e.EmployeeName,
                            .IsSubmitted = r.IsSubmitted,
                            .IsAuthorised = r.IsAuthorised,
                            .ReviewCode = r.ReviewCode,
                            .ReviewFrom = r.StartDate,
                            .ReviewId = r.ReviewId,
                            .ReviewInstruction = r.ReviewInstruction,
                            .ReviewName = r.Description,
                            .ReviewTo = r.EndDate
                            }).ToList()

                                           End Function)






    End Function

    Public Async Function Get_All_Review_Submitted_Assignments_List_Async(Optional filterOptions As RecordFilterOption = Nothing) As Task(Of List(Of BranchReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.Select(Of BranchVm)(Function(f) New BranchVm With {.BranchCode = f.Code, .BranchId = f.BranchId, .BranchName = f.Name}).ToList()

                                               Dim _empList As List(Of EmployeeVm) = Get_EmployeeList().ToList()

                                               Dim _review = _context.BranchReviews.Include("Review").AsEnumerable()  '.Where(Function(f) f.IsSubmitted = True)   a.IsAuthorised = True And

                                               If Not String.IsNullOrWhiteSpace(filterOptions.branchCode) Then
                                                   _branchList = _branchList.Where(Function(f) f.BranchCode = filterOptions.branchCode).ToList()
                                                   _review = _review.Where(Function(f) f.BranchCode = filterOptions.branchCode)
                                               End If

                                               If filterOptions.employeeId > 0 Then
                                                   _empList = _empList.Where(Function(f) f.EmployeeId = filterOptions.employeeId)
                                                   _review = _review.Where(Function(f) f.EmployeeId = filterOptions.employeeId)
                                               End If

                                               If Not filterOptions.reviewId = New Guid Then
                                                   _review = _review.Where(Function(a) a.ReviewId = filterOptions.reviewId).AsEnumerable()
                                               End If

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewCode) Then
                                                   _review = _review.Where(Function(a) a.Review.ReviewCode = filterOptions.reviewCode).AsEnumerable()
                                               End If

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewName) Then
                                                   _review = _review.Where(Function(a) a.Review.Description.Contains(filterOptions.reviewName)).AsEnumerable()
                                               End If

                                               If Not filterOptions.fromDate = New DateTime Then
                                                   _review = _review.Where(Function(f) f.CreatedDate.Date >= filterOptions.fromDate.Date)
                                               End If

                                               If Not filterOptions.toDate = New DateTime Then
                                                   _review = _review.Where(Function(f) f.CreatedDate.Date <= filterOptions.fromDate.Date)
                                               End If

                                               Dim _joined1 = (From r In _review
                                                               Join b In _branchList.ToList() On r.BranchCode Equals b.BranchCode
                                                               Select
                           b.BranchCode, b.BranchName, r.BranchReviewId, r.IsAuthorised, r.EmployeeId, r.IsSubmitted, r.StartDate, r.EndDate, r.Review.ReviewCode, r.Review.Description, r.ReviewInstruction, r.ReviewId).ToList()

                                               Return (From r In _joined1
                                                       Join e In _empList On r.EmployeeId Equals e.EmployeeId
                                                       Select New BranchReviewVm() With {
                            .BranchCode = r.BranchCode,
                            .BranchName = r.BranchName,
                            .BranchReviewId = r.BranchReviewId,
                            .EmployeeId = r.EmployeeId,
                            .EmployeeName = e.EmployeeName,
                            .IsSubmitted = r.IsSubmitted,
                            .IsAuthorised = r.IsAuthorised,
                            .ReviewCode = r.ReviewCode,
                            .ReviewFrom = r.StartDate,
                            .ReviewId = r.ReviewId,
                            .ReviewInstruction = r.ReviewInstruction,
                            .ReviewName = r.Description,
                            .ReviewTo = r.EndDate
                            }).ToList()

                                           End Function)






    End Function







    Public Async Function Get_UnAuthorisd_Review_Assignments_List_Async() As Task(Of List(Of BranchReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.ToList()
                                               Dim _empList = Get_EmployeeList().ToList()

                                               Return _context.BranchReviews.Include("Review").Where(Function(a) a.IsAuthorised = False And a.CreatedByID <> _currentUserId).AsEnumerable().Select(Of BranchReviewVm)(Function(x) New BranchReviewVm With {
                                                   .BranchCode = x.BranchCode,
                                                   .BranchName = _branchList.FirstOrDefault(Function(b) b.Code = x.BranchCode).Name,
                                                    .BranchReviewId = x.BranchReviewId,
                                                     .EmployeeId = x.EmployeeId,
                                                     .ReviewFrom = x.StartDate,
                                                     .ReviewId = x.ReviewId,
                                                     .ReviewTo = x.EndDate,
                                                     .ReviewInstruction = x.ReviewInstruction,
                                                     .ReviewCode = x.Review.ReviewCode,
                                                     .ReviewName = x.Review.Description,
                                                     .EmployeeName = _empList.FirstOrDefault(Function(e) e.EmployeeId = x.EmployeeId).EmployeeName
                                                }).ToList()
                                           End Function)

    End Function

    Public Async Function Authorise_Review_Assignment_Async(ByVal branchReviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _review As BranchReview = Await _context.BranchReviews.SingleOrDefaultAsync(Function(x) x.BranchReviewId = branchReviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Assignment record not found!"}

        _review.IsSelfAuthorise = True

        Call Update(_review)

        Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review assignment successfully authorised!"}

    End Function


    Public Async Function Submit_Review_Async(ByVal branchReviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _valid = _context.Findings.Where(Function(f) f.BranchReviewId = branchReviewId).Count > 0

        If Not _valid Then
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = True, .Message = $"Review Submission Validation Failed! Please ADD at leaset ONE [1] Finding To Continue!"}
        End If

        Dim _review As BranchReview = Await _context.BranchReviews.SingleOrDefaultAsync(Function(x) x.BranchReviewId = branchReviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record not found!"}

        _review.IsSubmitted = True
        _review.IsSelfAuthorise = True

        Call Update(_review)

        Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review status successfully changed submitted!"}

    End Function

    Public Async Function UnSubmit_Review_Async(ByVal branchReviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _review As BranchReview = Await _context.BranchReviews.SingleOrDefaultAsync(Function(x) x.BranchReviewId = branchReviewId)

        If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record not found!"}

        _review.IsSubmitted = False
        _review.IsSelfAuthorise = True

        Call Update(_review)

        Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review status successfully changed to un-submitted!"}

    End Function



    Public Async Function Close_Review_Async(ByVal reviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Dim _review As Review = Await _context.Reviews.SingleOrDefaultAsync(Function(x) x.ReviewId = reviewId)

            If IsNothing(_review) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review record not found!"}

            _review.IsClosed = True
            _review.ClosedById = Me._currentUserId
            _review.ClosedDate = DateTime.UtcNow.Date
            _review.IsSelfAuthorise = True

            Call Update(_review)

            Dim _brandReviews = _context.BranchReviews.Where(Function(r) r.ReviewId = _review.ReviewId).ToList()

            For Each _bReview In _brandReviews

                _bReview.IsClosed = True
                _bReview.IsSelfAuthorise = True

                Call Update(_bReview)

            Next

            trans.Commit()

            Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review status successfully changed to CLOSED!"}

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_review")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function






    Public Async Function Get_BranchReview_ById_Async(ByVal branchReviewId As Guid) As Task(Of BranchReviewVm)




        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.ToList()
                                               Dim _empList = Get_EmployeeList().ToList()

                                               Return _context.BranchReviews.Include("Review").Where(Function(a) a.IsAuthorised = True And a.BranchReviewId = branchReviewId).AsEnumerable().Select(Of BranchReviewVm)(Function(x) New BranchReviewVm With {
                                                    .BranchCode = x.BranchCode,
                                                   .BranchName = _branchList.FirstOrDefault(Function(b) b.Code = x.BranchCode).Name,
                                                    .BranchReviewId = x.BranchReviewId,
                                                     .EmployeeId = x.EmployeeId,
                                                     .ReviewFrom = x.StartDate,
                                                     .ReviewId = x.ReviewId,
                                                     .ReviewTo = x.EndDate,
                                                     .ReviewInstruction = x.ReviewInstruction,
                                                     .ReviewCode = x.Review.ReviewCode,
                                                     .ReviewName = x.Review.Description,
                                                     .IsClosed = x.IsClosed,
                                                     .IsAuthorised = x.IsAuthorised,
                                                     .IsSubmitted = x.IsSubmitted,
                                                     .EmployeeName = _empList.FirstOrDefault(Function(e) e.EmployeeId = x.EmployeeId).EmployeeName
                                                }).SingleOrDefault()
                                           End Function)

    End Function




    Public Async Function Get_Employee_BranchReviews_Async(ByVal filterOptions As RecordFilterOption) As Task(Of List(Of BranchReviewVm))


        'TO BE UPDATED AFTER FULL DEVELOPMENT   '   



        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.ToList()
                                               Dim _empList = Get_EmployeeList().ToList()

                                               Dim _query = _context.BranchReviews.Include("Review").Where(Function(a) a.IsAuthorised = True And a.EmployeeId = _currentUserId And a.BranchCode = filterOptions.branchCode)

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewCode) Then
                                                   _query = _query.Where(Function(f) f.Review.ReviewCode = filterOptions.reviewCode)
                                               End If

                                               If Not String.IsNullOrWhiteSpace(filterOptions.reviewName) Then
                                                   _query = _query.Where(Function(f) f.Review.Description = filterOptions.reviewName)
                                               End If

                                               If filterOptions.employeeId > 0 Then
                                                   _query = _query.Where(Function(f) f.EmployeeId = filterOptions.employeeId)
                                               End If

                                               If Not filterOptions.fromDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date >= filterOptions.fromDate.Date)
                                               End If

                                               If Not filterOptions.toDate = New DateTime Then
                                                   _query = _query.Where(Function(f) f.CreatedDate.Date <= filterOptions.fromDate.Date)
                                               End If

                                               Return _query.AsEnumerable().Select(Of BranchReviewVm)(Function(x) New BranchReviewVm With {
                                                    .BranchCode = x.BranchCode,
                                                   .BranchName = _branchList.FirstOrDefault(Function(b) b.Code = x.BranchCode).Name,
                                                    .BranchReviewId = x.BranchReviewId,
                                                     .EmployeeId = x.EmployeeId,
                                                     .ReviewFrom = x.StartDate,
                                                     .ReviewId = x.ReviewId,
                                                     .ReviewTo = x.EndDate,
                                                     .ReviewInstruction = x.ReviewInstruction,
                                                     .ReviewCode = x.Review.ReviewCode,
                                                     .ReviewName = x.Review.Description,
                                                     .IsSubmitted = x.IsSubmitted,
                                                     .IsClosed = x.IsClosed,
                                                     .IsAuthorised = x.IsAuthorised,
                                                     .EmployeeName = _empList.FirstOrDefault(Function(e) e.EmployeeId = x.EmployeeId).EmployeeName
                                                }).OrderByDescending(Of Date)(Function(d) d.ReviewFrom).ToList()
                                           End Function)

    End Function



    Public Async Function Get_BranchReviews_Async(Optional filterOptions As RecordFilterOption = Nothing) As Task(Of List(Of BranchReviewVm))

        Return Await Task.Factory.StartNew(Function()

                                               Dim _branchList = New BranchContext().Branches.ToList()
                                               Dim _empList = Get_EmployeeList().ToList()

                                               Return _context.BranchReviews.Include("Review").Where(Function(a) a.IsAuthorised = True And a.IsSubmitted = True And a.BranchCode = filterOptions.branchCode).AsEnumerable().Select(Of BranchReviewVm)(Function(x) New BranchReviewVm With {
                                                    .BranchCode = x.BranchCode,
                                                   .BranchName = _branchList.FirstOrDefault(Function(b) b.Code = x.BranchCode).Name,
                                                    .BranchReviewId = x.BranchReviewId,
                                                     .EmployeeId = x.EmployeeId,
                                                     .ReviewFrom = x.StartDate,
                                                     .ReviewId = x.ReviewId,
                                                     .ReviewTo = x.EndDate,
                                                     .ReviewInstruction = x.ReviewInstruction,
                                                     .ReviewCode = x.Review.ReviewCode,
                                                     .ReviewName = x.Review.Description,
                                                     .EmployeeName = _empList.FirstOrDefault(Function(e) e.EmployeeId = x.EmployeeId).EmployeeName
                                                }).ToList()
                                           End Function)

    End Function



    Public Async Function Delete_Review_Assignment_Async(ByVal branchReviewId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _reviewAssignment = _context.BranchReviews.SingleOrDefault(Function(x) x.BranchReviewId = branchReviewId)

        If IsNothing(_reviewAssignment) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Assignment record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_reviewAssignment)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Review assignment successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_branchreview")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function






#End Region




#Region "ACTION_PLAN"


    Public Async Function Add_Update_ActionPlan_Async(ByVal actionPlan As ActionPlanVm) As Task(Of ServiceActionResult(Of ActionPlanVm))


        If Not actionPlan.IsValid() Then
            Return New ServiceActionResult(Of ActionPlanVm) With {.Entity = Nothing, .Status = False, .Message = $"Validation Failed! One or more Invalid action plan record"}
        End If


        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Return Await Task.Factory.StartNew((Function()

                                                    Dim _observation = _context.Observations.SingleOrDefault(Function(x) x.ObservationId = actionPlan.ObservationId)

                                                    _observation.ReferenceOfficerIds = actionPlan.ReferenceOfficerIds
                                                    _observation.ResolutionTiming = actionPlan.ResolutionTiming

                                                    Call Update(_observation)

                                                    Dim _actionPlan As ActionPlan = _context.ActionPlans.SingleOrDefault(Function(x) x.ActionPlanId = actionPlan.ActionPlanId)

                                                    Dim _isNew As Boolean = _actionPlan Is Nothing

                                                    If _isNew Then

                                                        _actionPlan = New ActionPlan With
                                                            {
                                                                .ActionPlanId = actionPlan.ActionPlanId,
                                                                .Description = actionPlan.Description,
                                                                .ObservationId = actionPlan.ObservationId,
                                                                .ScheduleOfficerResponse = actionPlan.ScheduleOfficerResponse,
                                                                .IsSelfAuthorise = True
                                                            }

                                                        Call Add(_actionPlan)

                                                    Else

                                                        _actionPlan.Description = actionPlan.Description
                                                        _actionPlan.ScheduleOfficerResponse = actionPlan.ScheduleOfficerResponse
                                                        _actionPlan.IsSelfAuthorise = True

                                                        Call Update(_actionPlan)
                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of ActionPlanVm) With {.Entity = New ActionPlanVm() With {
                                                    .ActionPlanId = _actionPlan.ActionPlanId,
                                                    .Description = _actionPlan.Description,
                                                    .ObservationId = _actionPlan.ObservationId,
                                                    .ObservationStatusId = _observation.ObservationStatusId,
                                                    .ReferenceOfficerIds = _observation.ReferenceOfficerIds,
                                                    .ResolutionTiming = _observation.ResolutionTiming
                                                    }, .Status = True, .Message = $"Action plan successfully saved!"}

                                                End Function))


        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_actionplan")
            ex.Data.Clear()
            Return New ServiceActionResult(Of ActionPlanVm) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Get_Observation_ActionPlan_ById_Async(ByVal observationId As Guid) As Task(Of ActionPlanVm)

        Return Await Task.Factory.StartNew(Function()

                                               Dim _empList = Get_EmployeeList().ToList()

                                               Dim _actionPlan = _context.ActionPlans.Include("Observation").Where(Function(a) a.IsAuthorised = True And a.ObservationId = observationId).Select(Of ActionPlanVm)(Function(s) New ActionPlanVm() With {.ActionPlanId = s.ActionPlanId,
                                                    .Description = s.Description,
                                                    .ObservationId = s.ObservationId,
                                                    .ObservationStatusId = s.Observation.ObservationStatusId,
                                                    .ReferenceOfficerIds = s.Observation.ReferenceOfficerIds,
                                                    .ResolutionTiming = s.Observation.ResolutionTiming}).SingleOrDefault()

                                               If Not IsNothing(_actionPlan) Then
                                                   Dim t = _actionPlan.ReferenceOfficerIds.Split(",").ToList().Select(Of Int32)(Function(a) CInt(a)).ToList()
                                                   _actionPlan.AssignedEmployeeNames = String.Join(", ", _empList.Where(Function(f) t.Any(Function(a) f.EmployeeId = a)).Select(Of String)(Function(s) s.EmployeeName).ToList())
                                               Else
                                                   _actionPlan = New ActionPlanVm()
                                                   _actionPlan.ObservationId = observationId
                                               End If

                                               Return _actionPlan

                                           End Function)

    End Function





    Public Async Function Delete_ActionPlan_Async(ByVal actionPlanId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _actionPlan = _context.ActionPlans.SingleOrDefault(Function(x) x.ActionPlanId = actionPlanId)

        If IsNothing(_actionPlan) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Action plan record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_actionPlan)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Action plan successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_actionplan")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function



    Public Async Function Add_Update_Corrective_Action_Async(ByVal correctiveAction As CorrectiveActionVm) As Task(Of ServiceActionResult(Of CorrectiveActionVm))

        If Not correctiveAction.IsValid() Then
            Return New ServiceActionResult(Of CorrectiveActionVm) With {.Entity = Nothing, .Status = False, .Message = $"Invalid observation record"}
        End If

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)

        Try

            Dim _correctAction = _context.CorrectiveActions.SingleOrDefault(Function(x) x.CorrectiveActionId = correctiveAction.CorrectiveActionId)

            Dim _isNew = IsNothing(_correctAction)

            Return Await Task.Factory.StartNew((Function()

                                                    If _isNew Then

                                                        _correctAction = New CorrectiveAction With {
                                                            .ObservationId = correctiveAction.ObservationId,
                                                            .Remarks = correctiveAction.Remarks,
                                                            .CorrectionDate = correctiveAction.CorrectionDate,
                                                            .ObservationStatusId = correctiveAction.ObservationStatusId
                                                        }

                                                        Call Add(_correctAction)

                                                    Else

                                                        _correctAction.Remarks = correctiveAction.Remarks
                                                        _correctAction.CorrectionDate = correctiveAction.CorrectionDate
                                                        _correctAction.ObservationStatusId = correctiveAction.ObservationStatusId

                                                        Call Update(_correctAction)

                                                    End If

                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of CorrectiveActionVm) With {.Entity = New CorrectiveActionVm() With {
                                                    .CorrectionDate = _correctAction.CorrectionDate,
                                                    .CorrectiveActionId = _correctAction.CorrectiveActionId,
                                                    .ObservationId = _correctAction.CorrectiveActionId,
                                                    .ObservationStatusId = _correctAction.ObservationStatusId,
                                                    .Remarks = _correctAction.Remarks
                                                    }, .Status = True, .Message = $"Action Taken successfully saved!"}

                                                End Function))


        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_addedit_correctiveaction")
            ex.Data.Clear()
            Return New ServiceActionResult(Of CorrectiveActionVm) With {.Entity = Nothing, .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Async Function Delete_Corrective_Action_Async(ByVal correctiveActionId As Guid) As Task(Of ServiceActionResult(Of String))

        Dim _correctAction = _context.CorrectiveActions.SingleOrDefault(Function(x) x.CorrectiveActionId = correctiveActionId)

        If IsNothing(_correctAction) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Action taken record not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try
            Return Await Task.Factory.StartNew((Function()

                                                    Remove(_correctAction)
                                                    trans.Commit()

                                                    Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"Action take successfully removed!"}

                                                End Function))

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_correctiveaction")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Function Save_File_Upload_Info(ByVal uploadFileInfo As ReviewFileVm) As ServiceActionResult(Of String)

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try

            Dim _uploadFileInfo = _context.ReviewFiles.SingleOrDefault(Function(x) x.ReviewFileId = uploadFileInfo.ReviewFileId)

            If IsNothing(_uploadFileInfo) Then

                _uploadFileInfo = New ReviewFile() With {
                    .FileExtension = uploadFileInfo.FileExtension,
                    .Description = uploadFileInfo.Description,
                    .FileName = uploadFileInfo.FileName,
                    .ReferenceTypeId = uploadFileInfo.ReferenceTypeId,
                    .ReviewFileType = uploadFileInfo.ReviewFileType,
                    .IsSelfAuthorise = True
                }
                Add(_uploadFileInfo)
            Else

                _uploadFileInfo.FileExtension = uploadFileInfo.FileExtension
                _uploadFileInfo.Description = uploadFileInfo.Description
                _uploadFileInfo.FileName = uploadFileInfo.FileName
                _uploadFileInfo.ReferenceTypeId = uploadFileInfo.ReferenceTypeId
                _uploadFileInfo.ReviewFileType = uploadFileInfo.ReviewFileType
                _uploadFileInfo.IsSelfAuthorise = True

                Update(_uploadFileInfo)

            End If

            trans.Commit()

            Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"File upload successfully completed!"}

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_fileupload")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function



    Public Function Delete_ReviewFile_Async(ByVal reviewFileId As Guid) As ServiceActionResult(Of String)

        Dim _reviewFile = _context.ReviewFiles.SingleOrDefault(Function(x) x.ReviewFileId = reviewFileId)

        If IsNothing(_reviewFile) Then Return New ServiceActionResult(Of String) With {.Entity = Nothing, .Status = False, .Message = $"Review file not found!"}

        Dim trans = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted)
        Try

            Remove(_reviewFile)
            trans.Commit()
            Return New ServiceActionResult(Of String) With {.Entity = "Success", .Status = True, .Message = $"File successfully removed!"}

        Catch ex As Exception
            trans.Rollback()
            Call Write_Exception_Log(ex, "errlog_delete_reviewfile")
            ex.Data.Clear()
            Return New ServiceActionResult(Of String) With {.Entity = "Failed", .Status = False, .Message = $"An error occured while processing request! Please contact your IT Department! Error : {ex.Message}"}
        End Try

    End Function


    Public Function Get_Uploaded_Files_By_ReferenceTypeId(ByVal referenceTypeId As Guid) As List(Of ReviewFileVm)

        Return _context.ReviewFiles.Where(Function(x) x.ReferenceTypeId = referenceTypeId).Select(Of ReviewFileVm)(Function(s) New ReviewFileVm() With {
            .Description = s.Description,
            .ReferenceTypeId = s.ReferenceTypeId,
            .FileExtension = s.FileExtension,
            .FileName = s.FileName,
            .ReviewFileId = s.ReviewFileId,
            .ReviewFileType = s.ReviewFileType
        }).ToList()

    End Function

    Public Function Get_Uploaded_File_ByName(ByVal referenceTypeId As Guid, ByVal fileName As String) As ReviewFileVm

        Return _context.ReviewFiles.Where(Function(x) x.ReferenceTypeId = referenceTypeId).Where(Function(n) n.FileName.Equals(fileName)).Select(Of ReviewFileVm)(Function(s) New ReviewFileVm() With {
            .Description = s.Description,
            .ReferenceTypeId = s.ReferenceTypeId,
            .FileExtension = s.FileExtension,
            .FileName = s.FileName,
            .ReviewFileId = s.ReviewFileId,
            .ReviewFileType = s.ReviewFileType
        }).SingleOrDefault()

    End Function

    Public Function Get_Uploaded_File_By_ReviewFileId(ByVal reviewFileId As Guid) As ReviewFileVm

        Return _context.ReviewFiles.Where(Function(x) x.ReviewFileId = reviewFileId).Select(Of ReviewFileVm)(Function(s) New ReviewFileVm() With {
            .Description = s.Description,
            .ReferenceTypeId = s.ReferenceTypeId,
            .FileExtension = s.FileExtension,
            .FileName = s.FileName,
            .ReviewFileId = s.ReviewFileId,
            .ReviewFileType = s.ReviewFileType
        }).SingleOrDefault()

    End Function

#End Region






End Class
