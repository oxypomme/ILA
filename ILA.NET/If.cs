using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    internal class If : Instruction
    {
        #region Public Properties

        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public List<Tuple<IValue, List<Instruction>>> Elif { get; set; }
        public List<string> ElifComments { get; set; }
        public string ElseComment { get; set; }
        public IValue ElseCondition { get; set; }
        public List<Instruction> ElseInstructions { get; set; }
        public string EndComment { get; set; }
        public IValue IfCondition { get; set; }
        public List<Instruction> IfInstructions { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}