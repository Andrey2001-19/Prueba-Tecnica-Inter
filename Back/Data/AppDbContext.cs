using Microsoft.EntityFrameworkCore;
using PruebaTecnicaApi.Models;

namespace PruebaTecnicaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<EstudianteMateria> EstudianteMaterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EstudianteMateria>()
                .HasKey(em => new { em.EstudianteId, em.MateriaId });
        }
    }
}