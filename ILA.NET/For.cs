using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class For : Instruction
    {
        #region Public Properties

        public IValue End { get; set; }
        public Variable Index { get; set; }
        public List<Instruction> Instructions { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public IValue Start { get; set; }
        public IValue Step { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}