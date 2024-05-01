// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Xaml.Merger.XAMLCombine;
using MyNet.Xaml.Merger.UnitTests.TestHelpers;
using NUnit.Framework;
using VerifyNUnit;

namespace MyNet.Xaml.Merger.UnitTests
{
    [TestFixture]
    internal class XAMLCombinerTests
    {
        private string _targetFile = null!;

        [SetUp]
        public void SetUp()
        {
            _targetFile = Path.Combine(Path.GetTempPath(), "XAMLCombinerTests_Generic.xaml");

            File.Delete(_targetFile);
        }

        [TearDown]
        public void TearDown()
        {
            // Method intentionally left empty.
        }

        [Test]
        public async Task TestOutputAsync()
        {
            var timeout = Debugger.IsAttached ? 500000 : 5000;
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/Controls"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).Reverse().ToArray();

            var xamlCombiner = new XAMLCombiner();

            using (var cts = new CancellationTokenSource())
            {
                var combineTask = Task.Run(() => xamlCombiner.Combine(themeFilePaths, _targetFile), cts.Token);
                var delayTask = Task.Delay(timeout, cts.Token);

                var timeoutTask = Task.WhenAny(combineTask, delayTask).ContinueWith(t =>
                {
                    if (!combineTask.IsCompleted)
                    {
                        cts.Cancel();
                        throw new TimeoutException("Timeout waiting for method after " + timeout);
                    }

                    Assert.That(combineTask.Exception, Is.Null, combineTask.Exception?.ToString());
                }, cts.Token);

                await timeoutTask;
            }

            await Verifier.VerifyFile(_targetFile);
        }

        [Test]
        public async Task TestOutputWinUIAsync()
        {
            var timeout = Debugger.IsAttached ? 500000 : 5000;
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/WinUI"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).Reverse().ToArray();

            var xamlCombiner = new XAMLCombiner();

            using (var cts = new CancellationTokenSource())
            {
                var combineTask = Task.Run(() => xamlCombiner.Combine(themeFilePaths, _targetFile), cts.Token);
                var delayTask = Task.Delay(timeout, cts.Token);

                var timeoutTask = Task.WhenAny(combineTask, delayTask).ContinueWith(t =>
                {
                    if (!combineTask.IsCompleted)
                    {
                        cts.Cancel();
                        throw new TimeoutException("Timeout waiting for method after " + timeout);
                    }

                    Assert.That(combineTask.Exception, Is.Null, combineTask.Exception?.ToString());
                }, cts.Token);

                await timeoutTask;
            }

            await Verifier.VerifyFile(_targetFile);
        }

        [Test]
        public void TestDuplicateNamespaces()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/DuplicateNamespaces"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).Reverse().ToArray();

            var xamlCombiner = new XAMLCombiner();

            Assert.That(() => xamlCombiner.Combine(themeFilePaths, _targetFile),
                        Throws.Exception
                              .With.Message
                              .Contains("Namespace name \"controls\" with different values was seen in "));
        }

        [Test]
        public void TestDuplicateKeys()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/DuplicateKeys"));
            var themeFilePaths = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).Reverse().ToArray();

            var testLogger = new TestLogger();

            var xamlCombiner = new XAMLCombiner
            {
                Logger = testLogger
            };
            xamlCombiner.Combine(themeFilePaths, _targetFile);

            Assert.That(testLogger.Errors, Is.Empty);
            Assert.That(testLogger.Warnings, Has.Count.EqualTo(1));
            Assert.That(testLogger.Warnings[0], Does.StartWith("Key \"DuplicateDifferentContent\" was found in multiple imported files, with differing content, and was skipped.\r\nExisting: At: 9:6"));
        }
    }
}
