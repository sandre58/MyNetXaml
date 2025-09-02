// -----------------------------------------------------------------------
// <copyright file="TraceLogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace MyNet.Xaml.Merger;

public class TraceLogger : ILogger
{
    public void Debug(string message) => Trace.TraceInformation(message);

    public void Info(string message) => Trace.TraceInformation(message);

    public void InfoImportant(string message) => Trace.TraceInformation(message);

    public void Warn(string message) => Trace.TraceWarning(message);

    public void Error(string message) => Trace.TraceError(message);
}
