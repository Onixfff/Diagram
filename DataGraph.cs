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
        private int Time;

        public DataGraph(string nameTable, int idGraph, DateTime nowTime, string value, int time)
        {
            NameTable = nameTable;
            IdGraph = idGraph;
            NowTime = nowTime;
            Value = value;
            Time = time;
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

        public int GetTime()
        {
            return Time;
        }
    }
}