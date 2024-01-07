using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    //[Route("api/v{v:apiVersion}/teste")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.0 </h2></body></html>");
        }
    }
}
