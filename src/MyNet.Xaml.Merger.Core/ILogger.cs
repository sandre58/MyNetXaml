// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.Xaml.Merger.Core;

public interface ILogger
{
    void Debug(string message);

    void Info(string message);

    void InfoImportant(string message);

    void Warn(string message);

    void Error(string message);
}
