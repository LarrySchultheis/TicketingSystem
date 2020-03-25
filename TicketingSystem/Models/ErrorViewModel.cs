using System;

namespace TicketingSystem.Models
{
    public class ErrorViewModel
    {
        public string ErrorCode { get; set; }

        public bool ShowErrorCode => !string.IsNullOrEmpty(ErrorCode);
    }
}
