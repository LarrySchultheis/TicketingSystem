using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Services
{
    public class Utility
    {
        public static void Log(string log)
        {
            string path = Environment.CurrentDirectory;
            path += "\\logs";
            string now = DateTime.Now.ToString();
            now = now.Replace("/", "-");
            now = now.Replace(":", "-");

            using (StreamWriter outfile = new StreamWriter(Path.Combine(path, "Log_" + now + ".txt")))
            {
                outfile.WriteLine(log);
                outfile.Close();
            }
        }
    }
}
