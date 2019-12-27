using System;
using System.Collections.Generic;
using System.Text;

namespace ilaGUI
{
    public interface InstructionBlock
    {
        Editor.DummyInstruction EndInsturction { get; }
        IDropableInstruction[] Instructions { get; }

        void UpdateInternalInstructions();
    }
}