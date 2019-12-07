using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ILANET
{
    /// <summary>
    /// An instruction that calls a function
    /// </summary>
    public class FunctionCall : IValue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FunctionCall()
        {
            Args = new List<IValue>();
            CalledFunction = null;
        }

        #region Public Properties

        /// <summary>
        /// The arguments to pass to the function
        /// </summary>
        public List<IValue> Args { get; set; }

        /// <summary>
        /// The called function
        /// </summary>
        public Function CalledFunction { get; set; }

        public VarType Type
        {
            get
            {
                if (CalledFunction is Next || CalledFunction is Prev)
                    return Args.First().Type;
                else
                    return CalledFunction.ReturnType;
            }
        }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(CalledFunction.Name);
            textWriter.Write('(');
            for (int i = 0; i < Args.Count; i++)
            {
                if (i > 0)
                    textWriter.Write(", ");
                Args[i].WriteILA(textWriter);
            }
            textWriter.Write(')');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            if (!(CalledFunction is Prev || CalledFunction is Next))
            {
                textWriter.Write(CalledFunction.Name + "(");
                for (int i = 0; i < Args.Count; i++)
                {
                    if (i != 0)
                        textWriter.Write(", ");
                    Args[i].WritePython(textWriter);
                }
                textWriter.Write(")");
            }
            else
            {
                foreach (var parameter in Args)
                {
                    if (parameter is ConstantChar)
                    {
                        textWriter.Write("chr(ord(");
                        parameter.WritePython(textWriter);
                        textWriter.Write(")" + (CalledFunction is Prev ? "-" : "+") + "1)");
                    }
                    else if (parameter is ConstantBool)
                    {
                        if (CalledFunction is Prev)
                            textWriter.Write("False");
                        else if (CalledFunction is Next)
                            textWriter.Write("True");
                    }
                    else if (parameter is EnumType)
                    {
                        if (CalledFunction is Prev)
                            parameter.WritePython(textWriter);
                        // parameter.Index--;
                        if (CalledFunction is Next)
                            parameter.WritePython(textWriter);
                        // parameter.Index++;
                    }
                    else
                    {
                        parameter.WritePython(textWriter);
                        textWriter.Write((CalledFunction is Prev ? "-" : "+") + "1");
                    }
                }
            }
        }

        #endregion Public Properties
    }
}