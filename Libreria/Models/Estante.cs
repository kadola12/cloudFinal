using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class Estante
    {
        [Key]
        public int Id { get; set; }
        public string Genero { get; set; }
    }
}