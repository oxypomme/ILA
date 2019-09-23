using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ILA
{
    partial class Program
    {
        #region Private Methods

        private static void ParseAlgo(string start, ref int index)
        {
        }

        private static void ParseFile(string file)
        {
            string fileContent;
            using (var stream = new StreamReader(file))
                fileContent = stream.ReadToEnd();
            int index = 0;
            while (index < fileContent.Length)
            {
                if (fileContent.Substring(index, 5) == "algo ")
                {
                    var name = ReadAlphaString(fileContent, ref index);
                    SkipBlankSpaces(fileContent, ref index);
                }
            }
        }

        private static string ReadAlphaString(string str, ref int index)
        {
            string result = "";
            while (index < str.Length && char.IsLetter(str[index]))
            {
                result += str[index];
                index++;
            }
            return result;
        }

        private static void SkipBlankSpaces(string str, ref int index)
        {
            var skippedChars = new char[] { ' ', '\t', '\n' };
            while (index < str.Length && skippedChars.Contains(str[index]))
                index++;
        }

        #endregion Private Methods
    }
}