using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Assign : Instruction
    {
        #region Public Properties

        public Variable Left { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public IValue Right { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}