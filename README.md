# XAML Merger

[![Build status](https://img.shields.io/github/actions/workflow/status/sandre58/MyNetXaml/ci.yml?logo=github&label=CI&style=flat-square)](https://github.com/sandre58/MyNetXaml/actions)
[![Release](https://img.shields.io/github/release/sandre58/MyNetXaml.svg?style=flat-square)](https://github.com/sandre58/MyNetXaml/releases/latest)
[![Issues](https://img.shields.io/github/issues/sandre58/MyNetXaml.svg?style=flat-square)](https://github.com/sandre58/MyNetXaml/issues)
[![Downloads](https://img.shields.io/nuget/dt/MyNet.Xaml.Merger.MSBuild.svg?style=flat-square)](http://www.nuget.org/packages/MyNet.Xaml.Merger.MSBuild/)
[![Nuget](https://img.shields.io/nuget/vpre/MyNet.Xaml.Merger.MSBuild.svg?style=flat-square)](http://nuget.org/packages/MyNet.Xaml.Merger.MSBuild)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://github.com/sandre58/MyNetXaml/blob/fork/License.txt)

Various tools for easing the development of XAML related applications.

As i only use WPF myself everything is focused on WPF, but things should work for other XAML dialects (at least in theory).

You can either use the commandline tool `MyNet.Xaml.Merger` or the MSBuild version `MyNet.Xaml.Merger.MSBuild` to make use of the provided functionalities.

## XAMLCombine

Combines multiple XAML files to one large file.  
This is useful when you want to provide one `Generic.xaml` instead of multiple small XAML files.  
Using one large XAML file not only makes it easier to consume, but can also drastically improving loading performance.

### Using the MSBuild-Task

```
<XAMLCombineItems Include="Themes/Controls/*.xaml">
  <TargetFile>Themes/Generic.xaml</TargetFile>
</XAMLCombineItems>
```

The MSBuild-Task includes the items used for combining as pages during debug builds and removes them from pages during release builds.
This is done to reduce the binary size for release builds and still enable intellisense in debug builds for those XAML files.

**Remarks when using Rider**  
To get intellisense in debug builds inside the XAML files and to prevent duplicate display of those files you have to define:

```
<PropertyGroup Condition="'$(IsBuildingInsideRider)' == 'True'">
  <DefaultItemExcludes>$(DefaultItemExcludes);Themes\Controls\*.xaml</DefaultItemExcludes>
</PropertyGroup>

<ItemGroup Condition="'$(IsBuildingInsideRider)' == 'True'">
  <Page Include="Themes\Controls\*.xaml" />
</ItemGroup>
```

### Using the executable

`XAMLTools` accepts the following commandline parameters for the `combine` verb:

- `-s "Path_To_Your_SourceFile"` => A file containing a new line separated list of files to combine (lines starting with # are skipped)
- `-t "Path_To_Your_Target_File.xaml"`

## XAMLColorSchemeGenerator

Generates color scheme XAML files while replacing certain parts of a template file.

For an example on how this tool works see the [generator input](src/XAMLTools.Core/XAMLColorSchemeGenerator/GeneratorParameters.json) and [template](src/XAMLTools.Core/XAMLColorSchemeGenerator/ColorScheme.Template.xaml) files.

### Using the MSBuild-Task

```
<XAMLColorSchemeGeneratorItems Include="Themes\ColorScheme.Template.xaml">
  <ParametersFile>Themes\GeneratorParameters.json</ParametersFile>
  <OutputPath>Themes\ColorSchemes</OutputPath>
</XAMLColorSchemeGeneratorItems>
```

### Using the executable

`XAMLTools` accepts the following commandline parameters for the `colorscheme` verb:

- `-p "Path_To_Your_GeneratorParameters.json"`
- `-t "Path_To_Your_ColorScheme.Template.xaml"`
- `-o "Path_To_Your_Output_Folder"`
