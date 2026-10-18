using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaApi.Data;

namespace PruebaTecnicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriasController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetMaterias()
        {
            var materias = await _context.Materias
                .Include(m => m.Profesor)
                .Select(m => new {
                    m.Id,
                    m.Nombre,
                    m.Creditos,
                    ProfesorId = m.Profesor!.Id,
                    ProfesorNombre = m.Profesor.Nombre
                })
                .ToListAsync();

            return Ok(materias);
        }
    }
}