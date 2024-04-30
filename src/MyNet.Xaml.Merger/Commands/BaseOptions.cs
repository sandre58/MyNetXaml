// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using CommandLine;
using JetBrains.Annotations;

namespace MyNet.Xaml.Merger.Commands;

[PublicAPI]
public class BaseOptions
{
    [Option('v', HelpText = "Defines if logging should be verbose")]
    public bool Verbose { get; set; }
}
