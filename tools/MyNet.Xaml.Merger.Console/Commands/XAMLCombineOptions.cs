// -----------------------------------------------------------------------
// <copyright file="XAMLCombineOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using CommandLine;
using MyNet.Xaml.Merger.Helpers;
using MyNet.Xaml.Merger.XAMLCombine;

namespace MyNet.Xaml.Merger.Console.Commands;

[Verb("combine", HelpText = "Combine multiple XAML files to one target file.")]
internal sealed class XAMLCombineOptions : BaseOptions, IXamlCombinerOptions
{
    [Option('s', Required = true, HelpText = "Source file containing a new line separated list of files to combine")]
    public string SourceFile { get; set; } = null!;

    [Option('t', Required = true, HelpText = "Target file")]
    public string TargetFile { get; set; } = null!;

    [Option("md", Required = false, HelpText = "Import merged dictionary references from combined files to generated")]
    public bool ImportMergedResourceDictionaryReferences { get; set; } = false;

    [Option("WriteFileHeader", Required = false, HelpText = "Write file header or not")]
    public bool WriteFileHeader { get; set; } = XAMLCombiner.WriteFileHeaderDefault;

    [Option("FileHeader", Required = false, HelpText = "Text written as the file header")]
    public string FileHeader { get; set; } = XAMLCombiner.FileHeaderDefault;

    [Option("IncludeSourceFilesInFileHeader", Required = false, HelpText = "Include source files in file header")]
    public bool IncludeSourceFilesInFileHeader { get; set; } = XAMLCombiner.IncludeSourceFilesInFileHeaderDefault;

    public Task<int> Execute()
    {
        var combiner = new XAMLCombiner
        {
            ImportMergedResourceDictionaryReferences = ImportMergedResourceDictionaryReferences,
            WriteFileHeader = WriteFileHeader,
            FileHeader = FileHeader,
            IncludeSourceFilesInFileHeader = IncludeSourceFilesInFileHeader,
            Logger = new ConsoleLogger
            {
                Verbose = Verbose
            }
        };

        try
        {
            MutexHelper.ExecuteLocked(() => combiner.Combine(SourceFile, TargetFile), TargetFile);
        }
        catch (Exception)
        {
            return Task.FromResult(1);
        }

        return Task.FromResult(0);
    }
}
