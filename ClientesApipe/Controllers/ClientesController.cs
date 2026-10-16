using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ClientesApipe.Data;
using Microsoft.EntityFrameworkCore;
using ClientesApipe.Models;
using System.Threading.Tasks; // Asegúrate de incluir esto si marca falta de Task

namespace ClientesApipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Faltaba cerrar la comilla en [controller]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor (inicializar)
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
        {
            var clientes = await _context.Clientes.AsNoTracking().ToListAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/1 -> Busca clientes por su ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                // Error 404
                return NotFound(new { mensaje = $"No existe el cliente con ID: {id}" });
            }

            // Ok (método 200) - Eliminado el paréntesis extra al final
            return Ok(cliente);
        }

        // POST: api/clientes -> registrar
        [HttpPost]
        public async Task<ActionResult<Cliente>> Registrar([FromBody] Cliente cliente)
        {
            // Evitar que el cliente envíe un ID manualmente
            cliente.Id = 0;

            if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni))
            {
                // Server error 409
                return Conflict(new { mensaje = "Ya existe un cliente con ese DNI" });
            }

            // Procedemos al registro
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Server code 201 (save ok)
            return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
        }

        // PUT: api/clientes/1 -> actualizar
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                // Server code 400
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del cuerpo" });
            }

            // Validación 2: ¿Existe el id?
            var existente = await _context.Clientes.FindAsync(id);

            if (existente == null)
            {
                // Server code 404 - Faltaba el punto y coma (;) al final
                return NotFound(new { mensaje = $"No existe cliente con id: {id}" });
            }

            if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni && c.Id != id))
            {
                // Server error 409
                return Conflict(new { mensaje = "Ya existe un cliente con ese DNI" });
            }

            // Procedemos a realizar la actualización
            existente.Id = cliente.Id;
            existente.Apellidos = cliente.Apellidos;
            existente.Nombres = cliente.Nombres;
            existente.Telefono = cliente.Telefono;
            existente.Correo = cliente.Correo;
            existente.Dni = cliente.Dni;
            existente.Direccion = cliente.Direccion;

            // Confirmamos los cambios
            await _context.SaveChangesAsync();

            // Server Code 204
            return NoContent();
        }

        // DELETE: api/clientes/1 -> eliminar
        [HttpDelete("{id:int}")] // Corregido el cierre de comillas y llaves
        public async Task<ActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                // Corregidas las comillas del string interpolado en el mensaje
                return NotFound(new { mensaje = $"No existe cliente con ID: {id}" });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            // Server Code 204 (acción ejecutada correctamente)
            return NoContent();
        }
    }
}