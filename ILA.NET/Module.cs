using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        public Comment AboveComment { get; set; }
        Comment IExecutable.AboveComment => AboveComment;
        public List<IDeclaration> Declarations { get; set; }
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        public string InlineComment { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        #region Internal Properties

        string IExecutable.Comment => InlineComment;
        internal List<Instruction> Instructions { get; set; }

        public virtual void WritePython(TextWriter textWriter)
        {
            // Def the module
            textWriter.Write("def ");
            textWriter.Write(Name);
            textWriter.Write(" (");
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (i != 0)
                    textWriter.Write(", ");
                Parameters[i].WritePython(textWriter);
            }
            textWriter.Write("):\n");

            // write the input-output / output params
            foreach (var parameter in Parameters)
            {
                // ident++
                //x .generateIdent()
                if ((parameter.Mode & Parameter.Flags.OUTPUT) != 0)
                {
                    parameter.WritePython(textWriter);
                    textWriter.Write(" = 0\n");
                }
                // ident--
            }

            // write the instructions
            foreach (var instruction in Instructions)
            {
                // ident++
                instruction.WritePython(textWriter);
                // ident--
            }

            // return out vars
            // ident++
            //x .generateIdent()
            textWriter.Write("return ");
            for (int i = 0; i < Parameters.Count; i++)
            {
                if ((Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0)
                {
                    if (i != 0)
                        textWriter.Write(", ");
                    Parameters[i].WritePython(textWriter);
                }
            }
            // ident--
        }

        #endregion Internal Properties
    }
}