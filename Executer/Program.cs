using System;
using ILANET;
using ILANET.Parser;

namespace Executer
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var prg = Parser.Parse(
@"
en:type enumeration(VALUE1,VALUE2)
tab:type tableau[1..2, VALUE1 .. VALUE2 ]:entier
test_var:en
constant:const entier <- 4
//test
algo test
{
}

//fonction qui ne fait rien du tout
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