using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiToDo.Models
{
    public class Quadro
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }
        public int AmbienteId { get; set; }
        public Ambiente? Ambiente { get; set; }
    }
}