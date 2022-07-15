using Microsoft.AspNetCore.Mvc;

namespace LEN.Core.Api.Controllers
{
    public class BaseController : Controller
    {
        protected ICollection<string> Erros = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            return OperacaoValida() ? Ok(result) : BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected bool OperacaoValida() => !Erros.Any();

        protected void AdicionarErroProcessamento(string erro) => Erros.Add(erro);

        protected void LimparErrosProcessamento() => Erros.Clear();
    }
}
