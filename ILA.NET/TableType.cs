using System;
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
        public Range()
        {
            Max = null;
            Min = null;
        }

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
            Min.WritePython(textWriter);
            textWriter.Write(",");
            Max.WritePython(textWriter);
        }

        #endregion Public Constructors
    }

    /// <summary>
    /// A custom type represented by a table
    /// </summary>
    public class TableType : VarType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TableType()
        {
            DimensionsSize = new List<Range>();
            InternalType = null;
        }

        #region Public Properties

        /// <summary>
        /// The size of each dimension
        /// </summary>
        public virtual List<Range> DimensionsSize { get; set; }

        /// <summary>
        /// The type of the elements
        /// </summary>
        public virtual VarType InternalType { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}