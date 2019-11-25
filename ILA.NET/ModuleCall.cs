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

        public void WritePython(TextWriter textWriter)
        {
            if (!(CalledModule is Read || CalledModule is Print))
            {
                //x .generateIdent()
                for (int i = 0; i < Args.Count; i++)
                {
                    if ((CalledModule.Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0)
                    {
                        if (i != 0)
                            textWriter.Write(", ");
                        Args[i].WritePython(textWriter);
                    }
                }

                textWriter.Write(" = ");
                CalledModule.WritePython(textWriter);
                textWriter.Write("(");
                for (int i = 0; i < Args.Count; i++)
                {
                    if (Args[i] != null)
                    {
                        if (i != 0)
                            textWriter.Write(", ");

                        Args[i].WritePython(textWriter);
                    }
                }

                textWriter.Write(")");
            }
        }

        #endregion Public Properties
    }
}