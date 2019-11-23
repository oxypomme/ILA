﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    internal class If : Instruction
    {
        #region Public Properties

        public List<Tuple<IValue, List<Instruction>>> Elif { get; set; }
        public IValue ElseCondition { get; set; }
        public List<Instruction> ElseInstructions { get; set; }
        public IValue IfCondition { get; set; }
        public List<Instruction> IfInstructions { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            var sbIF = new StringBuilder().Append(
                    "if (" + IfCondition.PythonCode + ") :\n"
                );
            foreach (Instruction instruction in IfInstructions)
            {
                sbIF.Append(instruction.PythonCode + "\n");
            }
            // TODO : elif + else
            return sbIF.ToString();
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}