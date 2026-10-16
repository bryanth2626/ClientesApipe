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

        // Constructor
        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/productos -> Consulta todos los productos que hay
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Listar()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return Ok(productos);
        }

        // GET: api/productos/1 -> Consulta un producto por su ID
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

        // Registrar un nuevo producto
        [HttpPost]
        public async Task<ActionResult<Producto>> Registrar([FromBody] Producto producto)
        {
            // Evitar que el usuario envíe un ID manualmente
            producto.Id = 0;

            // Procedemos al registro
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            // Retornamos el producto registrado
            return Ok(new { mensaje = "Producto registrado con éxito", producto });
        }
    }
}