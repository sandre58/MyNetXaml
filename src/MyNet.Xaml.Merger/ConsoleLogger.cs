// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Xaml.Merger.Core;

namespace MyNet.Xaml.Merger;

public class ConsoleLogger : ILogger
{
    public bool Verbose { get; set; }

    public void Debug(string message) => Info(message);

    public void Info(string message)
    {
        if (!Verbose)
        {
            return;
        }

        Console.WriteLine(message);
    }

    public void InfoImportant(string message) => Console.WriteLine(message);

    public void Warn(string message)
    {
        var foreground = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(message);
        Console.ForegroundColor = foreground;
    }

    public void Error(string message)
    {
        var foreground = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Error.WriteLine(message);
        Console.ForegroundColor = foreground;
    }
}
