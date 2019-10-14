using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Program : IExecutable
    {
        #region Public Properties

        IDeclaration[] IExecutable.Declarations => Declarations;
        Instruction[] IExecutable.Instructions => Instructions;
        string IBaseObject.LuaCode => throw new NotImplementedException();
        public string Name { get; internal set; }
        string IBaseObject.PythonCode => throw new NotImplementedException();

        #endregion Public Properties

        #region Internal Properties

        internal IDeclaration[] Declarations { get; set; }
        internal Instruction[] Instructions { get; set; }

        #endregion Internal Properties
    }
}