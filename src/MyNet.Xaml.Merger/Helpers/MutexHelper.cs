// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;

namespace MyNet.Xaml.Merger.Helpers;

public static class MutexHelper
{
    public static T ExecuteLocked<T>(Func<T> action, string file, TimeSpan? timeout = null, string errorMessage = "Another instance of this application blocked the concurrent execution.", string? caller = null)
    {
        var mutexName = "Local\\XamlMerger_" + Path.GetFileName(file);

        using var mutex = new Mutex(false, mutexName);
        var acquired = false;

        try
        {
            acquired = mutex.WaitOne(timeout ?? TimeSpan.FromSeconds(10));
            return !acquired ? throw new TimeoutException(errorMessage) : action();
        }
        finally
        {
            if (acquired)
            {
                mutex.ReleaseMutex();
            }
        }
    }

    public static void ExecuteLocked(Action action, string file, TimeSpan? timeout = null, string errorMessage = "Another instance of this application blocked the concurrent execution.", string? caller = null)
    {
        var mutexName = "Local\\XamlMerger_" + Path.GetFileName(file);

        using var mutex = new Mutex(false, mutexName);
        var acquired = false;

        try
        {
            acquired = mutex.WaitOne(timeout ?? TimeSpan.FromSeconds(10));
            if (!acquired)
            {
                throw new TimeoutException(errorMessage);
            }

            action();
        }
        finally
        {
            if (acquired)
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
