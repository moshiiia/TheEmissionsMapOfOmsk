using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CheckPoints 
    {

        private readonly List<PointItem> _list;

        private readonly int _max;
        
        public CheckPoints(int max = 3)
        {
            _max = max < 1 ? 1 : max;
            _list = new(max);
        }

        private void Add(PointItem item)
        {
            if (_list.Contains(item)) return;
            if (_list.Count == _max )
                _list.RemoveAt(0);
            _list.Add(item);
        }

        public IEnumerable<PointItem> GetPoints() => _list;

        private void Delete(PointItem item)
        {
            _list.Remove(item);
        }

        public void AddOrDelete(PointItem point)
        {
            if (_list.Contains(point)) Delete(point); else Add(point);
        }

    }
}
