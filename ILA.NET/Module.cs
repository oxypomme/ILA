using System;
using System.Collections.Generic;
using System.IO;

namespace ILANET
{
    /// <summary>
    /// A module is a block of code that can be called from any executable
    /// </summary>
    public class Module : IExecutable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Module()
        {
            AboveComment = null;
            Declarations = new List<VariableDeclaration>();
            InlineComment = "";
            Name = "";
            Parameters = new List<Parameter>();
            Instructions = new List<Instruction>();
        }

        #region Public Properties

        /// <summary>
        /// The comment above its declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        Comment IExecutable.AboveComment => AboveComment;

        /// <summary>
        /// The variable declarations of this module
        /// </summary>
        public List<VariableDeclaration> Declarations { get; set; }

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        Instruction[] IExecutable.Instructions => Instructions.ToArray();

        /// <summary>
        /// The name to call to execute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameters required to execute
        /// </summary>
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        string IExecutable.Comment => InlineComment;
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();

        /// <summary>
        /// The instructions of the module
        /// </summary>
        public List<Instruction> Instructions { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
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

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
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

            foreach (var declaration in Declarations)
            {
                Program.Indent++;
                Program.GenerateIndent(textWriter);
                declaration.WritePython(textWriter);
                textWriter.Write(" = 0\n");
                Program.Indent--;
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
                        if (i < Parameters.Count - 1)
                            textWriter.Write(", ");
                    }
                }
                textWriter.Write("\n");
                Program.Indent--;
            }
        }
    }
}