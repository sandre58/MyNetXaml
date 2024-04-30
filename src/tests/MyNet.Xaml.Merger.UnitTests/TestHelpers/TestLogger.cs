// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MyNet.Xaml.Merger.Core;

namespace MyNet.Xaml.Merger.UnitTests.TestHelpers;

public class TestLogger : ILogger
{
    public List<string> Warnings { get; } = [];

    public List<string> Errors { get; } = [];

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
