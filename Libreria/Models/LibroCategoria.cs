using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class LibroCategoria
    {
        [Key]
        public int ID { get; set; }
        public int Libro { get; set; }
        public int Categoria { get; set; }
        public int Codigo { get; set; }
    }
}