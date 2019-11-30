using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A switch instruction
    /// </summary>
    public class Switch : Instruction
    {
        /// <summary>
        /// Each case : a list of value to be equal, a block of instructions, and an eventual comment
        /// </summary>
        public List<Tuple<List<IValue>, List<Instruction>>> Cases { get; set; }

        /// <summary>
        /// Integrated comment at start
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Default instructions
        /// </summary>
        public List<Instruction> Default { get; set; }

        /// <summary>
        /// Integrated comment at the end
        /// </summary>
        public string EndComment { get; set; }

        /// <summary>
        /// Comment after the default
        /// </summary>
        public IValue Value { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("cas ");
            Value.WriteILA(textWriter);
            textWriter.Write(" parmi");
            if (Comment != null && Comment.Length > 0)
                textWriter.Write(" //" + Comment);
            textWriter.WriteLine();
            foreach (var item in Cases)
            {
                Program.GenerateIndent(textWriter);
                for (int i = 0; i < item.Item1.Count; i++)
                {
                    if (i > 0)
                        textWriter.Write(", ");
                    item.Item1[i].WriteILA(textWriter);
                }
                textWriter.Write(" :\n");
                Program.Indent++;
                for (int i = 0; i < item.Item2.Count; i++)
                    item.Item2[i].WriteILA(textWriter);
                Program.Indent--;
            }
            if (Default != null && Default.Count > 0)
            {
                Program.GenerateIndent(textWriter);
                textWriter.WriteLine("defaut:");
                Program.Indent++;
                for (int i = 0; i < Default.Count; i++)
                    Default[i].WriteILA(textWriter);
                Program.Indent--;
            }
            Program.GenerateIndent(textWriter);
            textWriter.Write("fcas");
            if (EndComment != null && EndComment.Length > 0)
                textWriter.Write(" //" + EndComment);
            textWriter.WriteLine();
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            for (int i = 0; i < Cases.Count; i++)
            {
                if (Default.Count > 0)
                {
                    Program.GenerateIndent(textWriter);
                    if (i > 0)
                        textWriter.Write("else:\n");
                    else
                        textWriter.Write("if True :\n");
                    foreach (var instruction in Default)
                    {
                        Program.Indent++;
                        instruction.WritePython(textWriter);
                        textWriter.Write("\n");
                        Program.Indent--;
                    }
                }
                else if (i == 0)
                {
                    Program.GenerateIndent(textWriter);
                    textWriter.Write("if (");
                    for (int j = 0; i < Cases[i].Item1.Count; j++)
                    {
                        if (i != 0)
                            textWriter.Write(" || ");
                        Value.WritePython(textWriter);
                        textWriter.Write("==");
                        Cases[i].Item1[j].WritePython(textWriter);
                    }
                    textWriter.Write(") :\n");
                }
                else
                {
                    Program.GenerateIndent(textWriter);
                    textWriter.Write("elif (");
                    Value.WritePython(textWriter);
                    textWriter.Write("==");
                    Cases[i].Item1[i].WritePython(textWriter);
                    textWriter.Write(") :\n");
                }

                foreach (var instruction in Cases[i].Item2)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
        }
    }
}