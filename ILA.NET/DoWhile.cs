using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class DoWhile : Instruction
    {
        #region Public Properties

        public IValue Condition { get; set; }
        public List<Instruction> Instructions { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode => throw new NotImplementedException();
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}