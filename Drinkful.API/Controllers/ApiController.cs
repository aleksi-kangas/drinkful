using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Drinkful.API.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase {
  protected IActionResult Problem(List<Error> errors) {
    if (errors.Count is 0) {
      return Problem(); // Unclear why we got here...
    }

    return errors.All(error => error.Type == ErrorType.Validation)
      ? ValidationProblem(errors)
      : Problem(errors[0]);
  }

  /**
   * Transform a generic application layer error to an error response.
   */
  private IActionResult Problem(Error error) {
    var statusCode = error.Type switch {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      _ => StatusCodes.Status500InternalServerError
    };
    return Problem(statusCode: statusCode, title: error.Description);
  }

  /**
   * Overload to accept list of validation errors.
   */
  private IActionResult ValidationProblem(List<Error> errors) {
    var modelStateDictionary = new ModelStateDictionary();
    foreach (var error in errors) {
      modelStateDictionary.AddModelError(error.Code, error.Description);
    }

    return ValidationProblem(modelStateDictionary);
  }
}