using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Diagram
{
    public partial class Main : Form
    {
        Database _db = new Database();
        List<DataGraph> dataGraphs = new List<DataGraph>();

        private List<Graph> graphs = new List<Graph>();
        private Dictionary<ZedGraphControl, int> _position = new Dictionary<ZedGraphControl, int>();
        private int count = 0;
        private List<ZedGraphControl> zedGraphList = new List<ZedGraphControl>();

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
            DrawGraphs(1);
        }

        private void DrawGraphs(int startIndex)
        {
            startIndex = --startIndex;
            _position.Clear();

            switch (startIndex)
            {
                case 0:
                    _position.Add(zedGraphControlMainUp, 1);
                    _position.Add(zedGraphControlMainDown, 2);

                    _position.Add(zedGraphControlUpLeft1, 3);
                    _position.Add(zedGraphControlUpRight1, 4);
                    _position.Add(zedGraphControlDownLeft1, 5);
                    _position.Add(zedGraphControlDownRight1, 6);

                    _position.Add(zedGraphControlUpLeft2, 7);
                    _position.Add(zedGraphControlUpRight2, 8);
                    _position.Add(zedGraphControlDownLeft2, 9);
                    _position.Add(zedGraphControlDownRight2, 10);

                    break;
                case 10:
                    _position.Add(zedGraphControlMainUp, 11);
                    _position.Add(zedGraphControlMainDown, 12);

                    _position.Add(zedGraphControlUpLeft1, 13);
                    _position.Add(zedGraphControlUpRight1, 14);
                    _position.Add(zedGraphControlDownLeft1, 15);
                    _position.Add(zedGraphControlDownRight1, 16);

                    _position.Add(zedGraphControlUpLeft2, 17);
                    _position.Add(zedGraphControlUpRight2, 18);
                    _position.Add(zedGraphControlDownLeft2, 19);
                    _position.Add(zedGraphControlDownRight2, 20);

                    break;
                case 20:
                    _position.Add(zedGraphControlMainUp, 21);
                    _position.Add(zedGraphControlMainDown, 22);

                    _position.Add(zedGraphControlUpLeft1, 23);
                    _position.Add(zedGraphControlUpRight1, 24);
                    _position.Add(zedGraphControlDownLeft1, 25);
                    _position.Add(zedGraphControlDownRight1, 26);

                    _position.Add(zedGraphControlUpLeft2, 27);
                    _position.Add(zedGraphControlUpRight2, 28);
                    _position.Add(zedGraphControlDownLeft2, 29);
                    _position.Add(zedGraphControlDownRight2, 30);

                    break;
                default:
                    MessageBox.Show("Неправельно задали значения стартовое значение для вывода диаграм");
                    break;
            }

            foreach (var item in _position)
            {
                DrawGraph(item.Key, item.Value);
            }
        }

        private void SwitchZedGraphControlPosition(ZedGraphControl left, ZedGraphControl right)
        {
            // Список для новых ключей
            var newKeys = new List<ZedGraphControl>();
            Dictionary<ZedGraphControl, int> oldPosition = new Dictionary<ZedGraphControl, int>();

            // Создаем новый словарь для замены
            var newPosition = new Dictionary<ZedGraphControl, int>();
            int count = 0;
            // Заполняем новый словарь, заменяя ключи

            foreach (var pos in _position)
            {
                newPosition.Add(pos.Key, pos.Value);
            }

            ZedGraphControl zedGraphControl1 = null;
            int pos1 = 0;

            ZedGraphControl zedGraphControl2 = null;
            int pos2 = 0;

            foreach (var pos in _position)
            {
                foreach (var old in oldPosition)
                {
                    if(pos.Value == old.Value)
                    {
                        count++;
                        
                        if (count == 1)
                        {
                            zedGraphControl1 = pos.Key;
                            pos1 = pos.Value;
                        }
                        else if (count == 2)
                        {
                            zedGraphControl2 = pos.Key;
                            pos2 = pos.Value;
                        }
                    }
                }
            }

            foreach (var newpos in newPosition)
            {
                if(newpos.Key == zedGraphControl1)
                {
                    
                }
                else if(newpos.Key == zedGraphControl2)
                {

                }
            }

            // Заменяем старый словарь на новый
            _position = newPosition;

            foreach (var item in _position)
            {
                DrawGraph(item.Key, item.Value);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zedGraph"> Принимает диаграму</param>
        /// <param name="count"> Номер таблицы из бд (1-30)</param>
        private void DrawGraph(ZedGraphControl zedGraph, int count = 1)
        {
            count = --count;
            double xmin_limit = 0;
            double xmax_limit = 0;

            double ymin_limit = 0;
            double ymax_limit = 0;

            double step = 10;

            GraphPane pane = zedGraph.GraphPane;
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
                zedGraph.AxisChange();
                // Обновляем график
                zedGraph.Invalidate();
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
            if (checkBox1.Checked)
            {
                zedGraphList.Add((ZedGraphControl)sender);
                
                if(zedGraphList.Count == 2)
                {
                    SwitchZedGraphControlPosition(zedGraphList[0], zedGraphList[1]);
                    zedGraphList.Clear();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                zedGraphList.Clear();
        }
    }
}
