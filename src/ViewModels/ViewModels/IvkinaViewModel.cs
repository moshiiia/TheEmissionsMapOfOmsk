using MainModel.Entities;
using MainModel.Entities.Enums;
using System.Collections.ObjectModel;
using ViewModelBase.Commands;
using ViewModelBase.Commands.AsyncCommands;
using System.Collections;
using ViewModelBase.Commands.QuickCommands;
using ViewModels;

namespace ViewModels
{
    public class IvkinaViewModel : ViewModelBase.ViewModelBase
    {

        public ObservableCollection<PointItem> Points { get; set; } = new();
        public ObservableCollection<PointItem> Pushpins { get; } = new();
        public ObservableCollection<PolylineItem> Polylines { get; } = new();
        public ObservableCollection<DataGridCont> DataGridConts { get; } = new();

        private const Owner Ivkina = Owner.Ivkina;
        private readonly DataManager data;
        public IErrorHandler? Handler { get; set; }
        private CheckPoints checkPoints = new CheckPoints();

        private bool isBusy = false;
        public Command<PointItem> AddPointCommand { get; }

        public IvkinaViewModel()
        {
            //TestCommand = new Command<IEnumerable>(
            //    TestMethod, canExecute: _ => !isBusy);

            AddPointCommand = new Command<PointItem>(AddPoint, null, Handler);
             
            data = DataManager.Set(EfProvider.SqLite);

            Points = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p))); 
            
            Pushpins = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p)));

            DataGridConts = new ObservableCollection<DataGridCont>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => new DataGridCont()
                {
                    Num = p.Num,
                    PointName = p.Name,
                    Latitude = p.Coordinate.Latitude,
                    Longitude = p.Coordinate.Longitude,    
                    PollutionName = p.PollutionSet.Pollution.Name,
                    PollutionAmount = p.PollutionSet.Amount
                }
                ).OrderBy(y => y.Num));
        }

        private void AddPoint(PointItem obj)
        {
            //PointItem? result = Points.FirstOrDefault(y => y.Num == obj);
            if (obj == null) return;
            checkPoints.AddOrDelete(obj);
        }



        //private void TestMethod(IEnumerable? arg)
        //{
        //    isBusy = true;
        //    try
        //    {
        //        var items = arg as PointItem[] ??
        //                    arg?.Cast<PointItem>().ToArray() ??
        //                    throw new ArgumentNullException(nameof(arg));
        //        foreach (var item in items)
        //        {
        //            item.Name = "xo\nfog";
        //        }

        //        Points = new ObservableCollection<PointItem>(items);
        //        OnPropertyChanged(nameof(Points));
        //    }
        //    finally
        //    {
        //        isBusy = false;
        //    }
        //}
        //public Command<IEnumerable> TestCommand { get; }




    }

}
