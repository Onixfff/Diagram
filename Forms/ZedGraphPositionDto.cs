using System.Collections.Generic;

namespace Diagram
{
    public class ZedGraphPositionDtoData
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }

        public ZedGraphPositionDtoData(int id, string controlName, int position, string name) 
        {
            Id = id;
            ControlName = controlName;
            Position = position;
            Name = name;
        }
    }

    public class ZedGraphPositionDto
    {
        public int Id { get; private set; }

        public string ControlName { get; private set; }

        public int Position { get; private set; }

        public string Name { get; private set; }

        private ZedGraphPositionDto(int id, string controlName, int position, string name) 
        {
            Id = id;
            ControlName = controlName;
            Position = position;
            Name = name;
        }

        public static (ZedGraphPositionDto zedGraphPositionDto, string error) Create(int id, string controlName, int position, string name)
        {
            if(id <= 0)
            {
                return (null, "id can not be zero or negative");
            }

            //Валидация
            if (string.IsNullOrWhiteSpace(controlName))
            {
                return (null, "ControlName can not be empty or white Space");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, "Name can not be empty or white Space");
            }

            if (position <= 0)
            {
                return (null, "Position can not be zero or negative");
            }

            ZedGraphPositionDto zedGraphPositionDto = new ZedGraphPositionDto(id, controlName, position, name);

            return (zedGraphPositionDto, null);
        }

        public static (List<ZedGraphPositionDto> zedGraphPositionDtos, string error) ConvertToDto(List<ZedGraphPosition> position)
        {
            List<ZedGraphPositionDto> dto = new List<ZedGraphPositionDto>();

            //Валидация
            if (position != null && position.Count > 0)
            {

                foreach (var pos in position)
                {
                    if (pos.Control != null && pos.Position != 0 && !string.IsNullOrWhiteSpace(pos.Name))
                    {
                        var result = ZedGraphPositionDto.Create(pos.Id, pos.Name, pos.Position, pos.Name);

                        if (result.error != null)
                        {
                            return (null, "ZedGraphPositionDto can not be null" + "(result.error)");
                        }

                        dto.Add(result.zedGraphPositionDto);
                    }
                    else
                    {
                        return (null, "Position[i] Can not be null ");
                    }
                }

                if (dto.Count == position.Count)
                {
                    return (dto, null);
                }
            }

            return (null, "Position can not be null or can not be position.Count <= 0 ");
        }

        public void ChangeDto(int position, string name)
        {
            if(position > 0 && !string.IsNullOrWhiteSpace(name))
            {
                Position = position;
                Name = name;
            }
        }
    }
}
