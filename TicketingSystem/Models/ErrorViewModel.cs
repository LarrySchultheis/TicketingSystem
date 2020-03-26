using System;

namespace TicketingSystem.Models
{
    public class ErrorViewModel
    {
        public string ErrorCode { get; set; }
        public string Reason { get; set; }
        public bool ShowErrorCode => !string.IsNullOrEmpty(ErrorCode);
    }
}
