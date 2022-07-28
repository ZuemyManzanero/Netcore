using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Area
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AAnayaProgramacionNCapasContext context = new DL.AAnayaProgramacionNCapasContext())
                {
                    var query = context.Areas.FromSqlRaw("AreaGetAll");
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Area area = new ML.Area();

                            area.IdArea = obj.IdArea;
                            area.Nombre = obj.Nombre;

                            result.Objects.Add(area);
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
