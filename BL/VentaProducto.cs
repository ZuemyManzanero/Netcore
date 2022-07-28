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
    public class VentaProducto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    producto.Departamento.IdDepartamento = (producto.Departamento.IdDepartamento == null) ? 0 : producto.Departamento.IdDepartamento;
                    var query = context.Productos.FromSqlRaw($"ProductoGetByIdDepartamento {producto.Departamento.IdDepartamento}").ToList();

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
    }
}
