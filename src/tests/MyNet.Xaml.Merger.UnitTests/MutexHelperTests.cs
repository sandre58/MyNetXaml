// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Xaml.Merger.Core.Helpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace MyNet.Xaml.Merger.UnitTests
{
    [TestFixture]
    public class MutexHelperTests
    {
        [Test]
        public void MutexHelperTestTimeout()
        {
            var t1 = Task.Factory.StartNew(() => ThreadStartFunction());
            var t2 = Task.Factory.StartNew(() => ThreadStartFunction());
            var t3 = Task.Factory.StartNew(() => ThreadStartFunction());
            var t4 = Task.Factory.StartNew(() => ThreadStartFunction());
            var t5 = Task.Factory.StartNew(() => ThreadStartFunction());

            try
            {
                Task.WaitAll(t1, t2, t3, t4, t5);
            }
            catch (AggregateException ex)
            {
                // Expect TimeoutException, Mutex shoudn't throw any ApplicationException
                var innerException = ex.InnerException;
                ClassicAssert.IsTrue(innerException is TimeoutException, $"InnerException was {innerException?.GetType().Name}");
            }
        }

        private void ThreadStartFunction()
        {
            var currentAssemblyDir = Path.GetDirectoryName(GetType().Assembly.Location)!;
            var wpfAppDirectory = Path.GetFullPath(Path.Combine(currentAssemblyDir, "../../../../MyNet.Xaml.Merger.Wpf.TestApp"));
            var themeFilesDirectory = Path.GetFullPath(Path.Combine(wpfAppDirectory, "Themes/Controls"));
            var themeFileName = Directory.GetFiles(themeFilesDirectory, "*.xaml", SearchOption.AllDirectories).FirstOrDefault() ?? throw new NullReferenceException();
            MutexHelper.ExecuteLocked(() => Thread.Sleep(3000), themeFileName, timeout: TimeSpan.FromSeconds(2));
        }
    }
}
