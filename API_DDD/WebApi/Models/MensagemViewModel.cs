using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace WebApi.Models
{
    public class MensagemViewModel
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UserId { get; set; }

    }
}