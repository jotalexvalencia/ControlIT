using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(PropietarioSelect, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var result = new List<Propietario>();
                    Propietario prop = null;
                    int idAnterior = 0;
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        if (id != idAnterior)
                        {
                            idAnterior = id;
                            prop = MapPropietario(id, reader);
                            result.Add(prop);
                        }
                        CargarVehiculo(prop, reader);
                    }
                    return result;
                }
            }
        }

        public virtual Propietario ObtenerPropietario(int id)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(PropietarioSelect + " WHERE p.Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    Propietario prop = null;
                    while(reader.Read())
                    {
                        if (prop == null)
                        {
                            prop = MapPropietario(id, reader); 
                        }
                        CargarVehiculo(prop, reader);
                    }
                    return prop;
                }
            }
        }

        private Propietario MapPropietario(int id, SqlDataReader reader)
        {
            return new Propietario
            {
                Id = id,
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2)
            };
        }

        private static void CargarVehiculo(Propietario prop, SqlDataReader reader)
        {
            bool tieneVehiculo = !reader.IsDBNull(3);
            if (tieneVehiculo)
            {
                prop.Vehiculos.Add(new Vehiculo
                {
                    Id = reader.GetInt32(3),
                    Descripcion = reader.GetString(4),
                    Latitud = reader.GetDouble(5),
                    Longitud = reader.GetDouble(6),
                    Sitio = reader.GetString(7),
                    Propietario = prop.Id
                });
            }
        }

        public virtual void EliminarPropietario(int id)
        {
            using (var conn = new SqlConnection(connString))
            {
                var query = "DELETE Propietario WHERE Id = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            };
        }

        public virtual void CrearPropietario(Propietario prop)
        {
            using (var conn = new SqlConnection(connString))
            {
                var query = "INSERT INTO propietario values(@nombre, @apellido);"
                    + "SELECT SCOPE_IDENTITY()";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", prop.Nombre?.ToUpper());
                    cmd.Parameters.AddWithValue("@apellido", prop.Apellido?.ToUpper());

                    conn.Open();

                    prop.Id = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
        }

        public virtual void ActualizarPropietario(Propietario prop)
        {
            using (var conn = new SqlConnection(connString))
            {
                string sqlText = $"update propietario set nombre=@nombre,apellido=@apellido where id={prop.Id}";
                var cmd = new SqlCommand(sqlText, conn);
                cmd.Parameters.AddWithValue("@nombre", prop.Nombre.ToUpper().Trim());
                cmd.Parameters.AddWithValue("@apellido", prop.Apellido.ToUpper().Trim());
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static string PropietarioSelect =>
            "SELECT p.Id, p.Nombre, p.Apellido, v.Id as VehiculoId, " +
            "v.Descripcion, v.Latitud, v.Longitud, v.Sitio " +
            "FROM Propietario p LEFT JOIN Vehiculos v ON p.Id = v.Propietario";
    }
}