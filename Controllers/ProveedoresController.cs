using ClientesApipe.Data;
using ClientesApipe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApipe.Controllers
{
    [ApiController]
    [Route("api/proveedores")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Listar(
            [FromQuery] string? buscar, CancellationToken ct)
        {
            var consulta = _context.Proveedores.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var texto = buscar.Trim();
                consulta = consulta.Where(p => p.Nombre.Contains(texto) || p.Ruc.Contains(texto));
            }

            return Ok(await consulta.OrderBy(p => p.Nombre).ThenBy(p => p.Id).ToListAsync(ct));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Proveedor>> ObtenerPorId(int id, CancellationToken ct)
        {
            var proveedor = await _context.Proveedores.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);
            if (proveedor is null)
                return NotFound(new { mensaje = $"No existe proveedor con ID: {id}" });

            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> Registrar([FromBody] Proveedor proveedor, CancellationToken ct)
        {
            Normalizar(proveedor);
            if (!TryValidateModel(proveedor)) return ValidationProblem(ModelState);
            proveedor.Id = 0;

            if (await _context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc, ct))
                return Conflict(new { mensaje = "Ya existe un proveedor con ese RUC." });

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync(ct);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Proveedor proveedor, CancellationToken ct)
        {
            if (id <= 0 || id != proveedor.Id)
                return BadRequest(new { mensaje = "El ID de la URL debe coincidir con el ID del cuerpo y ser positivo." });

            Normalizar(proveedor);
            if (!TryValidateModel(proveedor)) return ValidationProblem(ModelState);
            var existente = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (existente is null)
                return NotFound(new { mensaje = $"No existe proveedor con ID: {id}" });

            if (await _context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc && p.Id != id, ct))
                return Conflict(new { mensaje = "Ya existe un proveedor con ese RUC." });

            existente.Nombre = proveedor.Nombre;
            existente.Ruc = proveedor.Ruc;
            existente.Telefono = proveedor.Telefono;
            existente.Direccion = proveedor.Direccion;
            await _context.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (proveedor is null)
                return NotFound(new { mensaje = $"No existe proveedor con ID: {id}" });

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync(ct);
            return NoContent();
        }

        private static void Normalizar(Proveedor proveedor)
        {
            proveedor.Nombre = proveedor.Nombre?.Trim() ?? string.Empty;
            proveedor.Ruc = proveedor.Ruc?.Trim() ?? string.Empty;
            proveedor.Telefono = proveedor.Telefono?.Trim() ?? string.Empty;
            proveedor.Direccion = string.IsNullOrWhiteSpace(proveedor.Direccion)
                ? null : proveedor.Direccion.Trim();
        }
    }
}
