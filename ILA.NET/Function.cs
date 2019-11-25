using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A function, a module that returns something
    /// </summary>
    public class Function : Module
    {
        #region Public Properties

        /// <summary>
        /// Type of the returned value. null if it is any type.
        /// </summary>
        public VarType ReturnType { get; set; }

        public override void WriteILA(TextWriter textWriter)
        {
            AboveComment?.WriteILA(textWriter);
            Program.GenerateIndent(textWriter);
            textWriter.Write("fonction ");
            textWriter.Write(Name);
            textWriter.Write('(');
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                    textWriter.Write(", ");
                Parameters[i].WriteILA(textWriter);
            }
            textWriter.Write("):");
            ReturnType.WriteILA(textWriter);
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

        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}