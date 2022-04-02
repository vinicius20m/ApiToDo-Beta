using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiToDo.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ApiToDo.Models
{
    public class Ambiente
    {
        
        public int Id { get; set; }
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "O Titulo do Ambiente não pode ser vazio")]
        public string Title { get; set; }
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}