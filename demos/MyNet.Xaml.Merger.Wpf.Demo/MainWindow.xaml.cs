// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows;

namespace MyNet.Xaml.Merger.Wpf.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Types should be internal", Justification = "Required to be public for WPF partial class compatibility")]
public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
}
