using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http;

namespace TicketingSystem.Services
{

	public static class Utility
	{
		/// <summary>
		/// Function to create an Error Model to display to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="reason"></param>
		/// <returns></returns>
		public static async Task<ServerErrorViewModel> CreateServerErrorView(HttpResponseException exception)
		{
			ServerErrorViewModel errorView = new ServerErrorViewModel();
			string response = await exception.Response.Content.ReadAsStringAsync();
			var strings = response.Split("|");
			string reason = strings[0];
			string guid = strings[1];
			try
			{
				int code = (int)exception.Response.StatusCode;
				string status = exception.Response.StatusCode.ToString();
				errorView.ErrorCode = code + " " + status;
				errorView.Message = "Please contact your system administrator with the following error code";
				errorView.Reason = reason;
				errorView.Guid = guid;
			}
			catch(Exception e)
			{
				ExceptionReporter.DumpException(e);
			}
			return errorView;
		}

		public static ErrorViewModel CreateErrorView(HttpResponseException exception, string reason)
		{
			ErrorViewModel errorView = new ErrorViewModel();

			try
			{
				int code = (int)exception.Response.StatusCode;
				string status = exception.Response.StatusCode.ToString();
				errorView.ErrorCode = code + " " + status;
				errorView.Reason = reason;
			}
			catch (Exception e)
			{
				ExceptionReporter.DumpException(e);
			}
			return errorView;
		}

		public static HttpResponseMessage CreateResponseMessage(Exception e)
		{
			string guid = ExceptionReporter.DumpException(e);
			HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.InternalServerError);

			message.Content = new StringContent(e.Message + "|" + guid);

			return message;
		}

		/// <summary>
		/// Get a list of names of users who are in the system
		/// </summary>
		/// <returns></returns>
		public static List<string> GetValidNames()
		{
			List<string> userNames = new List<string>();
			try
			{
				using (var db = new TicketingSystemDBContext())
				{
					var users = db.Users.ToList();
					foreach (Users u in users)
					{
						userNames.Add(u.FullName);
					}
				}
				return userNames;
			}
			catch (Exception e)
			{
				throw new HttpResponseException(Utility.CreateResponseMessage(e));
			}
		}

		/// <summary>
		/// Get a database user given their full name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Users GetUserByName(string name)
		{
			try
			{
				using (var db = new TicketingSystemDBContext())
				{
					return db.Users.Where(u => u.FullName == name).First();
				}
			}
			catch (Exception e)
			{
				throw new HttpResponseException(Utility.CreateResponseMessage(e));
			}
		}


	}
}
