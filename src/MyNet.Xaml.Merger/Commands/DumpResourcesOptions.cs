// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using CommandLine;
using JetBrains.Annotations;
using MyNet.Xaml.Merger.Core.ResourceDump;

namespace MyNet.Xaml.Merger.Commands;

[PublicAPI]
[Verb("dump-resources", HelpText = "Generate XAML color scheme files.")]
public class DumpResourcesOptions : BaseOptions
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
