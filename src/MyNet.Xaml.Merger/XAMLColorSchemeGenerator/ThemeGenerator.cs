// -----------------------------------------------------------------------
// <copyright file="ThemeGenerator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace MyNet.Xaml.Merger.XAMLColorSchemeGenerator;

public class ThemeGenerator
{
    public static ThemeGenerator Current { get; set; }

    static ThemeGenerator() => Current = new ThemeGenerator();

    public virtual ThemeGeneratorParameters GetParametersFromString(string input) => JsonSerializer.Deserialize<ThemeGeneratorParameters>(input) ?? new ThemeGeneratorParameters();

    // The order of the passed valueSources is important.
    // More specialized/concrete values must be passed first and more generic ones must follow.
    public virtual string GenerateColorSchemeFileContent(string templateContent, string themeName, string themeDisplayName, string baseColorScheme, string colorScheme, string alternativeColorScheme, bool isHighContrast, params Dictionary<string, string>[] valueSources)
    {
        templateContent = templateContent.Replace("{{ThemeName}}", themeName)
                                         .Replace("{{ThemeDisplayName}}", themeDisplayName)
                                         .Replace("{{BaseColorScheme}}", baseColorScheme)
                                         .Replace("{{ColorScheme}}", colorScheme)
                                         .Replace("{{AlternativeColorScheme}}", alternativeColorScheme)
                                         .Replace("{{IsHighContrast}}", isHighContrast.ToString());

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
                    var newTemplateContent = templateContent.Replace($"{{{{{value.Key}}}}}", finalValue);

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used by serialization")]
    public Dictionary<string, string> DefaultValues { get; set; } = [];

    public ThemeGeneratorBaseColorScheme[] BaseColorSchemes { get; set; } = [];

    public ThemeGeneratorColorScheme[] ColorSchemes { get; set; } = [];

    public AdditionalColorSchemeVariant[] AdditionalColorSchemeVariants { get; set; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class ThemeGeneratorBaseColorScheme
{
    public string Name { get; set; } = string.Empty;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used by serialization")]
    public Dictionary<string, string> Values { get; set; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class AdditionalColorSchemeVariant
{
    public string Name { get; set; } = string.Empty;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used by serialization")]
    public Dictionary<string, string> Values { get; set; } = [];
}

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class ThemeGeneratorColorScheme
{
    public string Name { get; set; } = string.Empty;

    public string ForBaseColor { get; set; } = string.Empty;

    public string ForColorSchemeVariant { get; set; } = string.Empty;

    public bool IsHighContrast { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used by serialization")]
    public Dictionary<string, string> Values { get; set; } = [];
}
