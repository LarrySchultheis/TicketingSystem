using System;

namespace TicketingSystem.Models
{
	//Models A basic Error for displaying to the user
	public class ErrorViewModel
	{
		public string ErrorCode { get; set; }
		public string Reason { get; set; }
		public bool ShowErrorCode => !string.IsNullOrEmpty(ErrorCode);
	}
}
