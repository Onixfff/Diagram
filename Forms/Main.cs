using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Diagram
{
    public partial class Main : Form
    {
        Database _db = new Database();
        List<DataGraph> dataGraphs = new List<DataGraph>();

        private List<ZedGraphControl> _zedGraphControls;
        private List<Graph> graphs = new List<Graph>();
        private ZedGraphControl[] _arrayZedGraphControlSwitch = new ZedGraphControl[2];
        private List<ZedGraphPosition> _graphPositions = new List<ZedGraphPosition>();

        private enum DiagramName
        {
            zedGraphControlMainUp,
            zedGraphControlMainDown,
            zedGraphControlUpLeft1,
            zedGraphControlUpRight1,
            zedGraphControlDownLeft1,
            zedGraphControlDownRight1,
            zedGraphControlUpLeft2,
            zedGraphControlUpRight2,
            zedGraphControlDownLeft2,
            zedGraphControlDownRight2,
        }

        public Main()
        {
            InitializeComponent();
            Task.Run(async () => graphs = await _db.LoadDataDb()).GetAwaiter().GetResult();
            _zedGraphControls = new List<ZedGraphControl>
            {
                zedGraphControlMainUp,
                zedGraphControlMainDown,
                zedGraphControlUpLeft1,
                zedGraphControlUpRight1,
                zedGraphControlDownLeft1,
                zedGraphControlDownRight1,
                zedGraphControlUpLeft2,
                zedGraphControlUpRight2,
                zedGraphControlDownLeft2,
                zedGraphControlDownRight2
            };
            DrawGraphs(1);

        }

        private void DrawGraphs(int startIndex)
        {
            startIndex = --startIndex;
            _graphPositions.Clear();

            switch (startIndex)
            {
                case 0:
                    CreateListZedGraphPositions(1);
                    
                    var result = TakeGraphPositionInJsonFile();

                    if(result.error != null)
                    {
                        new ArgumentException(result.error);
                    }

                    break;
                case 10:
                    CreateListZedGraphPositions(11);
                    TakeGraphPositionInJsonFile();
                    break;
                case 20:
                    CreateListZedGraphPositions(21);
                    TakeGraphPositionInJsonFile();
                    break;
                default:
                    MessageBox.Show("Неправельно задали стартовое значение для вывода диаграм");
                    break;
            }

            foreach (var item in _graphPositions)
            {
                DrawGraph(item, item.Position);
            }
        }

        private (List<ZedGraphPosition> position, string error) TakeGraphPositionInJsonFile()
        {
            string json = File.ReadAllText("UserSettings.json");

            var graphPositions = JsonConvert.DeserializeObject<List<ZedGraphPosition>>(json);

            if(graphPositions != null)
            {
                foreach (var pos in graphPositions)
                {
                    if(pos.Control != null && pos.Position != 0 && string.IsNullOrWhiteSpace(pos.Name))
                    {
                        return (graphPositions, null);
                    }
                }

                return (null, "Данные не могут быть null");
            }

            return (null, "graphPositions can't null argument");
        }

        private void CreateNewGraphPosition()
        {
            CreateListZedGraphPositions(1);
        }

        private void CreateListZedGraphPositions(int start)
        {
            for (int i = 0; i < _zedGraphControls.Count; i++)
            {
                var control = ZedGraphPosition.Create(_zedGraphControls[i], start, _zedGraphControls[i].Name);

                if (control.error != null)
                    new Exception("Ошибка создания ZedGraphPosition + \n" + control.error);

                _graphPositions.Add(control.zedGraphPosition);
                start++;
            }
        }

        private void SwitchZedGraphControlPosition()
        {
            _graphPositions = ZedGraphPosition.SwapPosition(_graphPositions, _arrayZedGraphControlSwitch);

            foreach (var item in _graphPositions)
            {
                DrawGraph(item, item.Position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zedGraph"> Принимает диаграму</param>
        /// <param name="count"> Номер таблицы из бд (1-30)</param>
        private void DrawGraph(ZedGraphPosition zedGraph, int count = 1)
        {
            count = --count;
            double xmin_limit = 0;
            double xmax_limit = 0;

            double ymin_limit = 0;
            double ymax_limit = 0;

            double step = 10;

            GraphPane pane = zedGraph.Control.GraphPane;
            pane.CurveList.Clear();

            PointPairList listPoints = new PointPairList();

            pane.Title.Text = zedGraph.Name;

            pane.XAxis.Title.Text = "Время";
            pane.YAxis.Title.Text = "Значение";

            //Заполнение таблицы
            dataGraphs = graphs[count].GetDataGraphs();

            // !!! Создадим список

            for (int j = 0; j < dataGraphs.Count; j++)
            {
                double time = dataGraphs[j].GetTime();
                double value = double.Parse(dataGraphs[j].GetValue());

                PointPair pointPair = new PointPair(time, value);
                listPoints.Add(pointPair);

                if(xmax_limit < time)
                    xmax_limit = time;
                if(ymax_limit < value)
                    ymax_limit = value;
            }

            // Создадим кривую с названием "Название из бд",
            // которая будет рисоваться голубым цветом (Color.Blue),
            // Опорные точки выделяться не будут (SymbolType.None)
            if (dataGraphs.Count <= 0)
            {

            }
            else
            {
                LineItem myCurve = pane.AddCurve(dataGraphs[0].GetNameTable(), listPoints, Color.Blue, SymbolType.Diamond);

                pane.XAxis.Scale.Min = xmin_limit;
                pane.XAxis.Scale.Max = xmax_limit;

                pane.YAxis.Scale.Min = ymin_limit;
                pane.YAxis.Scale.Max = ymax_limit + step;

            }
                zedGraph.Control.AxisChange();
                // Обновляем график
                zedGraph.Control.Invalidate();
        }

        private List<Graph> UpdateDataGraphs(List<Graph> baseGraphs, float timer = 20, bool isUpdate = true)
        {
            if (isUpdate)
            {
                baseGraphs.Clear();
                return null;
            }
            else
            {
                return baseGraphs;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DrawGraphs(1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DrawGraphs(11);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DrawGraphs(21);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SampleDiagram sampleDiagram = new SampleDiagram();
            sampleDiagram.Show();
        }

        private void zedGraphControlMainUp_MouseClick(object sender, MouseEventArgs e)
        {
            int countNotNull = 0;

            if (checkBox1.Checked)
            {
                for(int i = 0; i < _arrayZedGraphControlSwitch.Length; i++)
                {
                    countNotNull++;
                    
                    if (_arrayZedGraphControlSwitch[i] == null)
                    {
                        _arrayZedGraphControlSwitch[i] = (ZedGraphControl)sender;
                        break;
                    }

                }

                if (countNotNull == _arrayZedGraphControlSwitch.Length)
                {
                    SwitchZedGraphControlPosition();
                    ClearZedGrahpList();
                }
            }
        }

        private void ClearZedGrahpList()
        {
            for(int i = 0; i < _arrayZedGraphControlSwitch.Length; i++)
            {
                _arrayZedGraphControlSwitch[i] = null;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                ClearZedGrahpList();
        }

    }
}
