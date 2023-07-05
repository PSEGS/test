using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IObjectionRepository
    {
        Task<ServiceResult> CreateObjectionAsync(AddObjection model);
        Task<ObjectionDetails> GetAllObjectionsByRegId(string RegId);

        Task<ObjectionDetails> GetAllObjectionsByCollegId(string CollegeId);
        Task<GetSingleObjection> GetObjectionById(int ObjectionId);

    }
}

