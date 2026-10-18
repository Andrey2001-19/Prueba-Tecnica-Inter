using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaApi.Models
{
    public class Profesor
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}