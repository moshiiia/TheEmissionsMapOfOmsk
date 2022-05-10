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

        public ObservableCollection<PointItem> Points { get; set; }
        public ObservableCollection<PointItem> Pushpins { get; } = new();
        public ObservableCollection<PolylineItem> Polylines { get; } = new();
        public ObservableCollection<DataGridCont> DataGridConts { get; } = new();

        private const Owner Ivkina = Owner.Ivkina;
        private readonly DataManager data;
        public IErrorHandler? Handler { get; set; }
       
        private bool isBusy = false;
        public Command<PointItem> AddPointCommand { get; }

        public IvkinaViewModel()
        {
            CalculationCommand = new Command(AddCalculation,CanAddCalculation);
            
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

        private bool CanAddCalculation() 
        {
            var checks = Points.Where(y => y.IsSelected).Count();
            return checks is 2 or 3;
        }

        private void AddCalculation()
        {
            var checks = Points.Where(y => y.IsSelected);
            //var count = checks.Count(); 
            //переход в расчеты


        }

        public Command CalculationCommand { get; }

        public void RaiseCanCalculationCommand() => CalculationCommand.RaiseCanExecuteChanged();

      
    }

}
