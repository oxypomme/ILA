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

        IDeclaration[] IExecutable.Declarations => Declarations;
        Instruction[] IExecutable.Instructions => Instructions;
        string IBaseObject.LuaCode => LuaCode;
        public string Name { get; internal set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal IDeclaration[] Declarations { get; set; }
        internal Instruction[] Instructions { get; set; }
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