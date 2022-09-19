using Dominio.Interfaces.InterfaceServico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Interfaces;

namespace Dominio.Servicos
{
    public class ServicoMensagem : IServicoMensagem
    {
        private readonly IMensagem _IMessage;

        public ServicoMensagem(IMensagem iMessage)
        {
            this._IMessage = iMessage;
        }
    }
}
