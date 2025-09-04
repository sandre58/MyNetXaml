// -----------------------------------------------------------------------
// <copyright file="MutexHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Xaml.Merger.Helpers;
using Xunit;

namespace MyNet.Xaml.Merger.UnitTests
{
    public class MutexHelperTests
    {
        [Fact]
        public async Task MutexHelperTestTimeoutAsync()
        {
            var tasks = Enumerable.Range(0, 5)
                .Select(_ => Task.Run(() => ThreadStartFunction()))
                .ToArray();

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                // Expect TimeoutException, Mutex shoudn't throw any ApplicationException
                Assert.True(ex is TimeoutException, $"InnerException was {ex.GetType().Name}");
            }
        }

        private void ThreadStartFunction()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../../demos/MyNet.Xaml.Merger.Wpf.Demo"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/Controls"));
            var themeFileName = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).FirstOrDefault()!;
            MutexHelper.ExecuteLocked(() => Thread.Sleep(3000), themeFileName, timeout: TimeSpan.FromSeconds(2));
        }
    }
}
