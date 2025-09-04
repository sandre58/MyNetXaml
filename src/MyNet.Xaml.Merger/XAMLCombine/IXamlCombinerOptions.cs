// -----------------------------------------------------------------------
// <copyright file="IXamlCombinerOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Xaml.Merger.XAMLCombine;

public interface IXamlCombinerOptions
{
    bool ImportMergedResourceDictionaryReferences { get; }

    bool WriteFileHeader { get; }

    string FileHeader { get; }

    bool IncludeSourceFilesInFileHeader { get; }
}
