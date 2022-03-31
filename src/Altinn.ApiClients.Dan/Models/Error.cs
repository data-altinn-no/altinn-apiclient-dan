namespace Altinn.ApiClients.Dan.Models
{
    public class Error 
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string DetailCode { get; set; }
        public string DetailDescription { get; set; }
        public string StackTrace { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string InnerExceptionStackTrace { get; set; }
    }
}
