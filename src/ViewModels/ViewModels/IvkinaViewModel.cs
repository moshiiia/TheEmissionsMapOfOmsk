using MainModel.Entities;
using MainModel.Entities.Enums;
using System.Collections.ObjectModel;
using ViewModelBase.Commands;
using ViewModelBase.Commands.AsyncCommands;
using System.Collections;
using ViewModels;

namespace ViewModels
{
    public class IvkinaViewModel : ViewModelBase.ViewModelBase
    {
        public ObservableCollection<PointItem> Points { get; } = new();
        public ObservableCollection<PointItem> Pushpins { get; } = new();
        public ObservableCollection<PolylineItem> Polylines { get; } = new();

        private const Owner Ivkina = Owner.Ivkina;
        private readonly DataManager data;
        public IErrorHandler? Handler { get; set; }

        private bool isBusy = false;

        public IvkinaViewModel()
        {
            data = DataManager.Set(EfProvider.SqLite);
            Points = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p)));
        }
    }

}
