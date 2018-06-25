using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class LibroEstante
    {
        [Key]
        public int IdLibro { get; set; }
        public int IdEstante { get; set; }
    }
}