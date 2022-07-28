using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Diagnostics;

namespace PL_MVC.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            //ML.Producto producto = new ML.Producto();
            //producto.Departamento = new ML.Departamento();

            //ML.Result result = BL.Producto.GetAll(producto);
            //ML.Result resuldepartamento = BL.Departamento.GetAll();
 
            //producto.Productos = result.Objects;
            //producto.Departamento.Departamentos = resuldepartamento.Objects;

            //return View(producto);

            ML.Producto resulproducto = new ML.Producto();
            resulproducto.Productos = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5238");
                var responseTask = client.GetAsync("/api/Producto/GetAll");
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Producto resultItemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultItem.ToString()); 
                        resulproducto.Productos.Add(resultItemlist);
                    }
                    ML.Result resuldepa = BL.Departamento.GetAll();
                    resulproducto.Departamento = new ML.Departamento();
                    resulproducto.Departamento.Departamentos = resuldepa.Objects;
                }
            }
            return View(resulproducto);
        }

        public ActionResult GetAll(ML.Producto producto)
        {
            producto.Nombre = (producto.Nombre == null)? "" :producto.Nombre;

            ML.Result resuldepartamento = BL.Departamento.GetAll();
            ML.Result result = BL.Producto.GetAll(producto);
            if (result.Correct)
            {
                producto.Productos = result.Objects.ToList();
                producto.Departamento.Departamentos = resuldepartamento.Objects;
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
           
            if (resulProvedor.Correct && resulDepartamento.Correct)
            {
                if (IdProducto == null)
                {
                    producto.Proveedor = new ML.Proveedor();
                    producto.Proveedor.Proveedores = resulProvedor.Objects;

                    producto.Departamento = new ML.Departamento();
                    producto.Departamento.Departamentos = resulDepartamento.Objects;

                    return View(producto);
                }
                else
                {
                    ML.Result result = new ML.Result();
                    //result = BL.Producto.GetById(IdProducto.Value);

                    //if (result.Correct)
                    //{
                    //    producto = ((ML.Producto)result.Object);

                    //    producto.Proveedor.Proveedores = resulProvedor.Objects;

                    //    producto.Departamento.Departamentos = resulDepartamento.Objects;

                    //    return View(producto);
                    //}
                    ML.Producto resulproducto = new ML.Producto();
                    resulproducto.Productos = new List<object>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:5238");

                        var responsetask = client.GetAsync("/api/Producto/GetById/" + IdProducto);
                        responsetask.Wait();

                        var resul = responsetask.Result;
                        if (resul.IsSuccessStatusCode)
                        {
                            var readtask = resul.Content.ReadAsAsync<ML.Result>();
                            readtask.Wait();

                            ML.Producto resulitemlist = new ML.Producto();
                            resulitemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(readtask.Result.Object.ToString());
                            result.Object = resulitemlist;

                            producto = ((ML.Producto)result.Object);
                            producto.Proveedor.Proveedores = resulProvedor.Objects;
                            producto.Departamento.Departamentos = resulDepartamento.Objects;
                        }
                    }
                }
            }
            return View(producto);
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5238");

                    var postTask = client.PostAsJsonAsync<ML.Producto>("/api/Producto/Add", producto);
                    postTask.Wait();

                    var resultA = postTask.Result;

                    if (resultA.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "El producto se ha registrado correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "El producto no se ha registrado correctamente " + result.ErrorMessage;
                    }
                }
            }
            else
            {
                //result = BL.Producto.Update(producto);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5238");

                    var postTask = client.PostAsJsonAsync<ML.Producto>("/api/Producto/Update", producto);
                    postTask.Wait();

                    var resultA = postTask.Result;

                    if (resultA.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "El producto se ha registrado correctamente";
                    }
                    else
                    {
                        ViewBag.Mensaje = "El producto no se ha registrado correctamente " + result.ErrorMessage;
                    }
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int IdProducto)
        {
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5238");

                var postTask = client.GetAsync("/api/Producto/Delete/" + IdProducto);
                postTask.Wait();

                var resultA = postTask.Result;

                if (resultA.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "El producto se ha eliminado correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "El producto no se ha eliminado correctamente " + result.ErrorMessage;
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

    }
}
