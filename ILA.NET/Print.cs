﻿using System;
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
            base.WritePython(textWriter);
        }
    }
}