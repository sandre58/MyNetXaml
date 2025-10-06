<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="assets/MyXaml.png" width="256" height="256" alt="MyXaml">
</div>

<h1 align="center">My .NET</h1>

[![MIT License][license-shield]][license-url]
[![GitHub Stars](https://img.shields.io/github/stars/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml/network/members)
[![GitHub Issues](https://img.shields.io/github/issues/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml/issues)
[![Last Commit](https://img.shields.io/github/last-commit/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml/commits/main)
[![Contributors](https://img.shields.io/github/contributors/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml/graphs/contributors)
[![Repo Size](https://img.shields.io/github/repo-size/sandre58/myxaml?style=for-the-badge)](https://github.com/sandre58/myxaml)

Various tools for easing the development of XAML related applications.

As i only use WPF myself everything is focused on WPF, but things should work for other XAML dialects (at least in theory).

You can either use the commandline tool `MyNet.Xaml.Merger` or the MSBuild version `MyNet.Xaml.Merger.MSBuild` to make use of the provided functionalities.

[![Build][build-shield]][build-url]
[![Coverage](https://codecov.io/gh/sandre58/myxaml/branch/main/graph/badge.svg)](https://codecov.io/gh/sandre58/myxaml)
[![C#](https://img.shields.io/badge/language-C%23-blue)](#)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-purple)](#)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-purple)](#)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-purple)](#)

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

For an example on how this tool works see the [generator input](src/MyNet.Xaml.Merger/XAMLColorSchemeGenerator/GeneratorParameters.json) and [template](src/MyNet.Xaml.Merger/XAMLColorSchemeGenerator/ColorScheme.Template.xaml) files.

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

## License

Copyright © Stéphane ANDRE.

Distributed under the MIT License. See [LICENSE](./LICENSE) for details.

<!-- MARKDOWN LINKS & IMAGES -->
[license-shield]: https://img.shields.io/github/license/sandre58/MyNet?style=for-the-badge
[license-url]: https://github.com/sandre58/MyNet/blob/main/LICENSE
[build-shield]: https://img.shields.io/github/actions/workflow/status/sandre58/MyNet/ci.yml?logo=github&label=CI
[build-url]: https://github.com/sandre58/MyNet/actions