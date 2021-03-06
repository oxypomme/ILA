﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Length function, returns teh size of a string
    /// </summary>
    public class Length : Function, Native
    {
        /// <summary>
        /// The only instance of the function
        /// </summary>
        public static readonly Length Instance = new Length();

        internal Length()
        {
            Name = "longueur";
            Parameters = new List<Parameter>()
            {
                new Parameter()
                {
                    ImportedVariable = new Variable()
                    {
                        Constant = false,
                        Name = "input",
                        Type = null
                    },
                    Mode = Parameter.Flags.INPUT
                }
            };
            ReturnType = GenericType.Int;
            Instructions = null;
        }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WriteILA(TextWriter textWriter)
        {
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
        }
    }
}