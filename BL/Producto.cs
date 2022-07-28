
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, '{producto.Descripcion}', {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, '{producto.Imagen}'");
                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    producto.Departamento.IdDepartamento = (producto.Departamento.IdDepartamento == null) ? 0 : producto.Departamento.IdDepartamento;
                    var query = context.Productos.FromSqlRaw($"ProductoGetAll {producto.Departamento.IdDepartamento}, '{producto.Nombre}'").ToList();

                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            producto = new ML.Producto();
                            producto.IdProducto = obj.IdProducto;
                            producto.Nombre = obj.Nombre;
                            producto.PrecioUnitario = obj.PrecioUnitario;
                            producto.Stock = obj.Stock;
                            producto.Descripcion = obj.Descripcion;

                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = obj.IdProveedor.Value;
                            producto.Proveedor.Nombre = obj.ProveedorNombre;

                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = obj.IdDepartamento.Value;
                            producto.Departamento.Nombre = obj.DepartamentoNombre;

                            producto.Imagen = obj.Imagen;

                            result.Objects.Add(producto);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoDelete {IdProducto}");
                    result.Objects = new List<object>();
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetById {IdProducto}");
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Producto producto = new ML.Producto();

                            producto.IdProducto = obj.IdProducto;
                            producto.Nombre = obj.Nombre;
                            producto.PrecioUnitario = obj.PrecioUnitario;
                            producto.Stock = obj.Stock;

                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = obj.IdProveedor.Value;
                            producto.Proveedor.Nombre = obj.ProveedorNombre;

                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = obj.IdDepartamento.Value;
                            producto.Departamento.Nombre = obj.DepartamentoNombre;

                            producto.Imagen = obj.Imagen;

                            result.Object = producto;
                            result.Correct = true;
                        }
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto}, '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, '{producto.Descripcion}', {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, '{producto.Imagen}'"); 
                    result.Objects = new List<object>();
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result CargaMasiva()
        {
            ML.Result result = new ML.Result();
            ML.Result resultErrores = new ML.Result();

            StreamReader archivo = new System.IO.StreamReader(@"C:\Users\ALIEN 14\Documents\Alma Zuemy Anaya Manzanero\CargaMasivaProducto.txt");

            string line;

            line = archivo.ReadLine();
            while ((line = archivo.ReadLine()) != null)
            {
                string[] datos = line.Split('|');

                ML.Producto producto = new ML.Producto();

                producto.Nombre = datos[0];
                producto.PrecioUnitario = Convert.ToDecimal(datos[1]);
                producto.Stock = int.Parse(datos[2]);
                producto.Descripcion = datos[3];

                producto.Proveedor = new ML.Proveedor();
                producto.Proveedor.IdProveedor = Convert.ToInt32(datos[4]);

                producto.Departamento = new ML.Departamento();
                producto.Departamento.IdDepartamento = Convert.ToInt32(datos[5]);

                producto.Imagen = datos[6];

                result = BL.Producto.Add(producto);

                if (result.Correct == false)
                {
                    resultErrores.Objects.Add("No se pudo Ingresar el Nombre:  " + producto.Nombre + "" +
                                              "No se pudo ingresar el Precio Unitario" + producto.PrecioUnitario + "" +
                                              "No se pudo ingresar la descripcion: " + producto.Descripcion + "" +
                                              "No se pudo agregar el Id del Proveedor: " + producto.Proveedor.IdProveedor + "" +
                                              "No se pudo ingresar el Id del Departamento: " + producto.Departamento.IdDepartamento + "" +
                                              "No se pudo ingresar la imagen: " + producto.Imagen);
                }
            }
            archivo.Close();

            TextWriter tw = new StreamWriter(@"C:\Users\ALIEN 14\Documents\Alma Zuemy Anaya Manzanero\ErroresCargaMasiva.txt");

            foreach (string error in resultErrores.Objects)
            {
                tw.WriteLine(error);
                Console.WriteLine(error);
            }

            tw.Close();

            return result;
        }

        public static ML.Result ConvertXSLXtoDataTable(string connString)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;
                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;
                        DataTable tableProducto = new DataTable();
                        da.Fill(tableProducto);
                        if (tableProducto.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();
                            foreach (DataRow row in tableProducto.Rows)
                            {
                                ML.Producto producto = new ML.Producto();
                                producto.Nombre = row[0].ToString();
                                producto.PrecioUnitario = decimal.Parse(row[1].ToString());
                                producto.Stock = int.Parse(row[2].ToString());
                                producto.Descripcion = row[3].ToString();
                                producto.Proveedor = new ML.Proveedor();
                                producto.Proveedor.IdProveedor = int.Parse(row[4].ToString());
                                producto.Departamento = new ML.Departamento();
                                producto.Departamento.IdDepartamento = int.Parse(row[5].ToString());
                                result.Object = tableProducto;
                                result.Objects.Add(producto);
                            }
                            result.Correct = true;
                        }
                        if (tableProducto.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en el excel";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Producto producto in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    if (producto.Nombre == "")
                    {
                        error.Mensaje += "Ingresar el nombre  ";
                    }
                    if (producto.PrecioUnitario.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el precio unitario ";
                    }
                    if (producto.Stock.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el stock ";
                    }
                    if (producto.Descripcion == "")
                    {
                        error.Mensaje += "Ingresar la descripcion ";
                    }
                    if (producto.Proveedor.IdProveedor.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Id del proveedor ";
                    }
                    if (producto.Departamento.IdDepartamento.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Id del departamento ";
                    }

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        public static ML.Result AgregarExcel(DataTable tableProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    ML.Producto producto= new ML.Producto();
                    producto.Proveedor = new ML.Proveedor();
                    producto.Departamento = new ML.Departamento();

                    foreach (DataRow row in tableProducto.Rows)
                    {
                        //var query = context(empleado.NumeroEmpleado = row[0].ToString(), empleado.RFC = row[4].ToString(), empleado.Nombre = row[1].ToString(), empleado.ApellidoPaterno = row[2].ToString(), empleado.ApellidoMaterno = row[3].ToString(), empleado.Email, empleado.Telefono, empleado.FechaNacimiento, empleado.NSS, empleado.FechaIngreso, empleado.Foto, empleado.empresa.IdEmpresa);
                        var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, '{producto.Descripcion}', {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}");

                        if (query >= 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se ha podido realizar el insert";
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
