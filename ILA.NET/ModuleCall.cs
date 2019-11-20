using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Module CalledModule { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}