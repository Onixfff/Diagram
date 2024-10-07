using System;
using System.Collections.Generic;
using ZedGraph;

namespace Diagram
{
    public class ZedGraphPositionDto
    {
        public string ControlName { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
    }

    public class ZedGraphPosition
    {
        public ZedGraphControl Control { get; private set; }
        public int Position { get; private set; }

        public string Name { get; private set; }

        private ZedGraphPosition(ZedGraphControl control, int position, string name)
        {
            Control = control;
            Position = position;
            Name = name;
        }

        public static (ZedGraphPosition zedGraphPosition, string error) Create(ZedGraphControl control, int position, string nameZedGraphStart)
        {
            //Проверкм

            if(control == null)
                return (null, "Control can not be empty");

            if(position == 0)
                return (null, "Control can not be zero");

            if (string.IsNullOrWhiteSpace(nameZedGraphStart))
                return (null, "nameZedGraphStart con not be empty");

            //Создание
            var zedGraphPosition = new ZedGraphPosition(control, position, nameZedGraphStart);

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
            var zedGraphCopy = ZedGraphPosition.Create(Control = this.Control,
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
                    var resultCreate = Create(control, dto.Position, dto.Name);

                    if(resultCreate.error != null)
                    {
                        new ArgumentException(resultCreate.error);
                    }

                    result.Add(resultCreate.zedGraphPosition);
                }
                else
                {
                    throw new Exception($"Control with name {dto.ControlName} not found");
                }
            }

            return result;
        }
    }
}
