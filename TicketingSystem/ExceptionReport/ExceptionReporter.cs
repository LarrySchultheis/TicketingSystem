using System;
using System.IO;

namespace TicketingSystem.ExceptionReport
{
    public static class ExceptionReporter
    {
        /// <summary>
        /// Dumps exceptions to LatestException.txt and Archive directory
        /// </summary>
        /// <param name="e">Exception to dump</param>
        public static void DumpException(Exception e)
        {
            string path = Environment.CurrentDirectory + "\\ExceptionReport";
            using (StreamWriter outFile = new StreamWriter(Path.Combine(path, "LatestException.txt")))
            {
                outFile.WriteLine(e);
                outFile.Close();
            }
            path += "\\Archive";
            string now = DateTime.Now.ToString();
            now = now.Replace("/", "-");
            now = now.Replace(":", "-");

            using (StreamWriter outfile = new StreamWriter(Path.Combine(path, "Exception_" + now + ".txt")))
            {
                outfile.WriteLine(e);
                outfile.Close();
            }

        }
    }
}
