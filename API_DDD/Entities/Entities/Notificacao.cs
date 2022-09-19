using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Notificacao
    {
        public Notificacao()
        {
            Notifica = new List<Notificacao>();
        }

        [NotMapped]
        public string NomePropriedade { get; set; }

        [NotMapped]
        public string Mensagem { get; set; }

        [NotMapped]
        public List<Notificacao> Notifica { get; set; }

        public bool ValidarPropriedadeString(string valor, string nomePropriedade)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                Notifica.Add(new Notificacao()
                {
                    Mensagem = "Campo obrigatório",
                    NomePropriedade = nomePropriedade
                });

                return false;
            }

            return true;
        }

        public bool ValidarPropriedadeInt(int valor, string nomePropriedade)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                Notifica.Add(new Notificacao()
                {
                    Mensagem = "Campo obrigatório",
                    NomePropriedade = nomePropriedade
                });

                return false;
            }

            return true;
        }
    }
}
