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
}
fonction fct(var:entier) : entier
{
}"
);
            Console.WriteLine(prg.Instructions.Count);
        }

        #endregion Private Methods
    }
}