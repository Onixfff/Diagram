using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using static Diagram.Database;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Diagram
{
    public partial class SampleDiagram : Form
    {
        private List<Graph> _graphs = new List<Graph>();
        List<DataGraph> _dataGraphs = new List<DataGraph>();

        Database _db = new Database();

        public SampleDiagram()
        {
            InitializeComponent();
            comboBoxRoms.Items.Clear();
            comboBoxRoms.DataSource = Enum.GetValues(typeof(RoomNames));
            ShowFilterDiagram();
            DrawGraph();
        }

        //Выводит время начала заготовки
        private void button1_Click(object sender, EventArgs e)
        {
            ShowFilterDiagram();
        }

        private void ShowFilterDiagram()
        {
            DateTime dateTime = dateTimePicker.Value;
            _graphs = _db.GetId(dateTime, (RoomNames)Enum.Parse(typeof(RoomNames), comboBoxRoms.SelectedValue.ToString()));
            if (_graphs == null) { return; }
            SetDataComboBox(_graphs);
        }

        private void SetDataComboBox(List<Graph> graphs)
        {
            for(int i = 0; i < graphs.Count; i++)
            {
                comboBoxPreparationStartDates.DataSource = graphs[i].GetPreparationStartDates();
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
                    DateTime datetime = _dataGraphs[i].GetDateTime();
                    double value = double.Parse(_dataGraphs[i].GetValue());
                    pointList.Add(new XDate(datetime), value);
                }
            }

            return pointList;
        }

        private void DrawGraph()
        {
            GraphPane pane = zedGraphControlFilter.GraphPane;
            pane.CurveList.Clear();

            _dataGraphs = _graphs[0].GetDataGraphs();
            List<string> nameGraphs = new List<string>();
            nameGraphs.Add(_dataGraphs[0].GetNameTable());
            for (int i = 0; i < _dataGraphs.Count; i++)
            {
                for(int j = 0; j < nameGraphs.Count; j++)
                {
                    var name = _dataGraphs[i].GetNameTable();
                    if (name != nameGraphs[j])
                    {
                        nameGraphs.Add(_dataGraphs[i].GetNameTable());
                    }
                }
            }

            List<PointPairList> points = new List<PointPairList>();

            for(int i = 0; i < nameGraphs.Count; i++)
            {
                points.Add(GetPointPairList(nameGraphs[i]));
            }

            Random random = new Random();
            for(int i = 0; i <  points.Count / 2; i++)
            {
                Color color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                LineItem f1_curve = pane.AddCurve(i.ToString(), points[i], color, SymbolType.None);
            }

            zedGraphControlFilter.AxisChange();
            zedGraphControlFilter.Invalidate();
        }
    }
}
