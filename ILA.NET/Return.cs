using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Return : Instruction
    {
        public IValue Type { get; set; }
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode => throw new NotImplementedException();
    }
}
