using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IDeclaration : IBaseObject
    {
        public Comment AboveComment { get; }
        public string Comment { get; }
    }
}