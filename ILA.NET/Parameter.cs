﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Parameter of an executable block
    /// </summary>
    public class Parameter : IBaseObject
    {
        #region Public Enums

        /// <summary>
        /// Available parameter mode
        /// </summary>
        public enum Flags
        {
            /// <summary>
            /// The variable has to be defined
            /// </summary>
            INPUT = 0x1 << 0,

            /// <summary>
            /// The variable given can be modified in the caller block
            /// </summary>
            OUTPUT = 0x1 << 1,

            /// <summary>
            /// The variable has to be defined and can be modified in th caller block
            /// </summary>
            IO = INPUT | OUTPUT
        }

        #endregion Public Enums

        #region Public Properties

        /// <summary>
        /// The variable of the parameter
        /// </summary>
        public Variable ImportedVariable { get; set; }

        /// <summary>
        /// The way the variable is imported. INPUT by default
        /// </summary>
        public Flags Mode { get; set; }

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