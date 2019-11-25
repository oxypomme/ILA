using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Module CalledModule { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write(CalledModule.Name);
            textWriter.Write('(');
            for (int i = 0; i < Args.Count; i++)
            {
                if (Args[i] != null)
                {
                    if (i > 0)
                        textWriter.Write(", ");
                    Args[i].WriteILA(textWriter);
                }
            }
            textWriter.Write(')');
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}