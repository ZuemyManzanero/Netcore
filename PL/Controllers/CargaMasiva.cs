using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class CargaMasiva : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        public CargaMasiva(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        } //inyección de dependencias 

        [HttpGet]
        public ActionResult ProductoCargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }
        [HttpPost]
        public ActionResult ProductoCargaMasiva(ML.Producto producto)
        {

            IFormFile archivo = Request.Form.Files["FileExcel"];

            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                if (archivo.Length > 0)
                {
                    string fileName = Path.GetFileName(archivo.FileName);
                    string folderPath = _configuration["PathFolder:value"];
                    string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();
                    string extensionModulo = _configuration["TipoExcel"];

                    if (extensionArchivo == extensionModulo)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }

                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            ML.Result resultProductos = BL.Producto.ConvertXSLXtoDataTable(connectionString);

                            if (resultProductos.Correct)
                            {
                                ML.Result resultValidacion = BL.Producto.ValidarExcel(resultProductos.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }

                                return View(resultValidacion);
                            }
                            else
                            {
                                ViewBag.Message = "El excel no contiene registros";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Favor de seleccionar un archivo .xlsx, Verifique su archivo";
                    }
                }
                else
                {
                    ViewBag.Message = "No selecciono ningun archivo, Seleccione uno correctamente";
                }
            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Producto.ConvertXSLXtoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Producto productoItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Producto.Add(productoItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el Producto con nombre: " + productoItem.Nombre + " Precio Unitario" + productoItem.PrecioUnitario + "Stock:" + productoItem.Stock + "Decripcion:" + productoItem.Descripcion + "Id de proveedor:" + productoItem.Proveedor.IdProveedor + "Id de departamento:" + productoItem.Departamento.IdDepartamento + " Error: " + resultAdd.ErrorMessage);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\C:\Users\ALIEN 14\Documents\Alma Zuemy Anaya Manzanero\ErrorCargaMasiva.txt");
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los Productos No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los Productos han sido registrados correctamente";
                    }

                }

            }
            return PartialView("Modal");
        }
    }
}