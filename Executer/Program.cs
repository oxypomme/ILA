using System;
using System.IO;
using ILANET;
using ILANET.Parser;

namespace Executer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                const int NOTHING = 0;
                const int PYTHON = 1;
                const int ILA = int.MaxValue; //for debug only, can't run in this format
                int build = NOTHING;
                int run = NOTHING;
                string buildPath = null;
                string file = args[0];

                int index = 1;
                bool lastModifIsRun = true;
                while (index < args.Length)
                {
                    var arg = args[index];
                    switch (arg)
                    {
                        case "-py":
                            if (lastModifIsRun)
                                run = PYTHON;
                            else
                                build = PYTHON;
                            break;

                        case "-ila": //debug only
                            if (lastModifIsRun)
                                throw new Exception("Can't run in ILA");
                            else
                                build = ILA;
                            break;

                        case "-run":
                            lastModifIsRun = true;
                            break;

                        case "-build":
                            lastModifIsRun = false;
                            if (build == NOTHING)
                                build = PYTHON;
                            break;

                        default:
                            if (!lastModifIsRun)
                                buildPath = arg;
                            else
                                throw new Exception("Unknown parameter '" + arg + "'");
                            break;
                    }
                    index++;
                }
                if (run == NOTHING && build == NOTHING)
                    run = PYTHON;
                if (build != NOTHING && buildPath == null)
                    throw new Exception("No path specified for the build");
                switch (build)
                {
                    case PYTHON:
                        using (var stream = new StreamReader(file))
                        using (var output = new StreamWriter(buildPath))
                            Parser.Parse(stream.ReadToEnd()).WritePython(output);
                        break;

                    case ILA:
                        using (var stream = new StreamReader(file))
                        using (var output = new StreamWriter(buildPath))
                            Parser.Parse(stream.ReadToEnd()).WriteILA(output);
                        break;
                }
                switch (run)
                {
                    case PYTHON:
                        using (var stream = new StreamReader(file))
                        {
                            var pythonCode = new StringWriter();
                            Parser.Parse(stream.ReadToEnd()).WritePython(pythonCode);
                            //run that thing
                        }
                        break;
                }
            }
            else
                Console.WriteLine(
@"Use :
ila <file.ila> [<args>]
by default it runs the file in Python.

arguments :
-run [<format>]______________________:run the file (Python by default)
-build <outputFile> [<format>]_______:build the file (Python by default)

formats :
-py_________________:run or build in Python
-ila________________:build in ILA (run not available)");
        }
    }
}