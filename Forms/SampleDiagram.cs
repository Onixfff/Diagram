using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using static Diagram.Database;

namespace Diagram
{
    public partial class SampleDiagram : Form
    {
        private List<Graph> _graphs = new List<Graph>();
        List<DataGraph> _dataGraphs = new List<DataGraph>();
        List<DataGraph> pairs = new List<DataGraph>();

        Database _db = new Database();

        double xmin_limit = 0;
        double xmax_limit = 0;

        double ymin_limit = 0;
        double ymax_limit = 0;

        string textTime = "Время";
        string textValue = "Значение";

        public SampleDiagram()
        {
            InitializeComponent();
            comboBoxRoms.Items.Clear();
            comboBoxRoms.DataSource = System.Enum.GetValues(typeof(RoomNames));
            ShowFilterDiagram();
        }

        //Выводит время начала заготовки
        private void button1_Click(object sender, EventArgs e)
        {
            ShowFilterDiagram();
            //DrawGraph(, );
        }

        private void ShowFilterDiagram()
        {
            DateTime dateTime = dateTimePicker.Value;
            _graphs = _db.GetId(dateTime, (RoomNames)System.Enum.Parse(typeof(RoomNames), comboBoxRoms.SelectedValue.ToString()));
            if (_graphs == null) { return; }
            SetDataComboBox(_graphs);
        }

        private void SetDataComboBox(List<Graph> graphs)
        {
            comboBoxPreparationStartDates.DataSource = null;
            comboBoxPreparationStartDates.Items.Clear();

            for(int i = 0; i < graphs.Count; i++)
            {
                pairs = graphs[i].GetPreparationStartDates();
            }

            for (int i = 0; i < pairs.Count; i++)
            {
                comboBoxPreparationStartDates.Items.Add(pairs[i].GetDateTime());
            }
        }

        private void CountsNumberId(List<Graph> graphs)
        {
            for(int i = 0; i < graphs.Count; i++)
            {
                var id = graphs[i].GetCountID();
                MessageBox.Show(id.ToString());
            }
        }

        private PointPairList GetPointPairList(string nameDiagram)
        {
            PointPairList pointList = new PointPairList();
            _dataGraphs = _graphs[0].GetDataGraphs();

            for (int i = 0; i < _dataGraphs.Count; i++)
            {
                string name = _dataGraphs[i].GetNameTable();
                if (name == nameDiagram)
                {
                    var time = _dataGraphs[i].GetTime();
                    double value = double.Parse(_dataGraphs[i].GetValue());
                    pointList.Add(time, value);
                }
            }

            return pointList;
        }

        private void DrawGraph(RoomNames room, int diagramId)
        {
            GraphPane pane = zedGraphControlFilter.GraphPane;
            pane.CurveList.Clear();

            List<DataGraph> dataGraphs = new List<DataGraph>();
            dataGraphs = _db.GetDiagram(room, diagramId);

            pane.XAxis.Title.Text = textTime;
            pane.YAxis.Title.Text = textValue;

            for (int i = 0; i < dataGraphs.Count; i++)
            {
                var time = dataGraphs[i].GetTime();
                var value = Convert.ToDouble(dataGraphs[i].GetValue());

                if (xmax_limit < time)
                    xmax_limit = time;

                if (ymax_limit < value)
                    ymax_limit = value;
            }

            PointPairList list = new PointPairList();

            for (int i = 0; i < dataGraphs.Count; i++)
            {
                double time = dataGraphs[i].GetTime();
                double value = double.Parse(dataGraphs[i].GetValue());

                list.Add (new PointPair(time, value));
            }

            pane.XAxis.Scale.Min = xmin_limit;
            pane.XAxis.Scale.Max = xmax_limit;

            pane.YAxis.Scale.Min = ymin_limit;
            pane.YAxis.Scale.Max = ymax_limit;

            LineItem f1_curve = pane.AddCurve(dataGraphs[0].GetNameTable(), list , Color.Black, SymbolType.None);

            zedGraphControlFilter.AxisChange();
            zedGraphControlFilter.Invalidate();

        }

        private void comboBoxPreparationStartDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i] != null)
                {
                    var user = comboBoxPreparationStartDates.Text;
                    if (pairs[i].GetDateTime() == Convert.ToDateTime( user))
                    {
                        DrawGraph((RoomNames)System.Enum.Parse(typeof(RoomNames), pairs[i].GetNameTable()), pairs[i].GetIdGraph());
                    }
                }
            }
        }

        //private void DrawGraphs()
        //{
        //    GraphPane pane = zedGraphControlFilter.GraphPane;
        //    pane.CurveList.Clear();

        //    pane.XAxis.Title.Text = textTime;
        //    pane.YAxis.Title.Text = textValue;

        //    _dataGraphs = _graphs[0].GetDataGraphs();

        //    for (int i = 0; i < _dataGraphs.Count; i++)
        //    {
        //        var time = _dataGraphs[i].GetTime();
        //        var value = Convert.ToDouble(_dataGraphs[i].GetValue());

        //        if (xmax_limit < time)
        //            xmax_limit = time;

        //        if (ymax_limit < value)
        //            ymax_limit = value;
        //    }

        //    List<PointPairList> points = new List<PointPairList>();

        //    for(int i = 0; i < nameGraphs.Count; i++)
        //    {
        //        points.Add(GetPointPairList(nameGraphs[i]));
        //    }

        //    pane.XAxis.Scale.Min = xmin_limit;
        //    pane.XAxis.Scale.Max = xmax_limit;

        //    pane.YAxis.Scale.Min = ymin_limit;
        //    pane.YAxis.Scale.Max = ymax_limit;

        //    Random random = new Random();
        //    for(int i = 0; i <  points.Count / 2; i++)
        //    {
        //        Color color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
        //        LineItem f1_curve = pane.AddCurve(i.ToString(), points[i], color, SymbolType.None);
        //    }

        //    zedGraphControlFilter.AxisChange();
        //    zedGraphControlFilter.Invalidate();
        //}
    }
}
