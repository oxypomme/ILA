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

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}