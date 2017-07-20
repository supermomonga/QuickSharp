using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.Extensions.CommandLineUtils;

namespace QuickSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "QuickSharp";
            app.HelpOption("-?|-h|--help");
            app.Argument("file", "File to execure");

            app.OnExecute(async () =>
                    {
                        // Get file name
                        var fileArg = app.Arguments.Find((arg) => arg.Name == "file");
                        var file = fileArg.Value;
                        if (string.IsNullOrEmpty(file))
                        {
                            file = "./Program.cs";
                        }

                        // Get file path
                        try
                        {
                            file = Path.GetFullPath(file);
                        }
                        catch (Exception e) { RaiseAndExit(e, 1); }

                        if (!File.Exists(file))
                        {
                            RaiseAndExit($"File '{file}' not found", 1);
                            return 1;
                        }

                        // Execute and show result
                        try
                        {
                            var code = File.ReadAllText(file);
                            await CSharpScript.EvaluateAsync(code);
                            return 0;
                        }
                        catch (Exception e)
                        {
                            RaiseAndExit(e, 1);
                            return 1;
                        }
                    });

            app.Execute(args);
        }

        private static void RaiseAndExit(string e, int exitCode)
        {
            Console.WriteLine(e);
            Environment.Exit(exitCode);
        }

        private static void RaiseAndExit(Exception e, int exitCode)
        {
            RaiseAndExit(e.ToString(), exitCode);
        }
    }
}





