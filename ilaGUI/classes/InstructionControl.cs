using System;
using System.Collections.Generic;
using System.Text;

namespace ilaGUI.Classes
{
    internal interface InstructionControl
    {
        ILANET.Instruction ManagedInstruction { get; }
    }
}