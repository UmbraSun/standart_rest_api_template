using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record class LogResponseDto
    {
        public int StatusCode { get; init; }
        public string UserName { get; init; }
        public string BodyJson { get; init; }
    }
}
