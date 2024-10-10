using System;
using System.Collections.Generic;
using ZedGraph;

namespace Diagram
{

    public class ZedGraphPosition
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public int Id { get; private set; }
        public ZedGraphControl Control { get; private set; }
        public int Position { get; private set; }

        public string Name { get; private set; }

        private static int CountId = 0;

        private ZedGraphPosition(int id, ZedGraphControl control, int position, string name)
        {
            Id = id;
            Control = control;
            Position = position;
            Name = name;
        }

        public static (ZedGraphPosition zedGraphPosition, string error) Create(int id, ZedGraphControl control, int position, string nameZedGraphStart)
        {
            //Проверкм

            if(id <= 0 && id > CountId)
            {
                _logger.Error("Control can not be zero or negative");
                return (null, "Control can not be zero or negative");
            }

            if (control == null)
            {
                _logger.Error("Control can not be empty");
                return (null, "Control can not be empty");
            }

            if (position <= 0)
            {
                _logger.Error("Position can not be zero or negative");
                return (null, "Position can not be zero or negative");
            }

            if (string.IsNullOrWhiteSpace(nameZedGraphStart))
            {
                _logger.Error("Control can not be zero or negative");
                return (null, "nameZedGraphStart con not be empty");
            }

            //Создание
            var zedGraphPosition = new ZedGraphPosition(id, control, position, nameZedGraphStart);
            CountId++;

            return (zedGraphPosition, null);
        }

        public static List<ZedGraphPosition> SwapPosition(List<ZedGraphPosition> positions, ZedGraphControl[] controls)
        {
            List<ZedGraphPosition> swapPosition = new List<ZedGraphPosition>();

            for (int i = 0; i < positions.Count; i++) 
            {
                if (controls[0] == positions[i].Control)
                {
                    swapPosition.Add(positions[i].Clone());
                }
                else if (controls[1] == positions[i].Control)
                {
                    swapPosition.Add(positions[i].Clone());
                }
            }

            for (int i = 0; i < positions.Count; i++)
            {
                if (swapPosition[0].Control == positions[i].Control)
                {
                    positions[i].Position = swapPosition[1].Position;
                    positions[i].Name = swapPosition[1].Name;
                }
                else if (swapPosition[1].Control == positions[i].Control)
                {
                    positions[i].Position = swapPosition[0].Position;
                    positions[i].Name = swapPosition[0].Name;
                }
            }

            return positions;
        }

        private ZedGraphPosition Clone()
        {
            var zedGraphCopy = ZedGraphPosition.Create(
                Id = this.Id,
                Control = this.Control,
                Position = this.Position,
                Name = this.Name);

            if(zedGraphCopy.error != null)
            {
                new Exception(zedGraphCopy.error);
            }

            return zedGraphCopy.zedGraphPosition;
        }

        public static List<ZedGraphPosition> FromDtoList(List<ZedGraphPositionDto> dtoList, List<ZedGraphControl> availableControls)
        {
            var result = new List<ZedGraphPosition>();

            foreach (var dto in dtoList)
            {
                // Поиск ZedGraphControl по имени
                var control = availableControls.Find(c => c.Name == dto.ControlName);

                if (control != null)
                {
                    // Создаем ZedGraphPosition на основе найденного Control
                    var resultCreate = Create(dto.Id ,control, dto.Position, dto.Name);

                    if(resultCreate.error != null)
                    {
                        new ArgumentException(resultCreate.error);
                    }

                    result.Add(resultCreate.zedGraphPosition);
                }
                else
                {
                    _logger.Error($"Control with name {dto.ControlName} not found");
                    throw new Exception($"Control with name {dto.ControlName} not found");
                }
            }

            return result;
        }

    }
}
