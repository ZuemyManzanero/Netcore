using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WEBAPPI.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [Route("api/Login/GetByUserName/{UserName}")]
        public IActionResult Login(string UserName)
        {
            ML.Usuario usuario = new ML.Usuario();
            var result = BL.Usuario.GetByUsername(UserName);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
