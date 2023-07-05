using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping;
using AdmissionPortal_API.Domain.ApiModel.HdfcPaymentMapping;
using AdmissionPortal_API.Domain.ApiModel.Navigation;
using AdmissionPortal_API.Domain.ApiModel.RazorMappingModel;
using AdmissionPortal_API.Domain.ApiModel.Role;
using AdmissionPortal_API.Domain.ApiModel.RoleNavigation;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.University;
using AdmissionPortal_API.Domain.ApiModel.WorkflowProcess;
using AdmissionPortal_API.Domain.ApiModel.WorkflowStage;
using AdmissionPortal_API.Domain.ApiModel.WorkflowStageAction;
using AdmissionPortal_API.Domain.Model;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AdmissionPortal_API.Utility.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddRole, RoleMaster>();
            CreateMap<UpdateRole, RoleMaster>();
            CreateMap<RoleMaster, UpdateRole>();
            CreateMap<AddCitizen, CitizenMaster>().
                //ForMember(d => d.CommunicationAddressBlockId, o => o.MapFrom(s => s.CommunicationAddressBlockId == 0 ? null : s.CommunicationAddressBlockId)).
                //ForMember(d => d.CommunicationAddressDistrictId, o => o.MapFrom(s => s.CommunicationAddressDistrictId == 0 ? null : s.CommunicationAddressDistrictId)).
                //ForMember(d => d.CommunicationAddressTehsilId, o => o.MapFrom(s => s.CommunicationAddressTehsilId == 0 ? null : s.CommunicationAddressTehsilId)).
                //ForMember(d => d.CommunicationAddressVillageId, o => o.MapFrom(s => s.CommunicationAddressVillageId == 0 ? null : s.CommunicationAddressVillageId)).
                //ForMember(d => d.PermanentAddressBlockId, o => o.MapFrom(s => s.PermanentAddressBlockId == 0 ? null : s.PermanentAddressBlockId)).
                ForMember(d => d.PermanentAddressDistrictId, o => o.MapFrom(s => s.PermanentAddressDistrictId == 0 ? null : s.PermanentAddressDistrictId));
            //ForMember(d => d.PermanentAddressTehsilId, o => o.MapFrom(s => s.PermanentAddressTehsilId == 0 ? null : s.PermanentAddressTehsilId)).
            //ForMember(d => d.PermanentAddressVillageId, o => o.MapFrom(s => s.PermanentAddressVillageId == 0 ? null : s.PermanentAddressVillageId));
            CreateMap<UpdateCitizen, CitizenMaster>().
                //ForMember(d => d.CommunicationAddressBlockId, o => o.MapFrom(s => s.CommunicationAddressBlockId == 0 ? null : s.CommunicationAddressBlockId)).
                //ForMember(d => d.CommunicationAddressDistrictId, o => o.MapFrom(s => s.CommunicationAddressDistrictId == 0 ? null : s.CommunicationAddressDistrictId)).
                //ForMember(d => d.CommunicationAddressTehsilId, o => o.MapFrom(s => s.CommunicationAddressTehsilId == 0 ? null : s.CommunicationAddressTehsilId)).
                //ForMember(d => d.CommunicationAddressVillageId, o => o.MapFrom(s => s.CommunicationAddressVillageId == 0 ? null : s.CommunicationAddressVillageId)).
                //ForMember(d => d.PermanentAddressBlockId, o => o.MapFrom(s => s.PermanentAddressBlockId == 0 ? null : s.PermanentAddressBlockId)).
                ForMember(d => d.PermanentAddressDistrictId, o => o.MapFrom(s => s.PermanentAddressDistrictId == 0 ? null : s.PermanentAddressDistrictId));
            //ForMember(d => d.PermanentAddressTehsilId, o => o.MapFrom(s => s.PermanentAddressTehsilId == 0 ? null : s.PermanentAddressTehsilId)).
            //ForMember(d => d.PermanentAddressVillageId, o => o.MapFrom(s => s.PermanentAddressVillageId == 0 ? null : s.PermanentAddressVillageId));
            CreateMap<AddEmployee, EmployeeMaster>().
                //ForMember(d => d.CommunicationAddressBlockId, o => o.MapFrom(s => s.CommunicationAddressBlockId == 0 ? null : s.CommunicationAddressBlockId)).
                //ForMember(d => d.CommunicationAddressDistrictId, o => o.MapFrom(s => s.CommunicationAddressDistrictId == 0 ? null : s.CommunicationAddressDistrictId)).
                //ForMember(d => d.CommunicationAddressTehsilId, o => o.MapFrom(s => s.CommunicationAddressTehsilId == 0 ? null : s.CommunicationAddressTehsilId)).
                //ForMember(d => d.CommunicationAddressVillageId, o => o.MapFrom(s => s.CommunicationAddressVillageId == 0 ? null : s.CommunicationAddressVillageId)).
                //ForMember(d => d.PermanentAddressBlockId, o => o.MapFrom(s => s.PermanentAddressBlockId == 0 ? null : s.PermanentAddressBlockId)).
                ForMember(d => d.PermanentAddressDistrictId, o => o.MapFrom(s => s.PermanentAddressDistrictId == 0 ? null : s.PermanentAddressDistrictId));
            // ForMember(d => d.PermanentAddressTehsilId, o => o.MapFrom(s => s.PermanentAddressTehsilId == 0 ? null : s.PermanentAddressTehsilId)).
            // ForMember(d => d.PermanentAddressVillageId, o => o.MapFrom(s => s.PermanentAddressVillageId == 0 ? null : s.PermanentAddressVillageId)); 
            CreateMap<UpdateEmployee, EmployeeMaster>().
                //ForMember(d => d.CommunicationAddressBlockId, o => o.MapFrom(s => s.CommunicationAddressBlockId == 0 ? null : s.CommunicationAddressBlockId)).
                //ForMember(d => d.CommunicationAddressDistrictId, o => o.MapFrom(s => s.CommunicationAddressDistrictId == 0 ? null : s.CommunicationAddressDistrictId)).
                //ForMember(d => d.CommunicationAddressTehsilId, o => o.MapFrom(s => s.CommunicationAddressTehsilId == 0 ? null : s.CommunicationAddressTehsilId)).
                //ForMember(d => d.CommunicationAddressVillageId, o => o.MapFrom(s => s.CommunicationAddressVillageId == 0 ? null : s.CommunicationAddressVillageId)).
                //ForMember(d => d.PermanentAddressBlockId, o => o.MapFrom(s => s.PermanentAddressBlockId == 0 ? null : s.PermanentAddressBlockId)).
                ForMember(d => d.PermanentAddressDistrictId, o => o.MapFrom(s => s.PermanentAddressDistrictId == 0 ? null : s.PermanentAddressDistrictId));
            //ForMember(d => d.PermanentAddressTehsilId, o => o.MapFrom(s => s.PermanentAddressTehsilId == 0 ? null : s.PermanentAddressTehsilId)).
            //ForMember(d => d.PermanentAddressVillageId, o => o.MapFrom(s => s.PermanentAddressVillageId == 0 ? null : s.PermanentAddressVillageId)); 

            CreateMap<CitizenMaster, AddCitizen>();
            CreateMap<CitizenMaster, UpdateCitizen>();
            CreateMap<CitizenLogin, CitizenLoginMaster>();

            CreateMap<EmployeeMaster, AddEmployee>();
            CreateMap<EmployeeMaster, UpdateEmployee>();
            CreateMap<EmployeeLogin, EmployeeLoginMaster>();

            CreateMap<AddWorkflowStage, WorkflowStageMaster>();
            CreateMap<UpdateWorkflowStage, WorkflowStageMaster>();
            CreateMap<WorkflowStageMaster, AddWorkflowStage>();
            CreateMap<WorkflowStageMaster, UpdateWorkflowStage>();

            CreateMap<AddWorkflowProcess, WorkflowProcessMaster>().ForMember(d => d.ParentProcessIdFk, o => o.MapFrom(s => s.ParentProcessIdFk == 0 ? null : s.ParentProcessIdFk));
            CreateMap<UpdateWorkflowProcess, WorkflowProcessMaster>();
            CreateMap<WorkflowProcessMaster, AddWorkflowProcess>();
            CreateMap<WorkflowProcessMaster, UpdateWorkflowProcess>();

            CreateMap<UpdateWorkflowStageAction, WorkflowStageActionMaster>();
            CreateMap<AddWorkflowStageAction, WorkflowStageActionMaster>();
            CreateMap<WorkflowStageActionMaster, AddWorkflowStageAction>();
            CreateMap<WorkflowStageActionMaster, UpdateWorkflowStageAction>();
            CreateMap<AdminLogin, AdminLoginMaster>();


            CreateMap<NavigationMaster, AddNavigation>();
            CreateMap<AddNavigation, NavigationMaster>();

            CreateMap<UpdateNavigation, NavigationMaster>();
            CreateMap<NavigationMaster, UpdateNavigation>();


            CreateMap<AddRoleNavigation, RoleNavigationMaster>();
            CreateMap<UpdateRoleNavigation, RoleNavigationMaster>();
            CreateMap<RoleNavigationMaster, AddRoleNavigation>();
            CreateMap<RoleNavigationMaster, UpdateRoleNavigation>();
            CreateMap<AddEmployeeRoleMapping, EmployeeRoleMappingMaster>();
            CreateMap<UpdateEmployeeRoleMapping, EmployeeRoleMappingMaster>();
            CreateMap<EmployeeRoleMappingMaster, AddEmployeeRoleMapping>();
            CreateMap<EmployeeRoleMappingMaster, UpdateEmployeeRoleMapping>();

            CreateMap<UpdateEmployeeProfile, EmployeeMaster>();
            CreateMap<EmployeeMaster, UpdateEmployeeProfile>();
            CreateMap<UpdateCitizenProfile, CitizenMaster>();
            CreateMap<CitizenMaster, UpdateCitizenProfile>();

            CreateMap<AddRoleNavigation, RoleNavigationMapping>();
            CreateMap<AddUniversity, UniversityMaster>();

            CreateMap<UniversityMaster, AddUniversity>();
            CreateMap<AddCollege, CollegeMaster>();
            CreateMap<UpdateCollege, CollegeMaster>();
            CreateMap<CollegeMaster, GetCollege>();
            CreateMap<GetCollege, CollegeMaster>();
            CreateMap<UpdateUniversity, UniversityMaster>();

            CreateMap<UpdateUniversity, updateuniversityMaster>();
            CreateMap<UgPaymentLog, HdfcPaymentMapping>();
            CreateMap<PaymentResponseModel, HdfcPaymentResponseMapping>();
            CreateMap<RazorRoot, RazorMappingModel>();
            CreateMap<UploadNotification, NotificationMaster>();
            CreateMap <NotificaitonModel, NotificationMaster>();




        }
        private List<int> GetRole(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }
            else
            {
                return model.Split(',').Select(int.Parse).ToList();
            }
        }
    }
}

