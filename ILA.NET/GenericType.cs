using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Generic types are the native one, bool char float int string
    /// </summary>
    public interface IGenericType : VarType, Native
    {
    }

    public sealed class GenericType : IGenericType
    {
        #region Public Fields

        /// <summary>
        /// The bool type
        /// </summary>
        public static readonly IGenericType Bool = new GenericType(Flags.BOOL) { name = "bool" };

        /// <summary>
        /// The char type
        /// </summary>
        public static readonly IGenericType Char = new GenericType(Flags.CHAR) { name = "char" };

        /// <summary>
        /// The float type
        /// </summary>
        public static readonly IGenericType Float = new GenericType(Flags.FLOAT) { name = "float" };

        /// <summary>
        /// The int type
        /// </summary>
        public static readonly IGenericType Int = new GenericType(Flags.INT) { name = "int" };

        /// <summary>
        /// The string type
        /// </summary>
        public static readonly IGenericType String = new StringType();

        #endregion Public Fields

        #region Private Constructors

        internal string name;

        private GenericType(Flags type) => Type = type;

        /// <summary>
        /// The name of the type.
        /// </summary>
        public string Name { get => name; set => throw new InvalidOperationException("Unable to change the name of a generic type."); }

        #endregion Private Constructors

        #region Public Enums

        private enum Flags
        {
            INT,
            FLOAT,
            CHAR,
            BOOL
        }

        #endregion Public Enums

        #region Private Properties

        private Flags Type { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            switch (Type)
            {
                case Flags.BOOL:
                    textWriter.Write("booleen");
                    break;

                case Flags.INT:
                    textWriter.Write("entier");
                    break;

                case Flags.FLOAT:
                    textWriter.Write("reel");
                    break;

                case Flags.CHAR:
                    textWriter.Write("caractere");
                    break;
            }
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        { }

        #endregion Private Properties
    }

    internal sealed class StringType : TableType, IGenericType
    {
        private List<Range> dimensionsSize;
        private bool init = false;

        internal StringType()
        {
            dimensionsSize = new List<Range>()
            {
                new Range(new ConstantInt(){ Value = 1}, null)
            };
            base.Name = "string";
            init = true;
        }

        public override IList<Range> DimensionsSize { get => dimensionsSize; set { if (init) throw new InvalidOperationException("Unable to change the dimension of a string."); } }
        public override VarType InternalType { get => GenericType.Char as GenericType; set { if (init) throw new InvalidOperationException("Unable to change the internal type of a string."); } }
        public override string Name { get => base.Name; set => throw new InvalidOperationException("Unable to change the name of a generic type."); }

        public override void WriteILA(TextWriter textWriter)
        {
            textWriter.Write("chaine");
        }

        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }
    }
}