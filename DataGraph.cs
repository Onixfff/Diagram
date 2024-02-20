using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Diagram
{
    public class DataGraph
    {
        private string NameTable;
        private int IdGraph;
        private DateTime NowTime;
        private string Value;

        public DataGraph(string nameTable, int idGraph, DateTime time, string value)
        {
            NameTable = nameTable;
            IdGraph = idGraph;
            NowTime = time;
            Value = value;
        }

        public string GetNameTable()
        {
            return NameTable;
        }

        public int GetIdGraph()
        {
            return IdGraph;
        }
        
        public DateTime GetDateTime()
        {
            return NowTime;
        }
        
        public string GetValue()
        {
            return Value;
        }
    }
}