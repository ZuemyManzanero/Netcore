using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    usuario.Rol.IdRol = (usuario.Rol.IdRol == null) ? 0 : usuario.Rol.IdRol;
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}', '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Email}' ,'{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.FechaNacimiento}', '{usuario.Password}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}, {usuario.IdUsuario}, true");
                    
                    if (query > 1)
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

        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}'").ToList();

                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            usuario = new ML.Usuario();

                            usuario.IdUsuario = obj.IdUsuario;
                            usuario.UserName = obj.UserName;
                            usuario.Nombre = obj.Nombre;
                            usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            usuario.Email = obj.Email;
                            usuario.Sexo = obj.Sexo;
                            usuario.Telefono = obj.Telefono;
                            usuario.Celular = obj.Celular;
                            usuario.FechaNacimiento = obj.FechaNacimiento.ToString();
                            usuario.Password = obj.Password;
                            usuario.CURP = obj.CURP;

                            usuario.Rol = new ML.Rol();

                            usuario.Rol.IdRol = obj.IdRol.Value;
                            usuario.Rol.Nombre = obj.RolNombre;

                            usuario.Imagen = obj.Imagen;
                            usuario.Status = obj.Status;

                            usuario.Direccion = new ML.Direccion();

                            usuario.Direccion.Calle = obj.Calle;
                            usuario.Direccion.NumeroExterior = obj.NumeroExterior;
                            usuario.Direccion.NumeroInterior = obj.NumeroInterior;

                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = obj.IdColonia;
                            usuario.Direccion.Colonia.Nombre = obj.ColoniaNombre;
                            usuario.Direccion.Colonia.CodigoPostal = obj.CodigoPostal;

                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = obj.IdMunicipio;
                            usuario.Direccion.Colonia.Municipio.Nombre = obj.MunicipioNombre;

                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = obj.IdEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = obj.EstadoNombre;

                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = obj.IdPais;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = obj.PaisNombre;

                            result.Objects.Add(usuario);
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

        public static ML.Result Delete(int IdUsuario)
        {//SQL Client
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    {
                        var query = context.Database.ExecuteSqlRaw($"UsuarioDelete {IdUsuario}");

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
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {//SQL Client
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.UserName}', '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Email}', '{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.FechaNacimiento}', '{usuario.Password}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', {usuario.Status}, '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroExterior}', '{usuario.Direccion.NumeroInterior}', {usuario.Direccion.Colonia.IdColonia}");

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
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var obj = context.Usuarios.FromSqlRaw($"UsuarioGetById {IdUsuario}").AsEnumerable().FirstOrDefault();

                    result.Objects = new List<object>();

                    if (obj != null)
                    {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = obj.IdUsuario;
                            usuario.UserName = obj.UserName;
                            usuario.Nombre = obj.Nombre;
                            usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            usuario.Email = obj.Email;
                            usuario.Sexo = obj.Sexo;
                            usuario.Telefono = obj.Telefono;
                            usuario.Celular = obj.Celular;
                            usuario.FechaNacimiento = obj.FechaNacimiento.ToString();
                            usuario.Password = obj.Password;
                            usuario.CURP = obj.CURP;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = obj.IdRol.Value;

                            usuario.Imagen = obj.Imagen;
                            usuario.Status = obj.Status;

                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.Calle = obj.Calle;
                            usuario.Direccion.NumeroExterior = obj.NumeroExterior;
                            usuario.Direccion.NumeroInterior = obj.NumeroInterior;

                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = obj.IdColonia;
                            usuario.Direccion.Colonia.Nombre = obj.ColoniaNombre;
                            usuario.Direccion.Colonia.CodigoPostal = obj.CodigoPostal;

                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = obj.IdMunicipio;
                            usuario.Direccion.Colonia.Municipio.Nombre = obj.MunicipioNombre;

                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = obj.IdEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = obj.EstadoNombre;

                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = obj.IdPais;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = obj.PaisNombre;

                            result.Object = usuario;
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
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetByUsername(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var obj = context.Usuarios.FromSqlRaw($"UsuarioGetByUserName '{UserName}'").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();

                    if (obj != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.UserName = obj.UserName;
                        usuario.Password = obj.Password;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = obj.IdRol;
                        usuario.Rol.Nombre = obj.RolNombre;

                        result.Object = usuario;
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
                result.Ex = ex;
            }
            return result;
        }
    }
}
