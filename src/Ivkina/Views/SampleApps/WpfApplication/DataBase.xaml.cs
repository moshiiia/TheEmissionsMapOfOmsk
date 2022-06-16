﻿using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;
using ViewModels;

namespace SampleApplication
{
    /// <summary>
    /// Логика взаимодействия для DataBase.xaml
    /// </summary>
    public partial class DataBase : Window
    {
        public DataBase()
        {
            InitializeComponent();
            if (DataContext is IvkinaViewModel) model = (IvkinaViewModel)DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        IvkinaViewModel model;
        private void select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model?.RaiseCanDeleteCommand();
        }
    }
}
