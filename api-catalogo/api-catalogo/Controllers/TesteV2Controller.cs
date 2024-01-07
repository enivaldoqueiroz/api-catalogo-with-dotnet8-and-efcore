using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/teste")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Content("<html><body><h2>TesteV2Controller - V 2.0 </h2></body></html>");
        }
    }
}
