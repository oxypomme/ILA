using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class FunctionCall : IValue
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Function CalledFunction { get; set; }

        VarType IValue.Type => CalledFunction.ReturnType;

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(CalledFunction.Name);
            textWriter.Write('(');
            for (int i = 0; i < Args.Count; i++)
            {
                if (i > 0)
                    textWriter.Write(", ");
                Args[i].WriteILA(textWriter);
            }
            textWriter.Write(')');
        }

        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(CalledFunction.Name + "(");
            for (int i = 0; i < Args.Count; i++)
            {
                if (i != 0)
                    textWriter.Write(", ");
                Args[i].WritePython(textWriter);
            }
            textWriter.Write(")");
        }

        #endregion Public Properties
    }
}