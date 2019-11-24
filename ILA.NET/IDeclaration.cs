using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IDeclaration : IBaseObject
    {
        public Comment AboveComment { get; set; }
        public string Comment { get; set; }
    }
}