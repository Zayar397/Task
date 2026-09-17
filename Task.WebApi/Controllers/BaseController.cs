using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Domain.Models;

namespace Task.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult ReturnStatus<T>(Result<T> model)
        {
            if (model.IsValidationError)
            {
                return BadRequest();
            }
            else if (model.IsSystemError)
            {
                return StatusCode(500, model);
            }
            else
            {
                return Ok(model);
            }
        }
    }
}
