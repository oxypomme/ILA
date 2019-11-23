using System;
using ILANET;

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
tab:type tableau[1..2, VALUE1 .. VALUE2 ]:entier
test_var:en
//test
algo test
{
}
/*
    fonction qui ne fait rien du tout
*/
fonction fct(var:entier) : entier
var_local:entier
{
}"
);
            Console.WriteLine(prg.Declarations.Count);
        }

        #endregion Private Methods
    }
}