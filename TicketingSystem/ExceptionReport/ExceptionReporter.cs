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
                WriteDetails(e, outFile, guid);
            }
            path += "\\Archive";

            using (StreamWriter outFile = new StreamWriter(Path.Combine(path, "Exception_" + guid + ".txt")))
            {
                WriteDetails(e, outFile, guid);
            }
            return guid;
        }

        private static void WriteDetails(Exception e, StreamWriter outFile, string guid)
        {
            outFile.WriteLine(e);
            outFile.WriteLine("Exception thrown: " + DateTime.Now.ToString());
            outFile.WriteLine("Error Code: " + guid);
            outFile.Close();
        }
    }
}
