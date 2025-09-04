// -----------------------------------------------------------------------
// <copyright file="BaseOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandLine;

namespace MyNet.Xaml.Merger.Console.Commands;

internal class BaseOptions
{
    [Option('v', HelpText = "Defines if logging should be verbose")]
    public bool Verbose { get; set; }
}
