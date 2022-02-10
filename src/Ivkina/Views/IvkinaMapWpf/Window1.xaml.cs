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
using GMap.NET;
using System.Net;
using GMap.NET.MapProviders;

namespace IvkinaMapWpf
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Map_load(object sender, EventArgs e) 
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton=MouseButton.Left;

            gmap.MaxZoom = 18;
            gmap.MinZoom = 5;
            gmap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 10;
            gmap.ShowCenter = false;

            gmap.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(54.989347, 73.368221);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }
    }
}
