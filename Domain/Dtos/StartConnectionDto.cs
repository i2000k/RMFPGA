using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class StartConnectionDto
    {
        public Guid UserId { get; set; }
        public Guid StandId { get; set; }
    }
}
