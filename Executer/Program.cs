using System;

namespace Executer
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var prg = new ILANET.Program();
            prg.Parse(
@"//test
algo test
{
}"
);
            Console.WriteLine(prg.Instructions.Count);
        }

        #endregion Private Methods
    }
}