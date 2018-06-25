using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class Persona
    {
        [Key]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Estudiante { get; set; }
        public int Genero { get; set; }
        public int Edad { get; set; }
        public string Correo { get; set; }
       
    }




}