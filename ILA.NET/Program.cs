using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Program : IExecutable
    {
        #region Public Properties

        internal static int ilaIndent;
        Comment IExecutable.AboveComment => AlgoComment;
        public Comment AlgoComment { get; set; }
        string IExecutable.Comment => InlineComment;
        public List<IDeclaration> Declarations { get; set; }
        public List<Comment> FileComments { get; set; }
        public string InlineComment { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public List<Instruction> Instructions { get; set; }
        public List<Module> Methods { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void WriteILA(TextWriter textWriter)
        {
            ilaIndent = 0;
        }

        public void WritePython(TextWriter textWriter)
        {
            /*
             * attention aux Methods, elles contiennent les instances de Print et Read
             * qui ne doivent pas être définies
             */
            throw new NotImplementedException();
        }

        internal static void GenerateIndent(TextWriter textWriter, int spaces = 4)
        {
            for (int i = 0; i < ilaIndent * spaces; i++)
                textWriter.Write(' ');
        }

        #endregion Public Methods
    }
}