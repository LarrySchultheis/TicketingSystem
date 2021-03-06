﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    public enum ReportFormat
    {
        PDF,
        CSV
    }
    public enum ReportName
    {
        LaborHoursByJob,
        IncentiveReport,
        LaborHoursByJobAndEmployee
        
    }

    //Models the different access levels that the system uses
    public static class ModelUtility
    {
        public static readonly string AccessLevel1 = "access:lvl1";
        public static readonly string AccessLevel2 = "access:lvl2";
        public static readonly string AccessLevel3 = "access:lvl3";
        public static readonly string AccessLevel4 = "access:lvl4";
    }
}
