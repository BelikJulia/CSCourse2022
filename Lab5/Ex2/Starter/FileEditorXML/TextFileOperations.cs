using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileEditorXML
{

    /// <summary>
    /// Perform IO operations on text files
    /// </summary>
    class TextFileOperations
    {
        /// <summary>
        /// Read contents of a text file
        /// </summary>
        /// <param name="fileName">Full file name including path</param>
        /// <returns>File contents</returns>
        public static string ReadTextFileContents(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public static string ReadAndFilterTextFileContents(string fileName)
        {
            StringBuilder fileContents = new StringBuilder();
            int charCode;
            StreamReader fileReader = new StreamReader(fileName);
            while ((charCode = fileReader.Read()) != -1)
            {
                switch (charCode)
                {
                    case 34:
                        fileContents.Append("&quot;");
                        break;
                    case 38:
                        fileContents.Append("&amp;");
                        break;
                    case 39:
                        fileContents.Append("&apos;");
                        break;
                    case 60:
                        fileContents.Append("&lt;");
                        break;
                    case 62:
                        fileContents.Append("&gt;");
                        break;
                    default:
                        fileContents.Append((char)charCode);
                        break;
                }
            }
            return fileContents.ToString();
        }

        /// <summary>
        /// Write to a text file
        /// </summary>
        /// <param name="fileName">Full file name including path</param>
        /// <param name="text">Text to write to file</param>
        public static void WriteTextFileContents(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }
    }
}
