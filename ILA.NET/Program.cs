using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Class representing the entire file and the main algorithm
    /// </summary>
    public partial class Program : IExecutable
    {
        #region Public Properties

        internal static int Indent;
        internal static int IndentMultiplier = 4;
        Comment IExecutable.AboveComment => AlgoComment;

        /// <summary>
        /// The comment block above the algorithm declaration
        /// </summary>
        public Comment AlgoComment { get; set; }

        string IExecutable.Comment => InlineComment;

        /// <summary>
        /// The declarations of the algorithm
        /// </summary>
        public List<IDeclaration> Declarations { get; set; }

        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();

        /// <summary>
        /// The comments at the beggining of the file
        /// </summary>
        public List<Comment> FileComments { get; set; }

        /// <summary>
        /// The integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        Instruction[] IExecutable.Instructions => Instructions.ToArray();

        /// <summary>
        /// The block of instructions
        /// </summary>
        public List<Instruction> Instructions { get; set; }

        /// <summary>
        /// The declared methods
        /// </summary>
        public List<Module> Methods { get; set; }

        /// <summary>
        /// The name of the algorithm
        /// </summary>
        public string Name { get; set; }

        string IExecutable.Name => Name;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Indent = 0;
            foreach (var item in FileComments)
                item.WriteILA(textWriter);
            foreach (var item in Declarations)
                item.WriteILA(textWriter);
            foreach (var item in Methods)
                item.WriteILA(textWriter);
            AlgoComment?.WriteILA(textWriter);
            GenerateIndent(textWriter);
            textWriter.Write("algo ");
            textWriter.Write(Name);
            if (InlineComment != null && InlineComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(InlineComment);
            }
            textWriter.WriteLine();
            GenerateIndent(textWriter);
            textWriter.WriteLine('{');
            Indent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Indent--;
            GenerateIndent(textWriter);
            textWriter.WriteLine('}');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            Indent = 0;

            foreach (var declaration in Declarations)
            {
                declaration.WritePython(textWriter);
            }

            foreach (var module in Methods)
            {
                if (!(module is Read || module is Print))
                    module.WritePython(textWriter);
            }

            foreach (var instruction in Instructions)
            {
                instruction.WritePython(textWriter);
            }
        }

        internal static void GenerateIndent(TextWriter textWriter)
        {
            for (int i = 0; i < Indent * IndentMultiplier; i++)
                textWriter.Write(' ');
        }

        #endregion Public Methods
    }
}