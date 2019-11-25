﻿using System;
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
            Parameters = new List<Parameter>()
            {
             new Parameter()
             {
                 ImportedVariable = new Variable()
                 {
                         Constant = false,
                       Name = "read",
                      Type = null
                 }
             }
            };
            Instructions = null;
        }

        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }
    }
}