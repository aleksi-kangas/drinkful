using Microsoft.AspNetCore.Mvc;

namespace Drinkful.API.Controllers;

public class ErrorController : ControllerBase {
  [Route("/error")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult Error() => Problem();
}