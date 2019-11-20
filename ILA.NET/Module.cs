using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        public Comment Comment { get; set; }
        Comment IExecutable.Comment => Comment;
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        string IBaseObject.LuaCode => LuaCode;
        public List<Parameter> Parameters { get; set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal List<Instruction> Instructions { get; set; }
        internal virtual string LuaCode => throw new NotImplementedException();
        internal virtual string PythonCode => throw new NotImplementedException();

        #endregion Internal Properties
    }
}