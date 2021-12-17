using MainModel.Entities;
using MainModel.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBase;

namespace ViewModels;
public class TurkovaViewModel : ViewModelBase.ViewModelBase
{
    private const Owner owner = Owner.Turkova;
    private readonly DataManager data;

    public ObservableCollection<PollutionSet> PollutionSets { get; set; }

    public TurkovaViewModel()
    {
        this.data = DataManager.Set(EfProvider.SqLite);
        PollutionSets = new ObservableCollection<PollutionSet>(
            data.PollutionSet.Items.Where(i => i.Owner == Owner.Turkova));
    }

   
}
