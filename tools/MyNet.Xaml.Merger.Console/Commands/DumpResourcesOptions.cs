// -----------------------------------------------------------------------
// <copyright file="DumpResourcesOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using CommandLine;

namespace MyNet.Xaml.Merger.Console.Commands;

[Verb("dump-resources", HelpText = "Generate XAML color scheme files.")]
internal sealed class DumpResourcesOptions : BaseOptions
{
    [Option('a', Required = true, HelpText = "Assembly file")]
    public string AssemblyFile { get; set; } = null!;

    [Option('o', Required = true, HelpText = "Output path")]
    public string OutputPath { get; set; } = null!;

    public Task<int> Execute()
    {
        ResourceDumper.DumpResources(AssemblyFile, OutputPath);

        return Task.FromResult(0);
    }
}
