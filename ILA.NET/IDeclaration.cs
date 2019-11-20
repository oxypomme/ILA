using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IDeclaration : IBaseObject
    {
        public string Comment { get; }
    }
}