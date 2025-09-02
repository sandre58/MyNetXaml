// -----------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace MyNet.Xaml.Merger.Helpers;

public static class FileHelper
{
    private const int BufferSize = 32768; // 32 Kilobytes

    public static string ReadAllTextSharedWithRetry(string file, ushort retries = 5)
    {
        for (var i = 0; i < retries; i++)
        {
            try
            {
                return ReadAllTextShared(file);
            }
            catch (IOException) when (i != retries - 1)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        throw new IOException($"File \"{file}\" could not be read.");
    }

    public static string ReadAllTextShared(string file)
    {
        using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);

        using var textReader = new StreamReader(stream, Encoding.UTF8);
        return textReader.ReadToEnd();
    }
}
