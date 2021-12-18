using MainModel.Entities;
using MainModel.Entities.Enums;
using System.Collections.ObjectModel;
using ViewModelBase.Commands;
using ViewModelBase.Commands.AsyncCommands;
using System.Collections;
using TurkovaCalculation;

namespace ViewModels;
public class TurkovaViewModel : ViewModelBase.ViewModelBase
{
    private const Owner Turkova = Owner.Turkova;
    private readonly DataManager data;
    public IErrorHandler? Handler { get; set; }


    public AsyncCommand<Point?> SetDataAsyncCommand { get; }
    public ObservableCollection<Point> Points { get; set; }
    private bool isBusy = false;

    public TurkovaViewModel()
    {
        this.data = DataManager.Set(EfProvider.SqLite);
        Points = new ObservableCollection<Point>(
            data.Point.Items.Where(i => i.Owner == Turkova));


        SetDataAsyncCommand = new AsyncCommand<Point?>(
            SetDataAsync,
            default,
            p => !isBusy && p != null,//
            Handler
            );
    }

    private async Task SetDataAsync(Point? item, CancellationToken? _)
    {
        isBusy = true;  

        try
        {
            await Test.SetDate(item!.PollutionSet!);
            await data.PollutionSet.UpdateAsync(item.PollutionSet!);
            var ind = Points.IndexOf(item);
            if (ind < 0) throw new Exception("Что за хрень?");
            Points.RemoveAt(ind);
            Points.Insert(ind, item);
        }
        finally
        {
            isBusy = false;
        }
    }

}
