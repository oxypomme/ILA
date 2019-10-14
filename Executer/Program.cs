using System;

namespace Executer
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var test = new ILANET.Program();
            Console.WriteLine(test.GenerateCode(ILANET.Program.Language.PYTHON));
        }

        #endregion Private Methods
    }
}