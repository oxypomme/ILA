﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A switch instruction
    /// </summary>
    public class Switch : Instruction
    {
        /// <summary>
        /// Each case : a list of value to be equal, a block of instructions, and an eventual comment
        /// </summary>
        public List<Tuple<List<IValue>, List<Instruction>>> Cases { get; set; }

        /// <summary>
        /// Integrated comment at start
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Default instructions
        /// </summary>
        public List<Instruction> Default { get; set; }

        /// <summary>
        /// Integrated comment at the end
        /// </summary>
        public string EndComment { get; set; }

        /// <summary>
        /// Comment after the default
        /// </summary>
        public IValue Value { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            for (int i = 0; i < Cases.Count; i++)
            {
                if (i == 0)
                {
                    Program.GenerateIndent(textWriter);
                    textWriter.Write("if (");
                    Value.WritePython(textWriter);
                    textWriter.Write("==");
                    Cases[i].Item1[i].WritePython(textWriter);
                    textWriter.Write(") :\n");
                }
                else
                {
                    Program.GenerateIndent(textWriter);
                    textWriter.Write("elif (");
                    Value.WritePython(textWriter);
                    textWriter.Write("==");
                    Cases[i].Item1[i].WritePython(textWriter);
                    textWriter.Write(") :\n");
                }

                foreach (var instruction in Cases[i].Item2)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
            // Generate the default case
            if (Default.Count > 0)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("else:\n");
                foreach (var instruction in Default)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
        }
    }
}