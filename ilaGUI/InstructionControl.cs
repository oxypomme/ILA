using System;
using System.Collections.Generic;
using System.Text;

namespace ilaGUI
{
    internal interface InstructionControl
    {
        ILANET.Instruction ManagedInstruction { get; }
    }
}