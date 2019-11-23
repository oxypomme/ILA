﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The assign instruction sets a variable to a given value
    /// </summary>
    public class Assign : Instruction
    {
        #region Public Properties

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// Left operand, the variable
        /// </summary>
        public Variable Left { get; set; }

        /// <summary>
        /// The right operand, the value to assign to
        /// </summary>
        public IValue Right { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}