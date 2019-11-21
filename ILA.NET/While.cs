using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class While : Instruction
    {
        #region Public Properties

        public IValue Condition { get; set; }
        public List<Instruction> Instructions { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode
        {
            get
            {
                var sbWHILE = new StringBuilder().Append("while (");
                sbWHILE.Append(Condition.PythonCode).Append(") :");
                foreach (Instruction instruction in Instructions)
                {
                    sbWHILE.Append(instruction.PythonCode + "\n");
                }
                return sbWHILE.ToString();
            }
        }

        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}