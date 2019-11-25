using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IBaseObject
    {
        public void WritePython(System.IO.TextWriter textWriter);
    }
}