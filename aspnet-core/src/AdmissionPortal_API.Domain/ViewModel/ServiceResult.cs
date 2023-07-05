namespace AdmissionPortal_API.Domain.ViewModel
{
    public class ServiceResult
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public dynamic ResultData { get; set; }
    }
}

