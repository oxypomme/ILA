﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IBaseObject
    {
        public void WriteILA(System.IO.TextWriter textWriter);

        public void WritePython(System.IO.TextWriter textWriter);
    }
}