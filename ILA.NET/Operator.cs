using System;

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

        public Tag OpTag { get; set; }
        public IValue Left { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode
        {
            get
            {
                switch (OpTag)
                {
                    case Tag.MINUS:
                        return "-" + Right.PythonCode;

                    case Tag.ADD:
                        return "(" + Left.PythonCode + " + " + Right.PythonCode + ")";

                    case Tag.SUB:
                        return "(" + Left.PythonCode + " - " + Right.PythonCode + ")";

                    case Tag.DIV:
                        return "(" + Left.PythonCode + " / " + Right.PythonCode + ")";

                    case Tag.MULT:
                        return "(" + Left.PythonCode + " * " + Right.PythonCode + ")";

                    case Tag.INT_DIV:
                        return "(" + Left.PythonCode + " // " + Right.PythonCode + ")";

                    case Tag.MOD:
                        return "(" + Left.PythonCode + " % " + Right.PythonCode + ")";

                    case Tag.AND:
                        return "(" + Left.PythonCode + " and " + Right.PythonCode + ")";

                    case Tag.OR:
                        return "(" + Left.PythonCode + " or " + Right.PythonCode + ")";

                    case Tag.NOT:
                        return "not " + Right.PythonCode;

                    case Tag.EQUAL:
                        return "(" + Left.PythonCode + " == " + Right.PythonCode + ")";

                    case Tag.DIFFRENT:
                        return "(" + Left.PythonCode + " != " + Right.PythonCode + ")";

                    case Tag.BIGGER:
                        return "(" + Left.PythonCode + " > " + Right.PythonCode + ")";

                    case Tag.BIGGER_EQUAL:
                        return "(" + Left.PythonCode + " >= " + Right.PythonCode + ")";

                    case Tag.SMALLER:
                        return "(" + Left.PythonCode + " < " + Right.PythonCode + ")";

                    case Tag.SMALLER_EQUAL:
                        return "(" + Left.PythonCode + " <= " + Right.PythonCode + ")";

                    default:
                        throw new Exception();
                }
            }
        }

        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }
    }
}