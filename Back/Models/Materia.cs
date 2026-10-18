using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaApi.Models
{
    public class Materia
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; } = 3;
        
        public int ProfesorId { get; set; }
        [ForeignKey("ProfesorId")]
        public Profesor? Profesor { get; set; }
    }
}