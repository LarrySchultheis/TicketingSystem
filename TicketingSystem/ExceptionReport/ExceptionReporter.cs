using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.ExceptionReport
{
    public class ExceptionReporter
    {
        public void DumpException(Exception e)
        {
            string path = Environment.CurrentDirectory + "\\ExceptionReport";
            using (StreamWriter outFile = new StreamWriter(Path.Combine(path, "LatestException.txt")))
            {
                outFile.WriteLine(e);
                outFile.Close();
            }

        }
    }
}
