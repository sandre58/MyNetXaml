// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

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
