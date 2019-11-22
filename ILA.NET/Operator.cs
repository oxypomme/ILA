using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Operator : IValue
    {
        #region Public Enums

        public enum Tag
        {
            /// <summary>
            /// the MINUS operator has only a right value and no left value.
            /// </summary>
            MINUS,
            ADD,
            SUB,
            DIV,
            MULT,
            INT_DIV,
            MOD,
            AND,
            OR,

            /// <summary>
            /// the NOT operator has only a right value and no left value.
            /// </summary>
            NOT,

            EQUAL,
            DIFFRENT,
            BIGGER,
            BIGGER_EQUAL,
            SMALLER,
            SMALLER_EQUAL
        }

        #endregion Public Enums

        #region Public Properties

        public IValue Left { get; set; }
        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}