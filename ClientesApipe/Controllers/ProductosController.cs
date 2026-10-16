using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientesApipe.Data;
using Microsoft.EntityFrameworkCore;
using ClientesApipe.Models;

namespace ClientesApipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Listar()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> ObtenerPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = $"No existe el producto con ID: {id}" });
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Registrar([FromBody] Producto producto)
        {
            producto.Id = 0;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto registrado con éxito", producto });
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del cuerpo" });
            }

            var existente = await _context.Productos.FindAsync(id);

            if (existente == null)
            {
                return NotFound(new { mensaje = $"No existe producto con ID: {id}" });
            }

            // Actualizamos los campos
            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Garantia = producto.Garantia;
            existente.Numserie = producto.Numserie;

            await _context.SaveChangesAsync();

            // CAMBIO AQUÍ: Ahora retornará un mensaje de éxito y los datos modificados
            return Ok(new
            {
                mensaje = "¡Producto modificado con éxito!",
                productoActualizado = existente
            });
        }

        // DELETE: api/productos/1 -> Eliminar un producto
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = $"No existe producto con ID: {id}" });
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            // CAMBIO AQUÍ: Ahora retornará un mensaje de confirmación en formato JSON
            return Ok(new
            {
                mensaje = "¡Producto eliminado con éxito!",
                idEliminado = id
            });
        }
    }
}