using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class While : Instruction
    {
        #region Public Properties

        public IValue Condition { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            var sbWHILE = new StringBuilder().Append(
                    "while (" + Condition.PythonCode + ") :\n"
                );
            foreach (Instruction instruction in Instructions)
            {
                sbWHILE.Append(instruction.PythonCode + "\n");
            }
            return sbWHILE.ToString();
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}