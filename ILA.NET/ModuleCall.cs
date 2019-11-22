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
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}