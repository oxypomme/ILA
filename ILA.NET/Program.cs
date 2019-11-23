using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Program : IExecutable
    {
        #region Public Enums

        public enum Language
        {
            PYTHON,
            LUA
        }

        #endregion Public Enums

        #region Public Properties
        public string InlineComment { get; set; }
        public Comment AlgoComment { get; set; }
        Comment IExecutable.AboveComment => AlgoComment;
        public List<IDeclaration> Declarations { get; set; }
        public List<Comment> FileComments { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public List<Instruction> Instructions { get; set; }
        public List<Module> Methods { get; set; }
        public string Name { get; set; }

        string IExecutable.Comment => InlineComment;

        #endregion Public Properties


        #region Public Methods

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}