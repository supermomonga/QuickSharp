using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace QuickSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        public static async Task MainAsync(string[] args)
        {
            // Get file name
            var file = "./Program.cs";
            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("f|file", ref file, "File to execute");
            });

            // Get file path
            try
            {
                file = Path.GetFullPath(file);
            }
            catch (Exception e) { RaiseAndExit(e, 1); }

            if (!File.Exists(file)) { RaiseAndExit($"File '{file}' not found", 1); }

            // Execute and show result
            try
            {
                var code = File.ReadAllText(file);
                await CSharpScript.EvaluateAsync(code);
            }
            catch (Exception e) { RaiseAndExit(e, 1); }
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


