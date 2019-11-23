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
@"
en:type enumeration(VALUE1,VALUE2)
test_var:en
//test
algo test
{
}
fonction fct(var:entier) : entier
{
}"
);
            Console.WriteLine(prg.Declarations.Count);
        }

        #endregion Private Methods
    }
}