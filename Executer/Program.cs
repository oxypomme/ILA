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
t_tab:type tableau[1..2, VALUE1 .. VALUE2 ]:entier
test_var:en
tab:t_tab
constant:const entier <- (4+5)<= ""test""
//test
algo test
{
}

//fonction qui ne fait rien du tout
fonction fct(var:entier) : entier
var_local:entier
{
    si vrai alors
        //test de comm
    sinon si faux alors
        /*
            hello world
        */
    sinon
        /*
            comm multi
        */
    fsi //t
}"
);
            Console.WriteLine(prg.Declarations.Count);
        }

        #endregion Private Methods
    }
}