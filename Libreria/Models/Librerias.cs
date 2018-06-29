using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Libreria.Models
{
    public class Librerias
    {
        [Key]
        public int id { get; set; }
        public List<Estante> estantes { get; set; }
        public List<Libro> libros { get; set; }
        public List<LibroEstante> libroEstantes { get; set; }
        public List <Persona> personas { get; set; }
        public List <PersonaLibro> personaLibros { get; set; }
        public List <FotoPersona> fotoPersonas { get; set; }
        
    }

    public class LibreroContext : DbContext
    {
        public LibreroContext() : base("Libreria")
        {

        }
        public DbSet<Estante> estantes { get; set; }
        public DbSet<Libro> libros { get; set; }
        public DbSet<LibroEstante> libroEstantes { get; set; }
        public DbSet<Persona> personas { get; set; }
        public DbSet<PersonaLibro> personaLibros { get; set; }
        public DbSet<Librerias> Librerias { get; set; }
        public DbSet<FotoPersona> fotoPersonas { get; set; }
    }

    public class LibreroInitializer : CreateDatabaseIfNotExists<LibreroContext>
    {
        protected override void Seed(LibreroContext context)
        {
            var estantes = new List<Estante>
            {
                new Estante{Genero="Novela Romantica"},
                new Estante{Genero="Ciencia ficcion"}
            };
            estantes.ForEach(t => context.estantes.Add(t));
            context.SaveChanges();

            var libros = new List<Libro>
            {
                new Libro{Titulo="Amor Divina Locura", Genero="Novela Romantica", Autor="Walter Risso", anio=2001}
            };
            libros.ForEach(u => context.libros.Add(u));
            context.SaveChanges();
            /*var personas = new List<Persona>
            {
                new Persona{Nombre="Paola Serpa", Genero=1, Estudiante="Universitario", Correo="paola.serpa@ucuenca.edu.ec", Edad=20}
            };
            personas.ForEach(v => context.personas.Add(v));
            context.SaveChanges();*/
        }
    }

    public class LibreroConfiguration : DbConfiguration
    {
        public LibreroConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }



}