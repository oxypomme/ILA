using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An instruction that calls a module
    /// </summary>
    public class ModuleCall : Instruction
    {
        #region Public Properties

        /// <summary>
        /// The arguments given to the module
        /// </summary>
        public List<IValue> Args { get; set; }

        /// <summary>
        /// The called module/function
        /// </summary>
        public Module CalledModule { get; set; }

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
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

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            if (!(CalledModule is Print || CalledModule is Read))
            {
                int outParameters = 0;

                Program.GenerateIndent(textWriter);
                for (int i = 0; i < Args.Count; i++)
                {
                    if (CalledModule.Parameters.Count > 0 && (CalledModule.Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0)
                    {
                        if (Args[i] != null)
                        {
                            if (outParameters != 0)
                                textWriter.Write(", ");
                            outParameters++;
                            Args[i].WritePython(textWriter);
                        }
                    }
                }

                if (outParameters != 0)
                    textWriter.Write(" = ");
                if (!(CalledModule is Prev || CalledModule is Next))
                {
                    for (int i = 0; i < Args.Count; i++)
                    {
                        if (Args[i] != null)
                        {
                            if (i != 0)
                                textWriter.Write(", ");

                            Args[i].WritePython(textWriter);
                        }
                    }
                    textWriter.Write(CalledModule.Name + "(");
                }
                else
                {
                    throw new NotImplementedException();
                }

                textWriter.Write(")\n");
            }
            else if (CalledModule is Print)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("print(");
                for (int i = 0; i < Args.Count; i++)
                {
                    if (i != 0)
                        textWriter.Write(", ");
                    Args[i].WritePython(textWriter);
                }
                textWriter.Write(")\n");
            }
            else
            {
                Program.GenerateIndent(textWriter);
                for (int i = 0; i < Args.Count; i++)
                {
                    if (!(Args[i].Type == GenericType.String))
                    {
                        Args[i].WritePython(textWriter);
                        textWriter.Write(" = ");
                        textWriter.Write(Args[i].Type.Name + "(");
                        textWriter.Write("input()");
                        textWriter.Write(")\n");
                    }
                    else
                    {
                        Args[i].WritePython(textWriter);
                        textWriter.Write(" = input()\n");
                    }
                }
            }
        }

        #endregion Public Properties
    }
}