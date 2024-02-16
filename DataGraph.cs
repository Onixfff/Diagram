using System;
using System.Windows.Forms;

namespace Diagram
{
    public class DataGraph
    {
        private string _nameTable;
        private int _idGraph;
        private DateTime _time;
        private string _value;

        public DataGraph(string nameTable, int idGraph, DateTime time, string value)
        {
            _nameTable = nameTable;
            _idGraph = idGraph;
            _time = time;
            _value = value;
        }

        public string GetNameTable()
        {
            return _nameTable;
        }

        public int GetIdGraph()
        {
            return _idGraph;
        }
        
        public DateTime GetDateTime()
        {
            return _time;
        }
        
        public string GetValue()
        {
            return _value;
        }
    }
}