// -----------------------------------------------------------------------
// <copyright file="MSBuildCompileTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Xunit;

namespace MyNet.Xaml.Merger.UnitTests;

public class MSBuildCompileTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task CheckCompileOutputAfterGitCleanAsync()
    {
        var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
        var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../../demos/MyNet.Xaml.Merger.Wpf.Demo"));

        Assert.True(Directory.Exists(wpfAppDirectory));

        const string assemblyName = "MyNet.Xaml.Merger.Wpf.Demo.dll";

        var assemblyConfigurationAttribute = GetType().Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
        var configuration = assemblyConfigurationAttribute?.Configuration;
        var binPath = Path.GetFullPath(Path.Combine(wpfAppDirectory, "bin", configuration ?? string.Empty));

        var result = await Cli.Wrap("git")
                              .WithArguments("clean -fxd")
                              .WithWorkingDirectory(Path.Combine(wpfAppDirectory, "Themes"))
                              .WithValidation(CommandResultValidation.None)
                              .ExecuteBufferedAsync();

        Assert.Equal(0, result.ExitCode);

        result = await Cli.Wrap("dotnet")
                          .WithArguments("build /p:XAMLColorSchemeGeneratorEnabled=true /p:XAMLCombineEnabled=true /nr:false --no-dependencies -v:diag")
                          .WithWorkingDirectory(wpfAppDirectory)
                          .WithValidation(CommandResultValidation.None)
                          .ExecuteBufferedAsync();

        Assert.Equal(0, result.ExitCode);

        var assemblyFile = Path.Combine(binPath, assemblyName);
        var outputPath = Path.GetDirectoryName(assemblyFile)!;

        var xamlMergerExe = Path.Combine(currentAssemblyDir, "XamlMerger.exe");
        Assert.True(File.Exists(xamlMergerExe));

        result = await Cli.Wrap(xamlMergerExe)
                          .WithArguments($"dump-resources -a \"{assemblyFile}\" -o \"{outputPath}\"")
                          .WithWorkingDirectory(currentAssemblyDir)
                          .WithValidation(CommandResultValidation.None)
                          .ExecuteBufferedAsync();

        Assert.True(result.ExitCode <= 0);

        var resourceNames = await File.ReadAllLinesAsync(Path.Combine(outputPath, "ResourceNames")).ConfigureAwait(true);

        Assert.Equal(new[]
        {
            "MyNet.Xaml.Merger.Wpf.Demo.g.resources",
            "MyNet.Xaml.Merger.Wpf.Demo.Themes.GeneratorParameters.json",
            "MyNet.Xaml.Merger.Wpf.Demo.Themes.ColorScheme.Template.xaml"
        },
        resourceNames);

        var xamlResourceNames = await File.ReadAllLinesAsync(Path.Combine(outputPath, "XAMLResourceNames")).ConfigureAwait(true);

        Assert.Equal(new[]
        {
            "themes/colorschemes/light.yellow.colorful.baml",
            "themes/colorschemes/dark.yellow.colorful.baml",
            "themes/colorschemes/light.yellow.baml",
            "themes/colorschemes/dark.blue.colorful.baml",
            "themes/colorschemes/dark.green.colorful.highcontrast.baml",
            "themes/controls/zconverters.baml",
            "themes/colorschemes/dark.yellow.baml",
            "themes/generic.baml",
            "themes/colorschemes/light.blue.baml",
            "themes/colorschemes/light.blue.colorful.baml",
            "mainwindow.baml",
            "themes/colorschemes/dark.green.highcontrast.baml",
            "themes/colorschemes/dark.blue.baml",
            "themes/colorschemes/light.green.highcontrast.baml",
            "themes/controls/control2.baml",
            "themes/controls/control1.baml",
            "themes/controls/control3.baml"
        },
        xamlResourceNames);
    }
}
