using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TicketingSystem.Controllers;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace Tests.ControllerTests
{
    [TestFixture]
    class EditControllerTests
    {
        [Test]
        public void IndexTest()
        {
            EditController ec = new EditController();
            var result = ec.Index();
        }
    }
}
