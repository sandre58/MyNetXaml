// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MyNet.Xaml.Merger.Helpers;
using MyNet.Xaml.Merger.XAMLColorSchemeGenerator;

namespace MyNet.Xaml.Merger.MSBuild
{
    public class XAMLColorSchemeGeneratorTask : Task
    {
        public const string ParametersFileMetadataName = "ParametersFile";

        public const string OutputPathMetadataName = "OutputPath";

        [Required]
        public ITaskItem[] Items { get; set; } = null!;

        [Output]
        public ITaskItem[]? GeneratedFiles { get; set; }

        public override bool Execute()
        {
            var generatedFiles = new List<ITaskItem>();

            foreach (var item in Items)
            {
                var templateFile = item.ItemSpec;
                var generatorParametersFile = item.GetMetadata(ParametersFileMetadataName);
                var outputPath = item.GetMetadata(OutputPathMetadataName);

                BuildEngine.LogMessageEvent(new BuildMessageEventArgs($"Generating XAML files from \"{templateFile}\" with \"{generatorParametersFile}\" to \"{outputPath}\".", string.Empty, nameof(XAMLColorSchemeGeneratorTask), MessageImportance.High));

                var generator = new ColorSchemeGenerator
                {
                    Logger = new MSBuildLogger(BuildEngine, nameof(XAMLColorSchemeGeneratorTask))
                };
                var currentGeneratedFiles = MutexHelper.ExecuteLocked(() => generator.GenerateColorSchemeFiles(generatorParametersFile, templateFile, outputPath), templateFile);

                foreach (var generatedFile in currentGeneratedFiles)
                {
                    generatedFiles.Add(new TaskItem(generatedFile));
                }
            }

            GeneratedFiles = [.. generatedFiles];

            return true;
        }
    }
}
