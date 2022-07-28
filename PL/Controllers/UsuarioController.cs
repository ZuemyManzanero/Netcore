using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace PL_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            //GET SIN SERVICIO
            //ML.Usuario usuario = new ML.Usuario();
            //ML.Result result = BL.Usuario.GetAll(usuario);//
            //usuario.Usuarios = result.Objects;
            //return View(usuario);

            //GET CON WEB APPI
            ML.Usuario resulusuario = new ML.Usuario();
            resulusuario.Usuarios = new List<Object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5238");
                var responseTask = client.GetAsync("Usuario/GetAll");
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Usuario resultItemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString()); ;
                        resulusuario.Usuarios.Add(resultItemlist);
                    }
                    ML.Result resul = BL.Usuario.GetAll(resulusuario);
                    resulusuario.Rol = new ML.Rol();
                    resulusuario.Rol.Roles = resul.Objects;
                    resulusuario.Direccion = new ML.Direccion();
                    resulusuario.Direccion.Direcciones = resul.Objects;
                    resulusuario.Direccion.Colonia = new ML.Colonia();
                    resulusuario.Direccion.Colonia.Colonias = resul.Objects;
                    resulusuario.Direccion.Colonia.Municipio = new ML.Municipio();
                    resulusuario.Direccion.Colonia.Municipio.Municipios = resul.Objects;
                    resulusuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                    resulusuario.Direccion.Colonia.Municipio.Estado.Estados = resul.Objects;
                    resulusuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                    resulusuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resul.Objects;
                }
            }
            return View(resulusuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            usuario.Nombre = (usuario.Nombre == null) ? "" : usuario.Nombre;
            usuario.ApellidoPaterno = (usuario.ApellidoPaterno == null) ? "" : usuario.ApellidoPaterno;
            usuario.ApellidoMaterno = (usuario.ApellidoMaterno == null) ? "" : usuario.ApellidoMaterno;

            ML.Result result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {
                usuario.Usuarios = result.Objects.ToList();
                return View(usuario);
            }
            else
            {
                ViewBag.Mensaje = "Ocurrio un error" + result.ErrorMessage;
                return View("Modal");
            }
        }


        //presentar la vista
        [HttpGet]
        public ActionResult Form(int? IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result resulRol = BL.Rol.GetAll();
            ML.Result resulPais = BL.Pais.GetAll();

            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resulPais.Objects;

            if (resulRol.Correct)
            {
                if (IdUsuario == null)
                {
                    usuario.Rol = new ML.Rol();
                    usuario.Rol.Roles = resulRol.Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resulPais.Objects;

                    return View(usuario);

                }
                else
                {
                    //ML.Result result = new ML.Result();
                    //result = BL.Usuario.GetById(IdUsuario.Value);
                    //if (result.Correct)
                    //{
                    //    usuario = ((ML.Usuario)result.Object);
                    //    usuario.Rol.Roles = resulRol.Objects;
                    //    ML.Result resultEstado = BL.Estado.EstadoGetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                    //    ML.Result resulMunicipio = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    //    ML.Result resulColonia = BL.Colonia.ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    //    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resulPais.Objects;
                    //    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
                    //    usuario.Direccion.Colonia.Municipio.Municipios = resulMunicipio.Objects;
                    //    usuario.Direccion.Colonia.Colonias = resulColonia.Objects;
                    //    return View(usuario);
                    //}
                    ML.Result result = new ML.Result();
                    ML.Usuario resulusuario = new ML.Usuario();
                    resulusuario.Usuarios = new List<object>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:5238");

                        var responsetask = client.GetAsync("/api/Usuario/GetById/" + IdUsuario);
                        responsetask.Wait();

                        var resul = responsetask.Result;
                        if (resul.IsSuccessStatusCode)
                        {
                            var readtask = resul.Content.ReadAsAsync<ML.Result>();
                            readtask.Wait();

                            ML.Usuario resulitemlist = new ML.Usuario();
                            resulitemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readtask.Result.Object.ToString());
                            result.Object = resulitemlist;

                            usuario = ((ML.Usuario)result.Object);
                            ML.Result resultestados = BL.Estado.EstadoGetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                            ML.Result resultmunicipios = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                            ML.Result resultcolonia = BL.Colonia.ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                            usuario.Rol.Roles = resulRol.Objects;
                            usuario.Direccion.Colonia.Colonias = resultcolonia.Objects;
                            usuario.Direccion.Colonia.Municipio.Municipios = resultmunicipios.Objects;
                            usuario.Direccion.Colonia.Municipio.Estado.Estados = resultestados.Objects;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resulPais.Objects;
                        }
                    }
                    return View(usuario);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();

            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] ImagenBytes = ConvertToBytes(file);
                usuario.Imagen = Convert.ToBase64String(ImagenBytes);
            }
            if (usuario.IdUsuario == 0)
            {
                //result = BL.Usuario.Add(usuario);
                //if (result.Correct)
                //{
                //    ViewBag.Mensaje = "Se ha agregado el usuario en la BD";
                //}
                //else
                //{
                //    ViewBag.Mensaje = "No se ha agregado el usuario en la BD" + result.ErrorMessage;
                //}
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5238");

                    var postTask = client.PostAsJsonAsync<ML.Usuario>("/api/Usuario/Add", usuario);
                    postTask.Wait();

                    var resultA = postTask.Result;

                    if (resultA.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "El Usuario se ha registrado correctamente";
                    }
                    else
                    {
                        ViewBag.Mensaje = "El Usuario no se ha registrado correctamente " + result.ErrorMessage;
                    }
                }
                return View(usuario);
            }
            else
            {
                //result = BL.Usuario.Update(usuario);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5238");

                    var postTask = client.PostAsJsonAsync<ML.Usuario>("/api/Usuario/Update", usuario);
                    postTask.Wait();

                    var resultA = postTask.Result;

                    if (resultA.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "El usuario se ha registrado correctamente";
                    }
                    else
                    {
                        ViewBag.Mensaje = "El usuario no se ha registrado correctamente " + result.ErrorMessage;
                    }
                }
            }
            return PartialView("Modal");

        }

        [HttpGet]
        public ActionResult Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5238");

                var postTask = client.GetAsync("/api/Usuario/Delete/" + IdUsuario);
                postTask.Wait();

                var resultA = postTask.Result;

                if (resultA.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "El usuario se ha eliminado correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "El usuario no se ha eliminado correctamente " + result.ErrorMessage;
                }
            }
            return PartialView("Modal");
        }

        public JsonResult EstadoGetByIdPais(int IdPais)
        {
            ML.Result result = BL.Estado.EstadoGetByIdPais(IdPais);

            return Json(result.Objects);
        }

        public JsonResult MunicipioGetByIdEstado(int IdEstado)
        {
            ML.Result result = BL.Municipio.MunicipioGetByIdEstado(IdEstado);

            return Json(result.Objects);
        }

        public JsonResult ColoniaGetByIdMunicipio(int IdMunicipio)
        {
            ML.Result result = BL.Colonia.ColoniaGetByIdMunicipio(IdMunicipio);

            return Json(result.Objects);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public ActionResult UpdateStatus(int IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = BL.Usuario.GetById(IdUsuario);

            if (result.Correct)
            {
                usuario = ((ML.Usuario)result.Object);
                usuario.Status = usuario.Status ? false : true;

                ML.Result resultUpdate = BL.Usuario.Update(usuario);
            }
            else
            {
                result.Correct = false;
            }

            return PartialView("Modal");
        }

    }
}
