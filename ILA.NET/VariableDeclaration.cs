using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class VariableDeclaration : IDeclaration
    {
        #region Public Properties

        public Variable CreatedVariable { get; internal set; }

        #endregion Public Properties

        #region Internal Properties

        string IBaseObject.PythonCode => PythonCode;
        string IBaseObject.LuaCode => LuaCode;

        #endregion Internal Properties

        internal string LuaCode => throw new NotImplementedException();

        internal string PythonCode => throw new NotImplementedException();
        public string Comment { get; set; }
        string IDeclaration.Comment => Comment;
    }
}