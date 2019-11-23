using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Module CalledModule { get; set; }

        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            var stringBuilder = new StringBuilder().Append(CalledModule.PythonCode + "(");
            for (int i = 0; i < Args.Count; i++)
                stringBuilder.Append(Args[i].PythonCode);
            return stringBuilder.Append(")").ToString();
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}