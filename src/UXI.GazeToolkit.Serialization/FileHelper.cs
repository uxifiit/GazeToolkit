using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization
{
    public static class FileHelper
    {
        public static TextReader CreateInputReader(string targetPath)
        {
            TextReader reader;
            if (String.IsNullOrWhiteSpace(targetPath))
            {
                reader = Console.In;
            }
            else
            {
                var fileStream = new FileStream(targetPath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fileStream);
            }

            return reader;
        }


        public static TextWriter CreateOutputWriter(string targetPath)
        {
            TextWriter outputWriter;
            if (String.IsNullOrWhiteSpace(targetPath))
            {
                outputWriter = Console.Out;
            }
            else
            {
                var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
                outputWriter = new StreamWriter(fileStream);
            }

            return outputWriter;
        }
    }
}
