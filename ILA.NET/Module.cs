using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        string IBaseObject.LuaCode => LuaCode;
        public List<Parameter> Parameters { get; set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal List<IDeclaration> Declarations { get; set; }
        internal List<Instruction> Instructions { get; set; }
        internal virtual string LuaCode => throw new NotImplementedException();
        internal virtual string PythonCode => throw new NotImplementedException();
        public List<Comment> Comments { get; set; }
        Comment[] IExecutable.Comments => Comments.ToArray();

        #endregion Internal Properties
    }
}