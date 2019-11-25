﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    internal class If : Instruction
    {
        #region Public Properties

        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        //List<...> => Liste de else, IValue => Condition, List<Instruction> => Bloc
        public List<Tuple<IValue, List<Instruction>>> Elif { get; set; }

        public List<string> ElifComments { get; set; }
        public string ElseComment { get; set; }
        public List<Instruction> ElseInstructions { get; set; }
        public string EndComment { get; set; }
        public IValue IfCondition { get; set; }
        public List<Instruction> IfInstructions { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("if (");
            IfCondition.WritePython(textWriter);
            textWriter.Write(") :\n");

            foreach (var instruction in IfInstructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }

            foreach (var elif in Elif)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("elif");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in elif.Item2)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }

            if (ElseInstructions != null && ElseInstructions.Count > 0)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("else (");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in ElseInstructions)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
        }

        #endregion Public Properties
    }
}