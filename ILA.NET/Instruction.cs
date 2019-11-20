using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface Instruction : IBaseObject
    {
        public string Comment { get; }
    }
}