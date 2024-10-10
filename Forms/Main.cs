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
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        Database _db = new Database();
        List<DataGraph> dataGraphs = new List<DataGraph>();

        private List<ZedGraphControl> _zedGraphControls;
        private List<Graph> graphs = new List<Graph>();
        private ZedGraphControl[] _arrayZedGraphControlSwitch = new ZedGraphControl[2];
        private List<ZedGraphPosition> _graphPositions = new List<ZedGraphPosition>();
        private List<ZedGraphPositionDto> _userSettingsDto;

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

            (List<ZedGraphPosition> position, string error) result = (null, null);

            switch (startIndex)
            {
                case 0:

                    result = TakeGraphPositionInJsonFile(0);

                    if (result.error != null)
                    {
                        new ArgumentException(result.error);
                    }

                    _graphPositions = result.position;

                    _logger.Trace("Защёл в case 0:");
                    break;
                case 10:

                    result = TakeGraphPositionInJsonFile(10);

                    if (result.error != null)
                    {
                        new ArgumentException(result.error);
                    }

                    _logger.Trace("Защёл в case 10:");

                    _graphPositions = result.position;

                    break;
                case 20:

                    result = TakeGraphPositionInJsonFile(20);

                    if (result.error != null)
                    {
                        new ArgumentException(result.error);
                    }

                    _logger.Trace("Защёл в case 20:");

                    _graphPositions = result.position;

                    break;
                default:
                    _logger.Error("Switch не прошёл\nобратитесь за помощью");
                    MessageBox.Show("Неправельно задали стартовое значение для вывода диаграм");
                    break;
            }

            foreach (var item in _graphPositions)
            {
                _logger.Trace("Обновление DrawGraph");
                DrawGraph(item, item.Position);
            }
        }

        private (List<ZedGraphPosition> position, string error) TakeGraphPositionInJsonFile(int startIndex)
        {
            _logger.Trace("Защёл в TakeGraphPositionInJsonFile");

            string json = File.ReadAllText("UserSettings.json");

            List<ZedGraphPositionDtoData> dtoListData = JsonConvert.DeserializeObject<List<ZedGraphPositionDtoData>>(json);

            List<ZedGraphPositionDto> dtoList = new List<ZedGraphPositionDto>();

            int count = 0;

            foreach (var temp in dtoListData)
            {
                var result = ZedGraphPositionDto.Create(temp.Id, temp.ControlName, temp.Position, temp.Name);

                if (result.error != null)
                {
                    _logger.Error(result.error);
                    new Exception(result.error);
                }
                else
                {
                    switch (startIndex)
                    {
                        case 0:
                            if (result.zedGraphPositionDto.Id >= 0 && result.zedGraphPositionDto.Id <= 10)
                            {
                                dtoList.Add(result.zedGraphPositionDto);
                            }
                            break;

                        case 10:
                            if(result.zedGraphPositionDto.Id >= 11 && result.zedGraphPositionDto.Id <= 20)
                            {
                                dtoList.Add(result.zedGraphPositionDto);
                            }
                            break;

                        case 20:
                            if (result.zedGraphPositionDto.Id >= 21 && result.zedGraphPositionDto.Id <= 30)
                            {
                                dtoList.Add(result.zedGraphPositionDto);
                            }
                            break;
                        default:
                            break;
                    }

                    // Обработка ошибки (например, логгирование)
                }
            }

            var resultGetListZedGraphControls = GetListZedGraphControls();

            if (resultGetListZedGraphControls.error != null)
            {
                new ArgumentException(resultGetListZedGraphControls.error);
            }

            List<ZedGraphPosition> graphPositions = ZedGraphPosition.FromDtoList(dtoList, resultGetListZedGraphControls.zedGraphControls);

            count = 0;

            if (graphPositions != null)
            {
                foreach (var pos in graphPositions)
                {
                    if (pos.Control != null)
                    {
                        if (pos.Position != 0)
                        {
                            if (!string.IsNullOrWhiteSpace(pos.Name))
                            {
                                count++;
                            }
                        }
                    }
                }

                if (count == graphPositions.Count)
                {
                    _userSettingsDto = dtoList;
                    return (graphPositions, null);
                }
                else
                {
                    return (null, "Данные не могут быть null");
                }
            }

            return (null, "graphPositions can't null argument");
        }

        private void UpdateUserSettings()
        {
            _logger.Trace("Защёл в UpdateUserSettings");

            var resultConvert = ZedGraphPositionDto.ConvertToDto(_graphPositions);

            if (resultConvert.error != null)
            {
                new Exception(resultConvert.error);
            }

            var resultUpdate = UpdateControlPosition(resultConvert.zedGraphPositionDtos, _userSettingsDto);

            if (resultUpdate.error != null)
            {
                new Exception(resultUpdate.error);
            }

            UpdateJsonFile(_userSettingsDto);
        }

        private void UpdateJsonFile(List<ZedGraphPositionDto> dtos)
        {
            _logger.Trace("Защёл в UpdateJsonFile");

            string json = File.ReadAllText("UserSettings.json");

            List<ZedGraphPositionDtoData> dtoListData = JsonConvert.DeserializeObject<List<ZedGraphPositionDtoData>>(json);

            List<ZedGraphPositionDto> dtoList = new List<ZedGraphPositionDto>();

            int count = 0;

            foreach (var temp in dtoListData)
            {
                var result = ZedGraphPositionDto.Create(temp.Id, temp.ControlName, temp.Position, temp.Name);

                if (result.error != null)
                {
                    _logger.Error(result.error);
                    new Exception(result.error);
                }

                dtoList.Add(result.zedGraphPositionDto);
            }

            foreach (var item in dtoList)
            {
                var result = dtos.Find(p => p.Id == item.Id);

                if(result != null)
                {
                    item.ChangeDto(result.Position, result.Name);
                }
            }

            json = JsonConvert.SerializeObject(dtoList, Formatting.Indented);

            File.WriteAllText("UserSettings.json", json);



        }

        //HACK Переделать выход данных очень плохо всё
        public static (bool isComplite, string error) UpdateControlPosition(List<ZedGraphPositionDto> newDto, List<ZedGraphPositionDto> jsonDto)
        {
            _logger.Trace("Защёл в UpdateControlPosition");

            bool isComplite = true;

            foreach (var js in jsonDto)
            {
                var existingPosition = newDto.Find(p => p.Id == js.Id);

                if (existingPosition != null)
                {
                    js.ChangeDto(existingPosition.Position, existingPosition.Name);
                }
            }

            if (isComplite == true)
                return (isComplite, null);
            else
                return (isComplite, "");

        }

        private void CreateNewGraphPosition()
        {
            CreateListZedGraphPositions(1);
        }

        private void CreateListZedGraphPositions(int start)
        {
            for (int i = 0; i < _zedGraphControls.Count; i++)
            {
                var control = ZedGraphPosition.Create(i, _zedGraphControls[i], start, _zedGraphControls[i].Name);

                if (control.error != null)
                    new Exception("Ошибка создания ZedGraphPosition + \n" + control.error);

                _graphPositions.Add(control.zedGraphPosition);
                start++;
            }
        }

        private void SwitchZedGraphControlPosition()
        {
            _logger.Trace("Защёл в SwitchZedGraphControlPosition");

            _graphPositions = ZedGraphPosition.SwapPosition(_graphPositions, _arrayZedGraphControlSwitch);

            foreach (var item in _graphPositions)
            {
                DrawGraph(item, item.Position);
            }

            UpdateUserSettings();
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

                if (xmax_limit < time)
                    xmax_limit = time;
                if (ymax_limit < value)
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

            zedGraph.Control.RestoreScale(zedGraph.Control.GraphPane);

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
                _logger.Trace("Работает Перестановка (запуск сохранения)");

                for (int i = 0; i < _arrayZedGraphControlSwitch.Length; i++)
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
            for (int i = 0; i < _arrayZedGraphControlSwitch.Length; i++)
            {
                _arrayZedGraphControlSwitch[i] = null;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                ClearZedGrahpList();
        }

        private (List<ZedGraphControl> zedGraphControls, string error) GetListZedGraphControls()
        {
            var zedGraphControls = new List<ZedGraphControl>();

            foreach (Control control in GetAllControls(this))
            {
                // Проверяем, является ли control экземпляром ZedGraphControl
                if (control is ZedGraphControl zedGraphControl)
                {
                    // Если да, добавляем его в список
                    zedGraphControls.Add(zedGraphControl);
                }
            }

            if (zedGraphControls != null && zedGraphControls.Count > 0)
                return (zedGraphControls, null);
            else
                return (null, "Cannot create zedGraphControls List");
        }

        // Рекурсивный метод для получения всех контролов на форме
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;

                // Если у контрола есть дочерние элементы, рекурсивно обходим их
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
        }

    }
}
