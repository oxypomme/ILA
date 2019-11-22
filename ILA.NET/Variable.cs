using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Variable : IValue
    {
        #region Public Properties

        public virtual bool Constant { get; set; }
        public virtual IValue ConstantValue { get; set; }
        public virtual string Name { get; set; }
        VarType IValue.Type => Type;

        #endregion Public Properties

        #region Internal Properties

        public virtual VarType Type { get; set; }

        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Properties
    }
}