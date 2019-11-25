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
        #region Public Properties

        /// <summary>
        /// The comment above its declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        Comment IExecutable.AboveComment => AboveComment;

        /// <summary>
        /// The variable declarations of this module
        /// </summary>
        public List<IDeclaration> Declarations { get; set; }

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
            Program.ilaIndent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
            Program.GenerateIndent(textWriter);
            textWriter.WriteLine('}');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}