using System;
using System.IO;

namespace TicketingSystem.ExceptionReport
{
    public static class ExceptionReporter
    {
        /// <summary>
        /// Dumps exceptions to LatestException.txt and Archive directory
        /// </summary>
        /// <param name="e">Exception to dump</param>.
        /// <returns>unique GUID for tracking purposes</returns>
        public static string DumpException(Exception e)
        {
            var guid = Guid.NewGuid().ToString();
            string path = Environment.CurrentDirectory + "\\ExceptionReport";
            using (StreamWriter outFile = new StreamWriter(Path.Combine(path, "LatestException.txt")))
            {
                outFile.WriteLine(e);
                outFile.Close();
            }
            path += "\\Archive";

            using (StreamWriter outfile = new StreamWriter(Path.Combine(path, "Exception_" + guid + ".txt")))
            {
                outfile.WriteLine(e);
                outfile.Close();
            }
            return guid;
        }
    }
}
