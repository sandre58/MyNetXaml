// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MyNet.Xaml.Merger.Core.Helpers;
using MyNet.Xaml.Merger.Core.XAMLCombine;

namespace MyNet.Xaml.Merger.MSBuild
{
    public class XAMLCombineTask : Task
    {
        [Required]
        public ITaskItem[] Items { get; set; } = null!;

        [Output]
        public ITaskItem[]? GeneratedFiles { get; set; }

        public override bool Execute()
        {
            var generatedFiles = new List<ITaskItem>();

            var grouped = Items.GroupBy(XAMLCombineTaskItemOptions.From);

            foreach (var group in grouped)
            {
                var options = group.Key;
                var targetFile = options.TargetFile;

                if (targetFile is null or { Length: 0 })
                {
                    continue;
                }

                var sourceFiles = group.Select(x => x.ItemSpec).ToList();

                BuildEngine.LogMessageEvent(new BuildMessageEventArgs($"Generating combined XAML file \"{targetFile}\".", string.Empty, nameof(XAMLCombineTask), MessageImportance.High));

                if (options.ImportMergedResourceDictionaryReferences)
                {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs($"Import for merged ResourceDictionary elements enabled for this generated content", string.Empty, "XAMLCombine", MessageImportance.Low));
                }

                var combiner = new XAMLCombiner
                {
                    ImportMergedResourceDictionaryReferences = options.ImportMergedResourceDictionaryReferences,
                    WriteFileHeader = options.WriteFileHeader,
                    FileHeader = options.FileHeader,
                    IncludeSourceFilesInFileHeader = options.IncludeSourceFilesInFileHeader,
                    Logger = new MSBuildLogger(BuildEngine, nameof(XAMLCombineTask))
                };

                try
                {
                    targetFile = MutexHelper.ExecuteLocked(() => combiner.Combine(sourceFiles, targetFile), targetFile);
                }
                catch (Exception exception)
                {
                    BuildEngine.LogErrorEvent(new BuildErrorEventArgs("XAMLCombine", "XAMLCombine_Exception", string.Empty, 0, 0, 0, 0, exception.ToString(), string.Empty, "XAMLCombine"));
                    return false;
                }

                generatedFiles.Add(new TaskItem(targetFile));
            }

            GeneratedFiles = [.. generatedFiles];

            return true;
        }
    }
}
