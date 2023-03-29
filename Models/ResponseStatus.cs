namespace RMS.Models
{
    public class ResponseStatus
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success
        {
            get
            {
                return StatusCode == 1;
            }
        }
    }
}
