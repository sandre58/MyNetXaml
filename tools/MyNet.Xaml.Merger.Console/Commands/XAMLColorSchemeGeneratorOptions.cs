// -----------------------------------------------------------------------
// <copyright file="XAMLColorSchemeGeneratorOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using CommandLine;
using MyNet.Xaml.Merger.Helpers;
using MyNet.Xaml.Merger.XAMLColorSchemeGenerator;

namespace MyNet.Xaml.Merger.Console.Commands;

[Verb("colorscheme", HelpText = "Generate XAML color scheme files.")]
internal sealed class XAMLColorSchemeGeneratorOptions : BaseOptions
{
    [Option('p', Required = true, HelpText = "Parameters file")]
    public string ParametersFile { get; set; } = null!;

    [Option('t', Required = true, HelpText = "Template file")]
    public string TemplateFile { get; set; } = null!;

    [Option('o', Required = true, HelpText = "Output path")]
    public string OutputPath { get; set; } = null!;

    public Task<int> Execute()
    {
        var generator = new ColorSchemeGenerator
        {
            Logger = new ConsoleLogger
            {
                Verbose = Verbose
            }
        };

        MutexHelper.ExecuteLocked(() => generator.GenerateColorSchemeFiles(ParametersFile, TemplateFile, OutputPath), TemplateFile);

        return Task.FromResult(0);
    }
}
