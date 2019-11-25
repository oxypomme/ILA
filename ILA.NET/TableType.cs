﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Range of scalar a type
    /// </summary>
    public class Range : IBaseObject
    {
        #region Public Fields

        /// <summary>
        /// The minimum value
        /// </summary>
        public readonly IValue Max;

        /// <summary>
        /// The maximum value
        /// </summary>
        public readonly IValue Min;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        public Range(IValue min, IValue max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Min.WriteILA(textWriter);
            textWriter.Write("..");
            Max.WriteILA(textWriter);
        }

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// A custom type represented by a table
    /// </summary>
    public class TableType : VarType
    {
        #region Public Properties

        /// <summary>
        /// The size of each dimension
        /// </summary>
        public List<Range> DimensionsSize { get; internal set; }

        /// <summary>
        /// The type of the elements
        /// </summary>
        public VarType InternalType { get; internal set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}