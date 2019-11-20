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

        string IBaseObject.PythonCode
        {
            get
            {
                var stringBuilder = new StringBuilder().Append(CalledModule.PythonCode + "(");
                for (int i = 0; i < Args.Count; i++) { stringBuilder.Append(Args[i].PythonCode); }
                return stringBuilder.Append(")").ToString();
            }
        }

        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}