using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.ReportingServices.Interfaces;

namespace TicketingSystem.Services
{
    public class ReportServerAuthenticator : IAuthenticationExtension
    {
        public string LocalizedName => throw new NotImplementedException();

        public void GetUserInfo(out IIdentity userIdentity, out IntPtr userId)
        {
            throw new NotImplementedException();
        }

        public bool IsValidPrincipalName(string principalName)
        {
            throw new NotImplementedException();
        }

        public bool LogonUser(string userName, string password, string authority)
        {
            throw new NotImplementedException();
        }

        public void SetConfiguration(string configuration)
        {
            throw new NotImplementedException();
        }
    }
}
