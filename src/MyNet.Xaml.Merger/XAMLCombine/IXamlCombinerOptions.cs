// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Xaml.Merger.XAMLCombine;

public interface IXamlCombinerOptions
{
    bool ImportMergedResourceDictionaryReferences { get; }

    bool WriteFileHeader { get; }

    string FileHeader { get; }

    bool IncludeSourceFilesInFileHeader { get; }
}
