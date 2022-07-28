using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Proveedors.FromSqlRaw("ProveedorGetAll").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Proveedor proveedor = new ML.Proveedor();

                            proveedor.IdProveedor = obj.IdProveedor;
                            proveedor.Nombre = obj.Nombre;
                            proveedor.Telefono = obj.Telefono;

                            result.Objects.Add(proveedor);
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

            public static ML.Result Add(ML.Proveedor proveedor)

            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                    {
                        var query = context.Database.ExecuteSqlRaw($"ProveedorAdd '{proveedor.Nombre}','{proveedor.Telefono}'");
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

            public static ML.Result Delete(ML.Proveedor proveedor)
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                    {
                        var query = context.Database.ExecuteSqlRaw($"ProveedorDelete {proveedor.IdProveedor}");
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

            public static ML.Result GetById(int IdProveedor)
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                    {
                        var query = context.Proveedors.FromSqlRaw($"ProveedorGetById {IdProveedor}");
                        result.Objects = new List<object>();

                        if (query != null)
                        {
                            foreach (var obj in query)
                            {
                                ML.Proveedor proveedor = new ML.Proveedor();

                                proveedor.IdProveedor = obj.IdProveedor;
                                proveedor.Nombre = obj.Nombre;
                                proveedor.Telefono = obj.Telefono;

                                result.Object = proveedor;
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

            public static ML.Result Update(ML.Proveedor proveedor)
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                    {
                        var query = context.Database.ExecuteSqlRaw($"ProveedorUpdate {proveedor.IdProveedor}, '{proveedor.Nombre}', '{proveedor.Telefono}'");
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
        }
    }
