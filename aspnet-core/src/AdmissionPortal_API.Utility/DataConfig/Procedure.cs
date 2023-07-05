namespace AdmissionPortal_API.Utility.DataConfig
{
    public static class Procedure
    {
        #region Employee Management
        public const string SaveEmployee = "Employee_Save";
        public const string UpdateEmployee = "Employee_Update";
        public const string DeleteEmployee = "Employee_Delete";
        public const string GetAllEmployee = "GetAllEmployee";
        public const string GetEmployeeById = "Employee_GetById";
        public const string GetEmployeeByEmail = "GetEmployeeByEmail";
        public const string ForgotPassword = "App_Login_ForgotPassword_SetOTP";
        public const string ResetPassword = "ResetPassword";
        public const string EmployeeLogin = "App_Login_Employee";
        public const string EmployementType = "EmployementType_GetAll";
        #endregion
        #region University
        public const string SaveUniversity = "University_Save";
        public const string GetUniversityById = "University_GetByID";
        public const string DeleteUniversityById = "University_DeleteByID";
        public const string UniversityUpdateByID = "University_UpdateByID";
        public const string GetAllUniversity = "University_GetAll";
        public const string GetAllUniversityPG = "University_PG_GetAll";
        #endregion
        #region Citizen Management
        public const string SaveCitizen = "SaveCitizen";
        public const string UpdateCitizen = "UpdateCitizen";
        public const string DeleteCitizen = "DeleteCitizen";
        public const string GetAllCitizen = "GetAllCitizen";
        public const string GetCitizenById = "GetCitizenById";
        public const string CitizenChangePassword = "Citizen_ChangePassword";
        public const string CitizenLogin = "Citizen_Login";
        #endregion
        #region Role Management
        public const string SaveRole = "App_Role_Save";
        public const string UpdateRole = "App_Role_Update";
        public const string DeleteRole = "App_Role_Delete";
        public const string GetAllRole = "App_Role_GetAll";
        public const string GetRoleById = "App_Role_GetByID";
        #endregion
        #region State Management
        public const string GetAllStates = "MD_States_GetAll";
        #endregion
        #region Geo Management
        public const string GetDistrictsByStateId = "MD_Districts_GetByStateId";
        public const string GetAllBlockByDistrict = "Md_Block_By_District";
        public const string GetAllTehsilByDistrict = "Md_Tehsil_By_District";
        public const string GetAllVillageByBlock = "Md_Village_By_Block";
        #endregion
        #region WorkFlow Process
        public const string SaveWorkflowProcess = "Md_Workflow_Process_INSERT_UPDATE";
        public const string UpdateWorkflowProcess = "Md_Workflow_Process_INSERT_UPDATE";
        public const string DeleteWorkflowProcess = "Md_Workflow_Process_DELETE";
        public const string GetAllWorkflowProcess = "Md_Workflow_Process_Select";
        public const string GetWorkflowProcessById = "Md_WorkFlow_Process_SelectById";
        #endregion
        #region WorkFlow Stage
        public const string SaveWorkflowStage = "Md_Workflow_Stage_INSERT";
        public const string UpdateWorkflowStage = "Md_Workflow_Stage_UPDATE";
        public const string DeleteWorkflowStage = "Md_Workflow_Stage_Delete";
        public const string GetAllWorkflowStage = "Md_Workflow_Stage_SELECT";
        public const string GetWorkflowStageById = "Md_Workflow_Stage_SELECT";
        #endregion
        #region WorkFlow Stage Action
        public const string SaveWorkflowStageAction = "Md_Workflow_Stage_Actions_Insert_Update";
        public const string UpdateWorkflowStageAction = "Md_Workflow_Stage_Actions_Insert_Update";
        public const string DeleteWorkflowStageAction = "Md_Workflow_Stage_Actions_Delete";
        public const string GetAllWorkflowStageAction = "Md_Workflow_Stage_Actions_Select";
        public const string GetWorkflowStageActionById = "Md_Workflow_Stage_Actions_Select";
        #endregion
        #region Department Management
        public const string GetAllDepartment = "MD_Department_GetAll";

        #endregion
        #region Admin Login
        public const string AdminLogin = "App_Login_Admin";
        public const string ChangePassword = "App_Login_ChangePassword";
        public const string ResetEmployeePassword = "App_Login_ResetPassword_Employee";
        public const string ForgotEmployeePassword = "App_Login_ForgotPassword_Employee";
        public const string CancelStudentRegistrationByRegId = "App_CancelStudentRegistrationByRegId";
        public const string ViewStudentObjections = "App_ViewStudentObjections";
        public const string UploadNotification = "Upload_Department_Notification";
        public const string GetDepartmentNotifications = "Get_Department_Notifications";

        public const string GetNotificationDetail = "GetNotificationDetail";

        #endregion
        #region Navigation
        public const string SaveNavigation = "App_Navigation_Save";
        public const string UpdateNavigation = "App_Navigation_Update";
        public const string DeleteNavigation = "App_Navigation_Delete";
        public const string GetAllNavigations = "App_Navigation_GetAll";
        public const string GetNavigationById = "App_Navigation_GetById";
        public const string GetNavigation = "App_Navigation_Get";

        #endregion
        #region RoleNavigation Management
        public const string SaveRoleNavigation = "App_Role_RoleNavigation_Mapping_Save";
        public const string UpdateRoleNavigation = "App_Role_RoleNavigation_Maping_Update";
        public const string DeleteRoleNavigation = "App_Role_RoleNavigation_Mapping_Delete";
        public const string GetAllRoleNavigation = "App_Role_GetAllRoleNavigation";
        public const string GetRoleNavigationById = "App_Role_Navigation_ByMappingId";
        #endregion
        #region Employee Role Mappipng Management
        public const string SaveEmployeeRoleMapping = "App_Role_EmployeeRole_Mapping_Save";
        public const string UpdateEmployeeRoleMapping = "App_Role_EmployeeRole_Mapping_Update";

        public const string GetEmployeeRoleMappingById = "App_Role_GetRoleEmployee_ByMappingId";
        public const string GetAllEmployeeRoleMapping = "Employee_GetAll_RoleMapping";
        #endregion
        #region Profile
        public const string GetProfile = "App_Profile_GetByID";
        public const string UpdateProfile = "App_Profile_Update";
        public const string UploadProfileImage = "App_Profile_UploadImage";
        public const string UploadProfileImageById = "App_Profile_UploadImageById";

        #endregion
        #region LogError
        public const string SaveLog = "App_LogApp_SaveError";
        public const string GetAllLog = "App_LogApp_GetAll";

        #endregion
        #region Stackholder
        public const string GetStackholder = "MD_StakeHolder_GetAll";
        #endregion
        #region LookUPAPI
        public const string GetAllCollegeType = "MD_CollegeType_GetAll";
        public const string GetAllCollegeMode = "MD_CollegeMode_GetAll";
        public const string GetallLookupTypeByType = "App_Get_LookupType";
        public const string GetReservationCategorys = "Get_Reservation_Categorys";
        public const string GetReservationCategorysPG = "Get_PG_Reservation_Categorys";
        public const string GetReservationCategorysOfflineAdmission = "Get_Reservation_Categorys_OfflineAdmission";
        #endregion
        #region College
        public const string SaveCollege = "College_Save";
        public const string Updatecollege = "College_update";
        public const string CollegeupdateBycollegeId = "College_updateBycollegeId";
        public const string DeletecollegebyId = "College_DeleteByID";
        public const string CollegeGetByID = "College_GetByID";
        public const string GetallCollege = "College_GetAllByUniversityID";
        public const string GetCollegeByDistrictId = "College_GetCollegeByDistrictId";
        public const string GetDistrictCollegesByGender = "College_GetDistrictCollegeListByGender";
        public const string GetAllColleges = "College_GetAll";
        public const string GetCollegeCourses = "College_GetCollegeCourses";
        public const string ResetPasswordByAdmin = "CollegeResetPassword";
        public const string CollegeActiveInactive = "College_ActiveDeActiveByID";
        public const string CollegeLockUnlock = "College_LockUnlockByID";
        public const string CollegeGenreateOTP = "College_GenreateOTP";
        public const string UploadCollegeProspectus = "College_UploadProspectus";
        public const string GetDistrictCollege = "College_Get_By_District_And_CollegeCourseType";
        public const string GetCollegeByCGtype = "College_GetCollegeByCGtype";
        public const string AppLoginReportProc = "App_Login_Report";
        public const string UploadCancelledCheque = "College_UploadCancelledCheque";
        public const string UnLockStudent = "Student_UnlockANDRevokeVerification_By_College";

        public const string DownloadStudentDocument= "Download_StudentDocument";
        public const string DownloadcollegeProspectus = "College_Get_Prospectus_ByID";
        public const string GetAllCollegeIslock = "College_GetAll_ISLOCK";





        #endregion
        #region College Course Mapping
        public const string SaveCollegeCourseMapping = "College_Course_Mapping_Save";
        public const string GetCoursesByCollegeId = "Courses_GetAllByCollegeId";
        public const string GetMappedCoursesByCollegeId = "Courses_GetMappedByCollegeId";
        public const string GetCombinationsCoursesByCollegeId = "Courses_GetCombinationCourseByCollegeId";
        public const string LockUnlockCoursesByCollegeID = "Courses_LockUnlockCoursesByCollegeID";
        #endregion
        #region PG College Course Mapping
        public const string PGSaveCollegeCourseMapping = "PG_College_Course_Mapping_Save";
        public const string PGGetCoursesByCollegeId = "PG_Courses_GetAllByCollegeId";
        public const string PGGetMappedCoursesByCollegeId = "PG_Courses_GetMappedByCollegeId";
        public const string PGGetCombinationsCoursesByCollegeId = "PG_Courses_GetCombinationCourseByCollegeId";
        public const string PGLockUnlockCoursesByCollegeID = "PG_Courses_LockUnlockCoursesByCollegeID";
        public const string GetPgMappedCourseByCollege = "Get_PG_CollegeMappedCourse";
        #endregion
        #region University Course Mapping
        public const string SaveUniversityCourseMapping = "University_Course_Mapping_Save";
        public const string GetCoursesByUniversityId = "Courses_GetAllByUniversityId";
        public const string GetMappedCoursesByUniversityId = "Courses_GetMappedByUniversityId";
        public const string GetCombinationsCoursesByUniversityId = "Courses_GetCombinationCoursesByUniversityId";
        #endregion
        #region PG University Course Mapping
        public const string PGSaveUniversityCourseMapping = "PG_University_Course_Mapping_Save";
        public const string PGGetCoursesByUniversityId = "PG_Courses_GetAllByUniversityId";
        public const string PGGetMappedCoursesByUniversityId = "PG_Courses_GetMappedByUniversityId";
        #endregion
        #region College Course Subject Mapping
        public const string SaveCollegeCourseSubjectMapping = "College_Course_Subject_Mapping_Save";
        public const string GetSubjectsByCollegeId = "Subjects_GetByCollegeId";
        public const string GetCombinationByCollegeId = "Combinations_GetByCollegeId";
        public const string GetCombinationByCollegeCourse = "Combinations_GetByCollegeCourse";
        public const string DeleteCombinationById = "Combination_Delete";
        public const string UpdateCombination = "Combination_Update";
        public const string LockUnlockCollegeCourseSubjectMappingByCollegeId = "LockUnlockCollegeCourse_SubjectMappingByCollegeId";
        #endregion
        #region Course Management
        public const string SaveCourse = "App_Course_Save";
        public const string UpdateCourse = "App_Course_Update";
        public const string DeleteCoursebyId = "App_Course_DeleteByID";
        public const string CourseGetByID = "App_Course_GetById";
        public const string GetallCourse = "App_All_Course";
        public const string GetallCourseType = "App_Get_All_CourseType";
        public const string GetallUgCourse = "App_All_UG_Course";

        #endregion
        #region PG Course Management
        public const string SavePgCourse = "App_PG_Course_Save";
        public const string UpdatePgCourse = "App_PG_Course_Update";
        public const string DeletePgCoursebyId = "App_PG_Course_DeleteByID";
        public const string CoursePgGetByID = "App_PG_Course_GetById";
        public const string GetallPgCourse = "App_All_PG_Course";
        #endregion
        #region Course Subject with course
        public const string SaveBoardSubject = "MD_Board_Subject_Save";
        public const string SaveSubject = "MD_Course_Subject_Save";
        public const string UpdateSubject = "MD_Course_Subject_Update";
        public const string DeleteSubjectbyId = "MD_Course_Subject_delete";
        public const string SubjectGetByID = "MD_Course_Subject_GetById";
        public const string GetallSubject = "MD_Course_Subject_GetAll";
        public const string GetallSubjectbyCourseId = "App_College_Course_Get_Subject_ById";
        public const string GetSubjectbyCourseId = "App_Get_Subject_ByCourseId";
        public const string GetSubjectbyCourseAndUniversityId = "App_Get_Subject_ByCourse_UniversityId";
        public const string GetCourseSubjectCombinationCheckByUniversity = "App_Get_Course_Subject_Combination_Check_By_University";
        public const string SaveCourseSubjectCombinationCheckByUniversity = "App_Save_Course_Subject_Combination_Check_By_University";

        #endregion
        #region Section
        public const string SaveSection = "Section_Save";
        public const string UpdateSection = "Section_Update";
        public const string DeleteSectionbyId = "Section_DeleteByID";
        public const string GetSectionByID = "Section_GetByID";
        public const string GetAllSection = "Section_GetAll";
        #endregion
        #region CollegeCourse Seat
        public const string SaveCollegeCourseSeat = "App_College_Course_Seat_Save";
        public const string UpdateCollegeCourseSeat = "App_College_Course_Seat_update";
        public const string DeleteCollegeCourseSeatbyId = "App_College_Course_Seat_Delete";
        public const string GetCollegeCourseSeatByID = "App_College_Course_Seat_GetById";
        public const string GetAllCollegeCourseSeat = "App_All_CollegeCourseSeat";
        public const string GetAllCollegeCourseSeatMatrix = "SeatMatrix_GetByCollegeId";
        public const string GetPGCollegeCourseSeatMatrix = "PG_SeatMatrix_GetByCollegeId";
        public const string ReservationQuota = "App_ReservationPercentage";
        public const string LockUnlockCourseSeatsByCollegeID = "App_College_CourseSeat_LockUnlockByCollegeID";
        public const string LockUnlockCourseSeatMatrixByCollegeID = "College_SeatMatrixLockUnlockByID";
        public const string PGLockUnlockCourseSeatMatrixByCollegeID = "College_PG_SeatMatrixLockUnlockByID";
        #endregion
        #region Student
        public const string SaveStudent = "Student_Save";
        public const string StudentLogin = "Student_Login";
        public const string UpdateStudentPersonalDetails = "Student_UpdatePersonalDetails";
        public const string UpdateBankDetails = "Student_UpdateBankDetails";
        public const string UpdateAddressDetails = "Student_UpdateAddressDetails";
        public const string UpdateAcademicDetails = "Student_UpdateAcademicDetails";
        public const string UpdateWeightage = "Student_UpdateWeightage";
        public const string UploadDocuments = "Student_UploadDocuments";
        public const string GetStudentById = "Student_GetStudentById";
        public const string GetSubjectByStudentId = "Student_GetSubjectByStudentId";
        public const string GetProgressBar = "Student_GetProgressBar";
        public const string UpdateDeclarations = "Student_UpdateDeclarations";
        public const string GenerateOTP = "Student_GenerateOTP";
        public const string UpdateCourseChoice = "Student_UpdateCourseChoice";
        public const string GetCourseChoiceFee = "Student_UG_GetCourseChoiseFee";
        public const string UpdateCourseChoiceWithSubject = "Student_UG_UpdateCourseChoiceWithSubject_WithGroup";
        public const string GetDocumentsByStudentId = "Student_GetDocumentsByStudentId";
        public const string GetCourseChoiceByStudentId = "Student_GetCourseChoiceByStudentId";
        public const string SaveRegisterationFees = "Student_RegisterationFees";
        public const string UnlockForm = "Student_UnlockForm";
        public const string UpdateStudentDetails = "Student_UpdateStudentDetails";
        public const string StudentForgotPassword = "App_Login_ForgotPassword_Student";
        public const string StudentDetails = "Student_GetDetailsByRegId";
        public const string StudentAppDetails = "Student_GetAppDetailsByRegId";
        public const string GetCourseChoiceByRegId = "Student_GetCourseChoiceByRegId";
        public const string GetSubjectByRegId = "Student_GetSubjectByRegId";
        public const string GetDocumentsByRegId = "Student_GetDocumentsByRegId";
        public const string SaveValidatedDocumentSrNo = "Student_SaveValidatedDocumentSrNo";
        public const string SaveValidatedDocumentSrNoPG = "Student_PG_SaveValidatedDocumentSrNo";
        public const string StudenAlreadyRegisted = "Student_ug_StudentAlreadyRegisted";
        public const string GetEligibleCourse = "student_GetCollegeCoursesByStudentId";
        public const string CheckCollegeCourseSubject = "Student_GetCollegeCourseSubjectCount";
        public const string GetObjectListByStudentId = "Student_GetObjectionsByStudentID";
        public const string GetAdmissionSeatDetailsByStudentId = "Student_GetAdmissionSeatDetailsByStudentId";
        public const string GetStudentReceiptByStudentId = "usp_UG_GetStudentReceiptByStudentId";
        public const string GetStudentMarksForRechecking = "get_studentForMarksReChecking";
        public const string SaveMarksForRechecking = "Save_ReCheckingNumber";
        public const string GetStudentReceiptListByStudentId = "usp_UG_GetStudentReceiptListByStudentId";
        
        #endregion
        #region MasterApi
        public const string GetAllBoards = "MD_Board_GetAll";
        public const string GetAllReligion = "MD_Religion_GetAll";
        public const string GetReservationCategory = "MD_Reservation_GetAll";
        public const string GetReservationSubCategory = "MD_Reservation_GetAllSubCategory";
        public const string GetAllOccupation = "App_Occupation_Get";
        public const string GetFatherOccupation = "App_Father_Occupation_Get";
        public const string GetAllHouseholdIncome = "App_HouseholdIncome_Get";
        public const string GetBoardSubject = "App_Get_Board_Subject";
        public const string GetAllBank = "GetAllBank";
        public const string IsLockDetails = "Get_LockDetails";
        public const string GetAllCountries = "MD_Countries_GetAll";
        #endregion
        #region FeeHead
        public const string savefeehead = "App_FeeHead_Save";
        public const string updatefeehead = "App_FeeHead_Update";
        public const string GetfeeheadbyId = "App_FeeHead_GetById";
        public const string DeletefeeheadbyId = "App_FeeHead_DeleteByID";
        public const string GetAllfeehead = "App_All_FeeHead";
        #endregion
        #region PG FeeHead

        public const string SavePGFeeHead = "App_PG_FeeHead_Save";
        public const string UpdatePGFeeHead = "App_PG_FeeHead_Update";
        public const string GetPGFeeHeadById = "App_PG_FeeHead_GetById";
        public const string DeletePGFeeHeadById = "App_PG_FeeHead_DeleteByID";
        public const string GetAllPGFeeHead = "App_All_PG_FeeHead";
        #endregion
        #region CourseFeeHead
        public const string courseFeeHead = "Course_FeeHead_Save";
        public const string courseFeeDetails = "App_All_CourseFeeDetails";
        public const string FeeHeadByLoginType = "App_All_FeeHeadLogintType";
        public const string courseFeeHeadWithCollegeType = "Course_FeeHead_WithCollegeType_Save";
        public const string coursefeewaveoff = "WaveOff_Save";
        public const string Getcoursefeewaveoff = "WaveOff_GetByID";
        public const string CoursesFeeHeadLockUnlockByUniversityID = "CoursesFeeHead_LockUnlockByUniversityID";
        public const string CoursesFeeHeadLockUnlockByCollegeID = "CoursesFeeHead_LockUnlockByCollegeID";
        #endregion
        #region PG CourseFeeHead
        public const string PgCourseFeeHead = "PG_Course_FeeHead_Save";
        public const string PgCourseFeeDetails = "App_All_PG_CourseFeeDetails";
        public const string PgFeeHeadByLoginType = "App_All_PG_FeeHeadLogintType";
        public const string PgCourseFeeHeadWithCollegeType = "PG_Course_FeeHead_WithCollegeType_Save";
        public const string PgSaveCourseFeeWaveOff = "PG_WaveOff_Save";
        public const string PgGetCourseFeeWaveOff = "PG_WaveOff_GetByID";
        public const string PgCoursesFeeHeadLockUnlockByUniversityID = "PG_CoursesFeeHead_LockUnlockByUniversityID";
        public const string PgCoursesFeeHeadLockUnlockByCollegeID = "PG_CoursesFeeHead_LockUnlockByCollegeID";
        #endregion
        #region CombinationSeat
        public const string GetCombinationSeat = "App_CombinationForSeat_GetByCollegeIdAndCourseId";
        public const string AddCombinationSeat = "App_CombinationSeat_Save";
        #endregion
        #region
        public const string GetCourseFee = "Get_Course_Fee";
        #endregion
        #region
        public const string GetPGCourseFee = "Get_PG_Course_Fee";
        #endregion
        #region dashboard ug pg
        public const string DashboardUg = "Get_DashboardUg";
        public const string DashboardChartUg = "Get_DashboardChartUg";
        public const string DashboardPg = "Get_DashboardPg";
        public const string DashboardChartPg = "Get_DashboardChartPg";
        #endregion
        #region Student PG
        public const string SaveStudentPG = "Student_PG_Save";
        public const string StudentLoginPG = "Student_PG_Login";
        public const string UpdateStudentPersonalDetailsPG = "Student_PG_UpdatePersonalDetails";
        public const string UpdateBankDetailsPG = "Student_PG_UpdateBankDetails";
        public const string UpdateAddressDetailsPG = "Student_PG_UpdateAddressDetails";
        public const string UpdateAcademicDetailsPG = "Student_PG_UpdateAcademicDetails";
        public const string UpdateWeightagePG = "Student_PG_UpdateWeightage";
        public const string UploadDocumentsPG = "Student_PG_UploadDocuments";
        public const string GetStudentByIdPG = "Student_PG_GetStudentById";
        public const string GetSubjectByStudentIdPG = "Student_PG_GetSubjectByStudentId";
        public const string GetProgressBarPG = "Student_PG_GetProgressBar";
        public const string UpdateDeclarationsPG = "Student_PG_UpdateDeclarations";
        public const string GenerateOTPPG = "Student_PG_GenerateOTP";
        public const string UpdateCourseChoicePG = "Student_PG_UpdateCourseChoice";
        public const string GetDocumentsByStudentIdPG = "Student_PG_GetDocumentsByStudentId";
        public const string GetCourseChoiceByStudentIdPG = "Student_PG_GetCourseChoiceByStudentId";
        public const string SaveRegisterationFeesPG = "Student_PG_RegisterationFees";
        public const string UnlockFormPG = "Student_PG_UnlockForm";
        public const string UpdateStudentDetailsPG = "Student_PG_UpdateStudentDetails";
        public const string StudentForgotPasswordPG = "App_Login_ForgotPassword_Student_PG";
        public const string StudentDetailsPG = "Student_PG_GetDetailsByRegId";
        public const string StudentAppDetailsPG = "Student_PG_GetAppDetailsByRegId";
        public const string GetCourseChoiceByRegIdPG = "Student_PG_GetCourseChoiceByRegId";
        public const string GetSubjectByRegIdPG = "Student_PG_GetSubjectByRegId";
        public const string GetDocumentsByRegIdPG = "Student_PG_GetDocumentsByRegId";
        public const string GetObjectionsBystudentId = "Student_GetObjectionsByStudentIDPG";
        public const string GetAdmissionSeatDetailsByStudentIdPG = "Student_PG_GetAdmissionSeatDetailsByStudentId";
        public const string GetStudentReceiptByStudentIdPG = "usp_PG_GetStudentReceiptByStudentId";
        public const string GetStudentReceiptListByStudentIdPG = "usp_PG_GetStudentReceiptListByStudentId";

        #endregion
        #region UG Objection
        public const string SaveObjection = "Objection_Save";
        public const string GetAllObjectionsByRegId = "GetAllObjections_ByRegId";
        public const string GetAllObjectionsByCollegeId = "GetAllObjections_ByCollegeId";
        public const string GetObjectionById = "GetObjectionById";
        #endregion
        #region PG Objection
        public const string PGSaveObjection = "PG_Objection_Save";
        public const string PGGetAllObjectionsByRegId = "PG_GetAllObjections_ByRegId";
        public const string PGGetAllObjectionsByCollegeId = "PG_GetAllObjections_ByCollegeId";
        public const string PGGetObjectionById = "PG_GetObjectionById";
        #endregion
        #region Verification
        public const string GetVerifiedStudentByRegIdCollege = "Verification_GetVerifiedStudentByRegIdCollege";
        public const string GetStudentByRegId = "Verification_GetStudentByRegId";
        public const string GetStudentsByCollege = "Verification_GetStudentsByCollege";
        public const string VerifyStudentWithSection = "Verification_VerifyStudentWithSection";
        public const string RevokeStudentVerification = "Verification_RevokeVerification";
        public const string UnlockAndRevokeVerification = "Student_UnlockANDRevokeVerification";
        public const string VerificationStudentCourseChoice = "Verification_StudentCourseChoice";
        public const string VerificationFInalSubmit = "Verification_FinalSubmit";
        public const string GetStudentsByCollegeBCOM = "Verification_GetStudentsByCollegeforBCOM";
        public const string GetStudentByRegIdBCOM = "Verification_GetStudentByRegIdForBCOM";
        public const string BCOMStudentWeightage = "ADD_BCOMStudentWeightage";
        public const string studentCourseCombination = "Verification_GetCombinationByCCId";
         


        #endregion
        #region PG Verification
        public const string PGGetVerifiedStudentByRegIdCollege = "Verification_PG_GetVerifiedStudentByRegIdCollege";
        public const string PGGetStudentsByCollege = "Verification_PG_GetStudentsByCollege";
        public const string PGVerifyStudentWithSection = "Verification_PG_VerifyStudentWithSection";
        public const string PGGetStudentByRegId = "Verification_PG_GetStudentByRegId";
        public const string PGVerificationFInalSubmit = "Verification_PG_FinalSubmit";
        public const string PGVerificationStudentCourseChoice = "Verification_PG_StudentCourseChoice";


        #endregion
        #region PGCollegeCourse Seat
        public const string PGSaveCollegeCourseSeat = "PG_App_College_Course_Seat_Save";
        public const string PGUpdateCollegeCourseSeat = "PG_App_College_Course_Seat_update";
        public const string PGDeleteCollegeCourseSeatbyId = "PG_App_College_Course_Seat_Delete";
        public const string PGGetCollegeCourseSeatByID = "PG_App_College_Course_Seat_GetById";
        public const string PGGetAllCollegeCourseSeat = "PG_App_All_CollegeCourseSeat";
        public const string PGCollegeCourseSeatMatrix = "PG_SeatMatrix_GetByCollegeId";
        public const string PGReservationQuota = "PG_App_ReservationPercentage";
        public const string PGLockUnlockCourseSeatsByCollegeID = "PG_App_College_CourseSeat_LockUnlockByCollegeID";
        #endregion
        public const string GetAppVersion = "GetAppVersion";
        #region MapCollegGroupSubject
        public const string SaveCollegeGroupSubject = "App_Group_MapSubject";
        public const string GetGroupWithSubject = "Get_Group_Details";
        public const string GroupSubjectDetails = "Get_Subject_ByGroupIdAndCollegeId";
        public const string CollegeGroups = "Get_Group_ListBucollege";
        public const string GetGroupWithSubjectEdit = "Get_GroupDetails_BygroupId";
        public const string CollegeGroupsNoPaging = "Get_Group_ListBucollegeNoPaging";
        public const string LockUnLockCollegeCourseGroupSubject = "College_CourseGroupSubjectLockUnlockByID";
        public const string GroupSubjectDetailsforStudent = "Get_Subject_ByGroupIdAndCollegeIdforstudent";
        public const string DeleteGroupByGroupId = "DELETE_Group_BygroupId";
        //public const string PGCollegeCourseSeatMatrix = "PG_SeatMatrix_GetByCollegeId";
        //public const string PGReservationQuota = "PG_App_ReservationPercentage";
        //public const string PGLockUnlockCourseSeatsByCollegeID = "PG_App_College_CourseSeat_LockUnlockByCollegeID";
        #endregion
        #region RazorPay PayemtnGateway PG
        public const string GenerateOrderPG = "usp_PG_GenerateOrder_RazorPay";
        public const string PaymentFailurePG = "usp_PG_PaymentFailure_RazorPay";
        public const string PaymentSuccessPG = "usp_PG_PaymentSuccess_RazorPay";
        public const string PaymentDetailPG = "usp_PG_PaymentDetail_RazorPay";
        public const string AccountDetailICICI = "Get_Icici_Payment_Id";
        public const string FetchPaymentPG = "usp_PG_FetchPayment_RazorPay";
        public const string FetchPaymentLog_PG = "usp_PG_Fetchpayment_Initiated_log";
        public const string FetchAllPaymentsForStudent = "usp_Fetch_All_Payment_Info_For_Student";
        public const string FetchAllPendingPaymentsTemp = "Get_All_Pending_Transaction";
        public const string FetchAllPendingRegistration_PG_PaymentsTemp = "Get_All_Pending_Transaction_PG_Registration";
        #endregion

        #region PayGov Paument Gateway
        public const string Payment_Initiated_PAYGOV = "usp_UG_Payment_Initiated_PAYGOV";
        public const string FindPendingTransactionPG = "usp_PG_Pending_Payment_PAYGOV";
        public const string FindPendingTransactionUG = "usp_UG_Pending_Payment_PAYGOV";
        #endregion

        #region RazorPay PayemtnGateway UG
        public const string GenerateOrderUG = "usp_UG_GenerateOrder_RazorPay";
        public const string PaymentFailureUG = "usp_UG_PaymentFailure_RazorPay";
        public const string PaymentSuccessUG = "usp_UG_PaymentSuccess_RazorPay";
        public const string PaymentDetailUG = "usp_UG_PaymentDetail_RazorPay";
        public const string FetchPaymentUG = "usp_UG_FetchPayment_RazorPay";
        public const string FetchPaymentLog_UG = "usp_UG_Fetchpayment_Initiated_log";

        public const string CheckSeatAvailabilityBeforePayment = "usp_UG_CheckSeatAvailabilityBeforePayment";
        public const string CheckSeatAvailabilityBeforePaymentPG = "usp_PG_CheckSeatAvailabilityBeforePayment";
        #endregion
        #region HDFC Payment Gateway
        public const string HDFCPaymentInitiated = "usp_UG_Payment_Initiated_HDFC";
        public const string HDFCPaymentResponse = "usp_UG_Payment_Response_HDFC";
        public const string GetHDFCPaymentDetail = "usp_Get_HDFC_PaymentDetail";
        public const string HDFCMatchHashPaymentResponse = "usp_UG_Payment_Match_Hash_Response_HDFC";


        public const string HDFCPaymentInitiatedPG = "usp_PG_Payment_Initiated_HDFC";
        public const string HDFCPaymentResponsePG = "usp_PG_Payment_Response_HDFC";
        public const string GetHDFCPaymentDetailPG = "usp_Get_PG_HDFC_PaymentDetail";
        public const string HDFCMatchHashPaymentResponsePG = "usp_PG_Payment_Match_Hash_Response_HDFC";
        public const string HDFCCollegeBank = "PROC_Hdfc_College_Banks";

        public const string HDFCVerifyPayment = "usp_UG_VerifyPayment_HDFC";
        public const string HDFCVerifyPaymentPG = "usp_PG_VerifyPayment_HDFC";
        public const string UG_ReconcilePayment = "usp_UG_Reconcile_Payment_Initiated_HDFC";
        public const string UG_Publicly_ReconcilePayment = "usp_UG_Publicly_Reconcile_Payment_Initiated_HDFC";
        public const string Reconciliation_Transaction_List = "PROC_Reconciliation_Transaction";

        #endregion
        #region Merit List
        public const string ProvisionalMeritListUG = "usp_UG_GetProvisionalMeritList";
        public const string WaitingListUG = "usp_UG_GetWaitingList";
        public const string ProvisionalMeritListPG = "usp_PG_GetProvisionalMeritList";
        public const string WaitingListPG = "usp_PG_GetWaitingList";
        public const string SaveAdmissionSeatUG = "usp_UG_SaveAdmissionSeat";
        public const string SaveAdmissionSeatPG = "usp_PG_SaveAdmissionSeat";
        public const string GetPGCourseFeeByStudentID = "Get_PG_CourseFeeByStudentID";
        public const string GetUGCourseFeeByStudentID = "Get_UG_CourseFeeByStudentID";
        public const string GetAdmissionSeatStatusUG = "usp_UG_GetAdmissionSeatStatus";
        public const string GetAdmissionSeatStatusPG = "usp_PG_GetAdmissionSeatStatus";
        public const string GetStudentCourseChoiceByRegIdUG = "usp_UG_GetCourseChoiceByRegId";
        public const string GetStudentReceiptByRegIdUG = "usp_UG_GetStudentReceiptByRegId";
        public const string GetStudentReceiptByRegIdPG = "usp_PG_GetStudentReceiptByRegId";
        public const string CollegeCourseSubjectCount = "usp_UG_CollegeCourseSubjectCount";
        public const string StudentFeeReceiptList = "usp_UG_GetStudentFeeReceiptListByRegId";
        public const string StudentFeeReceiptListPG = "usp_PG_GetStudentFeeReceiptListByRegId";
        public const string GetVacantSeatsByCategory = "usp_UG_GetVacantSeatsByCategory";
        public const string UpdateBookedSeatMatrix = "usp_UG_UpdateBookedSeatMatrix";
        public const string SaveAdmissionSeatUGForWaiting = "usp_UG_SaveAdmissionSeatForOpenMerit";
        public const string GetVacantSeat = "Get_VacantSeatByCollege";
        public const string GetAdmissionSeatStatusUGforwaitinglist = "usp_UG_GetAdmissionSeatStatusForwaitingList";
        public const string GetAdmissionSeatStatusPGforwaitinglist = "usp_PG_GetAdmissionSeatStatusForwaitingList";
        public const string SaveAdmissionSeatForwaitingList = "usp_PG_SaveAdmissionSeatForOpenMerit";
        public const string GetVacantSeatForPG = "Get_VacantSeatByCollegePG";

        
        #endregion

        #region Merit List
        public const string GetCategoryCombinationMaster = "Master_GetReservationCategoryCombination";
        #endregion

        #region OTP 

        public const string SaveToken = "Save_Token";
        public const string VerifyToken = "Verify_Token";
        public const string VerifyWaitingToken = "Verify_Waiting_Token";
        #endregion

        #region Student Admission Cancellation for College
        public const string StudentDetailsForCancellation = "Student_GetDetailsForCancellation";
        public const string StudentDetailsForCancellationPG = "Student_PG_GetDetailsForCancellation";
        public const string StudentCancelAdmissionSeat = "Student_CancelAdmissionSeat";
        public const string StudentCancelAdmissionSeatPG = "Student_PG_CancelAdmissionSeat";
        #endregion


        #region Revoke 
        public const string RevokeSeatPG = "Revoke_Seat_PG";
        public const string RevokeSeatUG = "Revoke_Seat_UG";
        public const string RevokeSeatUGForWaiting = "Revoke_Seat_UG_ForWaiting";
        public const string RevokeSeatPGForWaiting = "Revoke_Seat_PG_ForWaiting";
        #endregion
        #region OTP 
        public const string OfflineAdmission = "Student_Save_OfflineAdmission";

        #endregion

    }
}


