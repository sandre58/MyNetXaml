// -----------------------------------------------------------------------
// <copyright file="TestLogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace MyNet.Xaml.Merger.UnitTests.TestHelpers;

internal sealed class TestLogger : ILogger
{
    public Collection<string> Warnings { get; } = [];

    public Collection<string> Errors { get; } = [];

    public void Debug(string message)
    {
    }

    public void Info(string message)
    {
    }

    public void InfoImportant(string message)
    {
    }

    public void Warn(string message) => Warnings.Add(message);

    public void Error(string message) => Errors.Add(message);
}
