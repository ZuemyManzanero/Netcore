using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProveedorController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Proveedor.GetAll();
            ML.Proveedor proveedor = new ML.Proveedor();

            proveedor.Proveedores = result.Objects;

            return View(proveedor);
        }

        [HttpGet]
        public ActionResult Form(int? IdProveedor)
        {
            ML.Proveedor proveedor = new ML.Proveedor();
                if (IdProveedor == null)
                {
                    return View(proveedor);
                }
                else
                {
                    ML.Result result = new ML.Result();
                    result = BL.Proveedor.GetById(IdProveedor.Value);

                    if (result.Correct)
                    {
                        proveedor = ((ML.Proveedor)result.Object);

                        return View(proveedor);
                    }
                }
            return View(IdProveedor);
        }

        [HttpPost]
        public ActionResult Form(ML.Proveedor proveedor)
        {
            ML.Result result = new ML.Result();

            if (proveedor.IdProveedor == 0)
            {
                result = BL.Proveedor.Add(proveedor);

                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se ha agregado el proveedor en la BD";
                }
                else
                {
                    ViewBag.Mensaje = "No se ha agregado el proveedor en la BD" + result.ErrorMessage;
                }
            }
            else
            {
                result = BL.Proveedor.Update(proveedor);

                if (result.Correct)
                {
                    ViewBag.Mensaje = "El proveedor se actualizo correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al realizar la actualizacion" + result.ErrorMessage;
                }
            }
            return PartialView("Modal");
        }

    }
}
