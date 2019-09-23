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

        private static void ParseFile(string file)
        {
            string fileContent;
            using (var stream = new StreamReader(file))
                fileContent = stream.ReadToEnd();
        }

        private static void SkipBlankSpaces(string str, ref int index)
        {
            var skippedChars = new char[] { ' ', '\t' };
            while (index < str.Length && skippedChars.Contains(str[index]))
                index++;
        }

        #endregion Private Methods
    }
}