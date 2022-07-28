using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WEBAPPI.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        [Route("api/Producto/GetAll")]
        public IActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();
            producto.Proveedor = new ML.Proveedor();

            var result = BL.Producto.GetAll(producto);
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
        [Route("api/Producto/GetById/{IdProducto}")]
        public IActionResult GetById(int IdProducto)
        {
            ML.Result result = BL.Producto.GetById(IdProducto);

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
        [Route("api/Producto/Add")]
        public IActionResult Post([FromBody] ML.Producto producto)
        {

            ML.Result result = BL.Producto.Add(producto);

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
        [Route("api/Producto/Update")]
        public IActionResult Update([FromBody] ML.Producto producto)
        {
            var result = BL.Producto.Update(producto);
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
        [Route("api/Producto/Delete/{IdProducto}")]
        public IActionResult Delete(int IdProducto)
        {
            ML.Result result = BL.Producto.Delete(IdProducto);

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
