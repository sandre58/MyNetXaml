// -----------------------------------------------------------------------
// <copyright file="ThemeGenerator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

namespace MyNet.Xaml.Merger.XAMLColorSchemeGenerator;

// This class has to be kept in sync with https://github.com/ControlzEx/ControlzEx/blob/develop/src/ControlzEx/Theming/ThemeGenerator.cs
// Please do not remove unused code/properties here as it makes syncing more difficult.
public class ThemeGenerator
{
    public static ThemeGenerator Current { get; set; }

    static ThemeGenerator() => Current = new ThemeGenerator();

    public virtual ThemeGeneratorParameters GetParametersFromString(string input) => System.Text.Json.JsonSerializer.Deserialize<ThemeGeneratorParameters>(input) ?? new ThemeGeneratorParameters();

    // The order of the passed valueSources is important.
    // More specialized/concrete values must be passed first and more generic ones must follow.
    public virtual string GenerateColorSchemeFileContent(string templateContent, string themeName, string themeDisplayName, string baseColorScheme, string colorScheme, string alternativeColorScheme, bool isHighContrast, params Dictionary<string, string>[] valueSources)
    {
        templateContent = templateContent.Replace("{{ThemeName}}", themeName, System.StringComparison.InvariantCultureIgnoreCase)
            .Replace("{{ThemeDisplayName}}", themeDisplayName, System.StringComparison.InvariantCultureIgnoreCase)
            .Replace("{{BaseColorScheme}}", baseColorScheme, System.StringComparison.InvariantCultureIgnoreCase)
            .Replace("{{ColorScheme}}", colorScheme, System.StringComparison.InvariantCultureIgnoreCase)
            .Replace("{{AlternativeColorScheme}}", alternativeColorScheme, System.StringComparison.InvariantCultureIgnoreCase)
            .Replace("{{IsHighContrast}}", isHighContrast.ToString(), System.StringComparison.InvariantCultureIgnoreCase);

        bool contentChanged;

        // Loop till content does not change anymore.
        do
        {
            contentChanged = false;

            foreach (var valueSource in valueSources)
            {
                foreach (var value in valueSource)
                {
                    var finalValue = value.Value;
                    var newTemplateContent = templateContent.Replace($"{{{{{value.Key}}}}}", finalValue, System.StringComparison.InvariantCultureIgnoreCase);

                    if (templateContent != newTemplateContent)
                    {
                        contentChanged = true;
                    }

                    templateContent = newTemplateContent;
                }
            }
        }
        while (contentChanged);

        return templateContent;
    }
}

public class ThemeGeneratorParameters
{
    public Dictionary<string, string> DefaultValues { get; } = [];

    public ThemeGeneratorBaseColorScheme[] BaseColorSchemes { get; set; } = [];

    public ThemeGeneratorColorScheme[] ColorSchemes { get; set; } = [];

    public AdditionalColorSchemeVariant[] AdditionalColorSchemeVariants { get; set; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class ThemeGeneratorBaseColorScheme
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, string> Values { get; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class AdditionalColorSchemeVariant
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, string> Values { get; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class ThemeGeneratorColorScheme
{
    public string Name { get; set; } = string.Empty;

    public string ForBaseColor { get; set; } = string.Empty;

    public string ForColorSchemeVariant { get; set; } = string.Empty;

    public bool IsHighContrast { get; set; }

    public Dictionary<string, string> Values { get; } = [];
}
