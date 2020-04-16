using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Tests.ServiceTests
{
	[TestFixture]
	class Auth0APIClientTests : Controller
	{
		private TicketingSystemDBContext db;

		public Auth0APIClientTests()
		{
			db = new TicketingSystemDBContext();
		}
		[Test]
		public void GetUserDataTest()
		{
			Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
			UserData userData = Auth0APIClient.GetUserData(user.Auth0Uid);
			Assert.IsNotNull(userData);
		}

		[Test]
		public void InitAPITokenTest()
		{
			Assert.IsTrue(Auth0APIClient.InitAPIToken());
		}

		//[Test]
		//public void UpdateUsersTest()
		//{
		//	Assert.IsTrue(Auth0APIClient.UpdateUsers());
		//}

		[Test]
		public void GetAllUsersTest()
		{
			Assert.IsNotNull(Auth0APIClient.GetAllUsers());
		}

		[Test]
		public void GetPermissionsTest()
		{
			var users = Auth0APIClient.GetAllUsers();
			Assert.IsNotNull(Auth0APIClient.GetPermissions(users[0].user_id));
		}

		[Test]
		public void ValidateTokenTest()
		{
			Assert.IsTrue(Auth0APIClient.ValidateToken());
		}

		[Test]
		public void UpdateDBUserTest()
		{
			using (var db = new TicketingSystemDBContext())
			{
				Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
				var actual = Auth0APIClient.UpdateDBUser(user.Auth0Uid);

				Assert.IsNotNull(actual);
				Assert.IsTrue(actual);
			}
		}

		[Test]
		public void AddUserTest()
		{
			Users user = TestUtility.CreateTestUser();
			UserManager um = new UserManager();


			using (var db = new TicketingSystemDBContext())
			{
				string testerId = db.Users.Where(u => u.Email == "ticketingsystemtester@gmail.com").FirstOrDefault().Auth0Uid;
				UserData ud = Auth0APIClient.GetUserData(testerId);

				um.CreateUser(user, ud);

				string id = db.Users.Find(user.UserId).Auth0Uid;

				SetRoleTest(id, user.ShiftType);
				FetchRoleTest(user.ShiftType);
				GetUserRole(id, user.ShiftType);
				DeleteUserTest(id);

				Assert.IsNotNull(id);
			}

		}

		public void SetRoleTest(string auth0Id, string shiftType)
		{
			bool result = Auth0APIClient.SetRole(auth0Id, shiftType);
			Assert.IsTrue(result);
		}

		public void FetchRoleTest(string shiftType)
		{
			Auth0Role result = Auth0APIClient.FetchRole(shiftType);
			Assert.IsNotNull(result);
			Assert.AreEqual(result.name, shiftType);
		}

		public void GetUserRole(string auth0id, string shiftType)
		{
			var result = Auth0APIClient.GetUserRole(auth0id);
			Auth0Role role = result.ElementAt(0);
			Assert.AreEqual(role.name, shiftType);
		}

		public void DeleteUserTest(string Auth0id)
		{
			bool result = Auth0APIClient.DeleteUser(Auth0id);
			Assert.IsNotNull(result);
			Assert.IsTrue(result);
		}

		[TearDown]
		public void Cleanup()
		{
			TestUtility.UserCleanup();
		}
	}
}
