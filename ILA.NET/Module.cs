using System;
using System.Collections.Generic;
using System.IO;

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

        string IExecutable.Comment => InlineComment;
        public List<Instruction> Instructions { get; set; }

        public virtual void WriteILA(TextWriter textWriter)
        {
            AboveComment?.WriteILA(textWriter);
            Program.GenerateIndent(textWriter);
            textWriter.Write("module ");
            textWriter.Write(Name);
            textWriter.Write('(');
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                    textWriter.Write(", ");
                Parameters[i].WriteILA(textWriter);
            }
            textWriter.Write(')');
            if (InlineComment != null && InlineComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(InlineComment);
            }
            textWriter.WriteLine();
            foreach (var item in Declarations)
                item.WriteILA(textWriter);
            Program.GenerateIndent(textWriter);
            textWriter.WriteLine('{');
            Program.Indent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.Indent--;
            Program.GenerateIndent(textWriter);
            textWriter.WriteLine('}');
        }

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
                if ((parameter.Mode & Parameter.Flags.OUTPUT) != 0)
                {
                    Program.Indent++;
                    Program.GenerateIndent(textWriter);
                    parameter.WritePython(textWriter);
                    textWriter.Write(" = 0\n");
                    Program.Indent--;
                }
            }

            // write the instructions
            foreach (var instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                Program.Indent--;
            }

            // return out vars
            if (!(Instructions[Instructions.Count - 1] is Return))
            {
                Program.Indent++;
                Program.GenerateIndent(textWriter);
                textWriter.Write("return ");
                for (int i = 0; i < Parameters.Count; i++)
                {
                    if ((Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0)
                    {
                        Parameters[i].WritePython(textWriter);
                        if (i < Parameters.Count)
                            textWriter.Write(", ");
                    }
                }
                textWriter.Write("\n");
                Program.Indent--;
            }
        }
    }
}