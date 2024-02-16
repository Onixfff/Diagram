using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    internal class Graph
    {
        private List<DataGraph> _dataGraphs;

        public Graph(List<DataGraph> dataGraphs) 
        {
            _dataGraphs = dataGraphs;
        }

        public List<DataGraph> GetDataGraphs()
        {
            return _dataGraphs.ToList();
        }

        public int GetCountID()
        {
            int count = 0;
            int index = 0;
            for(int i = 0; i < _dataGraphs.Count; i++)
            {
                int value = _dataGraphs[i].GetIdGraph();
                if (value > index)
                {
                    index = value;
                    count++;
                }
            }

            return count;
        }

        public List<DateTime> GetPreparationStartDates()
        {
            List<DateTime> dates = new List<DateTime>();
            int index = 0;

            for(int i = 0; i < _dataGraphs.Count; ++i)
            {
                int value = _dataGraphs[i].GetIdGraph();
                if (index != value)
                {
                    index = value;
                    dates.Add(_dataGraphs[i].GetDateTime());
                }
            }

            return dates;
        }
    }
}
