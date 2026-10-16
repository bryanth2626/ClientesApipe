using ClientesApipe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientesApipe.Data
{
    public class AppDbContext : DbContext
    {
        //Constructor (inicializar)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        
        }
        //ORM tendra acceso al modelo cliente
        public DbSet<Cliente> Clientes { get; set; }    
        public DbSet<Producto> Productos { get; set; }
    }
}
