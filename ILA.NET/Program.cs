using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Class representing the entire file and the main algorithm
    /// </summary>
    public class Program : IExecutable
    {
        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            /*
             * attention aux Methods, elles contiennent les instances de Print et Read
             * qui ne doivent pas être définies
             */
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}