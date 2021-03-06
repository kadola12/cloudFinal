﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Genero { get; set; }
        public int Anio { get; set; }
        public string Foto { get; set; }
        public string Editorial { get; set; }
    }
}