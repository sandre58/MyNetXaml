// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using DiffEngine;
using NUnit.Framework;

namespace MyNet.Xaml.Merger.UnitTests;

[SetUpFixture]
public static class AssemblySetup
{
    [OneTimeSetUp]
    public static void SetUp() => DiffRunner.Disabled = !Debugger.IsAttached;
}
