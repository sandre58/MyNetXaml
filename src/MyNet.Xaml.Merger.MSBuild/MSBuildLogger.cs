// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Microsoft.Build.Framework;
using ILogger = MyNet.Xaml.Merger.Core.ILogger;

namespace MyNet.Xaml.Merger.MSBuild;

public class MSBuildLogger : ILogger
{
    private readonly IBuildEngine _buildEngine;
    private readonly string _senderName;

    public MSBuildLogger(IBuildEngine buildEngine, string senderName)
    {
        _buildEngine = buildEngine;
        _senderName = senderName;
    }

    public void Debug(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.Low));

    public void Info(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.Normal));

    public void InfoImportant(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.High));

    public void Warn(string message) => _buildEngine.LogWarningEvent(new(string.Empty, string.Empty, string.Empty, 0, 0, 0, 0, message, string.Empty, _senderName));

    public void Error(string message) => _buildEngine.LogErrorEvent(new(string.Empty, string.Empty, string.Empty, 0, 0, 0, 0, message, string.Empty, _senderName));
}
