// -----------------------------------------------------------------------
// <copyright file="MSBuildLogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Build.Framework;

namespace MyNet.Xaml.Merger.MSBuild;

public class MSBuildLogger(IBuildEngine buildEngine, string senderName) : ILogger
{
    private readonly IBuildEngine _buildEngine = buildEngine;
    private readonly string _senderName = senderName;

    public void Debug(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.Low));

    public void Info(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.Normal));

    public void InfoImportant(string message) => _buildEngine.LogMessageEvent(new(message, string.Empty, _senderName, MessageImportance.High));

    public void Warn(string message) => _buildEngine.LogWarningEvent(new(string.Empty, string.Empty, string.Empty, 0, 0, 0, 0, message, string.Empty, _senderName));

    public void Error(string message) => _buildEngine.LogErrorEvent(new(string.Empty, string.Empty, string.Empty, 0, 0, 0, 0, message, string.Empty, _senderName));
}
