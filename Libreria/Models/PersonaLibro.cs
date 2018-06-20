using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class PersonaLibro
    {
        public string IdPersona { get; set; }
        public int IdLibro { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaDevolucion { get; set; }

    }
}