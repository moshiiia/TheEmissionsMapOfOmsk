using MainModel.Entities;
using MainModel.Entities.Enums;
using ViewModelBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelBase.Commands;
using ViewModelBase.Commands.QuickCommands;
using ViewModelBase.Commands.AsyncCommands;
using System.Collections;
using TurkovaCalculation;

namespace ViewModels;
public class TurkovaViewModel : ViewModelBase.ViewModelBase
{
    private const Owner owner = Owner.Turkova;
    private readonly DataManager data;
    public IErrorHandler? handler { get; set; }
    public ObservableCollection<Point> Points { get; set; }
    private bool isBusy = false;

    public TurkovaViewModel()
    {
        this.data = DataManager.Set(EfProvider.SqLite);
        Points = new ObservableCollection<Point>(
            data.Point.Items.Where(i => i.Owner == Owner.Turkova));


        SetDataAsyncCommand = new AsyncCommand<IEnumerable>(
            SetDataAsync,
            default,
            list => !isBusy && list is not null,
            handler
            );


    }

    private async Task SetDataAsync(IEnumerable? items, CancellationToken? _)
    {
        isBusy = true;  

        try
        {
            var results = items!.Cast<Point>()!;
            var result = results.FirstOrDefault();
            await Test.SetDate(result!.PollutionSet);
            await data.PollutionSet.UpdateAsync(result.PollutionSet);
            //найти эту строку и обновить =result
        }
        finally
        {
            isBusy = false;
        }
    }

    public AsyncCommand<IEnumerable> SetDataAsyncCommand { get; }

}
