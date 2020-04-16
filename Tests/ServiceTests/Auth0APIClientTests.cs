﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Linq;

namespace Tests.ServiceTests
{
	[TestFixture]
	class Auth0APIClientTests
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

		//[Test]
		//public void AddUserTest()
		//{
		//	Users user = TestUtility.CreateTestUser();
		//	string id = Auth0APIClient.AddUser(user);

		//	Assert.IsNotNull(id);
		//}

		//[Test]
		//public void DeleteUserTest()
		//{
		//	using (var db = new TicketingSystemDBContext())
		//	{
		//		Users user = db.Users.Where(u => u.FullName == "Unit Test User").FirstOrDefault();
		//		bool result = Auth0APIClient.DeleteUser(user.Auth0Uid);
		//		Assert.IsNotNull(result);
		//		Assert.IsTrue(result);
		//	}
		//}
	}
}