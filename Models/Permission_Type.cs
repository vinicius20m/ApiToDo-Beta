using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiToDo.Models
{
    public class Permission_Type
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool AddMember { get; set; }
        public bool DeleteMember { get; set; }
        public bool EditQuadro { get; set; }
        public bool DeleteQuadro { get; set; }
    }
}