using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Assign : Instruction
    {
        #region Public Properties

        public Variable Left { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public IValue Right { get; internal set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}