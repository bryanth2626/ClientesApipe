using ClientesApipe.Data;
using ClientesApipe.Models;
// using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Timers;

namespace ClientesApipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; //Acceso BD
        private readonly IConfiguration _config; // Configuracion

        //Constructor
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //Registrar
        //POST: API/AUTH/REGISTRAR
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            //si el usuario ya existe, cancelamos el proceso
            if (await _context.Usuarios.AnyAsync(u=> u.Username == request.Username))
            {
                //Server code
                return Conflict(new { mensaje = "El usuario ya existe" });
            }
            //Si no existe, creamos uno nuevo utuilizando AuthDTOS
            var usuario = new Usuario
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Rol = "Usuario"
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado correctamente"});
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(U => U.Username == request.Username);
            //la contraseña de desencripta?
            //No, se utiliza un algoritmo matematico para verificar su autenticidad
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                //Server code 401
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrecta" });
            }
            //var => declaracion dinamica de objetos
            //Deseñalizacion

            var (token, expira)=GenerarToken(usuario);
            return Ok(new { token, expira });

        }
        //El método retornará:
        //Token: cadena larga de caracteres
        //fecha hora: vigencia del token
        private (string token, DateTime expira) GenerarToken (Usuario usuario)
        {
            //Json Web Token
            var jwt = _config.GetSection("Jwt");
            //Nota: Los parametros ["Param"] vienen del archivo de configuracion (JSON)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var expira = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"]!));
            var descriptor = new SecurityTokenDescriptor
            {

                //ID Usuario, nombre, rol, permiso
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Username),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                }),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                Expires = expira,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

            };
            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return (token, expira);
        }
    }
}
