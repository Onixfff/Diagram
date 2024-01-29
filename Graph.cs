using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
