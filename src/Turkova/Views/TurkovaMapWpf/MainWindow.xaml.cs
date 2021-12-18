using System;
using System.Windows;
using System.Windows.Controls;
using ViewModels;
using WpfLibrary;

namespace TurkovaMapWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly TurkovaViewModel _model; 
    public MainWindow()
    {
        InitializeComponent();
        _model=DataContext as TurkovaViewModel?? throw new Exception();
        _model.Handler = new ErrorHandle();
    }


    private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _model.SetDataAsyncCommand.RaiseCanExecuteChanged();
    }
}