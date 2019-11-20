using System;
using System.Collections.Generic;
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
        string IBaseObject.LuaCode => LuaCode;
        public string Name { get; set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal string LuaCode => throw new NotImplementedException();
        internal string PythonCode => throw new NotImplementedException();

        #endregion Internal Properties

        #region Public Methods

        public string GenerateCode(Language language)
        {
            switch (language)
            {
                case Language.PYTHON:
                    return PythonCode;

                case Language.LUA:
                    return LuaCode;

                default:
                    return null;
            }
        }

        #endregion Public Methods
    }
}