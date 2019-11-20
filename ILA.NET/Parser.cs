using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILANET
{
    public partial class Program
    {
        #region Public Methods

        public static string CatchString(string str, ref int index)
        {
            var res = "";
            if (index >= str.Length || !char.IsLetter(str[index]))
                return res;

            while (index < str.Length && char.IsLetterOrDigit(str[index]))
            {
                res += str[index];
                index++;
            }
            return res;
        }

        public static int CountRow(string str, int index)
        {
            var row = 1;
            for (int i = 0; i < str.Length && i < index; i++)
                if (str[i] == '\n')
                    row++;
            return row;
        }

        public void Parse(string ilaCode)
        {
            Declarations = new List<IDeclaration>();
            Instructions = new List<Instruction>();
            FileComments = new List<Comment>();
            AlgoComment = null;
            Name = "";
            int index = 0;
            try
            {
                string lastComment = null;
                bool multilineComm = true;
                while (index < ilaCode.Length)
                {
                    //inline comment
                    if (ilaCode.Substring(index, 2) == "//")
                    {
                        index += 2;
                        string comment = "";
                        while (index < ilaCode.Length && ilaCode[index] != '\n')
                        {
                            comment += ilaCode[index];
                            index++;
                        }
                        index++;
                        if (lastComment != null)
                            FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                        lastComment = comment;
                        multilineComm = false;
                    }
                    //multiline comment
                    else if (ilaCode.Substring(index, 2) == "/*")
                    {
                        index += 2;
                        string comment = "";
                        while (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) != "*/")
                        {
                            comment += ilaCode[index];
                            index++;
                        }
                        if (index == ilaCode.Length - 1)
                            throw new ILAException("Commentaire multi ligne non terminé à la ligne " + CountRow(ilaCode, index));
                        index += 2;

                        if (lastComment != null)
                            FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                        lastComment = comment;
                        multilineComm = true;
                    }
                    //start of an algo
                    else if (ilaCode.Length - index > 5 && ilaCode.Substring(index, 5) == "algo ")
                    {
                        if (lastComment != null)
                            AlgoComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var algoName = CatchString(ilaCode, ref index);
                        if (algoName == "")
                            throw new ILAException("Nom d'algo invalide : ligne " + CountRow(ilaCode, index));
                        Name = algoName;
                        SkipLine(ilaCode, ref index, true);
                        if (ilaCode[index] != '{')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : {   ligne " + CountRow(ilaCode, index));
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        while (ilaCode[index] != '}')
                            Instructions.Add(ParseInstru(ilaCode, ref index));
                        index++;
                    }
                    //start of an module
                    else if (ilaCode.Length - index > 7 && ilaCode.Substring(index, 7) == "module ")
                    {
                        var module = new Module();
                        if (lastComment != null)
                            module.Comment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var moduleName = CatchString(ilaCode, ref index);
                        if (moduleName == "")
                            throw new ILAException("Nom de module invalide : ligne " + CountRow(ilaCode, index));
                        Name = moduleName;
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode[index] != '(')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : (   ligne " + CountRow(ilaCode, index));
                        index++;
                        string parametersStr = "";
                        while (ilaCode.Length > index && ilaCode[index] != ')')
                            parametersStr += ilaCode[index++];
                        {
                            var singleParamsStr = parametersStr.Split(',');
                            foreach (var item in singleParamsStr)
                            {
                                var parameter = new Parameter();
                                item.Trim();
                                var comps = item.Split("::");
                                if (comps.Length > 2)
                                    throw new ILAException("Trop de mode de paramètre : ligne " + CountRow(ilaCode, index));
                                if (comps.Length > 1)
                                {
                                    comps[0] = RemoveBlanks(comps.First());
                                    foreach (var item2 in comps.First())
                                    {
                                        switch (item2)
                                        {
                                            case 'e':
                                                parameter.Mode |= Parameter.Flags.INPUT;
                                                break;

                                            case 's':
                                                parameter.Mode |= Parameter.Flags.OUTPUT;
                                                break;

                                            default:
                                                throw new ILAException("Mode de paramètre '" + item2 + "' inconnu : ligne " + CountRow(ilaCode, index));
                                        }
                                    }
                                }
                                else
                                    parameter.Mode = Parameter.Flags.INPUT;
                                var comps2 = comps.Last().Split(':');
                                if (comps.Length != 2)
                                    throw new ILAException("Erreur de synthaxe ligne " + CountRow(ilaCode, index));
                                parameter.ImportedVariable = new Variable
                                {
                                    Constant = false,
                                    Name = comps.First()
                                };
                                {
                                }
                            }
                        }
                        SkipLine(ilaCode, ref index, true);
                        while (ilaCode[index] != '}')
                            Instructions.Add(ParseInstru(ilaCode, ref index));
                        index++;
                    }
                    SkipLine(ilaCode, ref index);
                }
                if (lastComment != null)
                    FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                lastComment = null;
            }
            catch (ILAException)
            {
            }
            catch (Exception)
            {
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal static Instruction ParseInstru(string code, ref int index)
        {
            return null;
        }

        internal static string RemoveBlanks(string str)
        {
            var res = "";
            foreach (var item in str)
                if (!char.IsWhiteSpace(item))
                    res += item;
            return res;
        }

        #endregion Internal Methods

        #region Public Classes

        public class ILAException : Exception
        {
            #region Public Constructors

            public ILAException(string mess = "") : base(mess)
            {
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }
}