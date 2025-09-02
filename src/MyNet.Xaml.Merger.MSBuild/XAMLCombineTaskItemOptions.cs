// -----------------------------------------------------------------------
// <copyright file="XAMLCombineTaskItemOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Build.Framework;
using MyNet.Xaml.Merger.XAMLCombine;

namespace MyNet.Xaml.Merger.MSBuild;

public sealed class XAMLCombineTaskItemOptions : IXamlCombinerOptions, IEquatable<XAMLCombineTaskItemOptions>
{
    public string TargetFile { get; private set; } = string.Empty;

    public bool ImportMergedResourceDictionaryReferences { get; private set; } = XAMLCombiner.ImportMergedResourceDictionaryReferencesDefault;

    public bool WriteFileHeader { get; private set; } = XAMLCombiner.WriteFileHeaderDefault;

    public string FileHeader { get; private set; } = XAMLCombiner.FileHeaderDefault;

    public bool IncludeSourceFilesInFileHeader { get; private set; } = XAMLCombiner.IncludeSourceFilesInFileHeaderDefault;

    public static XAMLCombineTaskItemOptions From(ITaskItem taskItem)
    {
        var result = new XAMLCombineTaskItemOptions
        {
            TargetFile = taskItem.GetMetadata(nameof(TargetFile)),
            ImportMergedResourceDictionaryReferences = GetBool(taskItem, nameof(ImportMergedResourceDictionaryReferences), XAMLCombiner.ImportMergedResourceDictionaryReferencesDefault),
            WriteFileHeader = GetBool(taskItem, nameof(WriteFileHeader), XAMLCombiner.WriteFileHeaderDefault),
            FileHeader = GetString(taskItem, nameof(FileHeader), XAMLCombiner.FileHeaderDefault),
            IncludeSourceFilesInFileHeader = GetBool(taskItem, nameof(IncludeSourceFilesInFileHeader), XAMLCombiner.IncludeSourceFilesInFileHeaderDefault),
        };

        return result;
    }

    public override bool Equals(object? obj) => !(obj is not XAMLCombineTaskItemOptions other) && Equals(other);

    public bool Equals(XAMLCombineTaskItemOptions? other) => other is not null && (ReferenceEquals(this, other)
               || (TargetFile == other.TargetFile
               && ImportMergedResourceDictionaryReferences == other.ImportMergedResourceDictionaryReferences
               && WriteFileHeader == other.WriteFileHeader
               && FileHeader == other.FileHeader
               && IncludeSourceFilesInFileHeader == other.IncludeSourceFilesInFileHeader));

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TargetFile?.GetHashCode(StringComparison.InvariantCultureIgnoreCase) ?? 0;
            hashCode = (hashCode * 397) ^ ImportMergedResourceDictionaryReferences.GetHashCode();
            hashCode = (hashCode * 397) ^ WriteFileHeader.GetHashCode();
            hashCode = (hashCode * 397) ^ FileHeader.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
            hashCode = (hashCode * 397) ^ IncludeSourceFilesInFileHeader.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(XAMLCombineTaskItemOptions? left, XAMLCombineTaskItemOptions? right) => Equals(left, right);

    public static bool operator !=(XAMLCombineTaskItemOptions? left, XAMLCombineTaskItemOptions? right) => !Equals(left, right);

    private static bool GetBool(ITaskItem taskItem, string metadataName, bool defaultValue)
    {
        var metaData = taskItem.GetMetadata(metadataName);

        return bool.TryParse(metaData, out var value)
            ? value
            : defaultValue;
    }

    private static string GetString(ITaskItem taskItem, string metadataName, string defaultValue)
    {
        var metaData = taskItem.GetMetadata(metadataName);

        return string.IsNullOrEmpty(metaData) ? defaultValue : metaData;
    }
}
