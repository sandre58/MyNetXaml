// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommandLine;
using MyNet.Xaml.Merger.Console.Commands;

namespace MyNet.Xaml.Merger.Console;

public static class Program
{
    private static async Task<int> Main(string[] args)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = Parser.Default
                               .ParseArguments<XAMLCombineOptions, XAMLColorSchemeGeneratorOptions, DumpResourcesOptions>(args)
                               .MapResult(
                                   async (XAMLCombineOptions options) => await options.Execute(),
                                   async (XAMLColorSchemeGeneratorOptions options) => await options.Execute(),
                                   async (DumpResourcesOptions options) => await options.Execute(),
                                   ErrorHandler);

            return await result;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);

            if (Debugger.IsAttached)
            {
                System.Console.ReadLine();
            }

            return 1;
        }
        finally
        {
            System.Console.WriteLine($"Execution time: {stopwatch.Elapsed}");
        }
    }

    private static Task<int> ErrorHandler(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            System.Console.Error.WriteLine(error.ToString());
        }

        return Task.FromResult(1);
    }
}
