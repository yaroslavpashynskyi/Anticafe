using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class HallDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Turnkey { get; set; } = false;
        public ICollection<ActivityDto> activities { get; set; }
    }
}