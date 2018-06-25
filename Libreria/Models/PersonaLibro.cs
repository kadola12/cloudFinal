﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class PersonaLibro
    {
        [Key]
        public string IdPersona { get; set; }
        public int IdLibro { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaDevolucion { get; set; }

    }
}