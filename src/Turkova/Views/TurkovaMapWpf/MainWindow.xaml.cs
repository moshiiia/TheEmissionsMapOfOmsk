using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;
using WpfLibrary;

namespace TurkovaMapWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TurkovaViewModel model; 
        public MainWindow()
        {
            InitializeComponent();
            model=DataContext as TurkovaViewModel?? throw new Exception();
            model.handler = new ErrorHandle();
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.SetDataAsyncCommand.RaiseCanExecuteChanged();
        }
    }
}
