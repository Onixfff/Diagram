using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Diagram
{
    public partial class Main : Form
    {
        Database _db = new Database();
        private List<Graph> graphs = new List<Graph>();
        List<DataGraph> dataGraphs = new List<DataGraph>();

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
            graphs = _db.LoadDataDb();
            DrawGraphs(1);
            //_db.SendData(Database.RoomNames.graph2, 3, DateTime.Now, "12");

        }

        private void DrawGraphs(int startIndex)
        {
            startIndex = --startIndex;
            switch (startIndex)
            {
                case 0:
                    DrawGraph(zedGraphControlMainUp, 1);
                    DrawGraph(zedGraphControlMainDown, 2);

                    DrawGraph(zedGraphControlUpLeft1, 3);
                    DrawGraph(zedGraphControlUpRight1, 4);
                    DrawGraph(zedGraphControlDownLeft1, 5);
                    DrawGraph(zedGraphControlDownRight1, 6);

                    DrawGraph(zedGraphControlUpLeft2, 7);
                    DrawGraph(zedGraphControlUpRight2, 8);
                    DrawGraph(zedGraphControlDownLeft2, 9);
                    DrawGraph(zedGraphControlDownRight2, 10);
                    break;
                case 10:
                    DrawGraph(zedGraphControlMainUp, 11);
                    DrawGraph(zedGraphControlMainDown, 12);

                    DrawGraph(zedGraphControlUpLeft1, 13);
                    DrawGraph(zedGraphControlUpRight1, 14);
                    DrawGraph(zedGraphControlDownLeft1, 15);
                    DrawGraph(zedGraphControlDownRight1, 16);

                    DrawGraph(zedGraphControlUpLeft2, 17);
                    DrawGraph(zedGraphControlUpRight2, 18);
                    DrawGraph(zedGraphControlDownLeft2, 19);
                    DrawGraph(zedGraphControlDownRight2, 20);
                    break;
                case 20:
                    DrawGraph(zedGraphControlMainUp, 21);
                    DrawGraph(zedGraphControlMainDown, 22);

                    DrawGraph(zedGraphControlUpLeft1, 23);
                    DrawGraph(zedGraphControlUpRight1, 24);
                    DrawGraph(zedGraphControlDownLeft1, 25);
                    DrawGraph(zedGraphControlDownRight1, 26);

                    DrawGraph(zedGraphControlUpLeft2, 27);
                    DrawGraph(zedGraphControlUpRight2, 28);
                    DrawGraph(zedGraphControlDownLeft2, 29);
                    DrawGraph(zedGraphControlDownRight2, 30);
                    break;
                default:
                    MessageBox.Show("Неправельно задали значения стартовое значение для вывода диаграм");
                    break;
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
    }
}
