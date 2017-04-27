using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logging
{
    public class AppLogger
    {
        string datestamp;
        string statusResult;
        
        public AppLogger()
        {
            datestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Write Message Data to log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Write(string message, out string status)
        {
            try
            {
                // Set a variable to the My Documents path.
                string mydoc = Path.Combine(Properties.Settings.Default.LogFilePath, "log_" + datestamp + ".txt");
                // Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile =
                    new StreamWriter(mydoc, true))
                {
                   //this should append if already open or create if not
                   outputFile.WriteLine("\n" + message);
                }
                //should automatically close
                status = "Success";
                return true;
            }
            catch (IOException e){
                status = "Error: " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                status = "Error: " + e.Message;
                return false;
            }
        }

        
    }
}
