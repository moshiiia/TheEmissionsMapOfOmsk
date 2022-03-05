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

        private const Owner Ivkina = Owner.Ivkina;
        private readonly DataManager data;
        public IErrorHandler? Handler { get; set; }

        private bool isBusy = false;

        public IvkinaViewModel()
        {
            TestCommand = new Command<IEnumerable>(
                TestMethod, canExecute: _ => !isBusy) ;
            data = DataManager.Set(EfProvider.SqLite);
            Points = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p)));
        }

        private void TestMethod(IEnumerable? arg)
        {
            isBusy = true;
            try
            {
                var items = arg as PointItem[] ??
                            arg?.Cast<PointItem>().ToArray() ??
                            throw new ArgumentNullException(nameof(arg));
                foreach (var item in items)
                {
                    item.Name = "xo\nfog";
                }

                Points = new ObservableCollection<PointItem>(items);
                OnPropertyChanged(nameof(Points));
            }
            finally
            {
                isBusy = false;
            }
        }

        public Command<IEnumerable> TestCommand { get; }

    }

}
