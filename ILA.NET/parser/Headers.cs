﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ILANET.Parser
{
    public partial class Parser
    {
        #region Public Methods

        /// <summary>
        /// Parse the ila code and returns the structure of the code
        /// </summary>
        /// <param name="ilaCode">code to parse</param>
        /// <param name="ignoreConstValues">
        /// true if the parser doesn't throw an exception if a constant has a variable value
        /// </param>
        /// <returns>structure of the code</returns>
        public static Program Parse(string ilaCode, bool ignoreConstValues = false)
        {
            //quick fix to avoid crashes because my code sucks lmao
            ilaCode += "     ";
            bool constLock = !ignoreConstValues;
            var returnProg = new Program();
            ilaCode = new string(ilaCode.Where(c => c != '\r').ToArray());
            returnProg.Declarations = new List<IDeclaration>();
            returnProg.Instructions = new List<Instruction>();
            returnProg.FileComments = new List<Comment>();
            returnProg.Methods = new List<Module>();
            returnProg.Methods.Add(Print.Instance);
            returnProg.Methods.Add(Read.Instance);
            returnProg.Methods.Add(Next.Instance);
            returnProg.Methods.Add(Prev.Instance);
            returnProg.Methods.Add(Length.Instance);
            /*
             * The dispatcher will keep track of executables blocks to parse them at the end, once
             * all the variables, custom types and other executables header has been added.
             */
            var dispatcher = new Dictionary<int, IExecutable>();
            /*
           * These will keep track of declarationsto parse them at the end, so constants and types
           * can be declared, no matter the order. One string is the comment above and if multiline.
           * For the constant ones, the object is already declared so they can use each other without being defined.
           */
            var constDispatcher = new Dictionary<int, Tuple<VariableDeclaration, string, bool>>();
            var typesDispatcher = new Dictionary<int, Tuple<TypeDeclaration, string, bool>>();
            var variablesDispatcher = new Dictionary<int, Tuple<string, bool>>();
            returnProg.AlgoComment = null;
            returnProg.Name = "";
            int index = 0;
            try
            {
                string lastComment = null;
                bool multilineComm = true;
                SkipLine(ilaCode, ref index);
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
                        if (lastComment != null)
                            returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
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
                            throw new ILAException("Commentaire multi ligne non terminé");
                        index += 2;

                        if (lastComment != null)
                            returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                        lastComment = comment;
                        multilineComm = true;
                    }
                    //start of an algo
                    else if (ilaCode.Length - index > 5 && ilaCode.Substring(index, 5) == "algo ")
                    {
                        index += 5;
                        if (lastComment != null)
                            returnProg.AlgoComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var algoName = CatchString(ilaCode, ref index);
                        if (algoName == "")
                            throw new ILAException("Nom d'algo invalide");
                        returnProg.Name = algoName;
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode.Substring(index, 2) == "//")
                        {
                            string inlineComm = "";
                            while (ilaCode[index] != '\n')
                            {
                                inlineComm += ilaCode[index];
                                if (ilaCode.Length == index)
                                    break;
                            }
                            returnProg.InlineComment = inlineComm;
                        }
                        else
                            returnProg.InlineComment = "";
                        SkipLine(ilaCode, ref index, true);
                        if (index == ilaCode.Length)
                            throw new ILAException("Aucun corps d'expression");
                        if (ilaCode[index] != '{')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : {   ");
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        dispatcher.Add(index, returnProg);
                        {
                            int opened = 1;
                            bool inComms = false;
                            bool multilineComms = false;
                            while (opened > 0)
                            {
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                                    inComms = true;
                                if (ilaCode[index] == '\n' && !multilineComms)
                                    inComms = false;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "/*" && !inComms)
                                    inComms = multilineComms = true;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "*/" && multilineComms)
                                    inComms = multilineComms = false;
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ");
                                if (ilaCode[index] == '{' && !inComms)
                                    opened++;
                                if (ilaCode[index] == '}' && !inComms)
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
                        returnProg.Methods.Add(module);
                        if (lastComment != null)
                            module.AboveComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var moduleName = CatchString(ilaCode, ref index);
                        if (moduleName == "")
                            throw new ILAException("Nom de module invalide");
                        module.Name = moduleName;
                        FastForward(ilaCode, ref index, true);
                        dispatcher.Add(index, module);
                        while (ilaCode[index] != '{')
                        {
                            index++;
                            if (index == ilaCode.Length)
                                throw new ILAException("Aucun corps d'expression");
                        }
                        index++;
                        {
                            int opened = 1;
                            bool inComms = false;
                            bool multilineComms = false;
                            while (opened > 0)
                            {
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                                    inComms = true;
                                if (ilaCode[index] == '\n' && !multilineComms)
                                    inComms = false;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "/*" && !inComms)
                                    inComms = multilineComms = true;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "*/" && multilineComms)
                                    inComms = multilineComms = false;
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ");
                                if (ilaCode[index] == '{' && !inComms)
                                    opened++;
                                if (ilaCode[index] == '}' && !inComms)
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
                        returnProg.Methods.Add(fct);
                        if (lastComment != null)
                            fct.AboveComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                        lastComment = null;
                        FastForward(ilaCode, ref index, true);
                        var fctName = CatchString(ilaCode, ref index);
                        if (fctName == "")
                            throw new ILAException("Nom de module invalide");
                        fct.Name = fctName;
                        FastForward(ilaCode, ref index, true);
                        dispatcher.Add(index, fct);
                        while (ilaCode[index] != '{')
                        {
                            index++;
                            if (index == ilaCode.Length)
                                throw new ILAException("Aucun corps d'expression ");
                        }
                        index++;
                        {
                            int opened = 1;
                            bool inComms = false;
                            bool multilineComms = false;
                            while (opened > 0)
                            {
                                if (ilaCode.Length == index)
                                    throw new ILAException("Caractère attendu : '}' ");
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                                    inComms = true;
                                if (ilaCode[index] == '\n' && !multilineComms)
                                    inComms = false;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "/*" && !inComms)
                                    inComms = multilineComms = true;
                                if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "*/" && multilineComms)
                                    inComms = multilineComms = false;
                                if (ilaCode[index] == '{' && !inComms)
                                    opened++;
                                if (ilaCode[index] == '}' && !inComms)
                                    opened--;
                                index++;
                            }
                        }
                    }
                    //start a declaration
                    else
                    {
                        int startOfLine = index;
                        if (!IsLetter(ilaCode[index]))
                            throw new ILAException("Erreur : Déclaration de variable, type, algo, module ou fonction attendu");
                        string name = CatchString(ilaCode, ref index);
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode[index] != ':')
                            throw new ILAException("Caractère attendu ':' ");
                        index++;
                        FastForward(ilaCode, ref index, true);
                        string varType = CatchString(ilaCode, ref index);
                        if (varType == "type")
                        {
                            int oldPos = index;
                            FastForward(ilaCode, ref index, true);
                            var customType = CatchString(ilaCode, ref index);
                            switch (customType)
                            {
                                case "struct":
                                case "structure":
                                    {
                                        var tmp = new TypeDeclaration
                                        {
                                            CreatedType = new StructType
                                            {
                                                Name = name
                                            }
                                        };
                                        returnProg.Declarations.Add(tmp);
                                        typesDispatcher.Add(startOfLine, new Tuple<TypeDeclaration, string, bool>(tmp, lastComment, multilineComm));
                                    }
                                    break;

                                case "enum":
                                case "enumeration":
                                    {
                                        var tmp = new TypeDeclaration
                                        {
                                            CreatedType = new EnumType
                                            {
                                                Name = name
                                            }
                                        };
                                        returnProg.Declarations.Add(tmp);
                                        typesDispatcher.Add(startOfLine, new Tuple<TypeDeclaration, string, bool>(tmp, lastComment, multilineComm));
                                    }
                                    break;

                                case "table":
                                case "tableau":
                                    {
                                        var tmp = new TypeDeclaration
                                        {
                                            CreatedType = new TableType
                                            {
                                                Name = name
                                            }
                                        };
                                        returnProg.Declarations.Add(tmp);
                                        typesDispatcher.Add(startOfLine, new Tuple<TypeDeclaration, string, bool>(tmp, lastComment, multilineComm));
                                    }
                                    break;
                            }
                            index = oldPos;
                        }
                        else if (varType == "const")
                        {
                            var tmp = new VariableDeclaration
                            {
                                CreatedVariable = new Variable
                                {
                                    Constant = true,
                                    Name = name
                                }
                            };
                            returnProg.Declarations.Add(tmp);
                            constDispatcher.Add(startOfLine, new Tuple<VariableDeclaration, string, bool>(tmp, lastComment, multilineComm));
                        }
                        else
                            variablesDispatcher.Add(startOfLine, new Tuple<string, bool>(lastComment, multilineComm));
                        //we parse it to find the end, but we do nothing, we will parse it for real after
                        if (varType == "const")
                        {
                            while (ilaCode[index] != '\n')
                                index++;
                        }
                        else
                        {
                            if (varType != "type")
                            {
                                while (ilaCode[index] != '\n')
                                    index++;
                            }
                            else
                            {
                                FastForward(ilaCode, ref index, true);
                                var customType = CatchString(ilaCode, ref index);
                                switch (customType)
                                {
                                    case "structure":
                                    case "struct":
                                        {
                                            SkipLine(ilaCode, ref index, true);
                                            if (ilaCode[index] != '(')
                                                throw new ILAException("Caractère attendu : '(' ");
                                            index++;
                                            SkipLine(ilaCode, ref index, true);
                                            while (ilaCode[index] != ')')
                                            {
                                                string memberName = CatchString(ilaCode, ref index);
                                                FastForward(ilaCode, ref index, true);
                                                if (ilaCode[index] != ':')
                                                    throw new ILAException("Caractère attendu ':' ");
                                                index++;
                                                FastForward(ilaCode, ref index, true);
                                                string memberTypeStr = CatchString(ilaCode, ref index);
                                                SkipLine(ilaCode, ref index, true);
                                                if (ilaCode[index] != ',' && ilaCode[index] != ')')
                                                    throw new ILAException("Caractère attendu : ',' ou ')' ");
                                                if (ilaCode[index] == ',')
                                                {
                                                    index++;
                                                    SkipLine(ilaCode, ref index, true);
                                                }
                                            }
                                        }
                                        while (ilaCode[index] != '\n')
                                            index++;
                                        break;

                                    case "tableau":
                                    case "table":
                                        while (ilaCode[index] != '\n')
                                            index++;
                                        break;

                                    case "enumeration":
                                    case "enum":
                                        {
                                            SkipLine(ilaCode, ref index, true);
                                            if (ilaCode[index] != '(')
                                                throw new ILAException("Caractère attendu : '(' ");
                                            index++;
                                            SkipLine(ilaCode, ref index, true);
                                            while (ilaCode[index] != ')')
                                            {
                                                CatchString(ilaCode, ref index);
                                                SkipLine(ilaCode, ref index, true);
                                                if (ilaCode[index] != ',' && ilaCode[index] != ')')
                                                    throw new ILAException("Caractère attendu : ',' ou ')' ");
                                                if (ilaCode[index] == ',')
                                                {
                                                    index++;
                                                    SkipLine(ilaCode, ref index, true);
                                                }
                                            }
                                        }
                                        while (ilaCode[index] != '\n')
                                            index++;
                                        break;
                                }
                            }
                        }
                        lastComment = null;
                    }
                    SkipLine(ilaCode, ref index);
                }

                if (lastComment != null)
                    returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                lastComment = null;
                foreach (var item in constDispatcher)
                {
                    index = item.Key;
                    string name = CatchString(ilaCode, ref index);
                    FastForward(ilaCode, ref index, true);
                    if (ilaCode[index] != ':')
                        throw new ILAException("Caractère attendu ':' ");
                    index++;
                    FastForward(ilaCode, ref index, true);
                    string varType = CatchString(ilaCode, ref index);
                    var variable = item.Value.Item1.CreatedVariable;
                    var declaration = item.Value.Item1;
                    declaration.AboveComment = new Comment() { Message = item.Value.Item2, MultiLine = item.Value.Item3 };
                    declaration.CreatedVariable = variable;
                    variable.Constant = true;
                    variable.Name = name;
                    FastForward(ilaCode, ref index, true);
                    varType = CatchString(ilaCode, ref index);
                    switch (varType)
                    {
                        case "entier":
                            variable.Type = GenericType.Int;
                            break;

                        case "reel":
                            variable.Type = GenericType.Float;
                            break;

                        case "caractere":
                            variable.Type = GenericType.Char;
                            break;

                        case "chaine":
                            variable.Type = GenericType.String;
                            break;

                        case "booleen":
                            variable.Type = GenericType.Bool;
                            break;

                        default:
                            if (constLock)
                                throw new ILAException("Erreur : '" + varType + "' ne peut pas être constant ");
                            break;
                    }
                    FastForward(ilaCode, ref index, true);
                    if (ilaCode.Substring(index, 2) != "<-")
                        throw new ILAException("Opérateur attendu '<-' ");
                    index += 2;
                    FastForward(ilaCode, ref index, true);
                    var constValue = "";
                    while (ilaCode.Length - 1 > index && ilaCode.Substring(index, 2) != "//" && ilaCode[index] != '\n')
                    {
                        constValue += ilaCode[index];
                        index++;
                    }
                    variable.ConstantValue = ParseValue(constValue, returnProg, returnProg, constLock);
                    if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                    {
                        index += 2;
                        string comm = "";
                        while (ilaCode[index] != '\n')
                        {
                            comm += ilaCode[index];
                            index++;
                            if (index == ilaCode.Length)
                                break;
                        }
                        declaration.InlineComment = comm;
                    }
                    else
                        declaration.InlineComment = "";
                }
                foreach (var item in typesDispatcher)
                {
                    index = item.Key;
                    string name = CatchString(ilaCode, ref index);
                    FastForward(ilaCode, ref index, true);
                    if (ilaCode[index] != ':')
                        throw new ILAException("Caractère attendu ':' ");
                    index++;
                    FastForward(ilaCode, ref index, true);
                    string varType = CatchString(ilaCode, ref index);
                    IDeclaration decl;
                    {
                        var declaration = item.Value.Item1;
                        decl = declaration;
                        FastForward(ilaCode, ref index, true);
                        var customType = CatchString(ilaCode, ref index);
                        switch (customType)
                        {
                            case "structure":
                            case "struct":
                                {
                                    var type = declaration.CreatedType as StructType;
                                    type.Name = name;
                                    type.Members = new SortedDictionary<string, VarType>();
                                    SkipLine(ilaCode, ref index, true);
                                    if (ilaCode[index] != '(')
                                        throw new ILAException("Caractère attendu : '(' ");
                                    index++;
                                    SkipLine(ilaCode, ref index, true);
                                    while (ilaCode[index] != ')')
                                    {
                                        string memberName = CatchString(ilaCode, ref index);
                                        FastForward(ilaCode, ref index, true);
                                        if (ilaCode[index] != ':')
                                            throw new ILAException("Caractère attendu ':' ");
                                        index++;
                                        FastForward(ilaCode, ref index, true);
                                        string memberTypeStr = CatchString(ilaCode, ref index);
                                        VarType memberType = null;
                                        switch (memberTypeStr)
                                        {
                                            case "entier":
                                                memberType = GenericType.Int;
                                                break;

                                            case "reel":
                                                memberType = GenericType.Float;
                                                break;

                                            case "caractere":
                                                memberType = GenericType.Char;
                                                break;

                                            case "chaine":
                                                memberType = GenericType.String;
                                                break;

                                            case "booleen":
                                                memberType = GenericType.Bool;
                                                break;

                                            default:
                                                foreach (var item2 in returnProg.Declarations)
                                                {
                                                    if (item2 is TypeDeclaration td)
                                                        if (td.CreatedType.Name == memberTypeStr)
                                                            memberType = td.CreatedType;
                                                }
                                                break;
                                        }
                                        if (memberType == null)
                                            throw new ILAException("Type '" + memberTypeStr + "' non reconnu ");
                                        type.Members.Add(memberName, memberType);
                                        SkipLine(ilaCode, ref index, true);
                                        if (ilaCode[index] != ',' && ilaCode[index] != ')')
                                            throw new ILAException("Caractère attendu : ',' ou ')' ");
                                        if (ilaCode[index] == ',')
                                        {
                                            index++;
                                            SkipLine(ilaCode, ref index, true);
                                        }
                                    }
                                }
                                index++;
                                break;

                            case "tableau":
                            case "table":
                                {
                                    var type = declaration.CreatedType as TableType;
                                    type.Name = name;
                                    type.DimensionsSize = new List<Range>();
                                    FastForward(ilaCode, ref index, true);
                                    if (ilaCode[index] != '[')
                                        throw new ILAException("Caractère attendu : '[' ");
                                    index++;
                                    FastForward(ilaCode, ref index, true);
                                    while (ilaCode[index] != ']')
                                    {
                                        string min = "", max = "";
                                        while (ilaCode.Substring(index, 2) != "..")
                                        {
                                            min += ilaCode[index];
                                            index++;
                                            if (index == ilaCode.Length - 1)
                                                throw new ILAException("Erreur : données manquantes : ");
                                        }
                                        index += 2;
                                        while (ilaCode[index] != ']' && ilaCode[index] != ',')
                                        {
                                            max += ilaCode[index];
                                            index++;
                                            if (index == ilaCode.Length)
                                                throw new ILAException("Erreur : données manquantes : ");
                                        }
                                        if (ilaCode[index] == ',')
                                            index++;
                                        type.DimensionsSize.Add(new Range(ParseValue(min, returnProg, returnProg, constLock), ParseValue(max, returnProg, returnProg, constLock)));
                                    }
                                    index++;
                                    FastForward(ilaCode, ref index, true);
                                    if (ilaCode[index] != ':')
                                        throw new ILAException("Caractère attendu ':' ");
                                    index++;
                                    FastForward(ilaCode, ref index, true);
                                    var caseTypeStr = CatchString(ilaCode, ref index);
                                    VarType caseType = null;
                                    switch (caseTypeStr)
                                    {
                                        case "entier":
                                            caseType = GenericType.Int;
                                            break;

                                        case "reel":
                                            caseType = GenericType.Float;
                                            break;

                                        case "caractere":
                                            caseType = GenericType.Char;
                                            break;

                                        case "chaine":
                                            caseType = GenericType.String;
                                            break;

                                        case "booleen":
                                            caseType = GenericType.Bool;
                                            break;

                                        default:
                                            foreach (var item2 in returnProg.Declarations)
                                            {
                                                if (item2 is TypeDeclaration td)
                                                    if (td.CreatedType.Name == caseTypeStr)
                                                        caseType = td.CreatedType;
                                            }
                                            break;
                                    }
                                    if (caseType == null)
                                        throw new ILAException("Type '" + caseTypeStr + "' non reconnu ");
                                    type.InternalType = caseType;
                                }
                                break;

                            case "enumeration":
                            case "enum":
                                {
                                    var type = declaration.CreatedType as EnumType;
                                    type.Name = name;
                                    type.Values = new List<string>();
                                    SkipLine(ilaCode, ref index, true);
                                    if (ilaCode[index] != '(')
                                        throw new ILAException("Caractère attendu : '(' ");
                                    index++;
                                    SkipLine(ilaCode, ref index, true);
                                    while (ilaCode[index] != ')')
                                    {
                                        type.Values.Add(CatchString(ilaCode, ref index));
                                        SkipLine(ilaCode, ref index, true);
                                        if (ilaCode[index] != ',' && ilaCode[index] != ')')
                                            throw new ILAException("Caractère attendu : ',' ou ')' ");
                                        if (ilaCode[index] == ',')
                                        {
                                            index++;
                                            SkipLine(ilaCode, ref index, true);
                                        }
                                    }
                                }
                                index++;
                                break;
                        }
                    }
                    decl.AboveComment = new Comment() { Message = item.Value.Item2, MultiLine = item.Value.Item3 };
                    FastForward(ilaCode, ref index);
                    if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                    {
                        index += 2;
                        string comm = "";
                        while (ilaCode[index] != '\n')
                        {
                            comm += ilaCode[index];
                            index++;
                            if (index == ilaCode.Length)
                                break;
                        }
                        decl.Comment = comm;
                    }
                    else
                        decl.Comment = "";
                }
                foreach (var item in variablesDispatcher)
                {
                    index = item.Key;
                    string name = CatchString(ilaCode, ref index);
                    FastForward(ilaCode, ref index, true);
                    if (ilaCode[index] != ':')
                        throw new ILAException("Caractère attendu ':' ");
                    index++;
                    FastForward(ilaCode, ref index, true);
                    string varType = CatchString(ilaCode, ref index);
                    {
                        IDeclaration decl;
                        switch (varType)
                        {
                            case "entier":
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = GenericType.Int;
                                    variable.Name = name;
                                }
                                break;

                            case "reel":
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = GenericType.Float;
                                    variable.Name = name;
                                }
                                break;

                            case "caractere":
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = GenericType.Char;
                                    variable.Name = name;
                                }
                                break;

                            case "chaine":
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = GenericType.String;
                                    variable.Name = name;
                                }
                                break;

                            case "booleen":
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = GenericType.Bool;
                                    variable.Name = name;
                                }
                                break;

                            default:
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    decl = declaration;
                                    returnProg.Declarations.Add(decl);
                                    declaration.CreatedVariable = variable;
                                    variable.Type = null;
                                    foreach (var item2 in returnProg.Declarations)
                                    {
                                        if (item2 is TypeDeclaration td)
                                            if (td.CreatedType.Name == varType)
                                                variable.Type = td.CreatedType;
                                    }
                                    if (variable.Type == null)
                                        throw new ILAException("Erreur : type '" + varType + "' inconnu ");
                                    variable.Name = name;
                                }
                                break;
                        }
                        decl.AboveComment = new Comment() { Message = item.Value.Item1, MultiLine = item.Value.Item2 };
                        FastForward(ilaCode, ref index);
                        if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                        {
                            index += 2;
                            string comm = "";
                            while (ilaCode[index] != '\n')
                            {
                                comm += ilaCode[index];
                                index++;
                                if (index == ilaCode.Length)
                                    break;
                            }
                            decl.Comment = comm;
                        }
                        else
                            decl.Comment = "";
                    }
                }
                //parsing inside the executables
                foreach (var disp in dispatcher)
                {
                    if (disp.Value is Program)
                    {
                        index = disp.Key;
                        while (ilaCode[index] != '}')
                        {
                            returnProg.Instructions.Add(ParseInstruction(ilaCode, returnProg, returnProg, ref index, ignoreConstValues));
                            SkipLine(ilaCode, ref index, true);
                        }
                    }
                    else if (disp.Value is Function fct)
                    {
                        fct.Declarations = new List<VariableDeclaration>();
                        index = disp.Key;
                        if (ilaCode[index] != '(')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : (   ");
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
                                if (item.Length == 0)
                                    break;
                                var comps = item.Split("::");
                                if (comps.Length > 2)
                                    throw new ILAException("Trop de modes de paramètre : ");
                                else if (comps.Length == 2)
                                {
                                    comps[0] = RemoveBlanks(comps.First());
                                    foreach (var item2 in comps.First())
                                    {
                                        switch (item2)
                                        {
                                            case 'e':
                                                parameter.Mode = Parameter.Flags.INPUT;
                                                break;

                                            case 's':
                                                throw new ILAException("Impossible de déclarer une fonction avec des arguments de sortie");

                                            default:
                                                throw new ILAException("Mode de paramètre '" + item2 + "' inconnu ");
                                        }
                                    }
                                }
                                else
                                    parameter.Mode = Parameter.Flags.INPUT;
                                var comps2 = comps.Last().Split(':');
                                if (comps2.Length != 2)
                                    throw new ILAException("Erreur de synthaxe ");
                                string n = "", t = "";
                                if (!IsLetter(comps2.First().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de variable commence uniquement par une lettre ");
                                n = comps2.First().Trim();
                                foreach (var item2 in n)
                                    if (!IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de variable \"" + n + "\" est incorrect ");
                                if (!IsLetter(comps2.Last().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de type commence uniquement par une lettre ");
                                t = comps2.Last().Trim();
                                foreach (var item2 in t)
                                    if (!IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de type \"" + t + "\" est incorrect ");
                                VarType type = null;
                                foreach (var item3 in returnProg.Declarations)
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
                                    throw new ILAException("Type incorrect \"" + t + "\" ");
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
                            throw new ILAException("Caractère attendu : ':' ");
                        index++;
                        FastForward(ilaCode, ref index, true);
                        var strReturnType = CatchString(ilaCode, ref index);
                        VarType returnType = null;
                        foreach (var item3 in returnProg.Declarations)
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
                            throw new ILAException("Type incorrect \"" + strReturnType + "\" ");
                        fct.ReturnType = returnType;
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode.Substring(index, 2) == "//")
                        {
                            index += 2;
                            string inlineComm = "";
                            while (ilaCode[index] != '\n')
                            {
                                inlineComm += ilaCode[index];
                                if (ilaCode.Length == index)
                                    break;
                            }
                            fct.InlineComment = inlineComm;
                        }
                        else
                            fct.InlineComment = "";
                        SkipLine(ilaCode, ref index, true);
                        constDispatcher.Clear();
                        variablesDispatcher.Clear();
                        while (ilaCode[index] != '{')
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
                                if (lastComment != null)
                                    returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
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
                                    throw new ILAException("Commentaire multi ligne non terminé ");
                                index += 2;

                                if (lastComment != null)
                                    returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                                lastComment = comment;
                                multilineComm = true;
                            }
                            //declaration
                            else
                            {
                                var startOfLine = index;
                                if (!IsLetter(ilaCode[index]))
                                    throw new ILAException("Erreur : Déclaration de variable ou type attendu ");
                                string name = CatchString(ilaCode, ref index);
                                FastForward(ilaCode, ref index, true);
                                if (ilaCode[index] != ':')
                                    throw new ILAException("Caractère attendu ':' ");
                                index++;
                                FastForward(ilaCode, ref index, true);
                                string varType = CatchString(ilaCode, ref index);
                                if (varType == "const")
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    declaration.CreatedVariable = variable;
                                    variable.Constant = true;
                                    variable.Name = name;
                                    fct.Declarations.Add(declaration);
                                    constDispatcher.Add(startOfLine, new Tuple<VariableDeclaration, string, bool>(declaration, lastComment, multilineComm));
                                }
                                else
                                    variablesDispatcher.Add(startOfLine, new Tuple<string, bool>(lastComment, multilineComm));
                                while (ilaCode[index] != '\n')
                                    index++;
                            }
                            SkipLine(ilaCode, ref index, true);
                        }
                        var oldIndex = index;
                        foreach (var item in constDispatcher)
                        {
                            index = item.Key;
                            if (!IsLetter(ilaCode[index]))
                                throw new ILAException("Erreur : Déclaration de variable attendue");
                            string name = CatchString(ilaCode, ref index);
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode[index] != ':')
                                throw new ILAException("Caractère attendu ':' ");
                            index++;
                            FastForward(ilaCode, ref index, true);
                            string varType = CatchString(ilaCode, ref index);

                            var variable = item.Value.Item1.CreatedVariable;
                            var declaration = item.Value.Item1;
                            declaration.AboveComment = new Comment() { Message = item.Value.Item2, MultiLine = item.Value.Item3 };
                            variable.Constant = true;
                            variable.Name = name;
                            FastForward(ilaCode, ref index, true);
                            varType = CatchString(ilaCode, ref index);
                            switch (varType)
                            {
                                case "entier":
                                    variable.Type = GenericType.Int;
                                    break;

                                case "reel":
                                    variable.Type = GenericType.Float;
                                    break;

                                case "caractere":
                                    variable.Type = GenericType.Char;
                                    break;

                                case "chaine":
                                    variable.Type = GenericType.String;
                                    break;

                                case "booleen":
                                    variable.Type = GenericType.Bool;
                                    break;

                                default:
                                    throw new ILAException("Erreur : '" + varType + "' ne peut pas être constant ");
                            }
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode.Substring(index, 2) != "<-")
                                throw new ILAException("Opérateur attendu '<-' ");
                            index += 2;
                            FastForward(ilaCode, ref index, true);
                            var constValue = "";
                            while (ilaCode.Length - 1 > index && ilaCode.Substring(index, 2) != "//" && ilaCode[index] != '\n')
                            {
                                constValue += ilaCode[index];
                                index++;
                            }
                            variable.ConstantValue = ParseValue(constValue, returnProg, fct, constLock);
                            if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                            {
                                index += 2;
                                string comm = "";
                                while (ilaCode[index] != '\n')
                                {
                                    comm += ilaCode[index];
                                    index++;
                                    if (index == ilaCode.Length)
                                        break;
                                }
                                declaration.InlineComment = comm;
                            }
                            else
                                declaration.InlineComment = "";
                        }
                        foreach (var item in variablesDispatcher)
                        {
                            index = item.Key;
                            if (!IsLetter(ilaCode[index]))
                                throw new ILAException("Erreur : Déclaration de variable attendue");
                            string name = CatchString(ilaCode, ref index);
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode[index] != ':')
                                throw new ILAException("Caractère attendu ':' ");
                            index++;
                            FastForward(ilaCode, ref index, true);
                            string varType = CatchString(ilaCode, ref index);
                            var declaration = new VariableDeclaration();
                            var variable = new Variable();
                            fct.Declarations.Add(declaration);
                            declaration.CreatedVariable = variable;
                            variable.Name = name;
                            switch (varType)
                            {
                                case "entier":
                                    variable.Type = GenericType.Int;
                                    break;

                                case "reel":
                                    variable.Type = GenericType.Float;
                                    break;

                                case "caractere":
                                    variable.Type = GenericType.Char;
                                    break;

                                case "chaine":
                                    variable.Type = GenericType.String;
                                    break;

                                case "booleen":
                                    variable.Type = GenericType.Bool;
                                    break;

                                case "type":
                                    throw new ILAException("Erreur : impossible de créer un type dans une fonction");

                                default:
                                    variable.Type = null;
                                    foreach (var item2 in returnProg.Declarations)
                                    {
                                        if (item2 is TypeDeclaration td)
                                            if (td.CreatedType.Name == varType)
                                                variable.Type = td.CreatedType;
                                    }
                                    if (variable.Type == null)
                                        throw new ILAException("Erreur : type '" + varType + "' inconnu ");
                                    break;
                            }
                            if (lastComment != null)
                                declaration.AboveComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                            lastComment = null;
                            FastForward(ilaCode, ref index);
                            if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                            {
                                index += 2;
                                string comm = "";
                                while (ilaCode[index] != '\n')
                                {
                                    comm += ilaCode[index];
                                    index++;
                                    if (index == ilaCode.Length)
                                        break;
                                }
                                declaration.InlineComment = comm;
                            }
                            else
                                declaration.InlineComment = "";
                        }
                        index = oldIndex;
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        fct.Instructions = new List<Instruction>();
                        while (ilaCode[index] != '}')
                        {
                            fct.Instructions.Add(ParseInstruction(ilaCode, returnProg, fct, ref index, ignoreConstValues));
                            SkipLine(ilaCode, ref index, true);
                        }
                        index++;
                    }
                    else if (disp.Value is Module module)
                    {
                        module.Declarations = new List<VariableDeclaration>();
                        index = disp.Key;
                        if (ilaCode[index] != '(')
                            throw new ILAException("caractère non attendu '" + ilaCode[index] + "'. Caractère attendu : (   ");
                        index++;
                        string parametersStr = "";
                        while (ilaCode.Length > index && ilaCode[index] != ')')
                            parametersStr += ilaCode[index++];
                        {
                            var singleParamsStr = parametersStr.Split(',');
                            module.Parameters = new List<Parameter>();
                            foreach (var item in singleParamsStr)
                            {
                                var parameter = new Parameter();
                                item.Trim();
                                if (item.Length == 0)
                                    break;
                                var comps = item.Split("::");
                                if (comps.Length > 2)
                                    throw new ILAException("Trop de mode de paramètre ");
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
                                                throw new ILAException("Mode de paramètre '" + item2 + "' inconnu ");
                                        }
                                    }
                                }
                                else
                                    parameter.Mode = Parameter.Flags.INPUT;
                                var comps2 = comps.Last().Split(':');
                                if (comps2.Length != 2)
                                    throw new ILAException("Erreur de synthaxe ");
                                string n = "", t = "";
                                if (!IsLetter(comps2.First().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de variable commence uniquement par une lettre ");
                                n = comps2.First().Trim();
                                foreach (var item2 in n)
                                    if (!IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de variable \"" + n + "\" est incorrect ");
                                if (!IsLetter(comps2.Last().TrimStart().First()))
                                    throw new ILAException("Erreur : Un nom de type commence uniquement par une lettre ");
                                t = comps2.Last().Trim();
                                foreach (var item2 in t)
                                    if (!IsLetterOrDigit(item2))
                                        throw new ILAException("Erreur : Le nom de type \"" + t + "\" est incorrect ");
                                VarType type = null;
                                foreach (var item3 in returnProg.Declarations)
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
                                    throw new ILAException("Type incorrect \"" + t + "\" ");
                                parameter.ImportedVariable = new Variable
                                {
                                    Constant = false,
                                    Name = n,
                                    Type = type
                                };
                                module.Parameters.Add(parameter);
                            }
                        }
                        index++;
                        FastForward(ilaCode, ref index, true);
                        if (ilaCode.Substring(index, 2) == "//")
                        {
                            index += 2;
                            string inlineComm = "";
                            while (ilaCode[index] != '\n')
                            {
                                inlineComm += ilaCode[index];
                                if (ilaCode.Length == index)
                                    break;
                            }
                            module.InlineComment = inlineComm;
                        }
                        else
                            module.InlineComment = "";
                        SkipLine(ilaCode, ref index, true);
                        constDispatcher.Clear();
                        variablesDispatcher.Clear();
                        while (ilaCode[index] != '{')
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
                                if (lastComment != null)
                                    returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
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
                                    throw new ILAException("Commentaire multi ligne non terminé ");
                                index += 2;

                                if (lastComment != null)
                                    returnProg.FileComments.Add(new Comment() { Message = lastComment, MultiLine = multilineComm });
                                lastComment = comment;
                                multilineComm = true;
                            }
                            //declaration
                            else
                            {
                                var startOfLine = index;
                                if (!IsLetter(ilaCode[index]))
                                    throw new ILAException("Erreur : Déclaration de variable ou type attendu ");
                                string name = CatchString(ilaCode, ref index);
                                FastForward(ilaCode, ref index, true);
                                if (ilaCode[index] != ':')
                                    throw new ILAException("Caractère attendu ':' ");
                                index++;
                                FastForward(ilaCode, ref index, true);
                                string varType = CatchString(ilaCode, ref index);
                                if (varType == "const")
                                {
                                    var variable = new Variable();
                                    var declaration = new VariableDeclaration();
                                    declaration.CreatedVariable = variable;
                                    variable.Constant = true;
                                    variable.Name = name;
                                    module.Declarations.Add(declaration);
                                    constDispatcher.Add(startOfLine, new Tuple<VariableDeclaration, string, bool>(declaration, lastComment, multilineComm));
                                }
                                else
                                    variablesDispatcher.Add(startOfLine, new Tuple<string, bool>(lastComment, multilineComm));
                                while (ilaCode[index] != '\n')
                                    index++;
                            }
                            SkipLine(ilaCode, ref index, true);
                        }
                        var oldIndex = index;
                        foreach (var item in constDispatcher)
                        {
                            index = item.Key;
                            if (!IsLetter(ilaCode[index]))
                                throw new ILAException("Erreur : Déclaration de variable attendue");
                            string name = CatchString(ilaCode, ref index);
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode[index] != ':')
                                throw new ILAException("Caractère attendu ':' ");
                            index++;
                            FastForward(ilaCode, ref index, true);
                            string varType = CatchString(ilaCode, ref index);

                            var variable = item.Value.Item1.CreatedVariable;
                            var declaration = item.Value.Item1;
                            declaration.AboveComment = new Comment() { Message = item.Value.Item2, MultiLine = item.Value.Item3 };
                            variable.Constant = true;
                            variable.Name = name;
                            FastForward(ilaCode, ref index, true);
                            varType = CatchString(ilaCode, ref index);
                            switch (varType)
                            {
                                case "entier":
                                    variable.Type = GenericType.Int;
                                    break;

                                case "reel":
                                    variable.Type = GenericType.Float;
                                    break;

                                case "caractere":
                                    variable.Type = GenericType.Char;
                                    break;

                                case "chaine":
                                    variable.Type = GenericType.String;
                                    break;

                                case "booleen":
                                    variable.Type = GenericType.Bool;
                                    break;

                                default:
                                    throw new ILAException("Erreur : '" + varType + "' ne peut pas être constant ");
                            }
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode.Substring(index, 2) != "<-")
                                throw new ILAException("Opérateur attendu '<-' ");
                            index += 2;
                            FastForward(ilaCode, ref index, true);
                            var constValue = "";
                            while (ilaCode.Length - 1 > index && ilaCode.Substring(index, 2) != "//" && ilaCode[index] != '\n')
                            {
                                constValue += ilaCode[index];
                                index++;
                            }
                            variable.ConstantValue = ParseValue(constValue, returnProg, module, constLock);
                            if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                            {
                                index += 2;
                                string comm = "";
                                while (ilaCode[index] != '\n')
                                {
                                    comm += ilaCode[index];
                                    index++;
                                    if (index == ilaCode.Length)
                                        break;
                                }
                                declaration.InlineComment = comm;
                            }
                            else
                                declaration.InlineComment = "";
                        }
                        foreach (var item in variablesDispatcher)
                        {
                            index = item.Key;
                            if (!IsLetter(ilaCode[index]))
                                throw new ILAException("Erreur : Déclaration de variable attendue");
                            string name = CatchString(ilaCode, ref index);
                            FastForward(ilaCode, ref index, true);
                            if (ilaCode[index] != ':')
                                throw new ILAException("Caractère attendu ':' ");
                            index++;
                            FastForward(ilaCode, ref index, true);
                            string varType = CatchString(ilaCode, ref index);
                            var declaration = new VariableDeclaration();
                            var variable = new Variable();
                            module.Declarations.Add(declaration);
                            declaration.CreatedVariable = variable;
                            variable.Name = name;
                            switch (varType)
                            {
                                case "entier":
                                    variable.Type = GenericType.Int;
                                    break;

                                case "reel":
                                    variable.Type = GenericType.Float;
                                    break;

                                case "caractere":
                                    variable.Type = GenericType.Char;
                                    break;

                                case "chaine":
                                    variable.Type = GenericType.String;
                                    break;

                                case "booleen":
                                    variable.Type = GenericType.Bool;
                                    break;

                                case "type":
                                    throw new ILAException("Erreur : impossible de créer un type dans une fonction");

                                default:
                                    variable.Type = null;
                                    foreach (var item2 in returnProg.Declarations)
                                    {
                                        if (item2 is TypeDeclaration td)
                                            if (td.CreatedType.Name == varType)
                                                variable.Type = td.CreatedType;
                                    }
                                    if (variable.Type == null)
                                        throw new ILAException("Erreur : type '" + varType + "' inconnu ");
                                    break;
                            }
                            if (lastComment != null)
                                declaration.AboveComment = new Comment() { Message = lastComment, MultiLine = multilineComm };
                            lastComment = null;
                            FastForward(ilaCode, ref index);
                            if (index < ilaCode.Length - 1 && ilaCode.Substring(index, 2) == "//")
                            {
                                index += 2;
                                string comm = "";
                                while (ilaCode[index] != '\n')
                                {
                                    comm += ilaCode[index];
                                    index++;
                                    if (index == ilaCode.Length)
                                        break;
                                }
                                declaration.InlineComment = comm;
                            }
                            else
                                declaration.InlineComment = "";
                        }
                        index = oldIndex;
                        index++;
                        SkipLine(ilaCode, ref index, true);
                        module.Instructions = new List<Instruction>();
                        while (ilaCode[index] != '}')
                        {
                            module.Instructions.Add(ParseInstruction(ilaCode, returnProg, module, ref index, ignoreConstValues));
                            SkipLine(ilaCode, ref index, true);
                        }
                        index++;
                    }
                }
            }

#if DEBUG
            catch (FieldAccessException)//dummy exception to never catch anything, and throwing everything to the debugger
            { }

#else
            catch (ILAException e)
            {
                throw new ILAException(e.Message + " \\ ligne : " + CountRow(ilaCode, index), e);
            }
            catch (Exception e)
            {
                throw new ILAException("Internal error \\ ligne : " + CountRow(ilaCode, index), e);
            }
#endif
            return returnProg;
        }

        #endregion Public Methods
    }
}