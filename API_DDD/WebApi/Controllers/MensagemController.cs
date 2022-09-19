using AutoMapper;
using Dominio.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : ControllerBase
    {

        private readonly IMapper _imapper;
        private readonly IMensagem _imensagem;

        public MensagemController(IMapper Imapper, IMensagem IMensagem)
        {
            _imapper = Imapper;
            _imensagem = IMensagem;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<List<Notificacao>> Add(MensagemViewModel msg)
        {
            msg.UserId = await RetornarIdUsuarioLogado();
            var mensagemMap = _imapper.Map<Mensagem>(msg);
            await _imensagem.Add(mensagemMap);
            return mensagemMap.Notifica;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Update")]
        public async Task<List<Notificacao>> Update(MensagemViewModel msg)
        {
            var mensagemMap = _imapper.Map<Mensagem>(msg);
            await _imensagem.Update(mensagemMap);
            return mensagemMap.Notifica;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Delete")]
        public async Task<List<Notificacao>> Delete(MensagemViewModel msg)
        {
            var mensagemMap = _imapper.Map<Mensagem>(msg);
            await _imensagem.Delete(mensagemMap);
            return mensagemMap.Notifica;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetEntityById")]
        public async Task<MensagemViewModel> GetEntityById(Mensagem msg)
        {
            msg = await _imensagem.GetEntityById(msg.Id);
            var mensagemMap = _imapper.Map<MensagemViewModel>(msg);
            return mensagemMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/List")]
        public async Task<List<MensagemViewModel>> List()
        {
            var mensagens = await _imensagem.List();
            var mensagemMap = _imapper.Map<List<MensagemViewModel>>(mensagens);
            return mensagemMap;
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            if (User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }

            return string.Empty;
        }


    }
}
