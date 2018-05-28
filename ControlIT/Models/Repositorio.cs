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

        private Propietario MapPropietario(SqlDataReader reader)
        {
            var prop = new Propietario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2)
            };
            CargarVehiculos(prop);
            return prop;
        }

        private void CargarVehiculos(Propietario prop)
        {
            using(var conn = new SqlConnection(connString))
            {
                var query = VehiculosSelect + " WHERE Propietario = @PropId";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PropId", prop.Id);
                conn.Open();
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CargarVehiculo(prop, reader);
                    }
                }
            }
        }

        private static void CargarVehiculo(Propietario prop, SqlDataReader reader)
        {
            var vh = new Vehiculo
            {
                Id = reader.GetInt32(0),
                Descripcion = reader.GetString(1),
                Latitud = reader.GetDouble(2),
                Longitud = reader.GetDouble(3),
                Sitio = reader.GetString(4),
                Propietario = reader.GetInt32(5)    
            };
            prop.Vehiculos.Add(vh);
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
            using(var conn = new SqlConnection(connString))
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

        private static string PropietarioSelect => "SELECT Id, Nombre, Apellido FROM Propietario";

        private static string VehiculosSelect => "SELECT Id, Descripcion, Latitud, Longitud, Sitio, Propietario FROM Vehiculos";
    }
}