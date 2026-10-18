using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaApi.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int TotalCreditos { get; set; } = 9;

     
        public ICollection<EstudianteMateria> EstudianteMaterias { get; set; } = new List<EstudianteMateria>();
    }
}