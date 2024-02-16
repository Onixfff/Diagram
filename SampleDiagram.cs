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
            DrawGraph();
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

        private double f1(double x)
        {
            if (x == 0)
            {
                return 1;
            }

            return Math.Sin(x) / x;
        }

        private double f2(double x)
        {
            return Math.Sin(x / 2) / 2;
        }

        private void DrawGraph(int count = 1)
        {
            int LeftIndent = 10;
            int RightIndent = 10;
            int step = 15;
            double valueMin = double.MinValue;
            double valueMax = double.MaxValue;

            GraphPane pane = zedGraphControlFilter.GraphPane;
            pane.CurveList.Clear();

            pane.XAxis.Title.Text = "Время";
            pane.YAxis.Title.Text = "Значение";

            //Заполнение таблицы
            _dataGraphs = _graphs[0].GetDataGraphs();







            // Величина допуска для всех точек
            if (_dataGraphs != null && _dataGraphs.Count > 0)
            {
                valueMax = double.Parse(_dataGraphs[0].GetValue());
                valueMin = double.Parse(_dataGraphs[0].GetValue());
            }

            // Создадим кривую с названием "Название из бд",
            // которая будет рисоваться голубым цветом (Color.Blue),
            // Опорные точки выделяться не будут (SymbolType.None)
            if (_dataGraphs.Count <= 0)
            {

            }
            else
            {
                //LineItem myCurve = pane.AddCurve(_dataGraphs[0].GetNameTable(), listPoints, Color.Blue, SymbolType.None);

                //Подготовка начального вида графики ( начальные точки min max п x и y)
                DateTime minDateTime = _dataGraphs[0].GetDateTime();
                DateTime maxDateTime = _dataGraphs[_dataGraphs.Count - 1].GetDateTime();

                //Работа для x изменения визуализации графиков

                XDate minTime = new XDate
                    (
                        minDateTime.Year, minDateTime.Month, minDateTime.Day, minDateTime.Hour,
                        minDateTime.Minute, minDateTime.Minute - LeftIndent
                    );

                XDate maxTime = new XDate
                    (
                        maxDateTime.Year, maxDateTime.Month, maxDateTime.Day, maxDateTime.Hour,
                        maxDateTime.Minute, maxDateTime.Minute + RightIndent
                    );
                pane.XAxis.Scale.Max = maxTime;
                pane.XAxis.Scale.Min = minTime;
                pane.XAxis.Type = AxisType.Date;
                pane.XAxis.Scale.Format = "m:ss";
                pane.XAxis.Scale.MajorStep = step;

                pane.XAxis.Type = AxisType.Exponent;
                pane.YAxis.Scale.Max = valueMin + 5;
                pane.YAxis.Scale.Min = valueMax + 5;
                pane.YAxis.Scale.MajorStep = 5;
            }
            zedGraphControlFilter.AxisChange();
            // Обновляем график
            zedGraphControlFilter.Invalidate();
        }
    }
}
