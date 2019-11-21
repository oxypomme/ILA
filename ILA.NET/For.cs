﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class For : Instruction
    {
        #region Public Properties

        public IValue End { get; set; }
        public Variable Index { get; set; }
        public List<Instruction> Instructions { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode
        {
            get
            {
                var sbFOR = new StringBuilder().Append(Index.PythonCode + " = 0\n")
                .Append("while ")
                .Append(Index.PythonCode).Append(" > ").Append(Start.PythonCode)
                .Append(" and ")
                .Append(Index.PythonCode).Append(" < ").Append(End.PythonCode)
                .Append(" :\n");
                foreach (Instruction instruction in Instructions)
                {
                    sbFOR.Append(instruction.PythonCode + "\n");
                }
                return sbFOR.Append(Index.PythonCode +
                    " = " + Index.PythonCode + " + " + Step.PythonCode)
                    .ToString();
            }
        }

        public IValue Start { get; set; }
        public IValue Step { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties
    }
}