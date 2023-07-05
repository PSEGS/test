using AdmissionPortal_API.Domain.ViewModel.ModelInterface;

namespace AdmissionPortal_API.Domain.ViewModel
{
    public class LoginOutputModel : ILoginOutputModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}



