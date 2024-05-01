// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Xml.Linq;

namespace MyNet.Xaml.Merger.XAMLCombine
{
    /// <summary>
    /// Represents a XAML resource.
    /// </summary>
    public class ResourceElement
    {
        public ResourceElement(string key, XElement element, string[] usedKeys)
        {
            Key = key;
            Element = element;
            UsedKeys = usedKeys;
        }

        /// <summary>
        /// Resource key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Resource XML node.
        /// </summary>
        public XElement Element { get; }

        /// <summary>
        /// XAML keys used in this resource.
        /// </summary>
        public string[] UsedKeys { get; }

        public string? ElementDebugInfo { get; set; }

        public string? GetElementDebugInfo() => !string.IsNullOrEmpty(ElementDebugInfo) ? ElementDebugInfo : Element.ToString();
    }
}
