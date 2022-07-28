using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DepartamentoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Departamento.GetAll();
            ML.Departamento departamento = new ML.Departamento();

            departamento.Departamentos = result.Objects;

            return View(departamento);
        }

        [HttpGet]
        public ActionResult Form(int? IdDepartamento)
        {
            ML.Departamento departamento = new ML.Departamento();

            ML.Result resularea = BL.Area.GetAll();

            if (resularea.Correct)
            {
                if (IdDepartamento == null)
                {
                    departamento.Area = new ML.Area();

                    departamento.Area.Areas = resularea.Objects;

                    return View(departamento);
                }
                else
                {
                    ML.Result result = new ML.Result();
                    result = BL.Departamento.GetById(IdDepartamento.Value);

                    if (result.Correct)
                    {
                        departamento = ((ML.Departamento)result.Object);

                        departamento.Area.Areas = resularea.Objects;

                        return View(departamento);
                    }
                }
            }
            return View(IdDepartamento);
        }

        [HttpPost]
        public ActionResult Form(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();

            if (departamento.IdDepartamento == 0)
            {
                result = BL.Departamento.Add(departamento);

                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se ha agregado el departamento en la BD";
                }
                else
                {
                    ViewBag.Mensaje = "No se ha agregado el departamento en la BD" + result.ErrorMessage;
                }
            }
            else
            {
                result = BL.Departamento.Update(departamento);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "El departamento se actualizo correctamente";
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
