using Microsoft.AspNetCore.Mvc;

namespace Board.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class BaseController : Controller
    {
    }
}
