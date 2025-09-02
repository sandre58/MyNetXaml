// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommandLine;
using MyNet.Xaml.Merger.Console.Commands;

namespace MyNet.Xaml.Merger.Console;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = Parser.Default
                               .ParseArguments<XAMLCombineOptions, XAMLColorSchemeGeneratorOptions, DumpResourcesOptions>(args)
                               .MapResult(
                                   async (XAMLCombineOptions options) => await options.Execute().ConfigureAwait(false),
                                   async (XAMLColorSchemeGeneratorOptions options) => await options.Execute().ConfigureAwait(false),
                                   async (DumpResourcesOptions options) => await options.Execute().ConfigureAwait(false),
                                   ErrorHandler);

            return await result.ConfigureAwait(false);
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
            System.Console.Error.WriteLineAsync(error.ToString()).ConfigureAwait(false);
        }

        return Task.FromResult(1);
    }
}
