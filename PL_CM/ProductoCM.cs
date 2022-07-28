using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL_CM
{
    public class ProductoCM
    {
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
                    resultErrores.Objects.Add("No se pudo Ingresar el Nombre:  "+producto.Nombre+""+
                                              "No se pudo ingresar el Precio Unitario"+producto.PrecioUnitario+""+
                                              "No se pudo ingresar la Descripcion: "+producto.Descripcion+""+
                                              "No se pudo agregar el Id del Proveedor: "+producto.Proveedor.IdProveedor+""+
                                              "No se pudo ingresar el Id del Departamento: "+producto.Departamento.IdDepartamento+""+
                                              "No se pudo ingresar la Imagen: "+producto.Imagen);
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
    }
}
