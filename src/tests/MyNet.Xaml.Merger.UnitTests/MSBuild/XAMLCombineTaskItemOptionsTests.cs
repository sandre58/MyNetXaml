// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Utilities;
using MyNet.Xaml.Merger.Core.XAMLCombine;
using MyNet.Xaml.Merger.MSBuild;
using NUnit.Framework;

namespace MyNet.Xaml.Merger.UnitTests.MSBuild;

[TestFixture]
public class XAMLCombineTaskItemOptionsTests
{
    [Test]
    public void TestFromEmptyTaskItem()
    {
        var options = XAMLCombineTaskItemOptions.From(new TaskItem());

        Assert.That(options.TargetFile, Is.Empty);
        Assert.That(options.ImportMergedResourceDictionaryReferences, Is.EqualTo(XAMLCombiner.ImportMergedResourceDictionaryReferencesDefault));
        Assert.That(options.WriteFileHeader, Is.EqualTo(XAMLCombiner.WriteFileHeaderDefault));
        Assert.That(options.FileHeader, Is.EqualTo(XAMLCombiner.FileHeaderDefault));
        Assert.That(options.IncludeSourceFilesInFileHeader, Is.EqualTo(XAMLCombiner.IncludeSourceFilesInFileHeaderDefault));
    }

    [Test]
    public void TestEquality()
    {
        var options = XAMLCombineTaskItemOptions.From(new TaskItem());

        Assert.That(options, Is.EqualTo(new XAMLCombineTaskItemOptions()));
    }
}
