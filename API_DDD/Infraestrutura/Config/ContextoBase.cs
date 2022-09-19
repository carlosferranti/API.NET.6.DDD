using Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Config
{
    public class ContextoBase : IdentityDbContext<ApplicationUser>
    {
        public ContextoBase(DbContextOptions<ContextoBase> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }

        public string ObtemStringConexao()
        {
            return "Password=zoinho;Persist Security Info=True;User ID=sa;Initial Catalog=Commerce2;Data Source=DESKTOP-9CA3VTP\\SQLEXPRESS;MultipleActiveResultSets = true";
        }

        // se nao foi configurado a string de conexao entao configura  aqui
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ObtemStringConexao());
                base.OnConfiguring(optionsBuilder);
            }
        }

        // mapeando a chave primaria da tabela Indentities
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder);
        }
    }
}
