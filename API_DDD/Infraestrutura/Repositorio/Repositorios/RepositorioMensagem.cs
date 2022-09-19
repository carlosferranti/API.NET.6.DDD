using Dominio.Interfaces;
using Entities.Entities;
using Infraestrutura.Config;
using Infraestrutura.Repositorio.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio.Repositorios
{
    public class RepositorioMensagem : RepositorioGeneric<Mensagem>, IMensagem
    {
        private readonly DbContextOptions<ContextoBase> _OptionsBuilder;

        public RepositorioMensagem()
        {
            _OptionsBuilder = new DbContextOptions<ContextoBase>();
        }        
       
    }
}
