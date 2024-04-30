// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;

namespace MyNet.Xaml.Merger.Core;

public sealed class UTF8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}
