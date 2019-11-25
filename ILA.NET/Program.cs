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

        internal static int ilaIndent;
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

        /// <summary>
        /// The comments at the beggining of the file
        /// </summary>
        public List<Comment> FileComments { get; set; }
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();

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

        public void WriteILA(TextWriter textWriter)
        {
            ilaIndent = 0;
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
            ilaIndent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            ilaIndent--;
            GenerateIndent(textWriter);
            textWriter.WriteLine('}');
        }

        public void WritePython(TextWriter textWriter)
        {
            /*
             * attention aux Methods, elles contiennent les instances de Print et Read
             * qui ne doivent pas être définies
             */
            throw new NotImplementedException();
        }

        internal static void GenerateIndent(TextWriter textWriter)
        {
            for (int i = 0; i < ilaIndent * IndentMultiplier; i++)
                textWriter.Write(' ');
        }

        #endregion Public Methods
    }
}