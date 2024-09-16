using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ProcessFileDto
    {
        public byte[] Content { get; set; }
        public ProcessFileDto(byte[] content) 
        {
            Content = content;
        }
    }
}
