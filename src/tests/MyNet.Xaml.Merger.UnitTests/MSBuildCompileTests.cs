// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using NUnit.Framework;

namespace MyNet.Xaml.Merger.UnitTests;

[TestFixture]
public class MSBuildCompileTests
{
    [Test]
    [TestCase("Debug")]
    [TestCase("Release")]
    public async Task CheckCompileOutputAfterGitCleanAsync(string configuration)
    {
        var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
        var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));

        Assert.That(wpfAppDirectory, Does.Exist);

        const string assemblyName = "MyNet.Xaml.Merger.Wpf.TestApp.dll";
        const string framework = "net8.0-windows";

        var binPath = Path.Combine(wpfAppDirectory, "bin", configuration, framework);

        var result = await Cli.Wrap("git")
                              .WithArguments($"clean -fxd")
                              .WithWorkingDirectory(Path.Combine(wpfAppDirectory, "Themes"))
                              .WithValidation(CommandResultValidation.None)
                              .ExecuteBufferedAsync();

        Assert.That(result.ExitCode, Is.EqualTo(0), result.StandardError);

        result = await Cli.Wrap("dotnet")
                       .WithArguments($"build -c {configuration} /p:XAMLColorSchemeGeneratorEnabled=true /p:XAMLCombineEnabled=true /nr:false --no-dependencies -v:diag")
                       .WithWorkingDirectory(wpfAppDirectory)
                       .WithValidation(CommandResultValidation.None)
                       .ExecuteBufferedAsync();

        Assert.That(result.ExitCode, Is.EqualTo(0), result.StandardOutput);

        var assemblyFile = Path.Combine(binPath, assemblyName);
        var outputPath = Path.GetDirectoryName(assemblyFile)!;

        var xamlMergerExe = Path.Combine(currentAssemblyDir, "XamlMerger.exe");
        Assert.That(xamlMergerExe, Does.Exist);

        result = await Cli.Wrap(xamlMergerExe)
                              .WithArguments($"dump-resources -a \"{assemblyFile}\" -o \"{outputPath}\"")
                              .WithWorkingDirectory(currentAssemblyDir)
                              .WithValidation(CommandResultValidation.None)
                              .ExecuteBufferedAsync();

        Assert.That(result.ExitCode, Is.EqualTo(0), result.StandardError);

        var resourceNames = File.ReadAllLines(Path.Combine(outputPath, "ResourceNames"));

        Assert.That(resourceNames, Is.EquivalentTo(new[]
        {
                "MyNet.Xaml.Merger.Wpf.TestApp.g.resources",
                "MyNet.Xaml.Merger.Wpf.TestApp.Themes.ColorScheme.Template.xaml",
                "MyNet.Xaml.Merger.Wpf.TestApp.Themes.GeneratorParameters.json"
            }));

        var xamlResourceNames = File.ReadAllLines(Path.Combine(outputPath, "XAMLResourceNames"));

        if (configuration == "Debug")
        {
            Assert.That(xamlResourceNames, Is.EquivalentTo(
                            new[]
                            {
                                    "themes/colorschemes/light.yellow.colorful.baml",
                                    "themes/colorschemes/dark.yellow.colorful.baml",
                                    "themes/colorschemes/light.yellow.baml",
                                    "themes/colorschemes/dark.blue.colorful.baml",
                                    "themes/colorschemes/dark.green.colorful.highcontrast.baml",
                                    "themes/colorschemes/dark.yellow.baml",
                                    "themes/generic.baml",
                                    "themes/colorschemes/light.blue.baml",
                                    "themes/colorschemes/light.blue.colorful.baml",
                                    "mainwindow.baml",
                                    "themes/colorschemes/dark.green.highcontrast.baml",
                                    "themes/colorschemes/dark.blue.baml",
                                    "themes/colorschemes/light.green.highcontrast.baml",
                                    "themes/controls/control3.baml",
                                    "themes/controls/control2.baml",
                                    "themes/controls/control1.baml",
                                    "themes/controls/zconverters.baml",
                            }));
        }
        else
        {
            Assert.That(xamlResourceNames, Is.EquivalentTo(
                            new[]
                            {
                                    "themes/colorschemes/light.yellow.colorful.baml",
                                    "themes/colorschemes/dark.yellow.colorful.baml",
                                    "themes/colorschemes/light.yellow.baml",
                                    "themes/colorschemes/dark.blue.colorful.baml",
                                    "themes/colorschemes/dark.green.colorful.highcontrast.baml",
                                    "themes/colorschemes/dark.yellow.baml",
                                    "themes/generic.baml",
                                    "themes/colorschemes/light.blue.baml",
                                    "themes/colorschemes/light.blue.colorful.baml",
                                    "mainwindow.baml",
                                    "themes/colorschemes/dark.green.highcontrast.baml",
                                    "themes/colorschemes/dark.blue.baml",
                                    "themes/colorschemes/light.green.highcontrast.baml"
                            }));
        }
    }
}
