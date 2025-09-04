// -----------------------------------------------------------------------
// <copyright file="XAMLCombineTaskItemOptionsTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Build.Utilities;
using MyNet.Xaml.Merger.MSBuild;
using MyNet.Xaml.Merger.XAMLCombine;
using Xunit;

namespace MyNet.Xaml.Merger.UnitTests.MSBuild;

public class XAMLCombineTaskItemOptionsTests
{
    [Fact]
    public void TestFromEmptyTaskItem()
    {
        var options = XAMLCombineTaskItemOptions.From(new TaskItem());

        Assert.Equal(string.Empty, options.TargetFile);
        Assert.Equal(XAMLCombiner.ImportMergedResourceDictionaryReferencesDefault, options.ImportMergedResourceDictionaryReferences);
        Assert.Equal(XAMLCombiner.WriteFileHeaderDefault, options.WriteFileHeader);
        Assert.Equal(XAMLCombiner.FileHeaderDefault, options.FileHeader);
        Assert.Equal(XAMLCombiner.IncludeSourceFilesInFileHeaderDefault, options.IncludeSourceFilesInFileHeader);
    }

    [Fact]
    public void TestEquality()
    {
        var options = XAMLCombineTaskItemOptions.From(new TaskItem());

        Assert.Equal(new XAMLCombineTaskItemOptions(), options);
    }
}
