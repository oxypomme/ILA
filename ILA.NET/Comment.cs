using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Comment : Instruction
    {
        #region Public Properties

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();

        public bool MultiLine { get; set; }

        public string Message { get; set; }
        string Instruction.Comment => Message;

        #endregion Public Properties
    }
}
