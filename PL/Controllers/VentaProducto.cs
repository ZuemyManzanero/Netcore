using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class VentaProducto : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            ML.Result resuldepartamento = BL.Departamento.GetAll();
            ML.Result resularea = BL.Area.GetAll();
            ML.Result result = BL.VentaProducto.GetAll(producto);

            producto.Productos = result.Objects;
            producto.Departamento.Departamentos = resuldepartamento.Objects;
            producto.Departamento.Area.Areas = resularea.Objects;

            return View(producto);
        }

        public ActionResult GetAll(ML.Producto producto)
        {
            ML.Result resuldepartamento = BL.Departamento.GetAll();
            ML.Result result = BL.Producto.GetAll(producto);
            ML.Result resularea = BL.Area.GetAll();
            if (result.Correct)
            {
                producto.Productos = result.Objects.ToList();
                producto.Departamento.Departamentos = resuldepartamento.Objects;
                producto.Departamento.Area.Areas = resularea.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Mensaje = "Ocurrio un error" + result.ErrorMessage;
                return View("Modal");
            }
        }

        //presentar la vista
        [HttpGet]
        public ActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();

            ML.Result resulProvedor = BL.Proveedor.GetAll();

            ML.Result resulDepartamento = BL.Departamento.GetAll();

            ML.Result resularea = BL.Area.GetAll();
            if (resulProvedor.Correct && resulDepartamento.Correct && resularea.Correct)
            {
                if (IdProducto == null)
                {
                    producto.Proveedor = new ML.Proveedor();
                    producto.Proveedor.Proveedores = resulProvedor.Objects;

                    producto.Departamento = new ML.Departamento();
                    producto.Departamento.Departamentos = resulDepartamento.Objects;

                    producto.Departamento.Area = new ML.Area();
                    producto.Departamento.Area.Areas = resularea.Objects;

                    return View(producto);
                }
                else
                {
                    ML.Result result = new ML.Result();
                    result = BL.Producto.GetById(IdProducto.Value);

                    if (result.Correct)
                    {
                        producto = ((ML.Producto)result.Object);

                        producto.Proveedor.Proveedores = resulProvedor.Objects;

                        producto.Departamento.Departamentos = resulDepartamento.Objects;

                        producto.Departamento.Area.Areas = resularea.Objects;

                        return View(producto);
                    }
                }
            }
            return View(IdProducto);
        }

        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            ML.Result result = new ML.Result();


            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] ImagenBytes = ConvertToBytes(file);
                producto.Imagen = Convert.ToBase64String(ImagenBytes);
            }

            if (producto.IdProducto == 0)
            {
                result = BL.Producto.Add(producto);

                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se ha agregado el producto en la BD";
                }
                else
                {
                    ViewBag.Mensaje = "No se ha agregado el producto en la BD" + result.ErrorMessage;
                }
            }
            else
            {
                result = BL.Producto.Update(producto);

                if (result.Correct)
                {
                    ViewBag.Mensaje = "El producto se actualizo correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al realizar la actualizacion" + result.ErrorMessage;
                }
            }
            return PartialView("Modal");
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public JsonResult DepartamentoGetByIdArea(int IdArea)
        {
            ML.Result result = BL.Departamento.DepartamentoGetByIdArea(IdArea);

            return Json(result.Objects);
        }
    }
}
