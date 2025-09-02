// -----------------------------------------------------------------------
// <copyright file="XAMLCombinerTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Xaml.Merger.UnitTests.TestHelpers;
using VerifyXunit;
using Xunit;

namespace MyNet.Xaml.Merger.UnitTests
{
    public class XAMLCombinerTests
    {
        private readonly string _targetFile = null!;

        public XAMLCombinerTests()
        {
            _targetFile = Path.Combine(Path.GetTempPath(), "XAMLCombinerTests_Generic.xaml");
            File.Delete(_targetFile);
        }

        [Fact]
        public async Task TestOutputAsync()
        {
            var timeout = Debugger.IsAttached ? 500000 : 5000;
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.Demo"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/Controls"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories);

            var xamlCombiner = new XAMLCombine.XAMLCombiner();

            using (var cts = new CancellationTokenSource())
            {
                var combineTask = Task.Run(() => xamlCombiner.Combine(themeFilePaths, _targetFile), cts.Token);
                var delayTask = Task.Delay(timeout, cts.Token);

                var timeoutTask = Task.WhenAny(combineTask, delayTask).ContinueWith(_ =>
                {
                    if (!combineTask.IsCompleted)
                    {
                        cts.Cancel();
                        throw new TimeoutException("Timeout waiting for method after " + timeout);
                    }

                    Assert.Null(combineTask.Exception);
                },
                cts.Token,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

                await timeoutTask.ConfigureAwait(true);
            }

            await Verifier.VerifyFile(_targetFile);
        }

        [Fact]
        public async Task TestOutputWinUIAsync()
        {
            var timeout = Debugger.IsAttached ? 500000 : 5000;
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.Demo"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/WinUI"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories);

            var xamlCombiner = new XAMLCombine.XAMLCombiner();

            using (var cts = new CancellationTokenSource())
            {
                var combineTask = Task.Run(() => xamlCombiner.Combine(themeFilePaths, _targetFile), cts.Token);
                var delayTask = Task.Delay(timeout, cts.Token);

                var timeoutTask = Task.WhenAny(combineTask, delayTask).ContinueWith(_ =>
                {
                    if (!combineTask.IsCompleted)
                    {
                        cts.Cancel();
                        throw new TimeoutException("Timeout waiting for method after " + timeout);
                    }

                    Assert.Null(combineTask.Exception);
                },
                cts.Token,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

                await timeoutTask.ConfigureAwait(true);
            }

            await Verifier.VerifyFile(_targetFile);
        }

        [Fact]
        public void TestDuplicateNamespaces()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.Demo"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/DuplicateNamespaces"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories);

            var xamlCombiner = new MyNet.Xaml.Merger.XAMLCombine.XAMLCombiner();

            var ex = Assert.Throws<Exception>(() => xamlCombiner.Combine(themeFilePaths, _targetFile));
            Assert.Contains("Namespace name \"controls\" with different values was seen in ", ex.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void TestDuplicateKeys()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.Demo"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/DuplicateKeys"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories);

            var testLogger = new TestLogger();

            var xamlCombiner = new MyNet.Xaml.Merger.XAMLCombine.XAMLCombiner
            {
                Logger = testLogger
            };
            xamlCombiner.Combine(themeFilePaths, _targetFile);

            Assert.Empty(testLogger.Errors);
            Assert.Single(testLogger.Warnings);
            Assert.StartsWith("Key \"DuplicateDifferentContent\" was found in multiple imported files, with differing content, and was skipped.\r\nExisting: At: 9:6", testLogger.Warnings[0], StringComparison.Ordinal);
        }
    }
}
