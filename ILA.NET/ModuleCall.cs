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
                Program.GenerateIndent(textWriter);
                for (int i = 0; i < Args.Count; i++)
                {
                    if ((CalledModule.Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0)
                    {
                        if (Args[i] != null)
                        {
                            if (i != 0)
                                textWriter.Write(", ");
                            Args[i].WritePython(textWriter);
                        }
                    }
                }

                textWriter.Write(" = ");
                textWriter.Write(CalledModule.Name + "(");
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