namespace AdmissionPortal_API.Domain.ViewModel.ModelInterface
{
    public interface ILoginOutputModel
    {
        string UserId { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Token { get; set; }
    }
}



