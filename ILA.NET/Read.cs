using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public sealed class Read : Function
    {
        public static readonly Read Instance = new Read();

        internal Read()
        {
            Name = "lire";
            Parameters = new List<Parameter>();
            ReturnType = null;
            Instructions = null;
        }

        public override void WritePython(TextWriter textWriter)
        {
            //TODO: tester si correctement fait en python
            base.WritePython(textWriter);
        }
    }
}