using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public sealed class Read : Module
    {
        public static readonly Read Instance = new Read();

        internal Read()
        {
            Name = "lire";
            Parameters = new List<Parameter>();
            Instructions = null;
        }

        public override void WriteILA(TextWriter textWriter)
        {
        }

        public override void WritePython(TextWriter textWriter)
        {
            //Program.GenerateIndent(textWriter);
            //foreach (var parameter in Parameters)
            //{
            //    for (int i = 0; i < Parameters.Count; i++)
            //    {
            //        parameter.WritePython(textWriter);
            //        textWriter.Write(" = input()\n");
            //    }
            //}
            Name = "input";
        }
    }
}