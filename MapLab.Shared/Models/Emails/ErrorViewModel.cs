namespace MapLab.Shared.Models.Emails
{
    public class ErrorViewModel
    {
        public string? ErrorMessage { get; set; }
        public DateTime TimeOccured { get; set; }
        public int StatusCode { get; set; }
        public string? RequestId { get; set; }
        public string? RequestUrl { get; set; }
        public string? RequestMethod { get; set; }
        public string? UserIpAddress { get; set; }
        public string? Username { get; set; }
        public string? UserAgent { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
        public string? Environment { get; set; }
        public string? ApplicationVersion { get; set; }
    }
}
