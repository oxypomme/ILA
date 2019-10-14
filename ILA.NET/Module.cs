using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        IDeclaration[] IExecutable.Declarations => Declarations;
        Instruction[] IExecutable.Instructions => Instructions;
        string IBaseObject.LuaCode => LuaCode;
        public Parameter[] Parameters { get; internal set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal IDeclaration[] Declarations { get; set; }
        internal Instruction[] Instructions { get; set; }
        internal virtual string LuaCode => throw new NotImplementedException();
        internal virtual string PythonCode => throw new NotImplementedException();

        #endregion Internal Properties
    }
}