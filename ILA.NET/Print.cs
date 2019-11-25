using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public sealed class Print : Module
    {
        public static readonly Print Instance = new Print();

        internal Print()
        {
            Name = "ecrire";
            Parameters = new List<Parameter>();
            Instructions = null;
        }

        public override void WritePython(TextWriter textWriter)
        {
            //Program.GenerateIndent(textWriter);
            //textWriter.Write("print(");
            //foreach (var parameter in Parameters)
            //{
            //    for (int i = 0; i < Parameters.Count; i++)
            //    {
            //        if (i != 0)
            //            textWriter.Write(", ");
            //        parameter.WritePython(textWriter);
            //    }
            //}
            //textWriter.Write(")\n");
            Name = "print";
        }
    }
}