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
            Methods = new List<Module>();
            /*
             * The dispatcher will keep track of executables blocks to parse them at the end, once
             * all the variables, custom types and other executables header has been added.
             */
            var dispatcher = new Dictionary<int, IExecutable>();
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
                        index += 5;
                        if (lastComment != null)
                            AlgoComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var algoName = CatchString(ilaCode, ref index);
                        if (algoName == "")
                            throw new ILAException("Nom d'algo invalide : ligne " + CountRow(ilaCode, index));
                        Name = algoName;
                        SkipLine(ilaCode, ref index, true);
                        if (index == ilaCode.Length)
                            throw new ILAException("Aucun corps d'expression : ligne " + CountRow(ilaCode, index));
                        if (ilaCode[index] != '{')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : {   ligne " + CountRow(ilaCode, index));
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        dispatcher.Add(index, this);
                        {
                            int opened = 1;
                            while (opened > 0)
                            {
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ligne " + CountRow(ilaCode, index));
                                if (ilaCode[index] == '{')
                                    opened++;
                                if (ilaCode[index] == '}')
                                    opened--;
                                index++;
                            }
                        }
                    }
                    //start of an module
                    else if (ilaCode.Length - index > 7 && ilaCode.Substring(index, 7) == "module ")
                    {
                        index += 7;
                        var module = new Module();
                        Methods.Add(module);
                        if (lastComment != null)
                            module.Comment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var moduleName = CatchString(ilaCode, ref index);
                        if (moduleName == "")
                            throw new ILAException("Nom de module invalide : ligne " + CountRow(ilaCode, index));
                        module.Name = moduleName;
                        FastForward(ilaCode, ref index, true);
                        dispatcher.Add(index, module);
                        while (ilaCode[index] != '{')
                        {
                            index++;
                            if (index == ilaCode.Length)
                                throw new ILAException("Aucun corps d'expression : ligne " + CountRow(ilaCode, index));
                        }
                        index++;
                        {
                            int opened = 1;
                            while (opened > 0)
                            {
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ligne " + CountRow(ilaCode, index));
                                if (ilaCode[index] == '{')
                                    opened++;
                                if (ilaCode[index] == '}')
                                    opened--;
                                index++;
                            }
                        }
                    }
                    //start of a function
                    else if (ilaCode.Length - index > 9 && ilaCode.Substring(index, 9) == "fonction ")
                    {
                        index += 9;
                        var fct = new Function();
                        Methods.Add(fct);
                        if (lastComment != null)
                            fct.Comment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var fctName = CatchString(ilaCode, ref index);
                        if (fctName == "")
                            throw new ILAException("Nom de module invalide : ligne " + CountRow(ilaCode, index));
                        fct.Name = fctName;
                        FastForward(ilaCode, ref index, true);
                        dispatcher.Add(index, fct);
                        while (ilaCode[index] != '{')
                        {
                            index++;
                            if (index == ilaCode.Length)
                                throw new ILAException("Aucun corps d'expression : ligne " + CountRow(ilaCode, index));
                        }
                        index++;
                        {
                            int opened = 1;
                            while (opened > 0)
                            {
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ligne " + CountRow(ilaCode, index));
                                if (ilaCode[index] == '{')
                                    opened++;
                                if (ilaCode[index] == '}')
                                    opened--;
                                index++;
                            }
                        }
                    }
                    //start a declaration
                    else
                    {
                        if (!char.IsLetter(ilaCode[index]))
                            throw new ILAException("Erreur : Déclaration de variable, type, algo, module ou fonction attendu ligne " + CountRow(ilaCode, index));
                        string name = CatchString(ilaCode, ref index);
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode[index] != ':')
                            throw new ILAException("Caractère attendu ':' ligne " + CountRow(ilaCode, index));
                        index++;
                        FastForward(ilaCode, ref index, true);
                        string varType = CatchString(ilaCode, ref index);
                        if (varType == "const")
                        {
                            var variable = new Variable();
                            var declaration = new VariableDeclaration();
                            declaration.CreatedVariable = variable;
                            variable.Constant = true;
                            FastForward(ilaCode, ref index, true);
                            varType = CatchString(ilaCode, ref index);
                            switch (varType)
                            {
                                case "entier":
                                    variable.Type = GenericType.Int;
                                    FastForward(ilaCode, ref index, true);
                                    if (ilaCode.Substring(index, 2) != "<-")
                                        throw new ILAException("Opérateur attendu '<-' ligne " + CountRow(ilaCode, index));
                                    index += 2;
                                    FastForward(ilaCode, ref index, true);
                                    break;
                            }
                        }
                    }
                    SkipLine(ilaCode, ref index);
                }
                if (lastComment != null)
                    FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                lastComment = null;
                //parsing inside the executables
                foreach (var disp in dispatcher)
                {
                    if (disp.Value is Program)
                    {
                        index = disp.Key;
                        while (ilaCode[index] != '}')
                        {
                            Instructions.Add(ParseInstru(ilaCode, ref index));
                            SkipLine(ilaCode, ref index, true);
                        }
                    }
                    else if (disp.Value is Function fct)
                    {
                        index = disp.Key;
                        if (ilaCode[index] != '(')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : (   ligne " + CountRow(ilaCode, index));
                        index++;
                        string parametersStr = "";
                        while (ilaCode.Length > index && ilaCode[index] != ')')
                            parametersStr += ilaCode[index++];
                        fct.Parameters = new List<Parameter>();
                        if (parametersStr.Trim().Length > 0)
                        {
                            var singleParamsStr = parametersStr.Split(',');
                            foreach (var item in singleParamsStr)
                            {
                                var parameter = new Parameter();
                                item.Trim();
                                var comps = item.Split("::");
                                if (comps.Length > 2)
                                    throw new ILAException("Trop de mode de paramètre : ligne " + CountRow(ilaCode, index));
                                else if (comps.Length == 2)
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
                                if (comps2.Length != 2)
                                    throw new ILAException("Erreur de synthaxe ligne " + CountRow(ilaCode, index));
                                string n = "", t = "";
                                if (!char.IsLetter(comps2.First().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de variable commence uniquement par une lettre : ligne " + CountRow(ilaCode, index));
                                n = comps2.First().Trim();
                                foreach (var item2 in n)
                                    if (!char.IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de variable \"" + n + "\" est incorrect : ligne " + CountRow(ilaCode, index));
                                if (!char.IsLetter(comps2.Last().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de type commence uniquement par une lettre : ligne " + CountRow(ilaCode, index));
                                t = comps2.Last().Trim();
                                foreach (var item2 in t)
                                    if (!char.IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de type \"" + t + "\" est incorrect : ligne " + CountRow(ilaCode, index));
                                VarType type = null;
                                foreach (var item3 in Declarations)
                                {
                                    if (item3 is TypeDeclaration td)
                                        if (td.CreatedType.Name == t)
                                        {
                                            type = td.CreatedType;
                                            break;
                                        }
                                }
                                if (type == null)
                                {
                                    switch (t)
                                    {
                                        case "entier":
                                            type = GenericType.Int;
                                            break;
                                        case "reel":
                                            type = GenericType.Float;
                                            break;
                                        case "caractere":
                                            type = GenericType.Char;
                                            break;
                                        case "chaine":
                                            type = GenericType.String;
                                            break;
                                        case "booleen":
                                            type = GenericType.Bool;
                                            break;
                                    }
                                }
                                if (type == null)
                                    throw new ILAException("Type incorrect \"" + t + "\" : ligne " + CountRow(ilaCode, index));
                                parameter.ImportedVariable = new Variable
                                {
                                    Constant = false,
                                    Name = n,
                                    Type = type
                                };
                                fct.Parameters.Add(parameter);
                            }
                        }
                        index++;
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode[index] != ':')
                            throw new ILAException("Caractère attendu : ':' ligne " + CountRow(ilaCode, index));
                        index++;
                        FastForward(ilaCode, ref index, true);
                        var strReturnType = CatchString(ilaCode, ref index);
                        VarType returnType = null;
                        foreach (var item3 in Declarations)
                        {
                            if (item3 is TypeDeclaration td)
                                if (td.CreatedType.Name == strReturnType)
                                {
                                    returnType = td.CreatedType;
                                    break;
                                }
                        }
                        if (returnType == null)
                        {
                            switch (strReturnType)
                            {
                                case "entier":
                                    returnType = GenericType.Int;
                                    break;
                                case "reel":
                                    returnType = GenericType.Float;
                                    break;
                                case "caractere":
                                    returnType = GenericType.Char;
                                    break;
                                case "chaine":
                                    returnType = GenericType.String;
                                    break;
                                case "booleen":
                                    returnType = GenericType.Bool;
                                    break;
                            }
                        }
                        if (returnType == null)
                            throw new ILAException("Type incorrect \"" + strReturnType + "\" : ligne " + CountRow(ilaCode, index));
                        fct.ReturnType = returnType;
                        SkipLine(ilaCode, ref index, true);
                        if (ilaCode[index] != '{')
                            throw new ILAException("Caractère attendu : '{' ligne " + CountRow(ilaCode, index));
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        fct.Instructions = new List<Instruction>();
                        while (ilaCode[index] != '}')
                        {
                            fct.Instructions.Add(ParseInstru(ilaCode, ref index));
                            SkipLine(ilaCode, ref index, true);
                        }
                        index++;
                    }
                    else if (disp.Value is Module module)
                    {
                        index = disp.Key;
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
                                else if (comps.Length == 2)
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
                                if (comps2.Length != 2)
                                    throw new ILAException("Erreur de synthaxe ligne " + CountRow(ilaCode, index));
                                string n = "", t = "";
                                if (!char.IsLetter(comps2.First().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de variable commence uniquement par une lettre : ligne " + CountRow(ilaCode, index));
                                n = comps2.First().Trim();
                                foreach (var item2 in n)
                                    if (!char.IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de variable \"" + n + "\" est incorrect : ligne " + CountRow(ilaCode, index));
                                if (!char.IsLetter(comps2.Last().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de type commence uniquement par une lettre : ligne " + CountRow(ilaCode, index));
                                t = comps2.Last().Trim();
                                foreach (var item2 in t)
                                    if (!char.IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de type \"" + t + "\" est incorrect : ligne " + CountRow(ilaCode, index));
                                VarType type = null;
                                foreach (var item3 in Declarations)
                                {
                                    if (item3 is TypeDeclaration td)
                                        if (td.CreatedType.Name == t)
                                        {
                                            type = td.CreatedType;
                                            break;
                                        }
                                }
                                if (type == null)
                                {
                                    switch (t)
                                    {
                                        case "entier":
                                            type = GenericType.Int;
                                            break;
                                        case "reel":
                                            type = GenericType.Float;
                                            break;
                                        case "caractere":
                                            type = GenericType.Char;
                                            break;
                                        case "chaine":
                                            type = GenericType.String;
                                            break;
                                        case "booleen":
                                            type = GenericType.Bool;
                                            break;
                                    }
                                }
                                if (type == null)
                                    throw new ILAException("Type incorrect \"" + t + "\" : ligne " + CountRow(ilaCode, index));
                                parameter.ImportedVariable = new Variable
                                {
                                    Constant = false,
                                    Name = n,
                                    Type = type
                                };
                                module.Parameters = new List<Parameter>();
                                module.Parameters.Add(parameter);
                            }
                        }
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        if (ilaCode[index] != '{')
                            throw new ILAException("Caractère attendu : '{' ligne " + CountRow(ilaCode, index));
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        module.Instructions = new List<Instruction>();
                        while (ilaCode[index] != '}')
                        {
                            module.Instructions.Add(ParseInstru(ilaCode, ref index));
                            SkipLine(ilaCode, ref index, true);
                        }
                        index++;
                    }
                }
            }
            catch (ILAException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ILAException("Internal error", e);
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

            public ILAException(string mess = "", Exception inner = null) : base(mess, inner)
            {
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }
}