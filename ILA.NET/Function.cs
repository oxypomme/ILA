using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Function : Module
    {
        #region Public Properties

        public VarType ReturnType { get; set; }

        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}