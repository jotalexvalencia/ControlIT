using ControlIT.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControlIT.Models
{
    public class Repositorio
    {
        private readonly string connString;

        public Repositorio(string connString)
        {
            this.connString = connString;
        }

        public virtual IEnumerable<Propietario> ObtenerPropietarios()
        {
            using(var conn = new SqlConnection(connString))
            {
                var cmd = new SqlCommand(PropietarioSelect, conn);
                conn.Open();
                using(var reader = cmd.ExecuteReader())
                {
                    var result = new List<Propietario>();
                    while(reader.Read())
                    {
                        result.Add(MapPropietario(reader));
                    }
                    return result;
                }
            }
        }

        public virtual Propietario ObtenerPropietario(int id)
        {
            using (var conn = new SqlConnection(connString))
            {
                var sql = PropietarioSelect + " WHERE Id = @Id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapPropietario(reader);
                    }
                }
                return null;
            }
        }

        private static Propietario MapPropietario(SqlDataReader reader)
        {
            return new Propietario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2)
            };
        }

        private static string PropietarioSelect => "SELECT Id, Nombre, Apellido FROM Propietario";
    }
}