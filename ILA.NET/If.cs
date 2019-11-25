using System;
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
            //x .generateIndent()
            textWriter.Write("if (");
            IfCondition.WritePython(textWriter);
            textWriter.Write(") :\n");

            foreach (var instruction in IfInstructions)
            {
                // ident++
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                // ident--
            }

            foreach (var elif in Elif)
            {
                //x .generateIndent()
                textWriter.Write("elif");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in elif.Item2)
                {
                    // ident++
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    // ident--
                }
            }

            if (ElseInstructions != null && ElseInstructions.Count > 0)
            {
                //x .generateIndent()
                textWriter.Write("else (");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in ElseInstructions)
                {
                    // ident++
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    // ident--
                }
            }
        }

        #endregion Public Properties
    }
}