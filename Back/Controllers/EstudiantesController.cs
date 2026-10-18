using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaApi.Data;
using PruebaTecnicaApi.Models;

namespace PruebaTecnicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudiantesController(AppDbContext context)
        {
            _context = context;
        }

      
        public class EstudianteDto
        {
            public string Nombre { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public List<int> MateriaIds { get; set; } = new List<int>(); 
        }

        
        [HttpGet]
public async Task<IActionResult> GetEstudiantes()
{
    var estudiantes = await _context.Estudiantes
        .Include(e => e.EstudianteMaterias)
            .ThenInclude(em => em.Materia)
                .ThenInclude(m => m.Profesor)
        .ToListAsync();

    // Obtener todos los registros para cruzar compañeros
    var allRelaciones = await _context.EstudianteMaterias.ToListAsync();

    var resultado = estudiantes.Select(e => new
    {
        e.Id,
        e.Nombre,
        e.Correo,
        e.TotalCreditos,
        Materias = e.EstudianteMaterias.Select(em => new
        {
            Id = em.Materia.Id,
            Nombre = em.Materia.Nombre,
            Profesor = em.Materia.Profesor.Nombre,
            // Compañeros en esta misma materia (excluyendo al propio estudiante)
            Companieros = allRelaciones
    .Where(r => r.MateriaId == em.MateriaId && r.EstudianteId != e.Id)
    .Select(r => {
        var comp = _context.Estudiantes.FirstOrDefault(est => est.Id == r.EstudianteId);
        return comp != null ? new { Nombre = comp.Nombre, Correo = comp.Correo } : null;
    })
    .Where(c => c != null)
    .ToList()
        })
    });

    return Ok(resultado);
}

        
        [HttpPost]

        
        public async Task<IActionResult> PostEstudiante([FromBody] EstudianteDto dto)
        {
           
            if (dto.MateriaIds == null || dto.MateriaIds.Count != 3)
            {
                return BadRequest(new { mensaje = "El estudiante debe seleccionar exactamente 3 materias." });
            }

            
            var materiasSeleccionadas = await _context.Materias
                .Where(m => dto.MateriaIds.Contains(m.Id))
                .ToListAsync();

            if (materiasSeleccionadas.Count != 3)
            {
                return BadRequest(new { mensaje = "Una o más materias seleccionadas no existen." });
            }

           
            var profesoresIds = materiasSeleccionadas.Select(m => m.ProfesorId).Distinct();
            if (profesoresIds.Count() < 3)
            {
                return BadRequest(new { mensaje = "No puedes elegir materias dictadas por el mismo profesor. Cada materia debe tener un profesor diferente." });
            }

            // Crear el estudiante

            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Correo))
{
    return BadRequest(new { mensaje = "El nombre y el correo electrónico son obligatorios." });
}


            var nuevoEstudiante = new Estudiante
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                TotalCreditos = 9 
            };

            _context.Estudiantes.Add(nuevoEstudiante);
            await _context.SaveChangesAsync();

            foreach (var materiaId in dto.MateriaIds)
            {
                _context.EstudianteMaterias.Add(new EstudianteMateria
                {
                    EstudianteId = nuevoEstudiante.Id,
                    MateriaId = materiaId
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estudiante registrado con éxito", id = nuevoEstudiante.Id });
        }

        [HttpPut("{id}")]
public async Task<IActionResult> ActualizarEstudiante(int id, [FromBody] EstudianteDto dto)
{

    
    var estudiante = await _context.Estudiantes
        .Include(e => e.EstudianteMaterias)
        .FirstOrDefaultAsync(e => e.Id == id);

    if (estudiante == null) return NotFound(new { mensaje = "Estudiante no encontrado." });

    if (dto.MateriaIds == null || dto.MateriaIds.Count != 3)
        return BadRequest(new { mensaje = "El estudiante debe tener exactamente 3 materias." });

    // Validar profesores distintos
    var materiasSeleccionadas = await _context.Materias
        .Where(m => dto.MateriaIds.Contains(m.Id))
        .ToListAsync();

    var profesoresUnicos = materiasSeleccionadas.Select(m => m.ProfesorId).Distinct().Count();
    if (profesoresUnicos != 3)
        return BadRequest(new { mensaje = "No puedes seleccionar dos materias con el mismo profesor." });

    // Actualizar datos básicos

    if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Correo))
{
    return BadRequest(new { mensaje = "El nombre y el correo electrónico son obligatorios." });
}
    estudiante.Nombre = dto.Nombre;
    estudiante.Correo = dto.Correo;

    // Limpiar materias anteriores y asignar las nuevas
    _context.EstudianteMaterias.RemoveRange(estudiante.EstudianteMaterias);

    foreach (var materiaId in dto.MateriaIds)
    {
        estudiante.EstudianteMaterias.Add(new EstudianteMateria
        {
            EstudianteId = estudiante.Id,
            MateriaId = materiaId
        });
    }

    await _context.SaveChangesAsync();
    return Ok(new { mensaje = "Estudiante actualizado con éxito." });
}



        //LIMINAR ESTUDIANTE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound(new { mensaje = "Estudiante no encontrado." });
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estudiante eliminado correctamente." });
        }
    }
}