using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    internal class If : Instruction
    {
        #region Public Properties

        public Tuple<IValue, Instruction[]>[] Elif { get; internal set; }
        public IValue ElseCondition { get; internal set; }
        public Instruction[] ElseInstructions { get; internal set; }
        public IValue IfCondition { get; internal set; }
        public Instruction[] IfInstructions { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode => throw new NotImplementedException();
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}