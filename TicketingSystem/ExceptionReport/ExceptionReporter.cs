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
            //generate unique code for exception
            var guid = Guid.NewGuid().ToString();

            //Write exception to LatestException and archive
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

        /// <summary>
        /// Writes exception details to given StreamWriter
        /// </summary>
        /// <param name="e">The exception to write</param>
        /// <param name="outFile">The Stream of the file to write to</param>
        /// <param name="guid">A GUID to uniquely ID the exception</param>
        private static void WriteDetails(Exception e, StreamWriter outFile, string guid)
        {
            outFile.WriteLine(e);
            outFile.WriteLine("Exception thrown: " + DateTime.Now.ToString());
            outFile.WriteLine("Error Code: " + guid);
            outFile.Close();
        }
    }
}
