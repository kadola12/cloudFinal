using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class FotoPersona
    {
        [Key]
        public int id { get; set; }
        public int idPersona { get; set; }
        public string faceId { get; set; }
    }
}