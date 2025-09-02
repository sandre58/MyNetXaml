// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Xaml.Merger;

public interface ILogger
{
    void Debug(string message);

    void Info(string message);

    void InfoImportant(string message);

    void Warn(string message);

    void Error(string message);
}
