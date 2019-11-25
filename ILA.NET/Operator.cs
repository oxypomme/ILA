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
        public Tag OperatorType { get; set; }
        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write('(');
            if (Left != null)
                Left.WriteILA(textWriter);
            switch (OperatorType)
            {
                case Tag.ADD:
                    textWriter.Write(" + ");
                    break;

                case Tag.MINUS:
                    textWriter.Write("-");
                    break;

                case Tag.SUB:
                    textWriter.Write(" - ");
                    break;

                case Tag.DIV:
                    textWriter.Write(" / ");
                    break;

                case Tag.MULT:
                    textWriter.Write(" * ");
                    break;

                case Tag.INT_DIV:
                    textWriter.Write(" div ");
                    break;

                case Tag.MOD:
                    textWriter.Write(" mod ");
                    break;

                case Tag.AND:
                    textWriter.Write(" et ");
                    break;

                case Tag.OR:
                    textWriter.Write(" ou ");
                    break;

                case Tag.NOT:
                    textWriter.Write("non ");
                    break;

                case Tag.EQUAL:
                    textWriter.Write(" = ");
                    break;

                case Tag.DIFFRENT:
                    textWriter.Write(" != ");
                    break;

                case Tag.BIGGER:
                    textWriter.Write(" > ");
                    break;

                case Tag.BIGGER_EQUAL:
                    textWriter.Write(" >= ");
                    break;

                case Tag.SMALLER:
                    textWriter.Write(" < ");
                    break;

                case Tag.SMALLER_EQUAL:
                    textWriter.Write(" <= ");
                    break;
            }
            Right.WriteILA(textWriter);
            textWriter.Write(')');
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}