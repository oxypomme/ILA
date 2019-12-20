using System;
using System.Collections.Generic;
using System.Text;

namespace ilaGUI.Classes
{
    internal interface ValueControl
    {
        ILANET.IValue ManagedValue { get; }
    }
}