using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result EstadoGetByIdPais(int IdPais)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Estados.FromSqlRaw($"EstadoGetByIdPais {IdPais}" ).ToList();
                    result.Objects = new List<object>();

                    if(query != null)
                    {
                        foreach(var obj in query)
                        {
                            ML.Estado estado = new ML.Estado();

                            estado.IdEstado = obj.IdEstado;
                            estado.Nombre = obj.Nombre;

                            estado.Pais = new ML.Pais();
                            estado.Pais.IdPais = obj.IdPais.Value;

                            result.Objects.Add(estado);

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
