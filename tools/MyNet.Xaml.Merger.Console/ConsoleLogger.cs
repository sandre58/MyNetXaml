// -----------------------------------------------------------------------
// <copyright file="ConsoleLogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Xaml.Merger.Console;

internal sealed class ConsoleLogger : ILogger
{
    public bool Verbose { get; set; }

    public void Debug(string message) => Info(message);

    public void Info(string message)
    {
        if (!Verbose)
        {
            return;
        }

        System.Console.WriteLine(message);
    }

    public void InfoImportant(string message) => System.Console.WriteLine(message);

    public void Warn(string message)
    {
        var foreground = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = foreground;
    }

    public void Error(string message)
    {
        var foreground = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        System.Console.Error.WriteLine(message);
        System.Console.ForegroundColor = foreground;
    }
}
