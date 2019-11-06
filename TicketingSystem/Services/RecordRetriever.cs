using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCApp.Models;

namespace MVCApp.Services
{
    public class RecordRetriever
    {
        public IEnumerable<TicketData> RetrieveRecords()
        {
            using (var context = new TicketingSystemDBContext())
            {

                var tdata = context.TicketData.ToList();
                foreach (TicketData td in tdata)
                {
                    //td.Employee.EmployeeName = context.Employee.Find(td.EmployeeId).EmployeeName;

                    td.JobType = context.JobType.Find(td.JobTypeId);
                    // td.JobType.JobType1 = td.JobType.JobType1;
                    var x = 1;
                }


                return tdata;
            }
        }
    }
}
