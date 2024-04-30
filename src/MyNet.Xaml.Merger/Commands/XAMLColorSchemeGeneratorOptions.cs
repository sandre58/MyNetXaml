// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using CommandLine;
using JetBrains.Annotations;
using MyNet.Xaml.Merger.Core.Helpers;
using MyNet.Xaml.Merger.Core.XAMLColorSchemeGenerator;

namespace MyNet.Xaml.Merger.Commands;

[PublicAPI]
[Verb("colorscheme", HelpText = "Generate XAML color scheme files.")]
public class XAMLColorSchemeGeneratorOptions : BaseOptions
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
