using System;
using System.Collections.Generic;
using System.Text;

namespace ILA
{
    internal class Module : IInstruction
    {
        #region Public Constructors

        public Module()
        {
            Instructions = new List<IInstruction>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IInstruction> Instructions { get; set; }
        public string Name { get; set; }
        public List<Variable> Variables { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute(IEnumerable<IValue> args)
        {
            var available
        }

        public void Execute()
        {
            foreach (var instruction in Instructions)
                instruction.Execute();
        }

        #endregion Public Methods

        #region Public Classes

        public class Argument
        {
            #region Public Enums

            public enum Mode
            {
                INPUT = 1,
                OUTPUT = 2
            }

            #endregion Public Enums

            #region Public Properties

            public Mode Access { get; set; }

            #endregion Public Properties

            public
        }

        #endregion Public Classes
    }
}