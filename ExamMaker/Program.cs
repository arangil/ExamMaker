using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ExamMaker
{
    class Program
    {
        public static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {
            foreach (string path in args)
            {
                if (Directory.Exists(path))
                {
                    // Create HTML Header
                    sb.AppendLine(@"<!DOCTYPE html><html lang = ""en"" xmlns = ""http://www.w3.org/1999/xhtml""><head> <meta charset = ""utf-8""/><title></title></head><body>");
                    // Path of the Exam folder. eg: c:\Exambuilder\ixl
                    ProcessDirectory(path);
                    // Close HTML page
                    sb.AppendLine("</body></html>");
                    // Create html file                    
                    CreateExamPaper();
                    // Clear stream
                    sb.Clear();
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }

        /// <summary>
        /// Process all files in the directory passed in, recurse on any directories 
        /// </summary>
        /// <param name="targetDirectory"></param>
        public static void ProcessDirectory(string targetDirectory)
        {
            // Get random file from each sub directory
            string filename = getrandomfile(targetDirectory);
            // Build images in divs
            sb.AppendLine(@"<div style=""max-width:800px;""><img src = '" + filename + @"' style = ""max-width:inherit;""></div>");            
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        /// <summary>
        /// Returns random file from any directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getrandomfile(string path)
        {
            string file = null;
            if (!string.IsNullOrEmpty(path))
            {
                var extensions = new string[] { ".png", ".jpg", ".gif" };
                try
                {
                    var di = new DirectoryInfo(path);
                    var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                    Random R = new Random();
                    file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
                }                
                catch { }
            }
            return file;
        }

        /// <summary>
        /// Creates html file by overwriting existing file.
        /// </summary>
        public static void CreateExamPaper()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("ExamPaper.html", false))
            {
                file.WriteLine(sb.ToString());
            }
        }

    }
}
