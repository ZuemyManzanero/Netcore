using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WEBAPPI.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        [Route("api/subcategoria/GetAll")]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            usuario.Rol = new ML.Rol();

            var result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("api/Usuario/GetById/{IdUsuario}")]
        public IActionResult GetById(int IdUsuario)
        {
            ML.Result result = BL.Usuario.GetById(IdUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("api/Usuario/Add")]
        public IActionResult Post([FromBody]ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("api/Usuario/Update")]
        public IActionResult Update([FromBody] ML.Usuario usuario)
        {
            var result = BL.Usuario.Update(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("api/Usuario/Delete/{IdUsuario}")]
        public IActionResult Delete(int IdUsuario)
        {
            ML.Result result = BL.Usuario.Delete(IdUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}
