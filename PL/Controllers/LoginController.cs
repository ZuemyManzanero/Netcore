using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Login(ML.Usuario usuario)
        //{ 
        //    ML.Result result = BL.Usuario.GetByUsername(usuario.UserName);

        //    if (result.Correct)
        //    {
        //        ML.Usuario resulusuario = new ML.Usuario();
        //        resulusuario = (ML.Usuario)result.Object;
        //        if(usuario.UserName == resulusuario.UserName && usuario.Password == resulusuario.Password)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ViewBag.Mensaje = "Error en los datos introducidos...";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Mensaje = "No se encuentra el usuario en los registros";
        //    }

        //    return View("Index", "Home");

        //}

        public ActionResult Login(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            ML.Usuario resulusuario = new ML.Usuario();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5238");
                var postTask = client.GetAsync("api/Login/GetByUserName/" + usuario.UserName);
                postTask.Wait();
                var resultA = postTask.Result;
                if (resultA.IsSuccessStatusCode)
                {
                    var readtask = resultA.Content.ReadAsAsync<ML.Result>();
                    readtask.Wait();
                    resulusuario = JsonConvert.DeserializeObject<ML.Usuario>(readtask.Result.Object.ToString());
                    result.Object = resulusuario;
                    resulusuario = (ML.Usuario)result.Object;
                    if (usuario.UserName == resulusuario.UserName && usuario.Password == resulusuario.Password)
                    {
                        HttpContext.Session.SetString("Rol", JsonConvert.SerializeObject(resulusuario));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Mensaje = "Ingresa datos correctos";
                    }
                }
            }
            return View(usuario);
        }
    }
}
