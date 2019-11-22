﻿using System;
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

        public Comment AlgoComment { get; set; }
        Comment IExecutable.Comment => AlgoComment;
        public List<IDeclaration> Declarations { get; set; }
        public List<Comment> FileComments { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public List<Instruction> Instructions { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Internal Properties


        #endregion Internal Properties

        #region Public Methods

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}