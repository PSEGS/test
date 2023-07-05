using AdmissionPortal_API.Domain.ApiModel.PGObjection;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IPGObjectionRepository
    {
        Task<ServiceResult> CreateObjectionAsync(AddPgObjection model);
        Task<PgObjectionDetails> GetAllObjectionsByRegId(string RegId);

        Task<PgObjectionDetails> GetAllObjectionsByCollegId(string CollegeId);
        Task<GetSinglePgObjection> GetObjectionById(int ObjectionId);

    }
}

