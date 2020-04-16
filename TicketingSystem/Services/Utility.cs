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
using System.Security.Cryptography;
using System.Text;

namespace TicketingSystem.Services
{

	public static class Utility
	{
		/// <summary>
		/// Function to create a ServerErrorViewModel to display to the user
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


		/// <summary>
		/// Function to create a basic exception view
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static ErrorViewModel CreateBasicExceptionView(Exception exception, string guid)
		{
			ErrorViewModel errorView = new ErrorViewModel();

			try
			{
				errorView.ErrorCode = guid;
				errorView.Reason = exception.Message;
			}
			catch (Exception e)
			{
				ExceptionReporter.DumpException(e);
			}
			return errorView;
		}

		/// <summary>
		/// Create an ErrorViewModel to display to the user
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="reason"></param>
		/// <returns></returns>
		public static ErrorViewModel CreateHttpErrorView(HttpResponseException exception, string reason)
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

		/// <summary>
		/// Creates an HttpResponseMessage given an Exception
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
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


		public static string Encrypt(string encryptString)
		{
			string EncryptionKey = "crackM3IfYouC@n";
			byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					encryptString = Convert.ToBase64String(ms.ToArray());
				}
			}
			return encryptString;
		}

		public static string Decrypt(string cipherText)
		{
			string EncryptionKey = "crackM3IfYouC@n";
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

	}
}
