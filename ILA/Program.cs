using System;
using System.IO;
using System.Linq;

namespace ILA
{
    internal partial class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Init();
                ParseFile(args[0]);
            }
        }

        #endregion Private Methods
    }
}