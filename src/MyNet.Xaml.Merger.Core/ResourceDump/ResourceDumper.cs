// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyNet.Xaml.Merger.Core.ResourceDump;

public static class ResourceDumper
{
    public static void DumpResources(string assemblyFile, string outputPath)
    {
        assemblyFile = Path.GetFullPath(assemblyFile);
        var assembly = Assembly.Load(assemblyFile);

        var resourceNames = assembly.GetManifestResourceNames();

        var resourceNamesFile = Path.Combine(outputPath, "ResourceNames");
        File.WriteAllLines(resourceNamesFile, resourceNames, Encoding.UTF8);

        var xamlResourceName = Array.Find(resourceNames, x => x.EndsWith(".g.resources"));

        if (!string.IsNullOrEmpty(xamlResourceName))
        {
            using var xamlResourceStream = assembly.GetManifestResourceStream(xamlResourceName)!;
            using var reader = new System.Resources.ResourceReader(xamlResourceStream);
            var xamlResourceNames = reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();

            var xamlResourceNamesFile = Path.Combine(outputPath, "XAMLResourceNames");
            File.WriteAllLines(xamlResourceNamesFile, xamlResourceNames, Encoding.UTF8);
        }
    }
}
