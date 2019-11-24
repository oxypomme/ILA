using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    internal class If : Instruction
    {
        #region Public Properties

        //List<...> => Liste de else, IValue => Condition, List<Instruction> => Bloc
        public List<Tuple<IValue, List<Instruction>>> Elif { get; set; }

        public List<Instruction> ElseInstructions { get; set; }
        public IValue IfCondition { get; set; }
        public List<Instruction> IfInstructions { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            //x .generateIndent()
            textWriter.Write("if (");
            IfCondition.WritePython(textWriter);
            textWriter.Write(") :\n");

            foreach (var instruction in IfInstructions)
            {
                // ident++
                //x .generateIndent()
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
                    //x .generateIndent()
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    // ident--
                }
            }

            //x .generateIndent()
            textWriter.Write("else (");
            IfCondition.WritePython(textWriter);
            textWriter.Write(") :\n");

            foreach (var instruction in ElseInstructions)
            {
                // ident++
                //x .generateIndent()
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                // ident--
            }
        }

        #endregion Public Properties
    }
}