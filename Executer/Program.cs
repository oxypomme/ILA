using System;
using System.IO;
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
algo main
{
    si test_bool(vrai) = vrai alors
        ecrire(""hello world!"")
    fsi
}
fonction test_bool(var:booleen) : booleen
{
    si var = vrai alors
        test_bool<- non test_bool(non var)
    sinon
        test_bool<- var = parse(""true"")
    fsi
   }
fonction parse(str:chaine) : booleen
{
    si(str = ""true"") = vrai alors
        parse<- parse(""faux"")
    sinon
        parse<- faux = vrai
    fsi
}");
            using (var file = new StreamWriter("out.ila"))
                prg.WriteILA(file);
        }

        #endregion Private Methods
    }
}