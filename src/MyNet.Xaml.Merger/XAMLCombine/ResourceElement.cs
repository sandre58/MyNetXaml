// -----------------------------------------------------------------------
// <copyright file="ResourceElement.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Xml.Linq;

namespace MyNet.Xaml.Merger.XAMLCombine;

/// <summary>
/// Represents a XAML resource.
/// </summary>
public class ResourceElement(string key, XElement element, string[] usedKeys)
{
    /// <summary>
    /// Gets resource key.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Gets resource XML node.
    /// </summary>
    public XElement Element { get; } = element;

    /// <summary>
    /// Gets xAML keys used in this resource.
    /// </summary>
    public string[] UsedKeys { get; } = usedKeys;

    public string? ElementDebug { get; set; }

    public string? GetElementDebugInfo() => !string.IsNullOrEmpty(ElementDebug) ? ElementDebug : Element.ToString();
}
